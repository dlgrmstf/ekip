using EkipProjesi.Core.ArindirmaModulu;
using EkipProjesi.Core.DanismaMerkezleri;
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
    [Authorize(Roles = "4")]
    public class DanismaMerkezleriController : Controller
    {
        #region Const
        private DanismaMerkezleriRepository _danismaRepo;
        private ArindirmaMerkezleriRepository _arindirmaRepo;
        private VakaTakipRepository _vakaRepo;
        private readonly EKIPEntities _db;
        public DanismaMerkezleriController()
        {
            _danismaRepo = new DanismaMerkezleriRepository();
            _arindirmaRepo = new ArindirmaMerkezleriRepository();
            _vakaRepo = new VakaTakipRepository();
            _db = new EKIPEntities();
        }
        #endregion

        public ActionResult Index()
        {
            return View();
        }

        #region YEDAM
        public ActionResult DanismaBirimleriYEDAM()
        {
            ViewBag.AktifRandevuTalepleriKendi = _danismaRepo.AktifRandevuTalepleri().Where(x => x.TalepOlusturanKullaniciID == (int)Session["UserID"] && x.KurumID == 6).ToList();
            ViewBag.AktifRandevuTalepleri = _danismaRepo.AktifRandevuTalepleri().Where(x => x.KurumID == 6);
            ViewBag.VerilenRandevularKendi = _danismaRepo.VerilenRandevular().Where(x => x.TalepOlusturanKullaniciID == (int)Session["UserID"] && x.KurumID == 6).ToList();
            ViewBag.VerilenRandevular = _danismaRepo.VerilenRandevular().Where(x => x.KurumID == 6);
            return View(_danismaRepo.YEDAMHastaBilgileri());
        }
        public ActionResult YEDAMVakaDetay(int id)
        {
            IYEDAMHastaBilgileri data = new IYEDAMHastaBilgileri();
            data = _danismaRepo.YEDAMHastaBilgiDetay(id);
            return View(data);
        }
        #endregion

        #region İSM & Randevu
        public ActionResult IlceSaglikMudurlukleri()
        {
            ViewBag.AktifRandevuTalepleriKendi = _danismaRepo.AktifRandevuTalepleri().Where(x => x.TalepOlusturanKullaniciID == (int)Session["UserID"] && x.KurumID == 8).ToList();
            ViewBag.AktifRandevuTalepleri = _danismaRepo.AktifRandevuTalepleri().Where(x=>x.KurumID == 8);

            ViewBag.VerilenRandevularKendi = _danismaRepo.VerilenRandevular().Where(x => x.TalepOlusturanKullaniciID == (int)Session["UserID"] && x.KurumID == 8).ToList();
            ViewBag.VerilenRandevular = _danismaRepo.VerilenRandevular().Where(x => x.KurumID == 8);

            ViewBag.KendiHastalari = _vakaRepo.KendiHastalari((int)Session["UserID"]);
            ViewBag.KurumdakiHastalar = _vakaRepo.KurumdakiHastalar((int)Session["UserID"]);
            ViewBag.TumHastalar = _vakaRepo.HastaBilgileriGetir();

            ViewBag.KendiHastalariBasvuru = _arindirmaRepo.KendiHastalariBasvuru((int)Session["UserID"]);
            ViewBag.KurumdakiHastalarBasvuru = _arindirmaRepo.KurumdakiHastalarBasvuru((int)Session["UserID"]);
            ViewBag.TumBasvurular = _arindirmaRepo.BasvuruBilgileriGetir();

            return View();
        }
        [HttpPost]
        public JsonResult TalepDetayGetir(int talepId)
        {
            var data = _danismaRepo.TalepDetay(talepId);
            return Json(data);
        }
        [HttpPost]
        public JsonResult RandevuEkle(int ID, DateTime RandevuTarihi)
        {
            int userId = (int)Session["UserID"];
            var data = _danismaRepo.RandevuEkle(ID, RandevuTarihi, userId);
            return Json(data);
        }
        [HttpPost]
        public JsonResult RandevuDurumGuncelle(int ID, string RandevuDurumu)
        {
            var data = _danismaRepo.RandevuDurumGuncelle(ID, RandevuDurumu);
            return Json(data);
        }
        public ActionResult ISMBasvuruDetay(int id)
        {
            IArindirmaBasvuruBilgileri b = new IArindirmaBasvuruBilgileri();
            b = _arindirmaRepo.BasvuruBilgisiDetay(id);
            string hastaTC = b.HastaTCKimlikNo;
            string hastaEkipNo = b.HastaEkipNo;
            var KendiHastalari = _vakaRepo.KendiHastalari((int)Session["UserID"]);
            var KurumdakiHastalar = _vakaRepo.KurumdakiHastalar((int)Session["UserID"]);
            var TumHastalar = _vakaRepo.HastaBilgileriGetir();

            if (User.IsInRole("90") && KendiHastalari.Where(x => x.HastaTCKimlikNo == hastaTC || x.HastaEkipNo == hastaEkipNo).Any())
            {
                b = _arindirmaRepo.BasvuruBilgisiDetay(id);
                int hastaid = _db.Hastalar.Where(x => x.HastaEkipNo == b.HastaEkipNo).Select(x => x.HastaID).FirstOrDefault();
                b.Yatislar = _arindirmaRepo.YatisBilgisiDetay(hastaid);
                b.MaddeBilgileri = _arindirmaRepo.MaddeBilgisiDetay(id);
                return View(b);
            }
            else if (User.IsInRole("91") && KurumdakiHastalar.Where(x => x.HastaTCKimlikNo == hastaTC || x.HastaEkipNo == hastaEkipNo).Any())
            {
                b = _arindirmaRepo.BasvuruBilgisiDetay(id);
                int hastaid = _db.Hastalar.Where(x => x.HastaEkipNo == b.HastaEkipNo).Select(x => x.HastaID).FirstOrDefault();
                b.Yatislar = _arindirmaRepo.YatisBilgisiDetay(hastaid);
                b.MaddeBilgileri = _arindirmaRepo.MaddeBilgisiDetay(id);
                return View(b);
            }
            else if (User.IsInRole("92") && TumHastalar.Where(x => x.HastaTCKimlikNo == hastaTC || x.HastaEkipNo == hastaEkipNo).Any())
            {
                b = _arindirmaRepo.BasvuruBilgisiDetay(id);
                int hastaid = _db.Hastalar.Where(x => x.HastaEkipNo == b.HastaEkipNo).Select(x => x.HastaID).FirstOrDefault();
                b.Yatislar = _arindirmaRepo.YatisBilgisiDetay(hastaid);
                b.MaddeBilgileri = _arindirmaRepo.MaddeBilgisiDetay(id);
                return View(b);
            }
            return RedirectToAction("Error404", "Home");
        }
        public ActionResult HastaEkle()
        {
            //var kurum = _arindirmaRepo.KurumBilgileriGetir();
            //kurum.Insert(0, new IKurumlar() { KurumID = 0, KurumAdi = "-Seçiniz-" });
            //ViewBag.Kurumlar = new SelectList(kurum, "KurumID", "KurumAdi", "-Seçiniz-");
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
                ViewBag.KayitID = id;
                var data = _arindirmaRepo.HastaBilgiGetir(id);
                return View(data);
            }
            else if (User.IsInRole("91") && KurumdakiHastalar.Where(x => x.HastaID == id).Any())
            {
                ViewBag.KayitID = id;
                var data = _arindirmaRepo.HastaBilgiGetir(id);
                return View(data);
            }
            else if (User.IsInRole("92") && TumHastalar.Where(x => x.HastaID == id).Any())
            {
                ViewBag.KayitID = id;
                var data = _arindirmaRepo.HastaBilgiGetir(id);
                return View(data);
            }
            return RedirectToAction("Error404", "Home");
        }
        #endregion
    }
}