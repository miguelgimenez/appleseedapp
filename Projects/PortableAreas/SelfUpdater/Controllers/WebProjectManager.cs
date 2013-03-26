﻿using System.Runtime.Versioning;
using System.Web;
using NuGet;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.WebPages.Administration.PackageManager;


namespace SelfUpdater.Controllers
{


    public class WebProjectManager
    {
        private readonly IProjectManager _projectManager;

        public WebProjectManager()
        {
            _projectManager.Logger = new LoggerController();
        }

        public WebProjectManager(string remoteSource, string siteRoot)
        {
            string webRepositoryDirectory = GetWebRepositoryDirectory(siteRoot);
            Uri uri = new Uri(remoteSource);
            IPackageRepository repository = new DataServicePackageRepository(uri);//PackageRepositoryFactory.Default.CreateRepository(remoteSource);
            IPackagePathResolver resolver = new DefaultPackagePathResolver(webRepositoryDirectory);
            IPackageRepository repository2 = new LocalPackageRepository(webRepositoryDirectory, false); //PackageRepositoryFactory.Default.CreateRepository(webRepositoryDirectory);
            IProjectSystem system = new WebProjectSystem(siteRoot);
            ((DataServicePackageRepository)repository).ProgressAvailable += new EventHandler<ProgressEventArgs>(repository_ProgressAvailable);
            //((DataServicePackageRepository)repository).SendingRequest += new EventHandler<WebRequestEventArgs>(repository_sendingRequest);
            this._projectManager = new ProjectManager(repository, resolver, system, repository2);            
            _projectManager.Logger = new LoggerController();
        }

        void repository_ProgressAvailable(object sender, ProgressEventArgs e) {
            this.addLog(e.Operation + " " + e.PercentComplete.ToString());
        }

        void repository_sendingRequest(object sender, WebRequestEventArgs e) {
            this.addLog("Registro sending request " + e.Request.ToString() );
        }

        public IQueryable<IPackage> GetInstalledPackages()
        {
            return GetPackages(this.LocalRepository, false);
        }

        private static IEnumerable<IPackage> GetPackageDependencies(IPackage package, IPackageRepository localRepository, IPackageRepository sourceRepository)
        {
            IPackageRepository repository = localRepository;
            IPackageRepository repository2 = sourceRepository;
            ILogger instance = NullLogger.Instance;
            bool ignoreDependencies = false;
            var  walker = new InstallWalker(repository, repository2, new FrameworkName(".NETFramework", new Version("4.0")),  instance, ignoreDependencies,true);
            return walker.ResolveOperations(package).Where<PackageOperation>(delegate(PackageOperation operation)
            {
                return (operation.Action == PackageAction.Install);
            }).Select<PackageOperation, IPackage>(delegate(PackageOperation operation)
            {
                return operation.Package;
            });
        }

        internal static IQueryable<IPackage> GetPackages(IQueryable<IPackage> packages, bool filterTags)
        {
            return filterTags ? packages.Where(x => x.Tags.Contains("Appleseed") && x.IsAbsoluteLatestVersion) : packages.Where(x => x.IsAbsoluteLatestVersion);
        }

        internal static IQueryable<IPackage> GetPackages(IPackageRepository repository, bool filterTags)
        {
            var packages = repository.GetPackages();
            return GetPackages(packages, filterTags);
        }

        internal IEnumerable<IPackage> GetPackagesRequiringLicenseAcceptance(IPackage package)
        {
            IPackageRepository localRepository = this.LocalRepository;
            IPackageRepository sourceRepository = this.SourceRepository;
            return GetPackagesRequiringLicenseAcceptance(package, localRepository, sourceRepository);
        }

        internal static IEnumerable<IPackage> GetPackagesRequiringLicenseAcceptance(IPackage package, IPackageRepository localRepository, IPackageRepository sourceRepository)
        {
            return GetPackageDependencies(package, localRepository, sourceRepository).Where<IPackage>(delegate(IPackage p)
            {
                return p.RequireLicenseAcceptance;
            });
        }

        public IQueryable<IPackage> GetPackagesWithUpdates()
        {
            return GetPackages(PackageRepositoryExtensions.GetUpdates(this.LocalRepository, this.SourceRepository.GetPackages(), true,true).AsQueryable<IPackage>(),false);
        }

        public IQueryable<IPackage> GetRemotePackages()
        {
            return GetPackages(this.SourceRepository, true);
        }

        public IPackage GetUpdate(IPackage package)
        {
            var packages = PackageRepositoryExtensions.GetUpdates(this.SourceRepository,
                                                                  this.LocalRepository.GetPackages(), true, false);
            return packages.FirstOrDefault(x => x.Id == package.Id);
        }

        internal static string GetWebRepositoryDirectory(string siteRoot)
        {
            return Path.Combine(siteRoot, "packages");
        }

        public void InstallPackage(String id, SemanticVersion version)
        {
            
            _projectManager.AddPackageReference(id, version, false, true);
            
        }

        public bool IsPackageInstalled(IPackage package)
        {
            return this.LocalRepository.Exists(package);
        }

    
        public void UninstallPackage(IPackage package, bool removeDependencies)
        {
                bool forceRemove = false;
                bool flag1 = removeDependencies;
                this._projectManager.RemovePackageReference(package.Id, forceRemove, flag1);
           
        }

        public void UpdatePackage(IPackage package)
        {
            bool updateDependencies = true;
            this._projectManager.UpdatePackageReference(package.Id, package.Version, updateDependencies,true);
            
        }

        public IPackageRepository LocalRepository
        {
            get
            {
                return this._projectManager.LocalRepository;
            }
        }

        public IPackageRepository SourceRepository
        {
            get
            {
                return this._projectManager.SourceRepository;
            }
        }

        public void addLog(string msg) {

            _projectManager.Logger.Log(MessageLevel.Info, msg, null);
        }

        public string getLogs() {

            return ((LoggerController)_projectManager.Logger).getLogs();
        
        }



    }
}