using EkipProjesi.Web.Models;
using EkipProjesi.Core;
using EkipProjesi.Core.Hastalar;
using EkipProjesi.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EkipProjesi.Data;
using Newtonsoft.Json;
using System.IO;
using EkipProjesi.Core.KurumlarArasiSevkVeTakip;
using EkipProjesi.Web.Filter;
using EkipProjesi.Core.Randevu;

namespace EkipProjesi.Web.Controllers
{
    [SessionAttritube]
    [Authorize(Roles = "8")]
    public class KurumlarArasiSevkVeTakipController : Controller
    {
        #region Const
        IskurHastaFiltreModel _iskurhastafiltreModel;
        KurumlarArasiSevkVeTakipRepository _kastRepo;
        VakaTakipRepository _vakaRepo;
        ArindirmaMerkezleriRepository _arindirmaRepo;
        private readonly EKIPEntities _db;

        public KurumlarArasiSevkVeTakipController()
        {
            _iskurhastafiltreModel = new IskurHastaFiltreModel();
            _kastRepo = new KurumlarArasiSevkVeTakipRepository();
            _vakaRepo = new VakaTakipRepository();
            _arindirmaRepo = new ArindirmaMerkezleriRepository();
            _db = new EKIPEntities();
        }
        #endregion
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult KoordinasyonTakipModulu()
        {
            ViewBag.IskurRandevular = _kastRepo.IskurRandevular();
            ViewBag.YesilayRandevular = _kastRepo.YesilayRandevular();
            ViewBag.SHMRandevular = _kastRepo.SHMRandevular();
            ViewBag.ISMRandevular = _kastRepo.ISMRandevular();
            ViewBag.BakirkoyRandevular = _kastRepo.BakirkoyRandevular();
            ViewBag.ErenkoyRandevular = _kastRepo.ErenkoyRandevular();
            ViewBag.OrdRandevular = _kastRepo.OrdRandevular();
            return View();
        }

