using EkipProjesi.Core.Kullanici;
using EkipProjesi.Core.KurumlarArasiSevkVeTakip;
using EkipProjesi.Core.Randevu;
using EkipProjesi.Data;
using EkipProjesi.Data.Repositories;
using EkipProjesi.Web.Filter;
using EkipProjesi.Web.Security;
using Simple.ImageResizer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Excel;
using EkipProjesi.Core;
using System.Data;

namespace EkipProjesi.Web.Controllers
{
    [SessionAttritube]
    [Authorize(Roles = "3")]
    public class BirimPersonelleriController : Controller
    {
        #region Const
        private KullaniciRepository _kullaniciRepo;
        private ArindirmaMerkezleriRepository _arindirmaRepo;
        private KurumlarArasiSevkVeTakipRepository _kastRepo;
        private ExcelRepository _excelRepo;
        string[] contentTypes = new string[] { "application/excel", "application/vnd.ms-excel", "application/x-excel", "application/x-msexcel", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" };
        string[] resimFormatlari = new string[] { ".jpg", ".png", ".jpeg" };
        string[] mimeFormatlari = new string[] { "image/jpeg", "image/pjpeg", "image/png", "image/jpeg", "image/pjpeg" };
        private readonly EKIPEntities _db;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public BirimPersonelleriController()
        {
            _kullaniciRepo = new KullaniciRepository();
            _arindirmaRepo = new ArindirmaMerkezleriRepository();
            _kastRepo = new KurumlarArasiSevkVeTakipRepository();
            _excelRepo = new ExcelRepository();
            _db = new EKIPEntities();
        }
        #endregion
        public ActionResult Index()
        {
            return View(_kullaniciRepo.Kullanicilar());
        }       
        public ActionResult PersonelEkle()
        {
            ViewBag.Roller = _kullaniciRepo.Roller();

            var kurum = _arindirmaRepo.KurumBilgileriGetir();
            kurum.Insert(0, new IKurumlar() { KurumID = 0, KurumAdi = "-Seçiniz-" });
            ViewBag.Kurumlar = new SelectList(kurum, "KurumID", "KurumAdi", "-Seçiniz-");

            return View();
        }
        [HttpPost]
        public JsonResult PersonelEkle(IKullanici model, HttpPostedFileBase personelFoto)
        {
            if (model != null)
            {
                if (personelFoto != null && personelFoto.ContentLength != 0 && personelFoto.ContentLength <= 1024 * 1024 * 5)

                {
                    if (mimeFormatlari.Any(item => personelFoto.ContentType.EndsWith(item, StringComparison.OrdinalIgnoreCase)) && resimFormatlari.Any(item => personelFoto.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase)))
                    {

                        try
                        {
                            MemoryStream target = new MemoryStream();
                            personelFoto.InputStream.CopyTo(target);
                            byte[] data = target.ToArray();

                            var imageResizer = new Simple.ImageResizer.ImageResizer(data);
                            byte[] newbyte = imageResizer.Resize(1000, ImageEncoding.Jpg100);
                            model.PersonelFotoArray = newbyte;
                            model.ContentType = personelFoto.ContentType;
                        }
                        catch
                        {

                        }
                    }
                }
                if (_kullaniciRepo.KullaniciEkle(model, (int)Session["UserID"])) 
                { 
                    return Json(new { success = true, mesaj = "Personel Kaydı Başarılı Bir Şekilde Oluşturuldu. Sizi Personel Detayına Yönelendireceğiz."});
                }
                else
                {
                    //return null;
                    return Json(new { success = false, mesaj = "İşlemleriniz Sırasında Hata Oluştu."});
                }
            }
            return Json(new { success = false, mesaj = "Zorunlu Alanlar Boş Geçilemez. Lütfen Zorunlu Alanları Doldurunuz!" });
        }
        public ActionResult PersonelDetay(int id)
        {
            IKullanici model = _kullaniciRepo.KullaniciDetay(id);
            ViewBag.IletisimBilgileri = _kullaniciRepo.KullaniciIletisimBilgisiDetay(id);
            //ViewBag.BirimBilgileri = _kullaniciRepo.KullaniciBirimBilgisiDetay(id);
            return View(model);
        }
        public ActionResult PersonelGuncelle(int id)
        {
            IKullanici model = _kullaniciRepo.KullaniciDetay(id);

            model.IletisimBilgisi = _kullaniciRepo.KullaniciIletisimBilgisiDetay(id);
            //model.BirimBilgisi = _kullaniciRepo.KullaniciBirimBilgisiDetay(id);

            ViewBag.Roller = _kullaniciRepo.Roller();

            var kurum = _arindirmaRepo.KurumBilgileriGetir();
            kurum.Insert(0, new IKurumlar() { KurumID = 0, KurumAdi = "-Seçiniz-" });
            ViewBag.Kurumlar = new SelectList(kurum, "KurumID", "KurumAdi", "-Seçiniz-");

            return View(model);
        }
        [HttpPost]
        public JsonResult PersonelGuncelle(IKullanici model, HttpPostedFileBase personelFoto)
        {
            string hata = "";
            if (model != null && model.ID > 0)
            {
                if (personelFoto != null && personelFoto.ContentLength != 0 && personelFoto.ContentLength <= 1024 * 1024 * 5)

                {
                    if (mimeFormatlari.Any(item => personelFoto.ContentType.EndsWith(item, StringComparison.OrdinalIgnoreCase)) && resimFormatlari.Any(item => personelFoto.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase)))
                    {

                        try
                        {
                            MemoryStream target = new MemoryStream();
                            personelFoto.InputStream.CopyTo(target);
                            byte[] data = target.ToArray();

                            var imageResizer = new Simple.ImageResizer.ImageResizer(data);
                            byte[] newbyte = imageResizer.Resize(1000, ImageEncoding.Jpg100);
                            model.PersonelFotoArray = newbyte;
                            model.ContentType = personelFoto.ContentType;
                        }
                        catch
                        {

                        }
                    }
                }

                if (_kullaniciRepo.PersonelDuzenle(model, out hata))
                {
                    return Json(new { success = true, mesaj = "Personel Bilgileri Güncellendi. Sizi Personel Detayına Yönlendiriyoruz." });
                }
            }
            else
            {
                return Json(new { success = false, mesaj = "İşlemleriniz Sırasında Hata Oluştu. Zorunlu Alanlar Boş Geçilemez ve ID 'null' Olamaz.", hata = hata, });
            }
            return Json(new { success = false, mesaj = "İşlemleriniz Sırasında Hata Oluştu. ", hata = hata });
        }
        [HttpPost]
        public ActionResult PersonelSil(int id)
        {
            if (id > 0)
            {
                if (_kullaniciRepo.PersonelSil(id))
                {
                    return RedirectToAction("Index", "BirimPersonelleri");
                }
            }
            return RedirectToAction("Index", "BirimPersonelleri");
        }
        public ActionResult IlceGetir(int IlID)
        {
            return Json(_kullaniciRepo.Ilceler(IlID).OrderBy(x => x.IlceID), JsonRequestBehavior.AllowGet);
        }
        public ActionResult MahalleGetir(int IlceID)
        {
            return Json(_kullaniciRepo.Mahalleler(IlceID).OrderBy(x => x.ID), JsonRequestBehavior.AllowGet);
        }
        public ActionResult HizmetMerkeziGetir(int KurumID)
        {
            return Json(_kullaniciRepo.HizmetMerkezleriGetir(KurumID).OrderBy(x => x.ID), JsonRequestBehavior.AllowGet);
        }
        public ActionResult ErisimYetkileri(int id)
        {
            IKullaniciErisimBilgileri model = _kullaniciRepo.KullaniciErisimDetay(id);
            ViewBag.KullaniciBilgisi = _kullaniciRepo.KullaniciDetay(id);
            ViewBag.ErisimKodlari = _kullaniciRepo.ErisimKodlari();
            ViewBag.Kontrol = _db.KullaniciErisimBilgileri.Where(x => x.KullaniciID == id).Select(x=>x.ErisimKoduID).ToList();
            return View(model);
        }
        [HttpPost]
        public JsonResult KullaniciYetkiKaydet(int id, int kullaniciId)
        {
            var status = false;

            if (_kullaniciRepo.KullaniciYetkiKaydet(id, kullaniciId))
            {
                status = true;
                return new JsonResult { Data = new { status = status } };
            }
            else
            {
                return new JsonResult { Data = new { status = status } };
            }
        }
        [HttpPost]
        public JsonResult KullaniciYetkiSil(int id, int kullaniciId)
        {
            var status = false;
            if (_kullaniciRepo.KullaniciYetkiSil(id, kullaniciId))
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
        [HttpPost]
        public JsonResult KullaniciPasifYap(bool status, int id)
        {
            if (_kullaniciRepo.KullaniciAktifPasif(status, id))
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }
        public ActionResult ExcelPersonelEkle()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ExcelPersonelEkle(HttpPostedFileBase f)
        {
            if (f == null && !contentTypes.Any(item => f.ContentType.Contains(item)) && f.ContentLength <= 0)
            {
                TempData["Hata"] = "Excel Dosyanız Okunamadı. Lütfen Dosya Formatınızı Kontrol Ediniz.";
                return RedirectToAction("ExcelPersonelEkle");
            }

            IExcelDataReader excelReader;

            List<string> list = new List<string>();
            Dictionary<int, string> hata = new Dictionary<int, string>();
            List<int> kayitHata = new List<int>();
            List<IKullanici> kullanicilar = new List<IKullanici>();
            IKullaniciRol rol = new IKullaniciRol();
            IExcel model = new IExcel();
            model.ExcelTurleri = ExcelTurleri.Personel;
            model.UserID = (int)Session["UserID"];
            //model.SessionID = Session["SessionID"].ToString();
            model.ExcelID = Guid.NewGuid().ToString();
            try
            {
                string s;

                s = Path.GetExtension(f.FileName);

                if (s == ".xls")
                {
                    excelReader = ExcelReaderFactory.CreateBinaryReader(f.InputStream);
                }
                else
                {
                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(f.InputStream);
                }
                excelReader.IsFirstRowAsColumnNames = true;
                DataSet result = excelReader.AsDataSet();
                DataTableCollection tables = result.Tables;

                string sayfaAdi = "Sayfa1";
                string mesaj;
                foreach (DataTable table in tables)
                {
                    if (sayfaAdi.ToLower().Contains(table.TableName.ToLower()))
                    {
                        for (int i = 1; i < table.Rows.Count; i++)
                        {
                            for (int j = 0; j < table.Columns.Count; j++)
                            {
                                list.Add(table.Rows[i].ItemArray[j].ToString());
                            }
                            if (list.All(x => string.IsNullOrEmpty(x)))
                            {

                            }
                            else
                            {
                                IKullanici kullanici;
                                if (_excelRepo.ExcelPersonelKontrol(list, (int)Session["UserID"], out kullanici, out rol, out mesaj))
                                {

                                    kullanici.KaydedenKullaniciID = (int)Session["UserID"];
                                    kullanici.ExcelID = model.ExcelID;
                                    kullanicilar.Add(kullanici);
                                    kullanici.RolID = rol.RolId;
                                }
                                else
                                {
                                    hata.Add(i + 2, mesaj);
                                }
                                list.Clear();
                            }
                        }
                    }
                }
                excelReader.Close();
                List<string> aynitc = new List<string>();
                if (hata.Count() == 0)
                {
                    aynitc = kullanicilar.GroupBy(x => x.TC).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
                    if (aynitc.Count() == 0)
                    {
                        kayitHata = _excelRepo.CokluPersonelKayit(kullanicilar, rol, (int)Session["UserID"]);
                        if (kayitHata.Count() == 0)
                        {
                            if (kullanicilar.Count() > 0)
                            {
                                model.ExcelDurumlari = ExcelDurumlari.Basarili;
                                model.Durum = true;
                                TempData["Tip"] = "Firma Excel Yükleme ( " + kullanicilar.Count().ToString() + " Satır Yüklendi)";
                            }
                            else
                            {
                                model.ExcelDurumlari = ExcelDurumlari.ExcelHatasi;
                                model.Durum = false;
                                TempData["Tip"] = "Personel Excel Yüklemede Kayıt Edilecek Satır Bulunamadı!";
                            }
                        }
                        else
                        {
                            model.ExcelDurumlari = ExcelDurumlari.KayitHatasi;
                            model.HataliSatirSayisi = kayitHata.Count();
                            model.Durum = false;
                            TempData["Tip"] = "Personel Excel Yükleme";
                        }
                    }
                    else
                    {
                        model.ExcelDurumlari = ExcelDurumlari.ExcelHatasi;

                        model.HataliSatirSayisi = kullanicilar.Where(x => aynitc.Contains(x.TC)).Count();
                        int sira = 0;
                        foreach (var x in kullanicilar)
                        {
                            if (aynitc.Contains(x.TC))
                            {
                                if (hata.ContainsKey(sira + 3))
                                {
                                    hata[sira + 3] += " Listede aynı TC Kimlik Numarasına ait kayıt bulunmaktadır.";
                                }
                                else
                                {
                                    hata.Add(sira + 3, "Listede aynı TC Kimlik Numarasına ait kayıt bulunmaktadır.");
                                }
                            }
                            sira++;
                        }
                        model.Durum = false;
                        TempData["Tip"] = "Personel Excel Yükleme";
                    }

                }
                else
                {
                    model.HataliSatirSayisi = hata.Count();
                    model.ExcelDurumlari = ExcelDurumlari.ExcelHatasi;
                    TempData["Tip"] = "Personel Excel Yükleme";
                }

                TempData["ExcelHata"] = hata;
                TempData["KayitHata"] = kayitHata;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                model.ExcelDurumlari = ExcelDurumlari.ExcelHatasi;
                model.Durum = false;
                TempData["Hata"] = "İşlem Sırasında Hata Oluştu. Lüften Doğru Exceli Yüklediğinize Emin Olun!";
            }
            model.SatirSayisi = kullanicilar.Count();
            _excelRepo.ExcelLog(model);
            return RedirectToAction("Index","BirimPersonelleri");
        }
        public FileResult PersonelExcel()
        {
            string url = Url.Content("~/Content/excel/personel.xls");
            string name = "Personel - " + string.Format("{0:dd-MM-yyyy - HH:mm:ss}", DateTime.Now) + ".xls";
            return File(url, "multipart/form-data", name);
        }
    }
}