using EkipProjesi.Core.Kullanici;
using EkipProjesi.Data;
using EkipProjesi.Data.Repositories;
using EkipProjesi.Web.Filter;
using EkipProjesi.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EkipProjesi.Core.Kullanici;

namespace EkipProjesi.Web.Controllers
{
    [SessionAttritube]
    public class HomeController : Controller
    {
        private KullaniciRepository _kullaniciRepo;
        private readonly EKIPEntities _db;

        public HomeController()
        {
            _kullaniciRepo = new KullaniciRepository();
            _db = new EKIPEntities();
        }
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult TopMenu()
        {
            return PartialView();
        }
        public PartialViewResult LeftMenu()
        {
            return PartialView();
        }
        public ActionResult Error404()
        {
            return View();
        }
        public ActionResult Error401()
        {
            return View();
        }
        public ActionResult Profilim()
        {
            int id = (int)Session["UserID"];
            IKullanici model = _kullaniciRepo.KullaniciDetay(id);
            ViewBag.IletisimBilgileri = _kullaniciRepo.KullaniciIletisimBilgisiDetay(id);
            //ViewBag.BirimBilgileri = _kullaniciRepo.KullaniciBirimBilgisiDetay(id);
            return View(model);
        }
        public ActionResult Gorevlerim()
        {
            int id = (int)Session["UserID"];
            ViewBag.KullaniciID = (int)Session["UserID"];
            ViewBag.Yapilacaklar = _kullaniciRepo.PersonelGorevGetir(id).Where(x=>x.Durum == "1").ToList();
            ViewBag.DevamEdenler = _kullaniciRepo.PersonelGorevGetir(id).Where(x=>x.Durum == "2").ToList();
            ViewBag.Tamamlananlar = _kullaniciRepo.PersonelGorevGetir(id).Where(x=>x.Durum == "3").ToList();
            return View();
        }
        [HttpPost]
        public JsonResult KullaniciGorevEkle(IKullaniciGorevTakip model)
        {
            if (model != null)
            {
                if (_kullaniciRepo.PersonelGorevEkle(model))
                {
                    return Json(new { success = true, mesaj = "Görev Bilgileri Kaydı Başarılı Bir Şekilde Oluşturuldu." });
                }
                else
                {
                    return Json(new { success = false, mesaj = "İşlemleriniz Sırasında Hata Oluştu." });
                }
            }
            else
            {
                return Json(new { success = false, mesaj = "Zorunlu Alanlar Boş Geçilemez. Lütfen Zorunlu Alanları Doldurunuz!" });
            }
        }

        #region Kontroller
        [HttpPost]
        public JsonResult RandevuAyariPasiflikKontrol(int ayarId)
        {
            if (_kullaniciRepo.RandevuAyariPasiflikKontrol(ayarId))
            {
                return Json(new { success = true, mesaj = "Randevu Ayarları Başarılı Bir Şekilde Kontrol Edildi." });
            }
            else
            {
                return Json(new { success = false, mesaj = "İşlemleriniz Sırasında Hata Oluştu." });
            }
        }
        [HttpPost]
        public JsonResult PasifRandevuAyarlari()
        {
            var data = _kullaniciRepo.PasifRandevuAyarlari();
            return Json(data);
        }
        [HttpPost]
        public JsonResult RandevuBilgileri()
        {
            var data = _kullaniciRepo.RandevuBilgileriGetir();
            return Json(data);
        }
        [HttpPost]
        public JsonResult RandevuDurumBilgisiKontrol(int randevuId)
        {
            if (_kullaniciRepo.RandevuDurumBilgisiKontrol(randevuId))
            {
                return Json(new { success = true, mesaj = "Randevu Durumları Başarılı Bir Şekilde Kontrol Edildi." });
            }
            else
            {
                return Json(new { success = false, mesaj = "İşlemleriniz Sırasında Hata Oluştu." });
            }
        }
        #endregion       
    }
}