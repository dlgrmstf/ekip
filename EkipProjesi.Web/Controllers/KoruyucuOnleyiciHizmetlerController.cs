using EkipProjesi.Core.Faaliyet;
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
    [Authorize(Roles = "7")]
    public class KoruyucuOnleyiciHizmetlerController : Controller
    {
        #region Const
        private FaaliyetRepository _faaliyetRepo;
        private readonly EKIPEntities _db;

        public KoruyucuOnleyiciHizmetlerController()
        {
            _faaliyetRepo = new FaaliyetRepository();
            _db = new EKIPEntities();
        }
        #endregion
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Faaliyetler()
        {
            ViewBag.ToplamFaaliyetSayisi = _faaliyetRepo.Faaliyetler().Count();
            ViewBag.UlasilanKisiSayisi = (from f in _db.Faaliyetler select f.UlasilanKisiSayisi).Sum();
            ViewBag.TumFaaliyetler = _faaliyetRepo.Faaliyetler();
            return View();
        }
        public ActionResult FaaliyetEkle()
        {
            return View();
        }
        [HttpPost]
        public JsonResult FaaliyetEkle(IFaaliyet model)
        {
            if (model != null)
            {
                if (_faaliyetRepo.FaaliyetEkle(model))
                {
                    return Json(new { success = true, mesaj = "Faaliyet Kaydı Başarılı Bir Şekilde Oluşturuldu" });
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
        public JsonResult FaaliyetGuncelle(IFaaliyet model)
        {

            string hata = "";
            if (model != null && model.ID > 0)
            {

                if (_faaliyetRepo.FaaliyetDuzenle(model, out hata))
                {
                    return Json(new { success = true, mesaj = "Faaliyet Bilgileri Güncellendi." });
                }
            }
            else
            {
                return Json(new { success = false, mesaj = "İşlemleriniz Sırasında Hata Oluştu. Zorunlu Alanlar Boş Geçilemez ve ID 'null' Olamaz.", hata = hata, });
            }
            return Json(new { success = false, mesaj = "İşlemleriniz Sırasında Hata Oluştu. ", hata = hata });
        }
        public ActionResult FaaliyetDetay(int id)
        {
            if (TempData["Error"] != null)
            {
                ViewBag.Msg = TempData["Error"].ToString();
                TempData["Error"] = null;
            }
            else
            {
                ViewBag.Msg = "";
            }

            IFaaliyet f = new IFaaliyet();
            f = _faaliyetRepo.FaaliyetDetay(id);

            return View(f);
        }
        public ActionResult PartialFiltreSonuc(DateTime? baslangictarihi, DateTime? bitistarihi)
        {
            ViewBag.Faaliyetler = _faaliyetRepo.FaaliyetFiltreleme(baslangictarihi, bitistarihi);
            var data = _faaliyetRepo.FaaliyetFiltreleme(baslangictarihi, bitistarihi);

            return PartialView(data);
        }
        public ActionResult TumFaaliyetler()
        {
            ViewBag.ToplamFaaliyetSayisi = _faaliyetRepo.Faaliyetler().Count();
            ViewBag.UlasilanKisiSayisi = (from f in _db.Faaliyetler select f.UlasilanKisiSayisi).Sum();
            return View(_faaliyetRepo.Faaliyetler());
        }
    }
}