using EkipProjesi.Core.Hastalar;
using EkipProjesi.Core.Randevu;
using EkipProjesi.Core.RehabilitasyonMerkezleri;
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
    [Authorize(Roles = "10")]
    public class RehabilitasyonMerkezleriController : Controller
    {
        #region Const
        private IstihdamRepository _istihdamRepo;
        private ArindirmaMerkezleriRepository _arindirmaRepo;
        private RehabilitasyonRepository _rehabilitasyonRepo;
        private VakaTakipRepository _vakaRepo;
        public RehabilitasyonMerkezleriController()
        {
            _istihdamRepo = new IstihdamRepository();
            _arindirmaRepo = new ArindirmaMerkezleriRepository();
            _rehabilitasyonRepo = new RehabilitasyonRepository();
            _vakaRepo = new VakaTakipRepository();
        }
        #endregion
        public ActionResult Index()
        {
            ViewBag.AktifRandevuTalepleriKendi = _rehabilitasyonRepo.AktifRandevuTalepleri().Where(x => x.KaydedenKullaniciID == (int)Session["UserID"]).ToList();
            ViewBag.AktifRandevuTalepleri = _rehabilitasyonRepo.AktifRandevuTalepleri();

            ViewBag.VerilenRandevularKendi = _rehabilitasyonRepo.VerilenRandevular().Where(x => x.KaydedenKullaniciID == (int)Session["UserID"]).ToList();
            ViewBag.VerilenRandevular = _rehabilitasyonRepo.VerilenRandevular();

            ViewBag.KendiHastalari = _vakaRepo.KendiHastalari((int)Session["UserID"]);
            ViewBag.KurumdakiHastalar = _vakaRepo.KurumdakiHastalar((int)Session["UserID"]);
            ViewBag.TumHastalar = _vakaRepo.HastaBilgileriGetir();
            return View();
        }
        public ActionResult HastaEkle()
        {
            int UserCode = _vakaRepo.KullanicininKurumu((int)Session["UserID"]).KurumKodu;
            ViewBag.Kurum = _arindirmaRepo.KurumBilgileriGetir().Where(x => x.KurumKodu == UserCode);
            return View();
        }
        [HttpPost]
        public JsonResult HastaEkle(IHastalar model)
        {
            if (model != null)
            {
                if (_vakaRepo.HastaEkle(model, (int)Session["UserID"]))
                {
                    return Json(new { id = model.HastaID, success = true, mesaj = "Hasta Kaydı Başarılı Bir Şekilde Oluşturuldu" });
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
        public ActionResult VakaDetay(int id)
        {
            var KendiHastalari = _vakaRepo.KendiHastalari((int)Session["UserID"]);
            var KurumdakiHastalar = _vakaRepo.KurumdakiHastalar((int)Session["UserID"]);
            var TumHastalar = _vakaRepo.HastaBilgileriGetir();

            if (User.IsInRole("90") && KendiHastalari.Where(x => x.HastaID == id).Any())
            {
                ViewBag.VakaID = id;
                var data = _arindirmaRepo.HastaBilgiGetir(id);
                ViewBag.BaharFormlari = _rehabilitasyonRepo.BaharFormlari(id);
                ViewBag.OlceklerFormlari = _rehabilitasyonRepo.OlceklerFormlari(id);
                ViewBag.BSIFormlari = _rehabilitasyonRepo.BSIFormlari(id);
                return View(data);
            }
            else if (User.IsInRole("91") && KurumdakiHastalar.Where(x => x.HastaID == id).Any())
            {
                ViewBag.VakaID = id;
                var data = _arindirmaRepo.HastaBilgiGetir(id);
                ViewBag.BaharFormlari = _rehabilitasyonRepo.BaharFormlari(id);
                ViewBag.OlceklerFormlari = _rehabilitasyonRepo.OlceklerFormlari(id);
                ViewBag.BSIFormlari = _rehabilitasyonRepo.BSIFormlari(id);
                return View(data);
            }
            else if (User.IsInRole("92") && TumHastalar.Where(x => x.HastaID == id).Any())
            {
                ViewBag.VakaID = id;
                var data = _arindirmaRepo.HastaBilgiGetir(id);
                ViewBag.BaharFormlari = _rehabilitasyonRepo.BaharFormlari(id);
                ViewBag.OlceklerFormlari = _rehabilitasyonRepo.OlceklerFormlari(id);
                ViewBag.BSIFormlari = _rehabilitasyonRepo.BSIFormlari(id);
                return View(data);
            }
            return RedirectToAction("Error404", "Home");
        }
        public ActionResult BaharFormuEkle(int? VakaID)
        {
            var KendiHastalari = _vakaRepo.KendiHastalari((int)Session["UserID"]);
            var KurumdakiHastalar = _vakaRepo.KurumdakiHastalar((int)Session["UserID"]);
            var TumHastalar = _vakaRepo.HastaBilgileriGetir();

            if (User.IsInRole("90") && KendiHastalari.Where(x => x.HastaID == VakaID).Any())
            {
                ViewBag.VakaID = VakaID;
                int id = (int)VakaID;
                ViewBag.HastaAdi = _arindirmaRepo.HastaBilgiGetir(id).HastaAdi;
                ViewBag.HastaSoyadi = _arindirmaRepo.HastaBilgiGetir(id).HastaSoyadi;

                ViewBag.Kodlar = _rehabilitasyonRepo.Kodlar();

                return View();
            }
            else if (User.IsInRole("91") && KurumdakiHastalar.Where(x => x.HastaID == VakaID).Any())
            {
                ViewBag.VakaID = VakaID;
                int id = (int)VakaID;
                ViewBag.HastaAdi = _arindirmaRepo.HastaBilgiGetir(id).HastaAdi;
                ViewBag.HastaSoyadi = _arindirmaRepo.HastaBilgiGetir(id).HastaSoyadi;

                ViewBag.Kodlar = _rehabilitasyonRepo.Kodlar();

                return View();
            }
            else if (User.IsInRole("92") && TumHastalar.Where(x => x.HastaID == VakaID).Any())
            {
                ViewBag.VakaID = VakaID;
                int id = (int)VakaID;
                ViewBag.HastaAdi = _arindirmaRepo.HastaBilgiGetir(id).HastaAdi;
                ViewBag.HastaSoyadi = _arindirmaRepo.HastaBilgiGetir(id).HastaSoyadi;

                ViewBag.Kodlar = _rehabilitasyonRepo.Kodlar();

                return View();
            }
            return RedirectToAction("Error404", "Home");
        }
        [HttpPost]
        public ActionResult BaharFormuEkle(IHastaBaharFormlari model)
        {
            if (model != null)
            {
                if (_rehabilitasyonRepo.BaharFormuEkle(model, (int)Session["UserID"]))
                {
                    return RedirectToAction("VakaDetay", "RehabilitasyonMerkezleri", new { id = model.HastaID});
                }
                else
                {
                    return View();
                }
            }
            else
                return View();
        }
        public ActionResult BaharFormDetay(int ID)
        {
            var KendiHastalari = _vakaRepo.KendiHastalari((int)Session["UserID"]);
            var KurumdakiHastalar = _vakaRepo.KurumdakiHastalar((int)Session["UserID"]);
            var TumHastalar = _vakaRepo.HastaBilgileriGetir();

            if (User.IsInRole("90") && KendiHastalari.Where(x => x.HastaID == _rehabilitasyonRepo.BaharFormDetay(ID).HastaID).Any())
            {
                ViewBag.FormSonuc = _rehabilitasyonRepo.BaharFormSonucDetay(ID);
                var data = _rehabilitasyonRepo.BaharFormDetay(ID);
                int id = _rehabilitasyonRepo.BaharFormDetay(ID).HastaID;
                ViewBag.HastaAdi = _arindirmaRepo.HastaBilgiGetir(id).HastaAdi;
                ViewBag.HastaSoyadi = _arindirmaRepo.HastaBilgiGetir(id).HastaSoyadi;
                ViewBag.HastaTC = _arindirmaRepo.HastaBilgiGetir(id).HastaTCKimlikNo;
                ViewBag.Kodlar = _rehabilitasyonRepo.Kodlar();
                return View(data);
            }
            else if (User.IsInRole("91") && KurumdakiHastalar.Where(x => x.HastaID == _rehabilitasyonRepo.BaharFormDetay(ID).HastaID).Any())
            {
                ViewBag.FormSonuc = _rehabilitasyonRepo.BaharFormSonucDetay(ID);
                var data = _rehabilitasyonRepo.BaharFormDetay(ID);
                int id = _rehabilitasyonRepo.BaharFormDetay(ID).HastaID;
                ViewBag.HastaAdi = _arindirmaRepo.HastaBilgiGetir(id).HastaAdi;
                ViewBag.HastaSoyadi = _arindirmaRepo.HastaBilgiGetir(id).HastaSoyadi;
                ViewBag.HastaTC = _arindirmaRepo.HastaBilgiGetir(id).HastaTCKimlikNo;
                ViewBag.Kodlar = _rehabilitasyonRepo.Kodlar();
                return View(data);
            }
            else if (User.IsInRole("92") && TumHastalar.Where(x => x.HastaID == _rehabilitasyonRepo.BaharFormDetay(ID).HastaID).Any())
            {
                ViewBag.FormSonuc = _rehabilitasyonRepo.BaharFormSonucDetay(ID);
                var data = _rehabilitasyonRepo.BaharFormDetay(ID);
                int id = _rehabilitasyonRepo.BaharFormDetay(ID).HastaID;
                ViewBag.HastaAdi = _arindirmaRepo.HastaBilgiGetir(id).HastaAdi;
                ViewBag.HastaSoyadi = _arindirmaRepo.HastaBilgiGetir(id).HastaSoyadi;
                ViewBag.HastaTC = _arindirmaRepo.HastaBilgiGetir(id).HastaTCKimlikNo;
                ViewBag.Kodlar = _rehabilitasyonRepo.Kodlar();
                return View(data);
            }
            return RedirectToAction("Error404", "Home");
        }
        public ActionResult OlceklerFormuEkle(int? VakaID)
        {
            var KendiHastalari = _vakaRepo.KendiHastalari((int)Session["UserID"]);
            var KurumdakiHastalar = _vakaRepo.KurumdakiHastalar((int)Session["UserID"]);
            var TumHastalar = _vakaRepo.HastaBilgileriGetir();

            if (User.IsInRole("90") && KendiHastalari.Where(x => x.HastaID == VakaID).Any())
            {
                ViewBag.VakaID = VakaID;
                int id = (int)VakaID;
                ViewBag.HastaAdi = _arindirmaRepo.HastaBilgiGetir(id).HastaAdi;
                ViewBag.HastaSoyadi = _arindirmaRepo.HastaBilgiGetir(id).HastaSoyadi;

                return View();
            }
            else if (User.IsInRole("91") && KurumdakiHastalar.Where(x => x.HastaID == VakaID).Any())
            {
                ViewBag.VakaID = VakaID;
                int id = (int)VakaID;
                ViewBag.HastaAdi = _arindirmaRepo.HastaBilgiGetir(id).HastaAdi;
                ViewBag.HastaSoyadi = _arindirmaRepo.HastaBilgiGetir(id).HastaSoyadi;

                return View();
            }
            else if (User.IsInRole("92") && TumHastalar.Where(x => x.HastaID == VakaID).Any())
            {
                ViewBag.VakaID = VakaID;
                int id = (int)VakaID;
                ViewBag.HastaAdi = _arindirmaRepo.HastaBilgiGetir(id).HastaAdi;
                ViewBag.HastaSoyadi = _arindirmaRepo.HastaBilgiGetir(id).HastaSoyadi;

                return View();
            }
            return RedirectToAction("Error404", "Home");
        }
        [HttpPost]
        public ActionResult OlceklerFormuEkle(IHastaOlceklerFormu model)
        {
            if (model != null)
            {
                if (_rehabilitasyonRepo.OlceklerFormuEkle(model, (int)Session["UserID"]))
                {
                    return RedirectToAction("VakaDetay", "RehabilitasyonMerkezleri", new { id = model.HastaID });
                }
                else
                {
                    return View();
                }
            }
            else
                return View();
        }
        public ActionResult OlceklerFormDetay(int ID)
        {
            var KendiHastalari = _vakaRepo.KendiHastalari((int)Session["UserID"]);
            var KurumdakiHastalar = _vakaRepo.KurumdakiHastalar((int)Session["UserID"]);
            var TumHastalar = _vakaRepo.HastaBilgileriGetir();

            if (User.IsInRole("90") && KendiHastalari.Where(x => x.HastaID == _rehabilitasyonRepo.OlceklerFormDetay(ID).HastaID).Any())
            {
                var data = _rehabilitasyonRepo.OlceklerFormDetay(ID);
                int id = _rehabilitasyonRepo.OlceklerFormDetay(ID).HastaID;
                ViewBag.HastaAdi = _arindirmaRepo.HastaBilgiGetir(id).HastaAdi;
                ViewBag.HastaSoyadi = _arindirmaRepo.HastaBilgiGetir(id).HastaSoyadi;
                ViewBag.HastaTC = _arindirmaRepo.HastaBilgiGetir(id).HastaTCKimlikNo;
                return View(data);
            }
            else if (User.IsInRole("91") && KurumdakiHastalar.Where(x => x.HastaID == _rehabilitasyonRepo.OlceklerFormDetay(ID).HastaID).Any())
            {
                var data = _rehabilitasyonRepo.OlceklerFormDetay(ID);
                int id = _rehabilitasyonRepo.OlceklerFormDetay(ID).HastaID;
                ViewBag.HastaAdi = _arindirmaRepo.HastaBilgiGetir(id).HastaAdi;
                ViewBag.HastaSoyadi = _arindirmaRepo.HastaBilgiGetir(id).HastaSoyadi;
                ViewBag.HastaTC = _arindirmaRepo.HastaBilgiGetir(id).HastaTCKimlikNo;
                return View(data);
            }
            else if (User.IsInRole("92") && TumHastalar.Where(x => x.HastaID == _rehabilitasyonRepo.OlceklerFormDetay(ID).HastaID).Any())
            {
                var data = _rehabilitasyonRepo.OlceklerFormDetay(ID);
                int id = _rehabilitasyonRepo.OlceklerFormDetay(ID).HastaID;
                ViewBag.HastaAdi = _arindirmaRepo.HastaBilgiGetir(id).HastaAdi;
                ViewBag.HastaSoyadi = _arindirmaRepo.HastaBilgiGetir(id).HastaSoyadi;
                ViewBag.HastaTC = _arindirmaRepo.HastaBilgiGetir(id).HastaTCKimlikNo;
                return View(data);
            }
            return RedirectToAction("Error404", "Home");           
        }
        public ActionResult BSIFormuEkle(int? VakaID)
        {
            var KendiHastalari = _vakaRepo.KendiHastalari((int)Session["UserID"]);
            var KurumdakiHastalar = _vakaRepo.KurumdakiHastalar((int)Session["UserID"]);
            var TumHastalar = _vakaRepo.HastaBilgileriGetir();

            if (User.IsInRole("90") && KendiHastalari.Where(x => x.HastaID == VakaID).Any())
            {
                ViewBag.VakaID = VakaID;
                int id = (int)VakaID;
                ViewBag.HastaAdi = _arindirmaRepo.HastaBilgiGetir(id).HastaAdi;
                ViewBag.HastaSoyadi = _arindirmaRepo.HastaBilgiGetir(id).HastaSoyadi;

                return View();
            }
            else if (User.IsInRole("91") && KurumdakiHastalar.Where(x => x.HastaID == VakaID).Any())
            {
                ViewBag.VakaID = VakaID;
                int id = (int)VakaID;
                ViewBag.HastaAdi = _arindirmaRepo.HastaBilgiGetir(id).HastaAdi;
                ViewBag.HastaSoyadi = _arindirmaRepo.HastaBilgiGetir(id).HastaSoyadi;

                return View();
            }
            else if (User.IsInRole("92") && TumHastalar.Where(x => x.HastaID == VakaID).Any())
            {
                ViewBag.VakaID = VakaID;
                int id = (int)VakaID;
                ViewBag.HastaAdi = _arindirmaRepo.HastaBilgiGetir(id).HastaAdi;
                ViewBag.HastaSoyadi = _arindirmaRepo.HastaBilgiGetir(id).HastaSoyadi;

                return View();
            }
            return RedirectToAction("Error404", "Home");
        }
        [HttpPost]
        public ActionResult BSIFormuEkle(IHastaBagimlilikSiddetiFormlari model)
        {
            if (model != null)
            {
                if (_rehabilitasyonRepo.BSIFormuEkle(model, (int)Session["UserID"]))
                {
                    return RedirectToAction("VakaDetay", "RehabilitasyonMerkezleri", new { id = model.HastaID });
                }
                else
                {
                    return View();
                }
            }
            else
                return View();
        }
        public ActionResult BSIFormDetay(int ID)
        {
            var KendiHastalari = _vakaRepo.KendiHastalari((int)Session["UserID"]);
            var KurumdakiHastalar = _vakaRepo.KurumdakiHastalar((int)Session["UserID"]);
            var TumHastalar = _vakaRepo.HastaBilgileriGetir();

            if (User.IsInRole("90") && KendiHastalari.Where(x => x.HastaID == _rehabilitasyonRepo.BSIFormDetay(ID).HastaID).Any())
            {
                var data = _rehabilitasyonRepo.BSIFormDetay(ID);
                int id = _rehabilitasyonRepo.BSIFormDetay(ID).HastaID;
                ViewBag.HastaAdi = _arindirmaRepo.HastaBilgiGetir(id).HastaAdi;
                ViewBag.HastaSoyadi = _arindirmaRepo.HastaBilgiGetir(id).HastaSoyadi;
                ViewBag.HastaTC = _arindirmaRepo.HastaBilgiGetir(id).HastaTCKimlikNo;
                return View(data);
            }
            else if (User.IsInRole("91") && KurumdakiHastalar.Where(x => x.HastaID == _rehabilitasyonRepo.BSIFormDetay(ID).HastaID).Any())
            {
                var data = _rehabilitasyonRepo.BSIFormDetay(ID);
                int id = _rehabilitasyonRepo.BSIFormDetay(ID).HastaID;
                ViewBag.HastaAdi = _arindirmaRepo.HastaBilgiGetir(id).HastaAdi;
                ViewBag.HastaSoyadi = _arindirmaRepo.HastaBilgiGetir(id).HastaSoyadi;
                ViewBag.HastaTC = _arindirmaRepo.HastaBilgiGetir(id).HastaTCKimlikNo;
                return View(data);
            }
            else if (User.IsInRole("92") && TumHastalar.Where(x => x.HastaID == _rehabilitasyonRepo.BSIFormDetay(ID).HastaID).Any())
            {
                var data = _rehabilitasyonRepo.BSIFormDetay(ID);
                int id = _rehabilitasyonRepo.BSIFormDetay(ID).HastaID;
                ViewBag.HastaAdi = _arindirmaRepo.HastaBilgiGetir(id).HastaAdi;
                ViewBag.HastaSoyadi = _arindirmaRepo.HastaBilgiGetir(id).HastaSoyadi;
                ViewBag.HastaTC = _arindirmaRepo.HastaBilgiGetir(id).HastaTCKimlikNo;
                return View(data);
            }
            return RedirectToAction("Error404", "Home");
        }

        public ActionResult BSIFormEkle(int? VakaID)
        {
            var KendiHastalari = _vakaRepo.KendiHastalari((int)Session["UserID"]);
            var KurumdakiHastalar = _vakaRepo.KurumdakiHastalar((int)Session["UserID"]);
            var TumHastalar = _vakaRepo.HastaBilgileriGetir();

            if (User.IsInRole("90") && KendiHastalari.Where(x => x.HastaID == VakaID).Any())
            {
                ViewBag.VakaID = VakaID;
                int id = (int)VakaID;
                ViewBag.HastaAdi = _arindirmaRepo.HastaBilgiGetir(id).HastaAdi;
                ViewBag.HastaSoyadi = _arindirmaRepo.HastaBilgiGetir(id).HastaSoyadi;
                return View();
            }
            else if (User.IsInRole("91") && KurumdakiHastalar.Where(x => x.HastaID == VakaID).Any())
            {
                ViewBag.VakaID = VakaID;
                int id = (int)VakaID;
                ViewBag.HastaAdi = _arindirmaRepo.HastaBilgiGetir(id).HastaAdi;
                ViewBag.HastaSoyadi = _arindirmaRepo.HastaBilgiGetir(id).HastaSoyadi;
                return View();
            }
            else if (User.IsInRole("92") && TumHastalar.Where(x => x.HastaID == VakaID).Any())
            {
                ViewBag.VakaID = VakaID;
                int id = (int)VakaID;
                ViewBag.HastaAdi = _arindirmaRepo.HastaBilgiGetir(id).HastaAdi;
                ViewBag.HastaSoyadi = _arindirmaRepo.HastaBilgiGetir(id).HastaSoyadi;
                return View();
            }
            return RedirectToAction("Error404", "Home");
        }
        [HttpPost]
        public ActionResult BSIFormEkle(IHastaBagimlilikSiddetiFormlari model)
        {
            if (model != null)
            {
                if (_rehabilitasyonRepo.BSIFormuEkle(model, (int)Session["UserID"]))
                {
                    return RedirectToAction("VakaDetay", "RehabilitasyonMerkezleri", new { id = model.HastaID });
                }
                else
                {
                    return View();
                }
            }
            else
                return View();
        }
        public ActionResult BSIFormDetayi(int ID)
        {
            var KendiHastalari = _vakaRepo.KendiHastalari((int)Session["UserID"]);
            var KurumdakiHastalar = _vakaRepo.KurumdakiHastalar((int)Session["UserID"]);
            var TumHastalar = _vakaRepo.HastaBilgileriGetir();

            if (User.IsInRole("90") && KendiHastalari.Where(x => x.HastaID == _rehabilitasyonRepo.BSIFormDetay(ID).HastaID).Any())
            {
                var data = _rehabilitasyonRepo.BSIFormDetay(ID);
                int id = _rehabilitasyonRepo.BSIFormDetay(ID).HastaID;
                ViewBag.HastaAdi = _arindirmaRepo.HastaBilgiGetir(id).HastaAdi;
                ViewBag.HastaSoyadi = _arindirmaRepo.HastaBilgiGetir(id).HastaSoyadi;
                ViewBag.HastaTC = _arindirmaRepo.HastaBilgiGetir(id).HastaTCKimlikNo;
                return View(data);
            }
            else if (User.IsInRole("91") && KurumdakiHastalar.Where(x => x.HastaID == _rehabilitasyonRepo.BSIFormDetay(ID).HastaID).Any())
            {
                var data = _rehabilitasyonRepo.BSIFormDetay(ID);
                int id = _rehabilitasyonRepo.BSIFormDetay(ID).HastaID;
                ViewBag.HastaAdi = _arindirmaRepo.HastaBilgiGetir(id).HastaAdi;
                ViewBag.HastaSoyadi = _arindirmaRepo.HastaBilgiGetir(id).HastaSoyadi;
                ViewBag.HastaTC = _arindirmaRepo.HastaBilgiGetir(id).HastaTCKimlikNo;
                return View(data);
            }
            else if (User.IsInRole("92") && TumHastalar.Where(x => x.HastaID == _rehabilitasyonRepo.BSIFormDetay(ID).HastaID).Any())
            {
                var data = _rehabilitasyonRepo.BSIFormDetay(ID);
                int id = _rehabilitasyonRepo.BSIFormDetay(ID).HastaID;
                ViewBag.HastaAdi = _arindirmaRepo.HastaBilgiGetir(id).HastaAdi;
                ViewBag.HastaSoyadi = _arindirmaRepo.HastaBilgiGetir(id).HastaSoyadi;
                ViewBag.HastaTC = _arindirmaRepo.HastaBilgiGetir(id).HastaTCKimlikNo;
                return View(data);
            }
            return RedirectToAction("Error404", "Home");
        }

        [HttpPost]
        public ActionResult BSIFormGuncelle(IHastaBagimlilikSiddetiFormlari model)
        {
            if (model != null)
            {
                if (_rehabilitasyonRepo.BSIFormGuncelle(model))
                {
                    return RedirectToAction("BSIFormDetayi", "RehabilitasyonMerkezleri", new { id = model.ID });
                }
                else
                {
                    return View();
                }
            }
            else
                return View();
        }
    }
}