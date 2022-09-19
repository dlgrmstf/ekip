using EkipProjesi.Core.Hastalar;
using EkipProjesi.Core.Randevu;
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
    [Authorize(Roles = "11")]
    public class SosyalHizmetMerkezleriController : Controller
    {
        #region Const
        private SosyalHizmetRepository _shmRepo;
        private ArindirmaMerkezleriRepository _arindirmaRepo;
        private VakaTakipRepository _vakaRepo;
        public SosyalHizmetMerkezleriController()
        {
            _shmRepo = new SosyalHizmetRepository();
            _arindirmaRepo = new ArindirmaMerkezleriRepository();
            _vakaRepo = new VakaTakipRepository();
        }
        #endregion
        public ActionResult Index()
        {
            ViewBag.AktifRandevuTalepleriKendi = _shmRepo.AktifRandevuTalepleri().Where(x => x.TalepOlusturanKullaniciID == (int)Session["UserID"]).ToList();
            ViewBag.AktifRandevuTalepleri = _shmRepo.AktifRandevuTalepleri();

            ViewBag.VerilenRandevularKendi = _shmRepo.VerilenRandevular().Where(x => x.TalepOlusturanKullaniciID == (int)Session["UserID"]).ToList();
            ViewBag.VerilenRandevular = _shmRepo.VerilenRandevular();

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
        [HttpPost]
        public JsonResult TalepDetayGetir(int talepId)
        {
            var data = _shmRepo.TalepDetay(talepId);
            return Json(data);
        }
        [HttpPost]
        public JsonResult RandevuEkle(int ID, DateTime RandevuTarihi)
        {
            int userId = (int)Session["UserID"];
            var data = _shmRepo.RandevuEkle(ID, RandevuTarihi, userId);
            return Json(data);
        }
        [HttpPost]
        public JsonResult RandevuDurumGuncelle(int ID, string RandevuDurumu)
        {
            var data = _shmRepo.RandevuDurumGuncelle(ID, RandevuDurumu);
            return Json(data);
        }
    }
}