using EkipProjesi.Core.AcilServis;
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
    [Authorize(Roles = "5")]
    public class GenelHastanelerAcilServisController : Controller
    {
        #region Const
        AcilServisRepository _acilRepo;
        VakaTakipRepository _vakaRepo;
        private readonly EKIPEntities _db;

        public GenelHastanelerAcilServisController()
        {
            _acilRepo = new AcilServisRepository();
            _vakaRepo = new VakaTakipRepository();
            _db = new EKIPEntities();
        }
        #endregion
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult HastaneAcilServisleri()
        {
            ViewBag.KendiHastalariBasvuru = _acilRepo.KendiHastalariBasvuru((int)Session["UserID"]);
            ViewBag.KurumdakiHastalarBasvuru = _acilRepo.KurumdakiHastalarBasvuru((int)Session["UserID"]);
            ViewBag.TumBasvurular = _acilRepo.AcilServisVakaBilgileri();
            return View();
        }
        public ActionResult Acil112()
        {
            ViewBag.KendiHastalariBasvuru = _acilRepo.KendiHastalariBasvuru112((int)Session["UserID"]);
            ViewBag.KurumdakiHastalarBasvuru = _acilRepo.KurumdakiHastalarBasvuru112((int)Session["UserID"]);
            ViewBag.TumBasvurular = _acilRepo.VakaBilgileri112();
            return View();
        }
        public ActionResult VakaDetay(int id)
        {
            IAcilServisVakaBilgileri data = new IAcilServisVakaBilgileri();
            data = _acilRepo.AcilServisVakaBilgisiDetay(id);

            string hastaTC = data.HastaTCKimlikNo;
            string hastaEkipNo = data.HastaEkipNo;
            var KendiHastalari = _vakaRepo.KendiHastalari((int)Session["UserID"]);
            var KurumdakiHastalar = _vakaRepo.KurumdakiHastalar((int)Session["UserID"]);
            var TumHastalar = _vakaRepo.HastaBilgileriGetir();

            if (User.IsInRole("90") && KendiHastalari.Where(x => x.HastaTCKimlikNo == hastaTC || x.HastaEkipNo == hastaEkipNo).Any())
            {
                return View(data);
            }
            else if (User.IsInRole("91") && KurumdakiHastalar.Where(x => x.HastaTCKimlikNo == hastaTC || x.HastaEkipNo == hastaEkipNo).Any())
            {
                return View(data);
            }
            else if (User.IsInRole("92") && TumHastalar.Where(x => x.HastaTCKimlikNo == hastaTC || x.HastaEkipNo == hastaEkipNo).Any())
            {
                return View(data);
            }
            return RedirectToAction("Error404", "Home");
        }
        public ActionResult VakaDetay112(int id)
        {
            IVakaBilgileri112 data = new IVakaBilgileri112();
            data = _acilRepo.VakaBilgileri112Detay(id);

            string hastaTC = data.HastaTCKimlikNo;
            string hastaEkipNo = data.HastaEkipNo;
            var KendiHastalari = _vakaRepo.KendiHastalari((int)Session["UserID"]);
            var KurumdakiHastalar = _vakaRepo.KurumdakiHastalar((int)Session["UserID"]);
            var TumHastalar = _vakaRepo.HastaBilgileriGetir();

            if (User.IsInRole("90") && KendiHastalari.Where(x => x.HastaTCKimlikNo == hastaTC || x.HastaEkipNo == hastaEkipNo).Any())
            {
                return View(data);
            }
            else if (User.IsInRole("91") && KurumdakiHastalar.Where(x => x.HastaTCKimlikNo == hastaTC || x.HastaEkipNo == hastaEkipNo).Any())
            {
                return View(data);
            }
            else if (User.IsInRole("92") && TumHastalar.Where(x => x.HastaTCKimlikNo == hastaTC || x.HastaEkipNo == hastaEkipNo).Any())
            {
                return View(data);
            }
            return RedirectToAction("Error404", "Home");
        }
    }
}