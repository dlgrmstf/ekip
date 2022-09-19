using EkipProjesi.Web.Models;
using EkipProjesi.Core;
using EkipProjesi.Core.Randevu;
using EkipProjesi.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EkipProjesi.Data;
using Newtonsoft.Json;
using System.IO;
using EkipProjesi.Core.ArindirmaModulu;
using EkipProjesi.Core.Hastalar;
using EkipProjesi.Web.Filter;

namespace EkipProjesi.Web.Controllers
{
    [SessionAttritube]
    [Authorize(Roles = "2")]
    public class ArindirmaMerkezleriController : Controller
    {
        #region Const
        ArindirmaHizmetBilgileriFiltreModel _hizmetfiltreModel;
        ArindirmaMerkezleriRepository _arindirmaRepo;
        KurumlarArasiSevkVeTakipRepository _kastRepo;
        RandevuAyarlariModel _randevuModel;
        RandevularModel _randevularModel;
        VakaTakipRepository _vakaRepo;
        private readonly EKIPEntities _db;

        public ArindirmaMerkezleriController()
        {
            _hizmetfiltreModel = new ArindirmaHizmetBilgileriFiltreModel();
            _arindirmaRepo = new ArindirmaMerkezleriRepository();
            _kastRepo = new KurumlarArasiSevkVeTakipRepository();
            _randevuModel = new RandevuAyarlariModel();
            _randevularModel = new RandevularModel();
            _vakaRepo = new VakaTakipRepository();
            _db = new EKIPEntities();
        }
        #endregion
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Randevu()
        {
            return View();
        }
        public ActionResult VerilenRandevular()
        {
            return View(_arindirmaRepo.VerilenRandevulariGetir());
        }

        #region Hizmet Bilgileri
        public ActionResult HizmetBilgileri()
        {
            return View();
        }
        public ActionResult PartialFiltreSonuc(IArindirmaHizmetBilgileriFiltreModel model)
        {
            IArindirmaHizmetBilgileriFiltreModel data = new IArindirmaHizmetBilgileriFiltreModel();

            if (model != null)
            {
                data.HizmetBilgileri = _arindirmaRepo.HizmetBilgileriGetir(model);
            }

            return PartialView(data);
        }
        #endregion

        #region Personel Bilgileri
        public ActionResult PersonelBilgileri()
        {
            return View();
        }
        public ActionResult PartialFiltreSonucPersonel(IArindirmaPersonelBilgileriFiltreModel model)
        {
            IArindirmaPersonelBilgileriFiltreModel data = new IArindirmaPersonelBilgileriFiltreModel();

            if (model != null)
            {
                data.PersonelBilgileri = _arindirmaRepo.PersonelBilgileriGetir(model);
            }

            return PartialView(data);
        }
        #endregion

