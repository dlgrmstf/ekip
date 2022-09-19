using EkipProjesi.Core;
using EkipProjesi.Core.Kullanici;
using EkipProjesi.Core.KurumlarArasiSevkVeTakip;
using EkipProjesi.Core.LLog;
using EkipProjesi.Core.Personel;
using EkipProjesi.Core.Randevu;
using EkipProjesi.Cryptography.Cryptography;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace EkipProjesi.Data.Repositories
{
    public class KullaniciRepository
    {
        #region Const
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private EKIPEntities _db;
        private LogRepository _log;

        public KullaniciRepository()
        {
            _db = new EKIPEntities();
            _log = new LogRepository();
            _db.Configuration.LazyLoadingEnabled = false;
        }
        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();
        public static int RandomNumber(int min, int max)
        {
            lock (syncLock)
            { // synchronize
                return random.Next(min, max);
            }
        }
        public static string GenerateCode()
        {
            Random rand = new Random();
            string karakter = "abcdefghijklmnprstuvyz1234567890";
            string CareCode = string.Empty;
            for (int i = 0; i < 6; i++)
            {
                CareCode += karakter[RandomNumber(0, karakter.Length)];
            }
            return CareCode;
        }
        #endregion

        #region İL-İLÇE-MAHALLE
        public List<IIl> Iller()
        {
            List<IIl> model = new List<IIl>();
            try
            {
                model = (from i in _db.Il
                         orderby i.ILAdi
                         select new IIl
                         {
                             IlID = i.KOD.Value,
                             IlAdi = i.ILAdi,
                         }).ToList();
                return model;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return model;
            }
        }
        public List<IIlce> Ilceler(int IlId)
        {
            List<IIlce> model = new List<IIlce>();
            try
            {
                model = (from ilce in _db.Ilce
                         orderby ilce.ILCEAdi
                         where ilce.ILKOD == IlId
                         select new IIlce
                         {
                             IlceID = ilce.ID,
                             IlceAdi = ilce.ILCEAdi
                         }).ToList();
                return model;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return model;
            }
        }
        public List<IMahalle> Mahalleler(int IlceID)
        {
            List<IMahalle> model = new List<IMahalle>();
            try
            {
                model = (from m in _db.Mahalle
                         orderby m.ID
                         where m.IlceID == IlceID
                         select new IMahalle
                         {
                             ID = m.ID,
                             IlceID = (int)m.IlceID,
                             MahalleAdi = m.MahalleAdi
                         }).ToList();
                return model;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return model;
            }
        }
        #endregion

        #region KULLANICI
        public IKullanici Kullanicilar()
        {
            IKullanici model = new IKullanici();

            try
            {
                model.RolList = Roller();

                model.Kullanicilar = (from b in _db.Kullanicilar
                                     select new IKullanici
                                     {
                                         ID = b.ID,
                                         KullaniciAdi = b.KullaniciAdi,
                                         Sifre = b.Sifre,
                                         KullaniciDurumu = b.KullaniciDurumu,
                                         Ad = b.Ad,
                                         Soyad = b.Soyad,
                                         TC = b.TC,
                                         KurumEposta = b.KurumEposta,
                                         Telefon = _db.KullaniciIletisimBilgileri.Where(x => x.KullaniciID == b.ID).Select(x => x.Telefon).FirstOrDefault(),
                                         KurumTelefonu = _db.KullaniciIletisimBilgileri.Where(x => x.KullaniciID == b.ID).Select(x => x.KurumTelefonu).FirstOrDefault(),
                                         Roller = (from kr in _db.KullaniciRolleri
                                                   join r in _db.Roller on kr.RolId equals r.ID
                                                   where kr.KullaniciId == b.ID
                                                   select new IKullaniciRol
                                                   {
                                                       ID = kr.ID,
                                                       RolAdi = r.Rol,
                                                       RolId = kr.RolId
                                                   }).ToList(),
                                     }).OrderBy(x=>x.KullaniciDurumu).ToList();

                return model;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public IKullanici KullaniciDetay(int id)
        {
            try
            {
                IKullanici model = new IKullanici();
                model = (from b in _db.Kullanicilar
                         where b.ID == id
                         select new IKullanici
                         {
                             ID = b.ID,
                             KullaniciAdi = b.KullaniciAdi,
                             Sifre = b.Sifre,
                             KullaniciDurumu = b.KullaniciDurumu,
                             Ad = b.Ad,
                             Soyad = b.Soyad,
                             TC = b.TC,
                             KurumID = b.KurumID,
                             HizmetMerkeziID = b.HizmetMerkeziID,
                             DogumTarihi = b.DogumTarihi,
                             Telefon = b.Telefon,
                             KurumTelefonu = b.KurumTelefonu,
                             KurumEposta = b.KurumEposta,
                             Meslek = b.Meslek,
                             Cinsiyet = b.Cinsiyet,
                             KurumAdi = _db.Kurumlar.Where(x => x.KurumID == b.KurumID).Select(x => x.KurumAdi).FirstOrDefault(),
                             HizmetMerkeziAdi = _db.KurumHizmetMerkezleri.Where(x => x.ID == b.HizmetMerkeziID).Select(x => x.Ad).FirstOrDefault(),
                             FotoByte = _db.PersonelFotograflar.Where(x=>x.PersonelID == id).Select(x=>x.FileByte).FirstOrDefault(),
                             FotoContentType = _db.PersonelFotograflar.Where(x=>x.PersonelID == id).Select(x=>x.ContentType).FirstOrDefault(),
                             Roller = (from kr in _db.KullaniciRolleri
                                       join r in _db.Roller on kr.RolId equals r.ID
                                       where kr.KullaniciId == b.ID
                                       select new IKullaniciRol
                                       {
                                           ID = kr.ID,
                                           RolAdi = r.Rol,
                                           RolId = kr.RolId
                                       }).ToList(),
                         }).FirstOrDefault();

                model.RolList = Roller();

                return model;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public IKullaniciIletisimBilgileri KullaniciIletisimBilgisiDetay(int id)
        {
            try
            {
                IKullaniciIletisimBilgileri model = new IKullaniciIletisimBilgileri();
                model = (from b in _db.KullaniciIletisimBilgileri
                         where b.KullaniciID == id
                         select new IKullaniciIletisimBilgileri
                         {
                             ID = b.ID,
                             KullaniciID = b.KullaniciID,
                             Telefon = b.Telefon,
                             KurumTelefonu = b.KurumTelefonu

                         }).FirstOrDefault();

                return model;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public IKullaniciBirimBilgileri KullaniciBirimBilgisiDetay(int id)
        {
            try
            {
                IKullaniciBirimBilgileri model = new IKullaniciBirimBilgileri();
                model = (from b in _db.KullaniciBirimBilgileri
                         where b.KullaniciID == id
                         select new IKullaniciBirimBilgileri
                         {
                             ID = b.ID,
                             KullaniciID = b.KullaniciID,
                             Kurum = b.Kurum,
                             Bina = b.Bina,
                             Bolge = b.Bolge

                         }).FirstOrDefault();

                return model;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public List<IKurumHizmetMerkezleri> HizmetMerkezleriGetir(int KurumID)
        {
            List<IKurumHizmetMerkezleri> merkezler = new List<IKurumHizmetMerkezleri>();
            try
            {
                merkezler = (from k in _db.KurumHizmetMerkezleri
                             where k.KurumID == KurumID
                             select new IKurumHizmetMerkezleri
                             {
                                 ID = k.ID,
                                 KurumID = k.KurumID,
                                 Ad = k.Ad,
                                 Adres = k.Adres,
                                 Telefon = k.Telefon
                             }).ToList();

                return merkezler;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public IKullaniciErisimBilgileri KullaniciErisimDetay(int id)
        {
            try
            {
                IKullaniciErisimBilgileri model = new IKullaniciErisimBilgileri();
                model.ErisimBilgileriList = (from b in _db.KullaniciErisimBilgileri
                         where b.KullaniciID == id
                         select new IKullaniciErisimBilgileri
                         {
                             ID = b.ID,
                             KullaniciID = b.KullaniciID,
                             ErisimKoduID = b.ErisimKoduID

                         }).ToList();

                return model;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public List<IErisimKodlari> ErisimKodlari()
        {
            try
            {
                List<IErisimKodlari> model = new List<IErisimKodlari>();
                model = (from b in _db.ErisimKodlari
                         orderby b.ErisimKodu ascending
                         select new IErisimKodlari
                         {
                             ID = b.ID,
                             ErisimKodu = b.ErisimKodu

                         }).ToList();

                return model;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public bool KullaniciYetkiKaydet(int id, int kullaniciId)
        {
            try
            {

                KullaniciErisimBilgileri k = new KullaniciErisimBilgileri();

                k.ErisimKoduID = id;
                k.KullaniciID = kullaniciId;

                _db.KullaniciErisimBilgileri.Add(k);
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }
        public bool KullaniciYetkiSil(int id, int kullaniciId)
        {
            try
            {              
                KullaniciErisimBilgileri k = _db.KullaniciErisimBilgileri.Where(x=>x.ErisimKoduID == id && x.KullaniciID == kullaniciId).FirstOrDefault();

                _db.KullaniciErisimBilgileri.Remove(k);
                _db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }
        public bool KullaniciEkle(IKullanici model, int UserID)
        {
            try
            {
                using (var trans = _db.Database.BeginTransaction())
                {
                    #region Personel Bilgisi
                    Kullanicilar k = new Kullanicilar();
                    if (model.PersonelId != null)
                    {
                        k.AdSoyad = _db.Personeller.Where(x => x.ID == model.PersonelId).FirstOrDefault().Ad + _db.Personeller.Where(x => x.ID == model.PersonelId).FirstOrDefault().Soyad;
                        k.FotoByte = model.FotoByte;
                        k.FotoContentType = model.FotoContentType;
                        k.KullaniciDurumu = true;
                        k.SifreKontrol = false;
                        k.KurumID = model.KurumID;
                        k.HizmetMerkeziID = model.HizmetMerkeziID;
                        if (k.TC != null && k.Ad != null && k.Soyad != null)
                        {
                            string val1 = k.TC.Trim().Substring(0, 5);
                            string val2 = string.Concat(k.Ad.Trim().Substring(0, 1), k.Soyad.Trim().Substring(0,1));
                            k.Sifre = CryptoUtilities.Encrypt(string.Concat(val1,val2));
                        }
                        else
                        {
                            k.Sifre = CryptoUtilities.Encrypt(GenerateCode());
                        }
                    }
                    else
                    {
                        k.AdSoyad = model.Ad;
                        k.Ad = model.Ad;
                        k.Soyad = model.Soyad;
                        k.TC = model.TC;
                        k.KurumID = model.KurumID;
                        k.HizmetMerkeziID = model.HizmetMerkeziID;
                        k.DogumTarihi = model.DogumTarihi;
                        k.Cinsiyet = model.Cinsiyet;
                        k.Telefon = model.Telefon;
                        k.KurumTelefonu = model.KurumTelefonu;
                        k.KurumEposta = model.KurumEposta;
                        k.Meslek = model.Meslek;
                        k.KullaniciAdi = model.TC.Trim();
                        k.KullaniciDurumu = true;
                        if (k.TC != null && k.Ad != null && k.Soyad != null)
                        {
                            string val1 = k.TC.Trim().Substring(0, 5);
                            string val2 = string.Concat(k.Ad.Trim().Substring(0, 1), k.Soyad.Trim().Substring(0, 1));
                            k.Sifre = CryptoUtilities.Encrypt(string.Concat(val1, val2));
                        }
                        else
                        {
                            k.Sifre = CryptoUtilities.Encrypt(GenerateCode());
                        }
                        k.KaydedenKullaniciID = UserID;
                    }

                    _db.Kullanicilar.Add(k);
                    _db.SaveChanges();

                    k.PersonelId = k.ID;
                    model.PersonelId = k.ID;
                    #endregion

                    if (k.ID > 0)
                    {
                        #region İletişim Bilgileri
                        KullaniciIletisimBilgileri ib = new KullaniciIletisimBilgileri();
                        ib.KullaniciID = k.ID;
                        ib.Telefon = model.Telefon;
                        ib.KurumTelefonu = model.KurumTelefonu;

                        _db.KullaniciIletisimBilgileri.Add(ib);
                        _db.SaveChanges();
                        #endregion

                        //#region Birim Bilgileri
                        //KullaniciBirimBilgileri br = new KullaniciBirimBilgileri();
                        //br.KullaniciID = k.ID;
                        //br.Bina = model.BirimBilgisi.Bina;
                        //br.Bolge = model.BirimBilgisi.Bolge;
                        //br.Kurum = model.BirimBilgisi.Kurum;

                        //_db.KullaniciBirimBilgileri.Add(br);
                        //_db.SaveChanges();
                        //#endregion

                        #region Kullanıcı Rolleri
                        if (model.Roller2 != null)
                        {
                            foreach (var rol in model.Roller2)
                            {
                                KullaniciRolleri kr = new KullaniciRolleri();
                                kr.KullaniciId = k.ID;
                                kr.RolId = rol;

                                _db.KullaniciRolleri.Add(kr);
                                _db.SaveChanges();
                            }
                        }
                        #endregion

                        #region Kullanıcı Fotoğraf

                        if (model.PersonelFotoArray != null)
                        {
                            PersonelFotograflar pf = new PersonelFotograflar();
                            pf.ContentType = model.ContentType;
                            pf.FileByte = model.PersonelFotoArray;
                            pf.PersonelID = k.ID;
                            pf.Status = true;

                            _db.PersonelFotograflar.Add(pf);
                            _db.SaveChanges();
                        }

                        #endregion
                    }

                    trans.Commit();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }
        public bool KullaniciAktifPasif(bool status, int id)
        {
            Kullanicilar k = _db.Kullanicilar.Where(x => x.ID == id).FirstOrDefault();
            k.KullaniciDurumu = status;
            _db.SaveChanges();

            try
            {
                ILogLogin model = new ILogLogin();
                model.KullaniciId = id;
                if(k.KullaniciDurumu == false)
                {
                    model._LogTipi = (int)EkipProjesi.Core.LogTipi.PasifKullanici;
                }
                else
                {
                    model._LogTipi = (int)EkipProjesi.Core.LogTipi.AktifKullanici;
                }
                _log.LogKayit(model);

                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool KullaniciRolEkle(IKullaniciRol model)
        {
            try
            {
                foreach (var item in model.Roller)
                {
                    KullaniciRolleri kr = new KullaniciRolleri();
                    kr.KullaniciId = model.KullaniciId;
                    kr.RolId = item;

                    _db.KullaniciRolleri.Add(kr);
                    _db.SaveChanges();
                }
                return true;

            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }
        public List<IKullaniciRol> KullaniciRolYetki(int kullaniciID)
        {
            List<IKullaniciRol> model = new List<IKullaniciRol>();
            try
            {
                model = (from kr in _db.KullaniciRolleri
                         join r in _db.Roller on kr.RolId equals r.ID
                         where kr.KullaniciId == kullaniciID
                         select new IKullaniciRol
                         {
                             ID = kr.ID,
                             RolId = kr.RolId,
                             RolAdi = r.Rol,
                             Yetkiler = (from ry in _db.RolYetkileri
                                         join y in _db.Yetkiler on ry.YetkiId equals y.ID
                                         where ry.RolId == kr.RolId
                                         select new IRolYetkileri
                                         {
                                             ID = ry.ID,
                                             RolId = ry.RolId,
                                             YetkiId = ry.YetkiId,
                                             YetkiAdi = y.YetkiAdi,
                                             Goruntuleme = ry.Goruntuleme,
                                             Duzenleme = ry.Duzenleme,
                                             Ekleme = ry.Ekleme,
                                             Silme = ry.Silme,
                                         }).ToList(),
                         }).ToList();
                return model;

            }
            catch (Exception ex)
            {
                log.Error(ex);
                return model;
            }
        }
        public bool SifreDegistir(IKullanici model, out string mesaj)
        {
            mesaj = "";
            try
            {
                string sifre = CryptoUtilities.Encrypt(model.EskiSifre);

                if (_db.Kullanicilar.Any(x => x.Sifre == sifre && x.ID == model.ID))
                {
                    Kullanicilar k = _db.Kullanicilar.FirstOrDefault(x => x.Sifre == sifre && x.ID == model.ID);
                    k.Sifre = CryptoUtilities.Encrypt(model.Sifre);
                    k.SifreKontrol = true;
                    _db.SaveChanges();
                    _log.SifreDegistirme(model.LogSifreDegistirme);
                }
                else
                {
                    mesaj = "Eski Şifreniz Uyuşmamaktadır.";
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }
        public bool SifreYenile(IKullanici model)
        {
            try
            {
                if (_db.Kullanicilar.Any(x => x.KullaniciAdi == model.KullaniciAdi))
                {
                    Kullanicilar k = _db.Kullanicilar.FirstOrDefault(x=>x.KullaniciAdi == model.KullaniciAdi);
                    k.Sifre = CryptoUtilities.Encrypt(model.Sifre);
                    k.SifreKontrol = true;
                    _db.SaveChanges();
                }
                else
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }
        public bool KullaniciFotoGuncelle(IKullanici model)
        {
            try
            {
                Kullanicilar k = _db.Kullanicilar.FirstOrDefault(x => x.ID == model.ID);
                k.FotoContentType = model.FotoContentType;
                k.FotoByte = model.FotoByte;
                _db.SaveChanges();

                if (k.PersonelId > 0)
                {
                    PersonelFotograflar p = new PersonelFotograflar();
                    p.PersonelID = k.PersonelId.GetValueOrDefault();
                    p.Status = true;
                    p.ContentType = k.FotoContentType;
                    p.FileByte = k.FotoByte;
                    _db.PersonelFotograflar.Add(p);
                    _db.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }
        public bool SifreSifirla(string email, out string mesaj, out IKullanici kullanici)
        {
            mesaj = "";
            kullanici = new IKullanici();
            try
            {
                if (_db.Kullanicilar.Any(x => x.KullaniciAdi.Trim().ToLower() == email))
                {
                    Kullanicilar k = _db.Kullanicilar.FirstOrDefault(x => x.KullaniciAdi.Trim().ToLower() == email);
                    string sifre = GenerateCode();
                    k.Sifre = CryptoUtilities.Encrypt(sifre);
                    k.SifreKontrol = false;
                    _db.SaveChanges();
                    kullanici.KullaniciAdi = k.KullaniciAdi;
                    kullanici.Sifre = sifre;
                    kullanici.Ad = k.AdSoyad;
                    kullanici.ID = k.ID;
                }
                else
                {
                    mesaj = "Kullanıcı Bulunamadı!";
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }
        public bool PersonelSil(int personelID)
        {
            try
            {
                using (var trans = _db.Database.BeginTransaction())
                {
                    try
                    {
                        KullaniciIletisimBilgileri iletisim = _db.KullaniciIletisimBilgileri.FirstOrDefault(x => x.KullaniciID == personelID);
                        _db.KullaniciIletisimBilgileri.Remove(iletisim);
                        _db.SaveChanges();

                        //KullaniciBirimBilgileri birim = _db.KullaniciBirimBilgileri.FirstOrDefault(x => x.KullaniciID == personelID);
                        //_db.KullaniciBirimBilgileri.Remove(birim);
                        //_db.SaveChanges();

                        List<KullaniciRolleri> roller = _db.KullaniciRolleri.Where(x => x.KullaniciId == personelID).ToList();
                        if (roller != null && roller.Count() > 0)
                        {
                            foreach (var y in roller)
                            {
                                KullaniciRolleri rol = _db.KullaniciRolleri.FirstOrDefault(x => x.KullaniciId == y.KullaniciId);

                                _db.KullaniciRolleri.Remove(rol);
                                _db.SaveChanges();
                            }
                        }

                        PersonelFotograflar foto = _db.PersonelFotograflar.FirstOrDefault(x => x.PersonelID == personelID);
                        if(foto != null)
                        {
                            _db.PersonelFotograflar.Remove(foto);
                            _db.SaveChanges();
                        }

                        Kullanicilar model = _db.Kullanicilar.FirstOrDefault(x => x.ID == personelID);
                        _db.Kullanicilar.Remove(model);
                        _db.SaveChanges();

                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        log.Error(ex);
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }
        public bool PersonelDuzenle(IKullanici model, out string hata)
        {
            hata = "";
            try
            {
                using (var trans = _db.Database.BeginTransaction())
                {
                    try
                    {
                        #region Personel Bilgisi

                        Kullanicilar k = _db.Kullanicilar.FirstOrDefault(x => x.ID == model.ID);

                        if (model.PersonelId != null)
                        {
                            k.AdSoyad = _db.Personeller.Where(x => x.ID == model.PersonelId).FirstOrDefault().Ad + _db.Personeller.Where(x => x.ID == model.PersonelId).FirstOrDefault().Soyad;
                            k.FotoByte = model.FotoByte;
                            k.FotoContentType = model.FotoContentType;
                            k.KullaniciDurumu = true;
                            k.SifreKontrol = false;
                            k.KurumID = model.KurumID;
                            k.HizmetMerkeziID = model.HizmetMerkeziID;
                            if (k.TC != null)
                            {
                                k.Sifre = CryptoUtilities.Encrypt(k.TC.Trim());
                            }
                            else
                            {
                                k.Sifre = CryptoUtilities.Encrypt(GenerateCode());
                            }
                        }
                        else
                        {
                            k.AdSoyad = model.Ad;
                            k.Ad = model.Ad;
                            k.Soyad = model.Soyad;
                            k.TC = model.TC;
                            k.KurumID = model.KurumID;
                            k.HizmetMerkeziID = model.HizmetMerkeziID;
                            k.DogumTarihi = model.DogumTarihi;
                            k.Cinsiyet = model.Cinsiyet;
                            k.Telefon = model.Telefon;
                            k.KurumTelefonu = model.KurumTelefonu;
                            k.KurumEposta = model.KurumEposta;
                            k.Meslek = model.Meslek;
                            k.KullaniciAdi = model.KullaniciAdi.ToLower().Trim();
                            k.KullaniciDurumu = true;
                            if (k.TC != null)
                            {
                                k.Sifre = CryptoUtilities.Encrypt(k.TC);
                            }
                            else
                            {
                                k.Sifre = CryptoUtilities.Encrypt(GenerateCode());
                            }
                        }

                        _db.SaveChanges();

                        k.PersonelId = k.ID;
                        model.PersonelId = k.ID;
                        #endregion

                        if (k.ID > 0)
                        {
                            #region İletişim Bilgileri
                            KullaniciIletisimBilgileri ib = _db.KullaniciIletisimBilgileri.Where(x=>x.KullaniciID == k.ID).FirstOrDefault();
                            ib.KullaniciID = k.ID;
                            ib.Telefon = model.Telefon;
                            ib.KurumTelefonu = model.KurumTelefonu;

                            _db.SaveChanges();
                            #endregion

                            //#region Birim Bilgileri
                            //KullaniciBirimBilgileri br = new KullaniciBirimBilgileri();
                            //br.KullaniciID = k.ID;
                            //br.Bina = model.BirimBilgisi.Bina;
                            //br.Bolge = model.BirimBilgisi.Bolge;
                            //br.Kurum = model.BirimBilgisi.Kurum;

                            //_db.SaveChanges();
                            //#endregion

                            #region Kullanıcı Fotoğraf

                            if (model.PersonelFotoArray != null)
                            {
                                PersonelFotograflar pf = new PersonelFotograflar();
                                pf.ContentType = model.ContentType;
                                pf.FileByte = model.PersonelFotoArray;
                                pf.PersonelID = k.ID;
                                pf.Status = true;

                                _db.SaveChanges();
                            }

                            #endregion

                            trans.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        log.Error(JsonConvert.SerializeObject(model));
                        log.Error(ex);
                        hata = "(*) Alanlar Zorunlu Alandır Ve Boş Geçilemez. Lütfen Bu Alanları Tekrar Kontrol Edin.";
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                hata = "Güncelleme İşlemi Sırasında Bir Hata Meydana Geldi." + ex.Message + " - " + ex.InnerException;
                return false;
            }
        }
        public List<IKullaniciGorevTakip> PersonelGorevGetir(int id)
        {
            List<IKullaniciGorevTakip> gorevler = new List<IKullaniciGorevTakip>();
            try
            {

                gorevler = (from b in _db.KullaniciGorevTakip
                          where b.KullaniciID == id
                          select new IKullaniciGorevTakip
                          {
                              ID = b.ID,
                              KullaniciID = b.KullaniciID,
                              Baslik = b.Baslik,
                              Aciklama = b.Aciklama,
                              Tarih = (DateTime)b.Tarih,
                              Durum = b.Durum
                          }).ToList();

                return gorevler;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public bool PersonelGorevEkle(IKullaniciGorevTakip model)
        {
            try
            {
                KullaniciGorevTakip k = new KullaniciGorevTakip();

                k.KullaniciID = model.KullaniciID;
                k.Baslik = model.Baslik;
                k.Aciklama = model.Aciklama;
                k.Tarih = model.Tarih;
                k.Durum = model.Durum;

                _db.KullaniciGorevTakip.Add(k);
                _db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }

        //public List<IKullaniciIzin> IzinTalepleri(int kullaniciID)
        //{
        //    List<IKullaniciIzin> model = new List<IKullaniciIzin>();
        //    try
        //    {
        //        model = (from k in _db.KullaniciIzinleri
        //                 join it in _db.IzinTuru on k.IzinTuruId equals it.ID
        //                 where k.KullanıcıId == kullaniciID
        //                 select new IKullaniciIzin
        //                 {
        //                     ID = k.ID,
        //                     IzinBaslangicTarihi = k.IzinBaslangicTarihi,
        //                     IzinBitisTarihi = k.IzinBitisTarihi,
        //                     IzinDurum = k.IzinDurum,
        //                     IzinTuru = it.TurAdi,
        //                     Aciklama = k.Aciklama,
        //                     KayitTarihi = k.KayitTarihi,

        //                 }).OrderBy(x=>x.KayitTarihi).ToList();
        //        return model;
        //    }
        //    catch(Exception ex)
        //    {
        //        return model;
        //    }
        //}
        //public bool KullaniciIzinEkle(IKullaniciIzin model)
        //{
        //    try
        //    {
        //        KullaniciIzinleri k = new KullaniciIzinleri();
        //        k.Aciklama = model.Aciklama;
        //        k.IzinBaslangicTarihi = model.IzinBaslangicTarihi;
        //        k.IzinBitisTarihi = model.IzinBitisTarihi;
        //        k.IzinDurum = false;
        //        k.IzinTuruId = model.IzinTuruId;
        //        k.KayitTarihi = DateTime.Now;
        //        k.KullanıcıId = model.KullanıcıId;

        //        _db.KullaniciIzinleri.Add(k);
        //        _db.SaveChanges();
        //        return true;
        //    }
        //    catch(Exception ex)
        //    {
        //        return false;
        //    }
        //}
        //public bool KullaniciIzinDuzenle(IKullaniciIzin model)
        //{
        //    try
        //    {
        //        KullaniciIzinleri k = _db.KullaniciIzinleri.FirstOrDefault(x => x.ID == model.ID);
        //        k.Aciklama = model.Aciklama;
        //        k.IzinBaslangicTarihi = model.IzinBaslangicTarihi;
        //        k.IzinBitisTarihi = model.IzinBitisTarihi;
        //        k.IzinTuruId = model.IzinTuruId;

        //        _db.SaveChanges();
        //        return true;
        //    }
        //    catch(Exception ex)
        //    {
        //        return false;
        //    }
        //}
        #endregion

        #region ROL
        public bool RolEkle(IRol model)
        {
            try
            {
                Roller r = new Roller();
                r.Rol = model.Rol;

                _db.Roller.Add(r);
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }
        public bool RolDuzenle(IRol model)
        {
            try
            {
                Roller r = _db.Roller.Where(x => x.ID == model.ID).FirstOrDefault();
                r.Rol = model.Rol;

                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }
        public List<IRol> Roller()
        {
            List<IRol> model = new List<IRol>();
            try
            {
                model = (from r in _db.Roller
                         select new IRol
                         {
                             ID = r.ID,
                             Rol = r.Rol,
                         }).ToList();
                return model;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return model;
            }
        }
        public List<IRolYetkileri> RolYetkileri(int rolID)
        {
            List<IRolYetkileri> model = new List<IRolYetkileri>();
            try
            {
                model = (from ry in _db.RolYetkileri
                         join r in _db.Roller on ry.RolId equals r.ID
                         join y in _db.Yetkiler on ry.YetkiId equals y.ID
                         where ry.RolId == rolID
                         select new IRolYetkileri
                         {
                             ID = ry.ID,
                             RolId = ry.RolId,
                             RolAdi = r.Rol,
                             YetkiId = ry.YetkiId,
                             YetkiAdi = y.YetkiAdi,
                             Goruntuleme = ry.Goruntuleme,
                             Ekleme = ry.Ekleme,
                             Duzenleme = ry.Duzenleme,
                             Silme = ry.Silme
                         }).ToList();
                return model;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return model;
            }
        }
        #endregion

        #region YETKİ

        #endregion

        #region LOGİN İŞLEMLERİ

        public bool KullaniciGiris(IKullanici model, out IKullanici outKullanici, out string mesaj)
        {

            IKullanici kullanici = new IKullanici();
            outKullanici = new IKullanici();
            mesaj = "";
            if (model.Log == null) model.Log = new ILogLogin();
            try
            {
                model.KullaniciAdi = model.TC.ToLower().Trim();
                model.Sifre = CryptoUtilities.Encrypt(model.Sifre);
                kullanici = (from k in _db.Kullanicilar
                             where k.KullaniciAdi.ToLower().Trim() == model.TC
                             select new IKullanici
                             {
                                 ID = k.ID,
                                 KullaniciAdi = k.KullaniciAdi,
                                 TC = k.TC,
                                 Ad = k.AdSoyad,
                                 PersonelId = k.PersonelId,
                                 KullaniciDurumu = k.KullaniciDurumu,
                                 Sifre = k.Sifre,
                                 SifreKontrol = k.SifreKontrol,
                                 FotoByte = k.FotoByte,
                                 FotoContentType = k.FotoContentType,
                             }).FirstOrDefault();


                if (kullanici != null)
                {
                    if (model.Sifre == kullanici.Sifre)
                    {
                        if (kullanici.KullaniciDurumu == true)
                        {
                            mesaj = "Kullanıcı Adı ve Şifre Doğrulandı.";

                            if (kullanici.PersonelId > 0)
                            {
                                kullanici.PersonelFoto = (from pf in _db.PersonelFotograflar
                                                          where pf.PersonelID == kullanici.PersonelId && pf.Status == true
                                                          select new IPersonelFoto
                                                          {
                                                              FileByte = pf.FileByte,
                                                              ContentType = pf.ContentType,
                                                          }).FirstOrDefault();
                                if (kullanici.PersonelFoto != null)
                                {
                                    kullanici.FotoByte = kullanici.PersonelFoto.FileByte;
                                    kullanici.FotoContentType = kullanici.PersonelFoto.ContentType;
                                }
                            }

                            kullanici.Roller = (from kr in _db.KullaniciRolleri
                                                join r in _db.Roller on kr.RolId equals r.ID
                                                where kr.KullaniciId == kullanici.ID
                                                select new IKullaniciRol
                                                {
                                                    RolId = kr.RolId,
                                                    RolAdi = r.Rol,
                                                    _RolEnum = r.ID,
                                                    Yetkiler = (from yet in _db.RolYetkileri
                                                                join y in _db.Yetkiler on yet.YetkiId equals y.ID
                                                                where yet.RolId == kr.RolId
                                                                select new IRolYetkileri
                                                                {
                                                                    RolId = yet.RolId,
                                                                    YetkiId = yet.YetkiId,
                                                                    YetkiAdi = y.YetkiAdi,
                                                                    Goruntuleme = yet.Goruntuleme,
                                                                    Duzenleme = yet.Duzenleme,
                                                                    Silme = yet.Silme,
                                                                    Ekleme = yet.Ekleme,
                                                                }).ToList()
                                                }).ToList();

                            foreach (var r in kullanici.Roller)
                            {
                                var yetki = (from yet in _db.RolYetkileri
                                             join y in _db.Yetkiler on yet.YetkiId equals y.ID
                                             where yet.RolId == r.RolId
                                             select new IRolYetkileri
                                             {
                                                 RolId = yet.RolId,
                                                 YetkiId = yet.YetkiId,
                                                 YetkiAdi = y.YetkiAdi,
                                                 Goruntuleme = yet.Goruntuleme,
                                                 Duzenleme = yet.Duzenleme,
                                                 Silme = yet.Silme,
                                                 Ekleme = yet.Ekleme,
                                             }).ToList();

                                kullanici.RolModulYetkileri.AddRange(yetki);
                            }
                            //kullanici.RolModulYetkileri = 

                            outKullanici = kullanici;
                            model.Log._LogTipi = (int)EkipProjesi.Core.LogTipi.Giris;
                            model.Log.KullaniciId = kullanici.ID;
                            _log.LogKayit(model.Log);
                            return true;
                        }
                        else
                        {
                            mesaj = "Sisteme Giriş Yetkiniz Bulunmamaktadır.";
                            model.Log._LogTipi = (int)EkipProjesi.Core.LogTipi.HataliGiris;
                            model.Log.KullaniciId = kullanici.ID;
                            _log.LogKayit(model.Log);
                            return false;

                        }
                    }
                    else
                    {
                        mesaj = "Kullanıcı Adınız veya Şifreniz Hatalıdır.";
                        model.Log._LogTipi = (int)EkipProjesi.Core.LogTipi.HataliGiris;
                        model.Log.KullaniciId = kullanici.ID;
                        _log.LogKayit(model.Log);
                        return false;

                    }

                }
                mesaj = "Kullanıcı Bulunamadı.";
                if (model.Log == null) model.Log = new ILogLogin();
                model.Log._LogTipi = (int)EkipProjesi.Core.LogTipi.HataliGiris;
                model.Log.KullaniciId = 0;
                _log.LogKayit(model.Log);
                return false;
            }
            catch (Exception ex)
            {
                mesaj = ex.Message;
                if (!string.IsNullOrEmpty(ex.Message) && ex.Message.Contains("The underlying provider failed on Open"))
                {
                    mesaj = "Veri Tabanı ile Bağlantı Kurulamadı. <br/> Sistem yöneticinize Başvurun!";
                }

                log.Fatal(model.KullaniciAdi);
                log.Error(ex);
                return false;
            }
        }

        //public bool EmailDegistirme(string email, int userid, string sessionID)
        //{
        //    string eskimail = "";
        //    try
        //    {
        //        Kullanicilar k = _db.Kullanicilar.FirstOrDefault(x => x.ID == userid);
        //        eskimail = k.KullaniciAdi;
        //        k.KullaniciAdi = email.Trim();
        //        _db.SaveChanges();

        //        LogMailDegistirme l = new LogMailDegistirme();
        //        l.EskiMail = eskimail;
        //        l.SessionID = sessionID;
        //        l.Tarih = DateTime.Now;
        //        l.UserID = userid;
        //        l.YeniMail = email;

        //        _db.LogMailDegistirme.Add(l);
        //        _db.SaveChanges();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex);
        //        return false;
        //    }
        //}

        public bool EmailKontrol(string email)
        {
            if (_db.Kullanicilar.Where(x => x.KullaniciAdi == email).Count() == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Kontroller
        public List<IRandevuAyarlari> PasifRandevuAyarlari()
        {
            List<IRandevuAyarlari> model = new List<IRandevuAyarlari>();

            try
            {
                model = (from r in _db.RandevuAyarlari
                         where r.Durum == false
                         select new IRandevuAyarlari
                         {
                             ID = r.ID,
                             KurumID = r.KurumID,
                             PoliklinikTuruID = r.PoliklinikTuruID,
                             KurumAdi = _db.Kurumlar.Where(x => x.KurumID == r.KurumID).Select(x => x.KurumAdi).FirstOrDefault(),
                             PoliklinikAdi = _db.KurumPoliklinikTurleri.Where(x => x.KurumID == r.KurumID && x.ID == r.PoliklinikTuruID).Select(x => x.PoliklinikTuru).FirstOrDefault(),
                             Gun = r.Gun,
                             BaslangicSaati = r.BaslangicSaati,
                             BitisSaati = r.BitisSaati,
                             SlotSayisi = r.SlotSayisi,
                             Durum = r.Durum,
                             PasifBaslangicTarihi = r.PasifBaslangicTarihi,
                             PasifBitisTarihi = r.PasifBitisTarihi
                         }).OrderByDescending(x => x.KurumID).ToList();
                return model;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return model;
            }
        }
        public bool RandevuAyariPasiflikKontrol(int ayarId)
        {
            try
            {
                RandevuAyarlari r = _db.RandevuAyarlari.Where(x => x.ID == ayarId && x.Durum == false).FirstOrDefault();

                if (r.Durum == false)
                {
                    if (!(r.PasifBaslangicTarihi <= DateTime.Now && DateTime.Now <= r.PasifBitisTarihi))
                    {
                        r.Durum = true;
                        r.PasifBaslangicTarihi = null;
                        r.PasifBitisTarihi = null;
                        _db.SaveChanges();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }
        public List<IRandevuBilgileri> RandevuBilgileriGetir()
        {
            List<IRandevuBilgileri> randevular = new List<IRandevuBilgileri>();
            try
            {

                randevular = (from b in _db.RandevuBilgileri
                              where b.RandevuDurumu != null
                              select new IRandevuBilgileri
                              {
                                  ID = b.ID,
                                  KurumID = b.KurumID,
                                  PoliklinikTuruID = b.PoliklinikTuruID,
                                  HastaID = _db.Hastalar.Where(x => x.HastaTCKimlikNo == b.HastaTCKimlikNo).Select(x => x.HastaID).FirstOrDefault(),
                                  HastaAdi = b.HastaAdi,
                                  HastaSoyadi = b.HastaSoyadi,
                                  HastaTCKimlikNo = b.HastaTCKimlikNo,
                                  Telefon = b.Telefon,
                                  YakinTelefonu = b.YakinTelefonu,
                                  Aciklama = b.Aciklama,
                                  Renk = b.Renk,
                                  RandevuBaslangicTarihi = (DateTime)b.RandevuBaslangicTarihi,
                                  RandevuBitisTarihi = (DateTime)b.RandevuBitisTarihi,
                                  RandevuBaslangicSaati = b.RandevuBaslangicSaati,
                                  RandevuBitisSaati = b.RandevuBitisSaati,
                                  RandevuDurumu = b.RandevuDurumu

                              }).ToList();

                return randevular;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public bool RandevuDurumBilgisiKontrol(int randevuId)
        {
            try
            {
                RandevuBilgileri r = _db.RandevuBilgileri.Where(x => x.ID == randevuId && x.RandevuDurumu != null).FirstOrDefault();

                if (r.RandevuDurumu.Contains("gelmedi"))
                {
                    r.Renk = "red";
                    _db.SaveChanges();
                }
                else if (r.RandevuDurumu.Contains("geldi"))
                {
                    r.Renk = "green";
                    _db.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }
        #endregion      
    }
}