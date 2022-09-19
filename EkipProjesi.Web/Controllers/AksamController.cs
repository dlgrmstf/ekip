using EkipProjesi.Core;
using EkipProjesi.Core.Formlar;
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
    [Authorize(Roles = "1")]
    public class AksamController : Controller
    {
        #region Const
        private VakaTakipRepository _vakaRepo;
        private AnketRepository _anketRepo;
        private ArindirmaMerkezleriRepository _arindirmaRepo;
        public AksamController()
        {
            _vakaRepo = new VakaTakipRepository();
            _anketRepo = new AnketRepository();
            _arindirmaRepo = new ArindirmaMerkezleriRepository();
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
            var KendiHastalari = _vakaRepo.KendiHastalari((int)Session["UserID"]);
            var KurumdakiHastalar = _vakaRepo.KurumdakiHastalar((int)Session["UserID"]);
            var TumHastalar = _vakaRepo.HastaBilgileriGetir();
            IHastalar h = new IHastalar();
            if (User.IsInRole("90") && KendiHastalari.Where(x => x.HastaID == id).Any())
            {
                h = _vakaRepo.VakaDetay(id);
                ViewBag.Formlarim = _anketRepo.KullaniciAktifAnketler(id);
                return View(h);
            }
            else if (User.IsInRole("91") && KurumdakiHastalar.Where(x => x.HastaID == id).Any())
            {
                h = _vakaRepo.VakaDetay(id);
                ViewBag.Formlarim = _anketRepo.KullaniciAktifAnketler(id);
                return View(h);
            }
            else if (User.IsInRole("92") && TumHastalar.Where(x => x.HastaID == id).Any())
            {
                h = _vakaRepo.VakaDetay(id);
                ViewBag.Formlarim = _anketRepo.KullaniciAktifAnketler(id);
                return View(h);
            }
            return RedirectToAction("Error404", "Home");
        }
        public ActionResult FormDetay(int id, int HastaID)
        {
            var KendiHastalari = _vakaRepo.KendiHastalari((int)Session["UserID"]);
            var KurumdakiHastalar = _vakaRepo.KurumdakiHastalar((int)Session["UserID"]);
            var TumHastalar = _vakaRepo.HastaBilgileriGetir();
            if (User.IsInRole("90") && KendiHastalari.Where(x => x.HastaID == HastaID).Any())
            {
                AnketDTO model = _anketRepo.KullaniciAnketDetayi(id, HastaID);
                if (model == null || model.ID == 0) return RedirectToAction("VakaDetay", "Aksam");
                ViewBag.HastaID = HastaID;
                return View(model);
            }
            else if (User.IsInRole("91") && KurumdakiHastalar.Where(x => x.HastaID == HastaID).Any())
            {
                AnketDTO model = _anketRepo.KullaniciAnketDetayi(id, HastaID);
                if (model == null || model.ID == 0) return RedirectToAction("VakaDetay", "Aksam");
                ViewBag.HastaID = HastaID;
                return View(model);
            }
            else if (User.IsInRole("92") && TumHastalar.Where(x => x.HastaID == HastaID).Any())
            {
                AnketDTO model = _anketRepo.KullaniciAnketDetayi(id, HastaID);
                if (model == null || model.ID == 0) return RedirectToAction("VakaDetay", "Aksam");
                ViewBag.HastaID = HastaID;
                return View(model);
            }
            return RedirectToAction("Error404", "Home");
        }
    }
}