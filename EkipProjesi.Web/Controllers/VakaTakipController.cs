using EkipProjesi.Core;
using EkipProjesi.Core.Formlar;
using EkipProjesi.Core.Hastalar;
using EkipProjesi.Core.Randevu;
using EkipProjesi.Data;
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
    [Authorize(Roles = "12")]
    public class VakaTakipController : Controller
    {
        #region Const
        private VakaTakipRepository _vakaRepo;
        private AnketRepository _anketRepo;
        private ArindirmaMerkezleriRepository _arindirmaRepo;
        private readonly EKIPEntities _db;
        public VakaTakipController()
        {
            _vakaRepo = new VakaTakipRepository();
            _anketRepo = new AnketRepository();
            _arindirmaRepo = new ArindirmaMerkezleriRepository();
            _db = new EKIPEntities();
        }
        #endregion
        public ActionResult Index()
        {
            ViewBag.KendiHastalari = _vakaRepo.KendiHastalari((int)Session["UserID"]);
            ViewBag.KurumdakiHastalar = _vakaRepo.KurumdakiHastalar((int)Session["UserID"]);
            ViewBag.TumHastalar = _vakaRepo.HastaBilgileriGetir();
            return View();
        }
        public ActionResult VakaDetay(int id)
        {
            IHastalar h = new IHastalar();

            var KendiHastalari = _vakaRepo.KendiHastalari((int)Session["UserID"]);
            var KurumdakiHastalar = _vakaRepo.KurumdakiHastalar((int)Session["UserID"]);
            var TumHastalar = _vakaRepo.HastaBilgileriGetir();

            if (User.IsInRole("90") && KendiHastalari.Where(x => x.HastaID == id).Any())
            {
                h = _vakaRepo.VakaDetay(id);
                ViewBag.Formlarim = _anketRepo.KullaniciAktifAnketler(id);
                ViewBag.YakinBilgileri = _vakaRepo.HastaYakinBilgileriGetir(id);
                ViewBag.IlkKayitBilgileri = _vakaRepo.HastaIlkKayitBilgileriGetir(id);
                ViewBag.IzlemBilgileri = _vakaRepo.HastaIzlemBilgileriGetir(id);
                ViewBag.TimelineBilgileri = _vakaRepo.HastaTimelineBilgileriGetir(id);
                ViewBag.RandevuBilgileri = _vakaRepo.HastaRandevuBilgileriGetir(id);
                ViewBag.HastaNotlari = _vakaRepo.HastaNotBilgileriGetir(id);
                return View(h);
            }
            else if (User.IsInRole("91") && KurumdakiHastalar.Where(x => x.HastaID == id).Any())
            {
                h = _vakaRepo.VakaDetay(id);
                ViewBag.Formlarim = _anketRepo.KullaniciAktifAnketler(id);
                ViewBag.YakinBilgileri = _vakaRepo.HastaYakinBilgileriGetir(id);
                ViewBag.IlkKayitBilgileri = _vakaRepo.HastaIlkKayitBilgileriGetir(id);
                ViewBag.IzlemBilgileri = _vakaRepo.HastaIzlemBilgileriGetir(id);
                ViewBag.TimelineBilgileri = _vakaRepo.HastaTimelineBilgileriGetir(id);
                ViewBag.RandevuBilgileri = _vakaRepo.HastaRandevuBilgileriGetir(id);
                ViewBag.HastaNotlari = _vakaRepo.HastaNotBilgileriGetir(id);
                return View(h);
            }
            else if (User.IsInRole("92") && TumHastalar.Where(x => x.HastaID == id).Any())
            {
                h = _vakaRepo.VakaDetay(id);
                ViewBag.Formlarim = _anketRepo.KullaniciAktifAnketler(id);
                ViewBag.YakinBilgileri = _vakaRepo.HastaYakinBilgileriGetir(id);
                ViewBag.IlkKayitBilgileri = _vakaRepo.HastaIlkKayitBilgileriGetir(id);
                ViewBag.IzlemBilgileri = _vakaRepo.HastaIzlemBilgileriGetir(id);
                ViewBag.TimelineBilgileri = _vakaRepo.HastaTimelineBilgileriGetir(id);
                ViewBag.RandevuBilgileri = _vakaRepo.HastaRandevuBilgileriGetir(id);
                ViewBag.HastaNotlari = _vakaRepo.HastaNotBilgileriGetir(id);
                return View(h);
            }
            return RedirectToAction("Error404","Home");
        }
        public ActionResult VakaIlkKayitDetay(int? id)
        {
            return View();
        }
        public ActionResult VakaIzlemBilgileriDetay(int? id)
        {
            return View();
        }
        public ActionResult HastaEkle()
        {
            int UserCode = _vakaRepo.KullanicininKurumu((int)Session["UserID"]).KurumKodu;
            ViewBag.Kurum = _arindirmaRepo.KurumBilgileriGetir().Where(x=>x.KurumKodu == UserCode);
            //kurum.Insert(0, new IKurumlar() { KurumID = 0, KurumAdi = "-Seçiniz-" });
            //ViewBag.Kurumlar = new SelectList(kurum, "KurumID", "KurumAdi", "-Seçiniz-");

            return View();
        }
        [HttpPost]
        public JsonResult HastaEkle(IHastalar model)
        {
            if (model != null)
            {
                if (_vakaRepo.HastaEkle(model, (int)Session["UserID"]))
                {
                    return Json(new { id = model.HastaID, success = true, mesaj = "Hasta Kaydı Başarılı Bir Şekilde Oluşturuldu"});
                }
                else
                {
                    return Json(new { id = 0, success = false, mesaj = "İşlemleriniz Sırasında Hata Oluştu." });
                }
            }
            else
            {
                return Json(new { success = false, mesaj = "Zorunlu Alanlar Boş Geçilemez. Lütfen Zorunlu Alanları Doldurunuz!" });
            }
        }

        [HttpPost]
        public JsonResult HastaYakinBilgileriEkle(IHastaYakinBilgileri model)
        {
            if (model != null)
            {
                if (_vakaRepo.HastaYakinBilgileriEkle(model))
                {
                    return Json(new { success = true, mesaj = "Yakın Bilgileri Kaydı Başarılı Bir Şekilde Oluşturuldu" });
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
        public JsonResult HastaIlkKayitBilgisiEkle(IHastaIlkKayitBilgileri model)
        {
            if (model != null)
            {
                if (_vakaRepo.HastaIlkKayitBilgisiEkle(model))
                {
                    return Json(new { success = true, mesaj = "Ilk Kayıt Bilgileri Kaydı Başarılı Bir Şekilde Oluşturuldu." });
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
        public JsonResult HastaIzlemBilgisiEkle(IHastaIzlemBilgileri model)
        {
            if (model != null)
            {
                if (_vakaRepo.HastaIzlemBilgisiEkle(model))
                {
                    return Json(new { success = true, mesaj = "İzlem Bilgileri Kaydı Başarılı Bir Şekilde Oluşturuldu." });
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
        public JsonResult HastaTimelineBilgisiEkle(IHastaTimeline model)
        {
            if (model != null)
            {
                if (_vakaRepo.HastaTimelineBilgisiEkle(model))
                {
                    return Json(new { success = true, mesaj = "Timeline Bilgileri Kaydı Başarılı Bir Şekilde Oluşturuldu." });
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
        public JsonResult HastaNotBilgisiEkle(IHastaNotlari model)
        {
            int kaydedenKullaniciID = (int)Session["UserID"];
            if (model != null)
            {
                if (_vakaRepo.HastaNotBilgisiEkle(model, kaydedenKullaniciID))
                {
                    return Json(new { success = true, mesaj = "Yakın Bilgileri Kaydı Başarılı Bir Şekilde Oluşturuldu" });
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
        public ActionResult FormDetay(int id, int HastaID)
        {
            AnketDTO model = _anketRepo.KullaniciAnketDetayi(id, HastaID);
            if (model == null || model.ID == 0) return RedirectToAction("VakaDetay", "VakaTakip");
            ViewBag.HastaID = HastaID;
            //IHastalar h = new IHastalar();
            //h = _vakaRepo.VakaDetay(hastaid);
            //ViewBag.Hastalar = _vakaRepo.HastaBilgiGetir(hastaid);
            return View(model);
        }
        [HttpPost]
        public JsonResult AnketCevapla(List<AnketKullaniciCevapDTO> model, int HastaID)
        {
            model.ForEach(x => x.UserID = HastaID);
            if (_anketRepo.CevapKayit(model))
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }
        [HttpPost]
        public JsonResult AnketSonlandir(long id, int HastaID)
        {
            if (_anketRepo.KullaniciAnketBitir(HastaID, id))
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }
    }
}