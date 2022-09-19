using EkipProjesi.Core;
using EkipProjesi.Core.Hastalar;
using EkipProjesi.Core.IstihdamModulu;
using EkipProjesi.Data.Repositories;
using EkipProjesi.Web.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EkipProjesi.Web.Controllers
{
    [SessionAttritube]
    [Authorize(Roles = "6")]
    public class IstihdamController : Controller
    {
        #region Const
        private IstihdamRepository _istihdamRepo;
        private KullaniciRepository _kullaniciRepo;
        ArindirmaMerkezleriRepository _arindirmaRepo;
        private VakaTakipRepository _vakaRepo;
        public IstihdamController()
        {
            _istihdamRepo = new IstihdamRepository();
            _kullaniciRepo = new KullaniciRepository();
            _arindirmaRepo = new ArindirmaMerkezleriRepository();
            _vakaRepo = new VakaTakipRepository();
        }
        #endregion
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult IstihdamEdilebilirlik()
        {
            return View();
        }
        public ActionResult IstihdamIsyerleri()
        {
            return View(_istihdamRepo.IstihdamIsyerleri());
        }
        public ActionResult IstihdamIsyeriEkle()
        {
            var il = _kullaniciRepo.Iller();
            il.Insert(0, new Core.IIl() { IlID = 0, IlAdi = "-Seçiniz-" });
            ViewBag.Iller = new SelectList(il, "IlID", "IlAdi", "-Seçiniz-");
            ViewBag.Sektorler = _istihdamRepo.IstihdamIsyeriSektorler();
            return View();
        }
        [HttpPost]
        public JsonResult IstihdamIsyeriEkle(IIstihdamIsyerleri model, int ilid)
        {
            if (model != null)
            {
                model.IsyeriAdresBilgisi.IlID = ilid;

                if (_istihdamRepo.IstihdamIsyeriEkle(model, (int)Session["UserID"]))
                {
                    return Json(new { success = true, mesaj = "İşyeri Kaydı Başarılı Bir Şekilde Oluşturuldu" });
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
        public ActionResult IstihdamIsyeriDetay(int id)
        {
            IIstihdamIsyerleri i = new IIstihdamIsyerleri();
            i = _istihdamRepo.IstihdamIsyeriDetay(id);
            ViewBag.IletisimKisileri = _istihdamRepo.IsyeriIletisimKisileri(id);
            ViewBag.Gorusmeler = _istihdamRepo.IsyeriGorusmeleri(id);
            ViewBag.AdresBilgisi = _istihdamRepo.IstihdamIsyeriAdresBilgisiDetay(id);
            ViewBag.Sektorler = _istihdamRepo.IstihdamIsyeriSektorler();
            return View(i);
        }
        public ActionResult IstihdamIsyeriGuncelle(int id)
        {
            IIstihdamIsyerleri model = _istihdamRepo.IstihdamIsyeriDetay(id);
            model.IsyeriAdresBilgisi = _istihdamRepo.IstihdamIsyeriAdresBilgisiDetay(id);
            ViewBag.Sektorler = _istihdamRepo.IstihdamIsyeriSektorler();

            var il = _kullaniciRepo.Iller();
            il.Insert(0, new Core.IIl() { IlID = 0, IlAdi = "-Seçiniz-" });
            ViewBag.Iller = new SelectList(il, "IlID", "IlAdi", "-Seçiniz-");

            return View(model);
        }
        [HttpPost]
        public JsonResult IstihdamIsyeriGuncelle(IIstihdamIsyerleri model, int ilid)
        {
            string hata = "";
            if (model != null && model.ID > 0)
            {
                model.IsyeriAdresBilgisi.IlID = ilid;

                if (_istihdamRepo.IstihdamIsyeriDuzenle(model, out hata))
                {
                    return Json(new { success = true, mesaj = "İşyeri Bilgileri Güncellendi." });
                }
            }
            else
            {
                return Json(new { success = false, mesaj = "İşlemleriniz Sırasında Hata Oluştu. Zorunlu Alanlar Boş Geçilemez ve ID 'null' Olamaz.", hata = hata, });
            }
            return Json(new { success = false, mesaj = "İşlemleriniz Sırasında Hata Oluştu. ", hata = hata });
        }
        [HttpPost]
        public JsonResult IletisimKisiEkle(IIstihdamIsyeriIletisimKisileri model)
        {
            if (model != null)
            {
                if (_istihdamRepo.IsyeriIletisimKisiEkle(model, (int)Session["UserID"]))
                {
                    return Json(new { success = true, mesaj = "İletişim Bilgileri Kaydı Başarılı Bir Şekilde Oluşturuldu." });
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
        [HttpPost]
        public JsonResult IsyeriIletisimKisiSil(int id)
        {
            var status = false;
            if (id > 0)
            {
                if (_istihdamRepo.IsyeriIletisimKisiSil(id))
                {
                    status = true;
                    return Json(status);
                }                
            }
            return Json(status);
        }
        [HttpPost]
        public JsonResult IsyeriGorusmeEkle(IIstihdamIsyeriGorusmeleri model)
        {
            if (model != null)
            {
                if (_istihdamRepo.IsyeriGorusmeEkle(model, (int)Session["UserID"]))
                {
                    return Json(new { success = true, mesaj = "Görüşme Bilgileri Kaydı Başarılı Bir Şekilde Oluşturuldu." });
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
        [HttpPost]
        public JsonResult GorusmeDetayGetir(int id)
        {
            var data = _istihdamRepo.GorusmeDetay(id);
            return Json(data);
        }
        [HttpPost]
        public JsonResult GorusmeGuncelle(IIstihdamIsyeriGorusmeleri model)
        {
            var data = _istihdamRepo.GorusmeGuncelle(model);
            return Json(data);
        }
        [HttpPost]
        public JsonResult GorusmeSil(int id)
        {
            var status = false;
            if (id > 0)
            {
                if (_istihdamRepo.GorusmeSil(id))
                {
                    status = true;
                    return Json(status);
                }
            }
            return Json(status);
        }
        public ActionResult IskurIslemleri()
        {
            ViewBag.AktifRandevuTalepleriKendi = _istihdamRepo.AktifRandevuTalepleri().Where(x=>x.TalepOlusturanKullaniciID == (int)Session["UserID"]).ToList();
            ViewBag.AktifRandevuTalepleri = _istihdamRepo.AktifRandevuTalepleri();

            ViewBag.VerilenRandevularKendi = _istihdamRepo.VerilenRandevular().Where(x => x.TalepOlusturanKullaniciID == (int)Session["UserID"]).ToList();           
            ViewBag.VerilenRandevular = _istihdamRepo.VerilenRandevular();

            ViewBag.KendiHastalari = _vakaRepo.KendiHastalari((int)Session["UserID"]);
            ViewBag.KurumdakiHastalar = _vakaRepo.KurumdakiHastalar((int)Session["UserID"]);
            ViewBag.TumHastalar = _vakaRepo.HastaBilgileriGetir();
            return View();
        }
        [HttpPost]
        public JsonResult TalepDetayGetir(int talepId)
        {
            var data = _istihdamRepo.TalepDetay(talepId);
            return Json(data);
        }
        [HttpPost]
        public JsonResult RandevuEkle(int ID, DateTime RandevuTarihi)
        {
            int userId = (int)Session["UserID"];
            var data = _istihdamRepo.RandevuEkle(ID, RandevuTarihi, userId);
            return Json(data);
        }
        [HttpPost]
        public JsonResult RandevuDurumGuncelle(int ID, string RandevuDurumu)
        {
            var data = _istihdamRepo.RandevuDurumGuncelle(ID, RandevuDurumu);
            return Json(data);
        }
        public ActionResult KayitEkle()
        {
            int UserCode = _vakaRepo.KullanicininKurumu((int)Session["UserID"]).KurumKodu;
            ViewBag.Kurum = _arindirmaRepo.KurumBilgileriGetir().Where(x => x.KurumKodu == UserCode);
            return View();
        }
        [HttpPost]
        public ActionResult KayitEkle(IHastalar model)
        {
            if (_istihdamRepo.KayitEkle(model, (int)Session["UserID"]))
            {
                return RedirectToAction("IskurIslemleri","Istihdam");
            }
            else
            {
                return View();
            }
        }
        public ActionResult KayitDetay(int id)
        {
            var KendiHastalari = _vakaRepo.KendiHastalari((int)Session["UserID"]);
            var KurumdakiHastalar = _vakaRepo.KurumdakiHastalar((int)Session["UserID"]);
            var TumHastalar = _vakaRepo.HastaBilgileriGetir();

            if (User.IsInRole("90") && KendiHastalari.Where(x => x.HastaID == id).Any())
            {
                ViewBag.KayitID = id;
                ViewBag.Gorusmeler = _istihdamRepo.IskurIslemleriKullaniciGorusmeleri(id);
                var data = _arindirmaRepo.HastaBilgiGetir(id);
                data.EgitimBilgileri = _istihdamRepo.HastaEgitimBilgileri(id);
                return View(data);
            }
            else if (User.IsInRole("91") && KurumdakiHastalar.Where(x => x.HastaID == id).Any())
            {
                ViewBag.KayitID = id;
                ViewBag.Gorusmeler = _istihdamRepo.IskurIslemleriKullaniciGorusmeleri(id);
                var data = _arindirmaRepo.HastaBilgiGetir(id);
                data.EgitimBilgileri = _istihdamRepo.HastaEgitimBilgileri(id);
                return View(data);
            }
            else if (User.IsInRole("92") && TumHastalar.Where(x => x.HastaID == id).Any())
            {
                ViewBag.KayitID = id;
                ViewBag.Gorusmeler = _istihdamRepo.IskurIslemleriKullaniciGorusmeleri(id);
                var data = _arindirmaRepo.HastaBilgiGetir(id);
                data.EgitimBilgileri = _istihdamRepo.HastaEgitimBilgileri(id);
                return View(data);
            }
            return RedirectToAction("Error404", "Home");
        }
        public ActionResult GorusmeEkle(int? KayitID)
        {
            var KendiHastalari = _vakaRepo.KendiHastalari((int)Session["UserID"]);
            var KurumdakiHastalar = _vakaRepo.KurumdakiHastalar((int)Session["UserID"]);
            var TumHastalar = _vakaRepo.HastaBilgileriGetir();

            if (User.IsInRole("90") && KendiHastalari.Where(x => x.HastaID == KayitID).Any())
            {
                ViewBag.KayitID = KayitID;
                return View();
            }
            else if (User.IsInRole("91") && KurumdakiHastalar.Where(x => x.HastaID == KayitID).Any())
            {
                ViewBag.KayitID = KayitID;
                return View();
            }
            else if (User.IsInRole("92") && TumHastalar.Where(x => x.HastaID == KayitID).Any())
            {
                ViewBag.KayitID = KayitID;
                return View();
            }
            return RedirectToAction("Error404", "Home");
        }
        [HttpPost]
        public JsonResult KullaniciGorusmeEkle(IHastaIskurGorusmeleri model, int? KayitID)
        {
            var data = _istihdamRepo.IskurIslemleriKullaniciGorusmeEkle(model, (int)Session["UserID"], KayitID);
            return Json(data);
        }
        public ActionResult GorusmeDetay(int id)
        {
            var model = _istihdamRepo.IskurIslemleriKullaniciGorusmeDetay(id);
            return View(model);
        }
    }
}