using EkipProjesi.Core;
using EkipProjesi.Core.Formlar;
//using EkipProjesi.Core.UyariDuyuruBildirimMesaj;
using EkipProjesi.Data.Repositories;
using EkipProjesi.Web.Filter;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace EkipProjesi.Web.Controllers
{
    [SessionAttritube]
    [Authorize(Roles = "13")]
    public class FormlarController : Controller
    {
        #region Const
        private AnketRepository _anketRepo;
        private ArindirmaMerkezleriRepository _arindirmaRepo;
        private VakaTakipRepository _vakaRepo;
        
        public FormlarController()
        {
            _anketRepo = new AnketRepository();
            _arindirmaRepo = new ArindirmaMerkezleriRepository();
            _vakaRepo = new VakaTakipRepository();
        }
        #endregion
        public ActionResult Index()
        {
            return View();
        }

        #region Formlar
        public ActionResult Formlar()
        {
            AnketDTO model = new AnketDTO();

            model.Anketler = _anketRepo.Anketler();
            model.AnketTipleri = _anketRepo.AnketTipleri().Where(x => x.Status == true).ToList();

            return View(model);
        }
        public ActionResult FormEkle()
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
            AnketDTO r = new AnketDTO();
            r.AnketTipleri = _anketRepo.AnketTipleri().Where(x => x.Status == true).ToList();
            return View(r);
        }
        [HttpPost]
        public ActionResult FormEkle(AnketDTO model)
        {          
            if (model.BitisTarihi < model.BaslangicTarihi)
            {
                return Json(new { success = false, message = "Anket Bitiş tarihi Başlangıç tarihinden küçük olamaz!" });
            }
            model.UserID = (int)Session["UserID"];
            if (_anketRepo.AnketOlustur(model))
            {
                return RedirectToAction("Formlar", "Formlar");
            }
            else
            {
                ViewBag.Msg = "İşleminiz Sırasında Hata Oluşmuştur";
                return View(model);
            }
        }
        public ActionResult FormDetay(int id)
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
            AnketDTO r = new AnketDTO();
            r = _anketRepo.FormDetayBilgi(id);
            r.AnketTipleri = _anketRepo.AnketTipleri().Where(x => x.Status == true).ToList();
            return View(r);
        }
        [HttpPost]
        public ActionResult FormDetay(AnketDTO model)
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
            if (_anketRepo.AnketGuncelle(model))
            {
                return RedirectToAction("Formlar", "Formlar");
            }
            else
            {
                TempData["Error"] = "Hata oluştu. Lütfen tekrar deneyiniz.";
                return RedirectToAction("FormDetay", "Formlar", new { id = model.ID });
            }
        }
        public ActionResult FormSoruEsle(long id)
        {
            AnketDTO model = _anketRepo.AnketDetay(id);
            model.AnketGruplari = _anketRepo.AnketGruplari(model.ID);
            model.Sorular = _anketRepo.AnketSorulari(model.ID, 0);
            model.SoruBankasi = _anketRepo.SoruBankasi(true);
            return View(model);
        }
        [HttpPost]
        public JsonResult GrupEkleGuncelle(AnketGruplariDTO model)
        {
            if (ModelState.IsValid)
            {
                model.UserID = (int)Session["UserID"];
                if (model.ID == 0)
                {
                    if (_anketRepo.AnketGrubuEkle(model))
                    {
                        return Json(new { success = true, model = model });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Kayıt Sırasında Hata Oluştu" });
                    }
                }
                else
                {
                    if (_anketRepo.AnketGrubuGuncelle(model))
                    {
                        return Json(new { success = true });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Kayıt Sırasında Hata Oluştu" });
                    }
                }
            }
            else
            {
                return Json(new { success = false, message = "Zorunlu Alanları Doldurunuz" });
            }
        }
        [HttpPost]
        public JsonResult GrupSil(long id)
        {
            if (_anketRepo.AnketGrupSil(id))
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = "Silme İşlemi Sırasında Hata Oluştu" });
            }
        }
        [HttpPost]
        public JsonResult FormSoruEkle(long AnketID, long GrupID, List<long> Sorular)
        {
            if (ModelState.IsValid)
            {
                if (_anketRepo.AnketSoruEkle(GrupID, AnketID, Sorular))
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = "Kayıt Sırasında Hata Oluştu" });
                }
            }
            else
            {
                return Json(new { success = false, message = "Zorunlu Alanları Doldurunuz" });
            }
        }
        [HttpPost]
        public JsonResult SoruSil(long id)
        {
            if (ModelState.IsValid)
            {
                if (_anketRepo.AnketSoruSil(id))
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = "Kayıt Sırasında Hata Oluştu" });
                }
            }
            else
            {
                return Json(new { success = false, message = "Zorunlu Alanları Doldurunuz" });
            }
        }
        [HttpPost]
        public JsonResult SoruTipGuncelle(AnketSorulariDTO model)
        {
            if (model != null)
            {
                if (_anketRepo.AnketSoruTipGuncelle(model))
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = "Kayıt Sırasında Hata Oluştu" });
                }
            }
            else
            {
                return Json(new { success = false, message = "Zorunlu Alanları Doldurunuz" });
            }
        }
        [HttpPost]
        public JsonResult SoruPuanGuncelle(List<AnketSorulariDTO> model)
        {
            if (ModelState.IsValid)
            {
                if (_anketRepo.AnketSoruPuanGuncelle(model))
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = "Kayıt Sırasında Hata Oluştu" });
                }
            }
            else
            {
                return Json(new { success = false, message = "Zorunlu Alanları Doldurunuz" });
            }
        }
        [HttpPost]
        public JsonResult SoruZorunluGuncelle(AnketSorulariDTO model)
        {
            if (model != null)
            {
                if (_anketRepo.AnketSoruZorunluGuncelle(model))
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = "Kayıt Sırasında Hata Oluştu" });
                }
            }
            else
            {
                return Json(new { success = false, message = "Zorunlu Alanları Doldurunuz" });
            }
        }
        [HttpPost]
        public JsonResult CevapSil(long id)
        {
            if (ModelState.IsValid)
            {
                if (_anketRepo.AnketCevapSil(id))
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = "Silme Sırasında Hata Oluştu" });
                }
            }
            else
            {
                return Json(new { success = false, message = "Zorunlu Alanları Doldurunuz" });
            }
        }
        [HttpPost]
        public JsonResult CevapEkleGuncelle(AnketCevaplariDTO model)
        {
            if (ModelState.IsValid)
            {
                if (model.ID == 0)
                {
                    if (_anketRepo.AnketCevapEkle(model))
                    {
                        return Json(new { success = true });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Kayıt Sırasında Hata Oluştu" });
                    }
                }
                else
                {
                    if (_anketRepo.AnketCevapGuncelle(model))
                    {
                        return Json(new { success = true });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Kayıt Sırasında Hata Oluştu" });
                    }
                }

            }
            else
            {
                return Json(new { success = false, message = "Zorunlu Alanları Doldurunuz" });
            }
        }
        public ActionResult HastaEsle(long id)
        {
            AnketDTO model = _anketRepo.AnketDetay(id);

            if (User.IsInRole("90"))
            {
                model.Personeller = _vakaRepo.KendiHastalari((int)Session["UserID"]);
                return View(model);
            }
            else if (User.IsInRole("91"))
            {
                model.Personeller = _vakaRepo.KurumdakiHastalar((int)Session["UserID"]);
                return View(model);
            }
            else if (User.IsInRole("92"))
            {
                model.Personeller = _vakaRepo.HastaBilgileriGetir();
                return View(model);
            }
            return RedirectToAction("Error404", "Home");
        }
        [HttpPost]
        public JsonResult KullaniciKaydet(List<int> personeller, long anket)
        {
            if (_anketRepo.AnketKullaniciEkle(personeller, anket))
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }
        [HttpPost]
        public JsonResult KullaniciSil(int user, long anket)
        {
            if (_anketRepo.AnketKullaniciSil(anket, user))
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }
        #endregion

        #region Form Tipleri İşlem
        public ActionResult FormTipleri()
        {
            return View(_anketRepo.AnketTipleri());
        }
        public ActionResult FormTipiEkle()
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
            AnketTipleriDTO r = new AnketTipleriDTO();
            return View(r);
        }
        [HttpPost]
        public ActionResult FormTipiEkle(AnketTipleriDTO model)
        {
            if (_anketRepo.AnketTipKayit(model, (int)Session["UserID"]))
            {
                return RedirectToAction("FormTipleri", "Formlar");
            }
            else
            {
                ViewBag.Msg = "İşleminiz Sırasında Hata Oluşmuştur";
                return View(model);
            }
        }
        public ActionResult FormTipiDetay(int id)
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
            AnketTipleriDTO r = new AnketTipleriDTO();
            r = _anketRepo.FormTipiDetay(id);
            return View(r);
        }
        [HttpPost]
        public ActionResult FormTipiDetay(AnketTipleriDTO model)
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
            if (_anketRepo.AnketTipGuncelle(model))
            {
                return RedirectToAction("FormTipleri", "Formlar");
            }
            else
            {
                TempData["Error"] = "Hata oluştu. Lütfen tekrar deneyiniz.";
                return RedirectToAction("FormTipiDetay", "Formlar", new { id = model.ID });
            }
        }
        #endregion

        #region Soru Bankası İşlem
        public ActionResult SoruBankasi()
        {
            return View(_anketRepo.SoruBankasi(null));
        }
        public ActionResult SoruEkle()
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
            AnketSoruBankasiDTO r = new AnketSoruBankasiDTO();
            return View(r);
        }
        [HttpPost]
        public ActionResult SoruEkle(AnketSoruBankasiDTO model)
        {
            if (_anketRepo.SoruKayit(model, (int)Session["UserID"]))
            {
                return RedirectToAction("SoruBankasi", "Formlar");
            }
            else
            {
                ViewBag.Msg = "İşleminiz Sırasında Hata Oluşmuştur";
                return View(model);
            }
        }
        public ActionResult SoruDetay(int id)
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
            AnketSoruBankasiDTO r = new AnketSoruBankasiDTO();
            r = _anketRepo.SoruDetay(id, (int)Session["UserID"]);
            return View(r);
        }
        [HttpPost]
        public ActionResult SoruDetay(AnketSoruBankasiDTO model)
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
            if (_anketRepo.SoruGuncelle(model, (int)Session["UserID"]))
            {
                return RedirectToAction("SoruBankasi", "Formlar");
            }
            else
            {
                TempData["Error"] = "Hata oluştu. Lütfen tekrar deneyiniz.";
                return RedirectToAction("SoruDetay", "Formlar", new { id = model.ID });
            }
        }
        #endregion

        #region Form Rapor
        public ActionResult AnketRapor()
        {
            return View();
        }
        public ActionResult Veriler(long? id)
        {
            if (id == null || id < 0) return RedirectToAction("AnketRapor", "Formlar");
            return View(_anketRepo.AnketVerileri(id.Value));
        }
        #endregion
    }
}