        #region Başvuru Bilgileri
        public ActionResult BasvuruBilgileri()
        {
            ViewBag.KendiHastalariBasvuru = _arindirmaRepo.KendiHastalariBasvuru((int)Session["UserID"]);
            ViewBag.KurumdakiHastalarBasvuru = _arindirmaRepo.KurumdakiHastalarBasvuru((int)Session["UserID"]);
            ViewBag.TumBasvurular = _arindirmaRepo.BasvuruBilgileriGetir();
            return View();
        }
        public ActionResult BasvuruDetay(int id)
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
                    if (_vakaRepo.KendiHastasiMi((int)Session["UserID"],model.HastaEkipNo,model.HastaTC).Count()>0)
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
        [HttpPost]
        public ActionResult PolGetir(int kurumid)
        {
            var model = _arindirmaRepo.PoliklinikBilgileriGetir(kurumid);
            return Json(model);
        }
        [HttpPost]
        public ActionResult HastaBilgiGetir(int hastaid)
        {
            var model = _arindirmaRepo.HastaBilgiGetir(hastaid);
            return Json(model);
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
        public JsonResult RandevulariGetir(int? kurumid, int? polid)
        {
            var events = _arindirmaRepo.RandevuBilgileriGetir(kurumid, polid);
            return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public JsonResult RandevuKaydet(RandevuBilgileri model)
        {
            var status = false;
            int UserID = (int)Session["UserID"];

            if (model.ID > 0)
            {
                if (_arindirmaRepo.RandevuDuzenle(model, UserID))
                {
                    status = true;
                    return new JsonResult { Data = new { status = status } };
                }
            }
            else
            {
                if (_arindirmaRepo.RandevuKaydet(model, UserID))
                {
                    status = true;
                    return new JsonResult { Data = new { status = status } };
                }
            }

            return new JsonResult { Data = new { status = status } };
        }
        [HttpPost]
        public JsonResult RandevuSil(int eventID, string iptalNedeni)
        {
            var status = false;
            var model = _arindirmaRepo.SeciliRandevuyuGetir(eventID);
            if (!string.IsNullOrEmpty(iptalNedeni))
            {
                if (_arindirmaRepo.RandevuSil(eventID, iptalNedeni, model))
                {
                    status = true;
                    return new JsonResult { Data = new { status = status } };
                }
                else
                {
                    status = false;
                    return new JsonResult { Data = new { status = status } };
                }
            }
            else
            {
                return new JsonResult { Data = new { status = status } };
            }
        }
        public JsonResult RandevuAyarGetir(DateTime gun, int kid, int pid)
        {
            var data = _arindirmaRepo.RandevuAyarGetir(gun, kid, pid);
            return Json(data);
        }
        public JsonResult RandevuSaatiKontrol(int kid, int pid, string gunbilgisi)
        {
            var data = _arindirmaRepo.RandevuSaatiKontrol(gunbilgisi, kid, pid);
            return new JsonResult { Data = data };
        }
        public JsonResult PasiflikKontrolu(int kid, int pid)
        {
            var data = _arindirmaRepo.PasiflikKontrolu(kid, pid);
            return Json(data);
        }
        public JsonResult RandevuAyariEksikMiKontrolu(int kid, int pid)
        {
            var status = false;
            if (_arindirmaRepo.RandevuAyariEksikMiKontrolu(kid, pid))
            {
                status = true;
                return new JsonResult { Data = status };
            }
            else
            {
                return new JsonResult { Data = status };
            }
        }
        #endregion

        #region Randevu Deneme
        public ActionResult Deneme()
        {
            return View();
        }
        public JsonResult GetEvents()
        {
            using (EKIPEntities dc = new EKIPEntities())
            {
                var events = dc.Events.ToList();
                return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
        [HttpPost]
        public JsonResult SaveEvent(Events e)
        {
            var status = false;
            using (EKIPEntities dc = new EKIPEntities())
            {
                if (e.EventID > 0)
                {
                    //Update the event
                    var v = dc.Events.Where(a => a.EventID == e.EventID).FirstOrDefault();
                    if (v != null)
                    {
                        v.Subject = e.Subject;
                        v.Start = e.Start;
                        v.End = e.End;
                        v.Description = e.Description;
                        v.IsFullDay = e.IsFullDay;
                        v.ThemeColor = e.ThemeColor;
                    }
                }
                else
                {
                    dc.Events.Add(e);
                }
                dc.SaveChanges();
                status = true;
            }
            return new JsonResult { Data = new { status = status } };
        }
        [HttpPost]
        public JsonResult DeleteEvent(int eventID)
        {
            var status = false;
            using (EKIPEntities dc = new EKIPEntities())
            {
                var v = dc.Events.Where(a => a.EventID == eventID).FirstOrDefault();
                if (v != null)
                {
                    dc.Events.Remove(v);
                    dc.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }
        #endregion

        #region Randevu Ayarları
        public ActionResult RandevuAyarlari()
        {
            _randevuModel.RandevuAyarlariList = _arindirmaRepo.RandevuAyarlariList();
            return View(_randevuModel);
        }
        public ActionResult RandevuAyariEkle()
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
            IRandevuAyarlari r = new IRandevuAyarlari();
            ViewBag.KurumlarListesi = _arindirmaRepo.KurumBilgileriGetir();
            return View(r);
        }
        [HttpPost]
        public JsonResult AyarEkle(int kid, int pol, int gun, int slot, TimeSpan bassaati, TimeSpan bitsaati)
        {
            int id;
            ViewBag.KurumlarListesi = _arindirmaRepo.KurumBilgileriGetir();
            if (_arindirmaRepo.RandevuAyariKayit(kid, pol, gun, slot, bassaati, bitsaati, out id, (int)Session["UserID"]))
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            else
            {
                ViewBag.Msg = "İşleminiz Sırasında Hata Oluşmuştur";
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult RandevuAyariDetay(int id)
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
            ViewBag.KurumlarListesi = _arindirmaRepo.KurumBilgileriGetir();
            ViewBag.PolListesi = _arindirmaRepo.PoliklinikBilgisiGetir();
            IRandevuAyarlari r = new IRandevuAyarlari();
            r = _arindirmaRepo.RandevuAyariDetay(id);
            if (r.PasifBaslangicTarihi != null && r.PasifBitisTarihi != null)
            {
                ViewBag.PasifBasTarihi = r.PasifBaslangicTarihi;
                ViewBag.PasifBitTarihi = r.PasifBitisTarihi;
            }
            return View(r);
        }
        [HttpPost]
        public ActionResult RandevuAyariDetay(IRandevuAyarlari model)
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
            ViewBag.KurumlarListesi = _arindirmaRepo.KurumBilgileriGetir();
            ViewBag.PolListesi = _arindirmaRepo.PoliklinikBilgisiGetir();
            if (_arindirmaRepo.RandevuAyarDuzenleme(model))
            {
                return RedirectToAction("RandevuAyariDetay", "ArindirmaMerkezleri", new { id = model.ID });
            }
            else
            {
                TempData["Error"] = "Hata oluştu. Lütfen tekrar deneyiniz.";
                return RedirectToAction("RandevuAyariDetay", "ArindirmaMerkezleri", new { id = model.ID });
            }
        }
        public JsonResult RandevuAyariKontrol(int kurum, int pol, int gun)
        {
            var status = false;
            var kontrol = _db.RandevuAyarlari.Where(x => x.KurumID == kurum && x.PoliklinikTuruID == pol && x.Gun == gun).Any();

            if (kontrol)
            {
                return new JsonResult { Data = status };
            }
            else
            {
                status = true;
                return new JsonResult { Data = status };
            }
        }
        #endregion

    }
}