        #region Mobil Ekip
        public ActionResult MobilEkipRandevuTalebiOlustur(int? hastaId)
        {
            var KendiHastalari = _vakaRepo.KendiHastalari((int)Session["UserID"]);
            var KurumdakiHastalar = _vakaRepo.KurumdakiHastalar((int)Session["UserID"]);
            var TumHastalar = _vakaRepo.HastaBilgileriGetir();

            if (User.IsInRole("90") && KendiHastalari.Where(x => x.HastaID == hastaId).Any())
            {
                var data = _kastRepo.HastaBilgiGetir(hastaId);
                ViewBag.ID = data.HastaID;
                ViewBag.Ad = data.HastaAdi;
                ViewBag.Soyad = data.HastaSoyadi;
                ViewBag.TC = data.HastaTCKimlikNo;

                var merkez = _kastRepo.HizmetMerkezleriGetir().Where(x => x.KurumID == 9).ToList();
                merkez.Insert(0, new IKurumHizmetMerkezleri() { ID = 0, Ad = "-Seçiniz-" });
                ViewBag.HizmetMerkezleri = new SelectList(merkez, "ID", "Ad", "-Seçiniz-");

                return View();
            }
            else if (User.IsInRole("91") && KurumdakiHastalar.Where(x => x.HastaID == hastaId).Any())
            {
                var data = _kastRepo.HastaBilgiGetir(hastaId);
                ViewBag.ID = data.HastaID;
                ViewBag.Ad = data.HastaAdi;
                ViewBag.Soyad = data.HastaSoyadi;
                ViewBag.TC = data.HastaTCKimlikNo;

                var merkez = _kastRepo.HizmetMerkezleriGetir().Where(x => x.KurumID == 9).ToList();
                merkez.Insert(0, new IKurumHizmetMerkezleri() { ID = 0, Ad = "-Seçiniz-" });
                ViewBag.HizmetMerkezleri = new SelectList(merkez, "ID", "Ad", "-Seçiniz-");

                return View();
            }
            else if (User.IsInRole("92") && TumHastalar.Where(x => x.HastaID == hastaId).Any())
            {
                var data = _kastRepo.HastaBilgiGetir(hastaId);
                ViewBag.ID = data.HastaID;
                ViewBag.Ad = data.HastaAdi;
                ViewBag.Soyad = data.HastaSoyadi;
                ViewBag.TC = data.HastaTCKimlikNo;

                var merkez = _kastRepo.HizmetMerkezleriGetir().Where(x => x.KurumID == 9).ToList();
                merkez.Insert(0, new IKurumHizmetMerkezleri() { ID = 0, Ad = "-Seçiniz-" });
                ViewBag.HizmetMerkezleri = new SelectList(merkez, "ID", "Ad", "-Seçiniz-");

                return View();
            }
            return RedirectToAction("Error404", "Home");
        }
        [HttpPost]
        public JsonResult MobilEkipRandevuTalebiOlustur(IRandevuTalepleri model)
        {
            int userId = (int)Session["UserID"];
            if (model != null)
            {
                if (_kastRepo.RandevuTalebiKaydet(model, userId))
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false });
                }
            }
            else
            {
                return Json(new { success = false });
            }
        }
        #endregion

        #region Danışma Merkezleri
        public ActionResult DanismaMerkezleri()
        {
            return View();
        }
        #region YEDAM
        public ActionResult YEDAM()
        {
            return View();
        }
        public ActionResult PartialFiltreSonucYEDAM(IHastaAramaModel model)
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
        public ActionResult YEDAMRandevuTalebiOlustur(int? hastaId)
        {
            var KendiHastalari = _vakaRepo.KendiHastalari((int)Session["UserID"]);
            var KurumdakiHastalar = _vakaRepo.KurumdakiHastalar((int)Session["UserID"]);
            var TumHastalar = _vakaRepo.HastaBilgileriGetir();

            if (User.IsInRole("90") && KendiHastalari.Where(x => x.HastaID == hastaId).Any())
            {
                var data = _kastRepo.HastaBilgiGetir(hastaId);
                ViewBag.ID = data.HastaID;
                ViewBag.Ad = data.HastaAdi;
                ViewBag.Soyad = data.HastaSoyadi;
                ViewBag.TC = data.HastaTCKimlikNo;

                var merkez = _kastRepo.HizmetMerkezleriGetir().Where(x => x.KurumID == 6).ToList();
                merkez.Insert(0, new IKurumHizmetMerkezleri() { ID = 0, Ad = "-Seçiniz-" });
                ViewBag.HizmetMerkezleri = new SelectList(merkez, "ID", "Ad", "-Seçiniz-");

                return View();
            }
            else if (User.IsInRole("91") && KurumdakiHastalar.Where(x => x.HastaID == hastaId).Any())
            {
                var data = _kastRepo.HastaBilgiGetir(hastaId);
                ViewBag.ID = data.HastaID;
                ViewBag.Ad = data.HastaAdi;
                ViewBag.Soyad = data.HastaSoyadi;
                ViewBag.TC = data.HastaTCKimlikNo;

                var merkez = _kastRepo.HizmetMerkezleriGetir().Where(x => x.KurumID == 6).ToList();
                merkez.Insert(0, new IKurumHizmetMerkezleri() { ID = 0, Ad = "-Seçiniz-" });
                ViewBag.HizmetMerkezleri = new SelectList(merkez, "ID", "Ad", "-Seçiniz-");

                return View();
            }
            else if (User.IsInRole("92") && TumHastalar.Where(x => x.HastaID == hastaId).Any())
            {
                var data = _kastRepo.HastaBilgiGetir(hastaId);
                ViewBag.ID = data.HastaID;
                ViewBag.Ad = data.HastaAdi;
                ViewBag.Soyad = data.HastaSoyadi;
                ViewBag.TC = data.HastaTCKimlikNo;

                var merkez = _kastRepo.HizmetMerkezleriGetir().Where(x => x.KurumID == 6).ToList();
                merkez.Insert(0, new IKurumHizmetMerkezleri() { ID = 0, Ad = "-Seçiniz-" });
                ViewBag.HizmetMerkezleri = new SelectList(merkez, "ID", "Ad", "-Seçiniz-");

                return View();
            }
            return RedirectToAction("Error404", "Home");           
        }
        [HttpPost]
        public JsonResult YEDAMRandevuTalebiOlustur(IRandevuTalepleri model)
        {
            int userId = (int)Session["UserID"];
            if (model != null)
            {
                if (_kastRepo.RandevuTalebiKaydet(model, userId))
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false });
                }
            }
            else
            {
                return Json(new { success = false });
            }
        }
        #endregion
        #region İSM
        public ActionResult IlceSaglikMudurlukleri()
        {
            return View();
        }
        public ActionResult PartialFiltreSonucISM(IHastaAramaModel model)
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
        public ActionResult ISMRandevuTalebiOlustur(int? hastaId)
        {
            var KendiHastalari = _vakaRepo.KendiHastalari((int)Session["UserID"]);
            var KurumdakiHastalar = _vakaRepo.KurumdakiHastalar((int)Session["UserID"]);
            var TumHastalar = _vakaRepo.HastaBilgileriGetir();

            if (User.IsInRole("90") && KendiHastalari.Where(x => x.HastaID == hastaId).Any())
            {
                var data = _kastRepo.HastaBilgiGetir(hastaId);
                ViewBag.ID = data.HastaID;
                ViewBag.Ad = data.HastaAdi;
                ViewBag.Soyad = data.HastaSoyadi;
                ViewBag.TC = data.HastaTCKimlikNo;

                var merkez = _kastRepo.HizmetMerkezleriGetir().Where(x => x.KurumID == 8).ToList();
                merkez.Insert(0, new IKurumHizmetMerkezleri() { ID = 0, Ad = "-Seçiniz-" });
                ViewBag.HizmetMerkezleri = new SelectList(merkez, "ID", "Ad", "-Seçiniz-");

                return View();
            }
            else if (User.IsInRole("91") && KurumdakiHastalar.Where(x => x.HastaID == hastaId).Any())
            {
                var data = _kastRepo.HastaBilgiGetir(hastaId);
                ViewBag.ID = data.HastaID;
                ViewBag.Ad = data.HastaAdi;
                ViewBag.Soyad = data.HastaSoyadi;
                ViewBag.TC = data.HastaTCKimlikNo;

                var merkez = _kastRepo.HizmetMerkezleriGetir().Where(x => x.KurumID == 8).ToList();
                merkez.Insert(0, new IKurumHizmetMerkezleri() { ID = 0, Ad = "-Seçiniz-" });
                ViewBag.HizmetMerkezleri = new SelectList(merkez, "ID", "Ad", "-Seçiniz-");

                return View();
            }
            else if (User.IsInRole("92") && TumHastalar.Where(x => x.HastaID == hastaId).Any())
            {
                var data = _kastRepo.HastaBilgiGetir(hastaId);
                ViewBag.ID = data.HastaID;
                ViewBag.Ad = data.HastaAdi;
                ViewBag.Soyad = data.HastaSoyadi;
                ViewBag.TC = data.HastaTCKimlikNo;

                var merkez = _kastRepo.HizmetMerkezleriGetir().Where(x => x.KurumID == 8).ToList();
                merkez.Insert(0, new IKurumHizmetMerkezleri() { ID = 0, Ad = "-Seçiniz-" });
                ViewBag.HizmetMerkezleri = new SelectList(merkez, "ID", "Ad", "-Seçiniz-");

                return View();
            }
            return RedirectToAction("Error404", "Home");
        }
        [HttpPost]
        public JsonResult ISMRandevuTalebiOlustur(IRandevuTalepleri model)
        {
            int userId = (int)Session["UserID"];
            if (model != null)
            {
                if (_kastRepo.RandevuTalebiKaydet(model, userId))
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false });
                }
            }
            else
            {
                return Json(new { success = false });
            }
        }
        #endregion
        #endregion

        #region Sosyal Hizmet Merkezleri
        public ActionResult SosyalHizmetBirimleri()
        {
            return View();
        }
        public ActionResult PartialFiltreSonucSHM(IHastaAramaModel model)
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
        public ActionResult SHMRandevuTalebiOlustur(int? hastaId)
        {
            var KendiHastalari = _vakaRepo.KendiHastalari((int)Session["UserID"]);
            var KurumdakiHastalar = _vakaRepo.KurumdakiHastalar((int)Session["UserID"]);
            var TumHastalar = _vakaRepo.HastaBilgileriGetir();

            if (User.IsInRole("90") && KendiHastalari.Where(x => x.HastaID == hastaId).Any())
            {
                var data = _kastRepo.HastaBilgiGetir(hastaId);
                ViewBag.ID = data.HastaID;
                ViewBag.Ad = data.HastaAdi;
                ViewBag.Soyad = data.HastaSoyadi;
                ViewBag.TC = data.HastaTCKimlikNo;

                var merkez = _kastRepo.HizmetMerkezleriGetir().Where(x => x.KurumID == 7).ToList();
                merkez.Insert(0, new IKurumHizmetMerkezleri() { ID = 0, Ad = "-Seçiniz-" });
                ViewBag.HizmetMerkezleri = new SelectList(merkez, "ID", "Ad", "-Seçiniz-");

                return View();
            }
            else if (User.IsInRole("91") && KurumdakiHastalar.Where(x => x.HastaID == hastaId).Any())
            {
                var data = _kastRepo.HastaBilgiGetir(hastaId);
                ViewBag.ID = data.HastaID;
                ViewBag.Ad = data.HastaAdi;
                ViewBag.Soyad = data.HastaSoyadi;
                ViewBag.TC = data.HastaTCKimlikNo;

                var merkez = _kastRepo.HizmetMerkezleriGetir().Where(x => x.KurumID == 7).ToList();
                merkez.Insert(0, new IKurumHizmetMerkezleri() { ID = 0, Ad = "-Seçiniz-" });
                ViewBag.HizmetMerkezleri = new SelectList(merkez, "ID", "Ad", "-Seçiniz-");

                return View();
            }
            else if (User.IsInRole("92") && TumHastalar.Where(x => x.HastaID == hastaId).Any())
            {
                var data = _kastRepo.HastaBilgiGetir(hastaId);
                ViewBag.ID = data.HastaID;
                ViewBag.Ad = data.HastaAdi;
                ViewBag.Soyad = data.HastaSoyadi;
                ViewBag.TC = data.HastaTCKimlikNo;

                var merkez = _kastRepo.HizmetMerkezleriGetir().Where(x => x.KurumID == 7).ToList();
                merkez.Insert(0, new IKurumHizmetMerkezleri() { ID = 0, Ad = "-Seçiniz-" });
                ViewBag.HizmetMerkezleri = new SelectList(merkez, "ID", "Ad", "-Seçiniz-");

                return View();
            }
            return RedirectToAction("Error404", "Home");
        }
        [HttpPost]
        public JsonResult SHMRandevuTalebiOlustur(IRandevuTalepleri model)
        {
            int userId = (int)Session["UserID"];
            if (model != null)
            {
                if (_kastRepo.RandevuTalebiKaydet(model, userId))
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false });
                }
            }
            else
            {
                return Json(new { success = false });
            }
        }
        #endregion        

        #region Iskur
        public ActionResult Iskur()
        {
            return View();
        }
        public ActionResult PartialFiltreSonucIskur(IHastaAramaModel model)
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
        public ActionResult IskurRandevuTalebiOlustur(int? hastaId)
        {
            var KendiHastalari = _vakaRepo.KendiHastalari((int)Session["UserID"]);
            var KurumdakiHastalar = _vakaRepo.KurumdakiHastalar((int)Session["UserID"]);
            var TumHastalar = _vakaRepo.HastaBilgileriGetir();

            if (User.IsInRole("90") && KendiHastalari.Where(x => x.HastaID == hastaId).Any())
            {
                var data = _kastRepo.HastaBilgiGetir(hastaId);
                ViewBag.ID = data.HastaID;
                ViewBag.Ad = data.HastaAdi;
                ViewBag.Soyad = data.HastaSoyadi;
                ViewBag.TC = data.HastaTCKimlikNo;

                var merkez = _kastRepo.HizmetMerkezleriGetir().Where(x => x.KurumID == 5).ToList();
                merkez.Insert(0, new IKurumHizmetMerkezleri() { ID = 0, Ad = "-Seçiniz-" });
                ViewBag.HizmetMerkezleri = new SelectList(merkez, "ID", "Ad", "-Seçiniz-");

                return View();
            }
            else if (User.IsInRole("91") && KurumdakiHastalar.Where(x => x.HastaID == hastaId).Any())
            {
                var data = _kastRepo.HastaBilgiGetir(hastaId);
                ViewBag.ID = data.HastaID;
                ViewBag.Ad = data.HastaAdi;
                ViewBag.Soyad = data.HastaSoyadi;
                ViewBag.TC = data.HastaTCKimlikNo;

                var merkez = _kastRepo.HizmetMerkezleriGetir().Where(x => x.KurumID == 5).ToList();
                merkez.Insert(0, new IKurumHizmetMerkezleri() { ID = 0, Ad = "-Seçiniz-" });
                ViewBag.HizmetMerkezleri = new SelectList(merkez, "ID", "Ad", "-Seçiniz-");

                return View();
            }
            else if (User.IsInRole("92") && TumHastalar.Where(x => x.HastaID == hastaId).Any())
            {
                var data = _kastRepo.HastaBilgiGetir(hastaId);
                ViewBag.ID = data.HastaID;
                ViewBag.Ad = data.HastaAdi;
                ViewBag.Soyad = data.HastaSoyadi;
                ViewBag.TC = data.HastaTCKimlikNo;

                var merkez = _kastRepo.HizmetMerkezleriGetir().Where(x => x.KurumID == 5).ToList();
                merkez.Insert(0, new IKurumHizmetMerkezleri() { ID = 0, Ad = "-Seçiniz-" });
                ViewBag.HizmetMerkezleri = new SelectList(merkez, "ID", "Ad", "-Seçiniz-");

                return View();
            }
            return RedirectToAction("Error404", "Home");
        }
        [HttpPost]
        public JsonResult IskurRandevuTalebiOlustur(IRandevuTalepleri model)
        {
            int userId = (int)Session["UserID"];
            if (model != null)
            {
                if (_kastRepo.RandevuTalebiKaydet(model, userId))
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false });
                }
            }
            else
            {
                return Json(new { success = false });
            }
        }
        #endregion

        #region Randevu Oluşturma Ekranı
        public ActionResult HastaSecimi()
        {
            return View();
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
        public ActionResult Kurumlar(int? hastaId)
        {
            var KendiHastalari = _vakaRepo.KendiHastalari((int)Session["UserID"]);
            var KurumdakiHastalar = _vakaRepo.KurumdakiHastalar((int)Session["UserID"]);
            var TumHastalar = _vakaRepo.HastaBilgileriGetir();

            if (User.IsInRole("90") && KendiHastalari.Where(x => x.HastaID == hastaId).Any())
            {
                var data = _kastRepo.HastaBilgiGetir(hastaId);
                ViewBag.ID = data.HastaID;
                ViewBag.TC = data.HastaTCKimlikNo;

                var kurum = _arindirmaRepo.KurumBilgileriGetir();
                kurum.Insert(0, new IKurumlar() { KurumID = 0, KurumAdi = "-Seçiniz-" });
                ViewBag.Kurumlar = new SelectList(kurum, "KurumID", "KurumAdi", "-Seçiniz-");
                return View();
            }
            else if (User.IsInRole("91") && KurumdakiHastalar.Where(x => x.HastaID == hastaId).Any())
            {
                var data = _kastRepo.HastaBilgiGetir(hastaId);
                ViewBag.ID = data.HastaID;
                ViewBag.TC = data.HastaTCKimlikNo;

                var kurum = _arindirmaRepo.KurumBilgileriGetir();
                kurum.Insert(0, new IKurumlar() { KurumID = 0, KurumAdi = "-Seçiniz-" });
                ViewBag.Kurumlar = new SelectList(kurum, "KurumID", "KurumAdi", "-Seçiniz-");
                return View();
            }
            else if (User.IsInRole("92") && TumHastalar.Where(x => x.HastaID == hastaId).Any())
            {
                var data = _kastRepo.HastaBilgiGetir(hastaId);
                ViewBag.ID = data.HastaID;
                ViewBag.TC = data.HastaTCKimlikNo;

                var kurum = _arindirmaRepo.KurumBilgileriGetir();
                kurum.Insert(0, new IKurumlar() { KurumID = 0, KurumAdi = "-Seçiniz-" });
                ViewBag.Kurumlar = new SelectList(kurum, "KurumID", "KurumAdi", "-Seçiniz-");
                return View();
            }
            return RedirectToAction("Error404", "Home");
        }
        public ActionResult RandevuOlusturmaEkrani(int? KurumID, int? PoliklinikTuruID, int? hastaId)
        {
            ViewBag.KurumAdi = _db.Kurumlar.Where(x => x.KurumID == KurumID).Select(x => x.KurumAdi).FirstOrDefault();
            ViewBag.PoliklinikTuruAdi = _db.KurumPoliklinikTurleri.Where(x => x.KurumID == KurumID && x.ID == PoliklinikTuruID).Select(x => x.PoliklinikTuru).FirstOrDefault();
            ViewBag.HastalarListesi = _arindirmaRepo.HastaBilgileriGetir();
            ViewBag.KID = KurumID;
            ViewBag.PID = PoliklinikTuruID;
            var data = _kastRepo.HastaBilgiGetir(hastaId);
            ViewBag.ID = data.HastaID;
            ViewBag.Ad = data.HastaAdi;
            ViewBag.Soyad = data.HastaSoyadi;
            ViewBag.TC = data.HastaTCKimlikNo;

            return View();
        }
        #endregion
    }
}