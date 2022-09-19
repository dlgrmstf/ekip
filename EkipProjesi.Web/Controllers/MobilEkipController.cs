using EkipProjesi.Core;
using EkipProjesi.Core.Hastalar;
using EkipProjesi.Core.MobilEkip;
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
    [Authorize(Roles = "9")]
    public class MobilEkipController : Controller
    {
        #region Const
        KurumlarArasiSevkVeTakipRepository _kastRepo;
        MobilEkipRepository _mobilRepo;
        VakaTakipRepository _vakaRepo;
        private readonly EKIPEntities _db;

        public MobilEkipController()
        {
            _kastRepo = new KurumlarArasiSevkVeTakipRepository();
            _mobilRepo = new MobilEkipRepository();
            _vakaRepo = new VakaTakipRepository();
            _db = new EKIPEntities();
        }
        #endregion
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MobilEkip()
        {
            return View();
        }
        public ActionResult Randevular()
        {
            ViewBag.AktifRandevuTalepleriKendi = _mobilRepo.AktifRandevuTalepleri().Where(x => x.TalepOlusturanKullaniciID == (int)Session["UserID"]).ToList();
            ViewBag.AktifRandevuTalepleri = _mobilRepo.AktifRandevuTalepleri();
            ViewBag.VerilenRandevularKendi = _mobilRepo.VerilenRandevular().Where(x => x.TalepOlusturanKullaniciID == (int)Session["UserID"]).ToList();
            ViewBag.VerilenRandevular = _mobilRepo.VerilenRandevular();
            return View();
        }
        [HttpPost]
        public JsonResult TalepDetayGetir(int talepId)
        {
            var data = _mobilRepo.TalepDetay(talepId);
            return Json(data);
        }
        [HttpPost]
        public JsonResult RandevuEkle(int ID, DateTime RandevuTarihi)
        {
            int userId = (int)Session["UserID"];
            var data = _mobilRepo.RandevuEkle(ID, RandevuTarihi, userId);
            return Json(data);
        }
        [HttpPost]
        public JsonResult RandevuDurumGuncelle(int ID, string RandevuDurumu)
        {
            var data = _mobilRepo.RandevuDurumGuncelle(ID, RandevuDurumu);
            return Json(data);
        }
        public ActionResult PartialFiltreSonucHasta(IHastaAramaModel model)
        {
            IHastaAramaModel data = new IHastaAramaModel();

            if (model != null)
            {
                if (User.IsInRole("90"))
                {
                    if (_vakaRepo.KendiHastasiMi((int)Session["UserID"], model.HastaEkipNo, model.HastaTC).Count() > 0)
                    {
                        int hastaId = _db.Hastalar.Where(x => x.HastaEkipNo == model.HastaEkipNo || x.HastaTCKimlikNo == model.HastaTC).Select(x => x.HastaID).FirstOrDefault();
                        ViewBag.Hastalar = _kastRepo.HastaBilgiGetir(hastaId);
                    }
                }
                if (User.IsInRole("91"))
                {
                    if (_vakaRepo.KurumdakiHastasiMi((int)Session["UserID"], model.HastaEkipNo, model.HastaTC).Count() > 0)
                    {
                        int hastaId = _db.Hastalar.Where(x => x.HastaEkipNo == model.HastaEkipNo || x.HastaTCKimlikNo == model.HastaTC).Select(x => x.HastaID).FirstOrDefault();
                        ViewBag.Hastalar = _kastRepo.HastaBilgiGetir(hastaId);
                    }
                }
                if (User.IsInRole("92"))
                    data.Hastalar = _kastRepo.HastaBilgileriGetir(model);
            }
            //data.Hastalar = _kastRepo.HastaBilgileriGetir(model);

            return PartialView(data);
        }
        public ActionResult VakaDetay(int HastaID)
        {
            IHastalar h = new IHastalar();
            var KendiHastalari = _vakaRepo.KendiHastalari((int)Session["UserID"]);
            var KurumdakiHastalar = _vakaRepo.KurumdakiHastalar((int)Session["UserID"]);
            var TumHastalar = _vakaRepo.HastaBilgileriGetir();

            if (User.IsInRole("90") && KendiHastalari.Where(x => x.HastaID == HastaID).Any())
            {
                h = _mobilRepo.VakaDetay(HastaID);
                ViewBag.HastaID = HastaID;
                ViewBag.Formlar = _mobilRepo.Formlar(HastaID);
                return View(h);
            }
            else if (User.IsInRole("91") && KurumdakiHastalar.Where(x => x.HastaID == HastaID).Any())
            {
                h = _mobilRepo.VakaDetay(HastaID);
                ViewBag.HastaID = HastaID;
                ViewBag.Formlar = _mobilRepo.Formlar(HastaID);
                return View(h);
            }
            else if (User.IsInRole("92") && TumHastalar.Where(x => x.HastaID == HastaID).Any())
            {
                h = _mobilRepo.VakaDetay(HastaID);
                ViewBag.HastaID = HastaID;
                ViewBag.Formlar = _mobilRepo.Formlar(HastaID);
                return View(h);
            }
            return RedirectToAction("Error404", "Home");
        }
        public ActionResult FormEkle(int? HastaID)
        {
            var KendiHastalari = _vakaRepo.KendiHastalari((int)Session["UserID"]);
            var KurumdakiHastalar = _vakaRepo.KurumdakiHastalar((int)Session["UserID"]);
            var TumHastalar = _vakaRepo.HastaBilgileriGetir();

            if (User.IsInRole("90") && KendiHastalari.Where(x => x.HastaID == HastaID).Any())
            {
                ViewBag.HastaID = HastaID;
                return View();
            }
            else if (User.IsInRole("91") && KurumdakiHastalar.Where(x => x.HastaID == HastaID).Any())
            {
                ViewBag.HastaID = HastaID;
                return View();
            }
            else if (User.IsInRole("92") && TumHastalar.Where(x => x.HastaID == HastaID).Any())
            {
                ViewBag.HastaID = HastaID;
                return View();
            }
            return RedirectToAction("Error404", "Home");
        }
        [HttpPost]
        public JsonResult VakaFormEkle(IMobilEkipVakaFormlari model, int? HastaID)
        {
            var KendiHastalari = _vakaRepo.KendiHastalari((int)Session["UserID"]);
            var KurumdakiHastalar = _vakaRepo.KurumdakiHastalar((int)Session["UserID"]);
            var TumHastalar = _vakaRepo.HastaBilgileriGetir();

            if (User.IsInRole("90") && KendiHastalari.Where(x => x.HastaID == HastaID).Any())
            {
                var data = _mobilRepo.VakaFormEkle(model, HastaID, (int)Session["UserID"]);
                return Json(data);
            }
            else if (User.IsInRole("91") && KurumdakiHastalar.Where(x => x.HastaID == HastaID).Any())
            {
                var data = _mobilRepo.VakaFormEkle(model, HastaID, (int)Session["UserID"]);
                return Json(data);
            }
            else if (User.IsInRole("92") && TumHastalar.Where(x => x.HastaID == HastaID).Any())
            {
                var data = _mobilRepo.VakaFormEkle(model, HastaID, (int)Session["UserID"]);
                return Json(data);
            }
            return Json(false);
        }
        public ActionResult FormDetay(int id)
        {
            var KendiHastalari = _vakaRepo.KendiHastalari((int)Session["UserID"]);
            var KurumdakiHastalar = _vakaRepo.KurumdakiHastalar((int)Session["UserID"]);
            var TumHastalar = _vakaRepo.HastaBilgileriGetir();

            if (User.IsInRole("90") && KendiHastalari.Where(x => x.HastaID == _mobilRepo.FormDetay(id).HastaID).Any())
            {
                int kID = _mobilRepo.FormDetay(id).KaydedenKullaniciID;
                int hID = _mobilRepo.FormDetay(id).HastaID;
                ViewBag.HastaAdi = _db.Hastalar.Where(x => x.HastaID == hID).Select(x => x.HastaAdi).FirstOrDefault();
                ViewBag.HastaSoyadi = _db.Hastalar.Where(x => x.HastaID == hID).Select(x => x.HastaSoyadi).FirstOrDefault();
                ViewBag.HastaTC = _db.Hastalar.Where(x => x.HastaID == hID).Select(x => x.HastaTCKimlikNo).FirstOrDefault();
                ViewBag.KaydedenKisiAd = _db.Kullanicilar.Where(x => x.ID == kID).Select(x => x.Ad).FirstOrDefault();
                ViewBag.KaydedenKisiSoyad = _db.Kullanicilar.Where(x => x.ID == kID).Select(x => x.Soyad).FirstOrDefault();
                return View(_mobilRepo.FormDetay(id));
            }
            else if (User.IsInRole("91") && KurumdakiHastalar.Where(x => x.HastaID == _mobilRepo.FormDetay(id).HastaID).Any())
            {
                int kID = _mobilRepo.FormDetay(id).KaydedenKullaniciID;
                int hID = _mobilRepo.FormDetay(id).HastaID;
                ViewBag.HastaAdi = _db.Hastalar.Where(x => x.HastaID == hID).Select(x => x.HastaAdi).FirstOrDefault();
                ViewBag.HastaSoyadi = _db.Hastalar.Where(x => x.HastaID == hID).Select(x => x.HastaSoyadi).FirstOrDefault();
                ViewBag.HastaTC = _db.Hastalar.Where(x => x.HastaID == hID).Select(x => x.HastaTCKimlikNo).FirstOrDefault();
                ViewBag.KaydedenKisiAd = _db.Kullanicilar.Where(x => x.ID == kID).Select(x => x.Ad).FirstOrDefault();
                ViewBag.KaydedenKisiSoyad = _db.Kullanicilar.Where(x => x.ID == kID).Select(x => x.Soyad).FirstOrDefault();
                return View(_mobilRepo.FormDetay(id));
            }
            else if (User.IsInRole("92") && TumHastalar.Where(x => x.HastaID == _mobilRepo.FormDetay(id).HastaID).Any())
            {
                int kID = _mobilRepo.FormDetay(id).KaydedenKullaniciID;
                int hID = _mobilRepo.FormDetay(id).HastaID;
                ViewBag.HastaAdi = _db.Hastalar.Where(x => x.HastaID == hID).Select(x => x.HastaAdi).FirstOrDefault();
                ViewBag.HastaSoyadi = _db.Hastalar.Where(x => x.HastaID == hID).Select(x => x.HastaSoyadi).FirstOrDefault();
                ViewBag.HastaTC = _db.Hastalar.Where(x => x.HastaID == hID).Select(x => x.HastaTCKimlikNo).FirstOrDefault();
                ViewBag.KaydedenKisiAd = _db.Kullanicilar.Where(x => x.ID == kID).Select(x => x.Ad).FirstOrDefault();
                ViewBag.KaydedenKisiSoyad = _db.Kullanicilar.Where(x => x.ID == kID).Select(x => x.Soyad).FirstOrDefault();
                return View(_mobilRepo.FormDetay(id));
            }
            return RedirectToAction("Error404", "Home");
        }
        [HttpPost]
        public JsonResult VakaFormGuncelle(IMobilEkipVakaFormlari model)
        {
            var data = _mobilRepo.VakaFormGuncelle(model);
            return Json(data);
        }
    }
}