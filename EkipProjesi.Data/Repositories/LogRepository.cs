using EkipProjesi.Core;
using EkipProjesi.Core.Kullanici;
using EkipProjesi.Core.LLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EkipProjesi.Data.Repositories
{
    public class LogRepository
    {
        private EKIPEntities _db;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public LogRepository()
        {
            _db = new EKIPEntities();
            _db.Configuration.LazyLoadingEnabled = false;
        }

        public bool LogKayit(ILogLogin model)
        {
            try
            {
                LogLoginIslemleri l = new LogLoginIslemleri();
                l.KullaniciId = model.KullaniciId;
                l.Tarih = DateTime.Now;
                l.IP = model.IP;
                l.IsletimSistemi = model.IsletimSistemi;
                l.TarayiciAdi = model.TarayiciAdi;
                l.LogTipi = model._LogTipi;
                l.MobilMi = model.MobilMi;
                l.SessionID = model.SessionID;
                l.MachineIP = model.MachineIP;
                l.MachineName = model.MachineName;
                _db.LogLoginIslemleri.Add(l);
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }

        public void SifreDegistirme(LogSifreDegistirmeModel model)
        {
            try
            {
                LogSifreDegistirme l = new LogSifreDegistirme();
                l.IP = model.IP;
                l.SessionID = model.SessionID;
                l.Tarih = DateTime.Now;
                l.UserID = model.UserID;

                _db.LogSifreDegistirme.Add(l);
                _db.SaveChanges();
            }
            catch
            {

            }
        }

        public bool HataBildirimi(HataBildirimModel model)
        {
            try
            {
                LogHataBildirimleri log = new LogHataBildirimleri();
                log.FileFormat = model.FileFormat;
                log.HTML = model.HTML;
                log.Konu = model.Konu;
                log.Mesaj = model.Mesaj;
                log.Tarih = DateTime.Now;
                log.Title = model.Title;
                log.Url = model.Url;
                log.UserID = model.UserID;
                log.ScreenShot = model.File;
                log.CozumDurumu = false;

                _db.LogHataBildirimleri.Add(log);
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }

        public List<HataBildirimModel> HataBildirimleriListe()
        {
            List<HataBildirimModel> model = new List<HataBildirimModel>();
            try
            {
                model = (from l in _db.LogHataBildirimleri
                         join k in _db.Kullanicilar on l.UserID equals k.ID
                         select new HataBildirimModel
                         {
                             KullaniciAdi = k.AdSoyad,
                             UserID = l.UserID,
                             Konu = l.Konu,
                             Mesaj = l.Mesaj,
                             Url = l.Url,
                             Tarih = l.Tarih,
                             Title = l.Title,
                             CozumDurumu = l.CozumDurumu,
                             CozumMesaji = l.CozumMesajı,
                         }).ToList();
            }
            catch
            {

            }
            return model;
        }

        public HataBildirimModel HataBildirimleriDetay(long id)
        {
            HataBildirimModel model = new HataBildirimModel();
            try
            {
                model = (from l in _db.LogHataBildirimleri
                         join k in _db.Kullanicilar on l.UserID equals k.ID
                         where l.ID == id
                         select new HataBildirimModel
                         {
                             KullaniciAdi = k.AdSoyad,
                             UserID = l.UserID,
                             Konu = l.Konu,
                             Mesaj = l.Mesaj,
                             Url = l.Url,
                             HTML = l.HTML,
                             Tarih = l.Tarih,
                             Title = l.Title,
                             CozumDurumu = l.CozumDurumu,
                             CozumMesaji = l.CozumMesajı,
                             File = l.ScreenShot,
                             FileFormat = l.FileFormat
                         }).FirstOrDefault();
            }
            catch
            {

            }
            return model;
        }

        public void LogCikis(int id, string sessionID, int sessionCount, string machineName, string MachineIP)
        {
            try
            {
                LogCikis l = new LogCikis();
                l.SessionID = sessionID;
                l.UserID = id;
                l.Tarih = DateTime.Now;
                l.SessionCount = sessionCount;
                l.MachineName = machineName;
                l.MachineIP = MachineIP;
                _db.LogCikis.Add(l);
                _db.SaveChanges();
            }
            catch
            {

            }
        }

        public void LogCikisEmail(string id, string sessionID)
        {
            try
            {
                LogCikis l = new LogCikis();
                l.SessionID = sessionID;
                l.UserID = _db.Kullanicilar.FirstOrDefault(x => x.KullaniciAdi == id).ID;
                l.Tarih = DateTime.Now;

                _db.LogCikis.Add(l);
                _db.SaveChanges();
            }
            catch
            {

            }
        }

        public List<ILogLogin> BugunGirisYapanlar(DateTime gun)
        {
            List<ILogLogin> model = new List<ILogLogin>();
            try
            {
                model = (from l in _db.LogLoginIslemleri
                         join k in _db.Kullanicilar on l.KullaniciId equals k.ID
                         where DbFunctions.TruncateTime(l.Tarih) == DbFunctions.TruncateTime(gun) && l.LogTipi == (int)LogTipi.Giris
                         select new ILogLogin
                         {
                             Tarih = l.Tarih,
                             SessionID = l.SessionID,
                             TarayiciAdi = l.TarayiciAdi,
                             MobilMi = l.MobilMi,
                             IsletimSistemi = l.IsletimSistemi,
                             KullaniciAdi = k.AdSoyad,
                             MachineIP = l.MachineIP,
                             MachineName = l.MachineName,
                             LogOutMachineIP = l.MachineIP,
                             LogOutMachineName = l.MachineName,
                             IP = l.IP
                         }).OrderByDescending(x => x.Tarih).ToList();
            }
            catch
            {

            }
            return model;
        }

        public List<ILogLogin> BugunCikisYapanlar(DateTime gun)
        {
            List<ILogLogin> model = new List<ILogLogin>();
            try
            {
                model = (from c in _db.LogCikis
                         join l in _db.LogLoginIslemleri on c.SessionID equals l.SessionID
                         join k in _db.Kullanicilar on c.UserID equals k.ID
                         where DbFunctions.TruncateTime(c.Tarih) == DbFunctions.TruncateTime(gun) && l.LogTipi == (int)LogTipi.Cikis
                         orderby c.Tarih descending
                         select new ILogLogin
                         {
                             CikisTarihi = c.Tarih,
                             SessionCount = c.SessionCount,
                             SessionID = c.SessionID,
                             Tarih = l.Tarih,
                             TarayiciAdi = l.TarayiciAdi,
                             MobilMi = l.MobilMi,
                             IsletimSistemi = l.IsletimSistemi,
                             KullaniciAdi = k.AdSoyad,
                             MachineIP = l.MachineIP,
                             MachineName = l.MachineName,
                             LogOutMachineIP = c.MachineIP,
                             LogOutMachineName = c.MachineName,
                             IP = l.IP
                         }).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return model;
        }

        public List<ILog> HataLogList(DateTime gun)
        {
            List<ILog> model = new List<ILog>();

            try
            {
                model = (from l in _db.Log
                         join k in _db.Kullanicilar on l.UserId equals k.ID
                         where DbFunctions.TruncateTime(l.Date) == DbFunctions.TruncateTime(gun) && l.Status == true
                         orderby l.Date
                         select new ILog
                         {
                             Application = l.Application,
                             Date = l.Date,
                             Exception = l.Exception,
                             Id = l.Id,
                             JsError = l.JsError,
                             Levels = l.Levels,
                             Logger = l.Logger,
                             Message = l.Message,
                             Url = l.Url,
                             UrlRefferer = l.UrlRefferer,
                             UserName = k.Ad,
                             UserId = l.UserId,
                             Status = l.Status,                            
                         }).ToList();
            }
            catch
            {

            }
            return model;
        }

        public bool LogStatusChange(int id)
        {
            try
            {
                Log log = _db.Log.FirstOrDefault(x => x.Id == id);
                log.Status = false;
                _db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}