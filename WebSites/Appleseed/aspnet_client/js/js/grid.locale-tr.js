;(function($){
/**
 * jqGrid Turkish Translation
 * H.İbrahim Yılmaz ibrahim.yilmaz@karmabilisim.net
 * http://www.arkeoloji.web.tr
 * Dual licensed under the MIT and GPL licenses:
 * http://www.opensource.org/licenses/mit-license.php
 * http://www.gnu.org/licenses/gpl.html
**/
$.jgrid = {};

$.jgrid.defaults = {
	recordtext: "Satır(lar)",
	loadtext: "Yükleniyor...",
	pgtext : "/"
};
$.jgrid.search = {
    caption: "Arama...",
    Find: "Bul",
    Reset: "Temizle",
    odata : ['eşittir', 'eşit değildir', 'küçük', 'küçük veya eşit','büyük','büyük veya eşit', 'ile başlayan','ile biten','içeren' ]
};
$.jgrid.edit = {
    addCaption: "Kayıt Ekle",
    editCaption: "Kayıt Düzenle",
    bSubmit: "Gönder",
    bCancel: "İptal",
	bClose: "Kapat",
    processData: "İşlem yapılıyor...",
    msg: {
        required:"Alan gerekli",
        number:"Lütfen bir numara giriniz",
        minValue:"girilen değer daha büyük ya da buna eşit olmalıdır",
        maxValue:"girilen değer daha küçük ya da buna eşit olmalıdır",
        email: "geçerli bir e-posta adresi değildir",
        integer: "Lütfen bir tamsayı giriniz",
		date: "Please, enter valid date value"
    }
};
$.jgrid.del = {
    caption: "Sil",
    msg: "Seçilen kayıtlar silinsin mi?",
    bSubmit: "Sil",
    bCancel: "İptal",
    processData: "İşlem yapılıyor..."
};
$.jgrid.nav = {
	edittext: " ",
    edittitle: "Seçili satırı düzenle",
	addtext:" ",
    addtitle: "Yeni satır ekle",
    deltext: " ",
    deltitle: "Seçili satırı sil",
    searchtext: " ",
    searchtitle: "Kayıtları bul",
    refreshtext: "",
    refreshtitle: "Tabloyu yenile",
    alertcap: "Uyarı",
    alerttext: "Lütfen bir satır seçiniz"
};
// setcolumns module
$.jgrid.col ={
    caption: "Sütunları göster/gizle",
    bSubmit: "Gönder",
    bCancel: "İptal"	
};
$.jgrid.errors = {
	errcap : "Hata",
	nourl : "Bir url yapılandırılmamış",
	norecords: "İşlem yapılacak bir kayıt yok"
};
})(jQuery);
