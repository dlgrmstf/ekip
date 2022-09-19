using EkipProjesi.Core.AcilServis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkipProjesi.Data.Repositories
{
    public class AcilServisRepository
    {
        #region Const
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private EKIPEntities _db;
        public AcilServisRepository()
        {
            _db = new EKIPEntities();
        }
        #endregion

        #region Acil Servis Vakaları
        public List<IAcilServisVakaBilgileri> AcilServisVakaBilgileri()
        {
            List<IAcilServisVakaBilgileri> bilgiler = new List<IAcilServisVakaBilgileri>();
            try
            {
                bilgiler = (from b in _db.AcilServisVakaBilgileri
                            select new IAcilServisVakaBilgileri
                            {
                                ID = b.ID,
                                KurumKodu = b.KurumKodu,
                                HastaAdi = b.HastaAdi,
                                HekimAdi = b.HekimAdi,
                                BasvuruTarihi = b.BasvuruTarihi,
                                GerceklestirilenIslemler = b.GerceklestirilenIslemler,
                                HastaEkipNo = b.HastaEkipNo,
                                HastaSoyadi = b.HastaSoyadi,
                                HastaTCKimlikNo = b.HastaTCKimlikNo,
                                TaburculukNotlari = b.TaburculukNotlari,
                                TaburculukTarihi = b.TaburculukTarihi,
                                KurumAdi = _db.Kurumlar.Where(x => x.KurumKodu == b.KurumKodu).Select(x => x.KurumAdi).FirstOrDefault(),
                                Tanilar = (from t in _db.AcilServisVakaTanilari
                                           where t.AcilServisVakaID == b.ID
                                           select new IAcilServisVakaTanilari
                                           {
                                               ID = t.ID,
                                               AcilServisVakaID = t.ID,
                                               TaniKoduID = (int)t.TaniKoduID,
                                               Tani = _db.TaniKodlari.Where(x => x.ID == t.TaniKoduID).Select(x => x.Tani).FirstOrDefault(),
                                               TaniKodu = _db.TaniKodlari.Where(x => x.ID == t.TaniKoduID).Select(x => x.ICDKodu).FirstOrDefault()
                                           }).ToList()

                            }).ToList();

                return bilgiler;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public IAcilServisVakaBilgileri AcilServisVakaBilgisiDetay(int id)
        {
            IAcilServisVakaBilgileri model = new IAcilServisVakaBilgileri();
            try
            {
                model = (from b in _db.AcilServisVakaBilgileri
                         where b.ID == id
                         select new IAcilServisVakaBilgileri
                         {
                             ID = b.ID,
                             KurumKodu = b.KurumKodu,
                             HastaAdi = b.HastaAdi,
                             HekimAdi = b.HekimAdi,
                             BasvuruTarihi = b.BasvuruTarihi,
                             GerceklestirilenIslemler = b.GerceklestirilenIslemler,
                             HastaEkipNo = b.HastaEkipNo,
                             HastaSoyadi = b.HastaSoyadi,
                             HastaTCKimlikNo = b.HastaTCKimlikNo,
                             TaburculukNotlari = b.TaburculukNotlari,
                             TaburculukTarihi = b.TaburculukTarihi,
                             KurumAdi = _db.Kurumlar.Where(x => x.KurumKodu == b.KurumKodu).Select(x => x.KurumAdi).FirstOrDefault(),
                             Tanilar = (from t in _db.AcilServisVakaTanilari
                                        where t.AcilServisVakaID == b.ID
                                        select new IAcilServisVakaTanilari
                                        {
                                            ID = t.ID,
                                            AcilServisVakaID = t.ID,
                                            TaniKoduID = (int)t.TaniKoduID,
                                            Tani = _db.TaniKodlari.Where(x=>x.ID == t.TaniKoduID).Select(x=>x.Tani).FirstOrDefault(),
                                            TaniKodu = _db.TaniKodlari.Where(x => x.ID == t.TaniKoduID).Select(x => x.ICDKodu).FirstOrDefault()
                                        }).ToList()

                         }).FirstOrDefault();
                return model;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return model;
            }
        }
        public List<IAcilServisVakaBilgileri> KendiHastalariBasvuru(int UserID)
        {
            List<IAcilServisVakaBilgileri> basvurular = new List<IAcilServisVakaBilgileri>();
            try
            {

                basvurular = (from b in _db.AcilServisVakaBilgileri
                              join h in _db.Hastalar on b.HastaEkipNo equals h.HastaEkipNo
                              where h.KaydedenKullaniciID == UserID
                              select new IAcilServisVakaBilgileri
                              {
                                  ID = b.ID,
                                  KurumKodu = b.KurumKodu,
                                  HastaAdi = b.HastaAdi,
                                  HekimAdi = b.HekimAdi,
                                  BasvuruTarihi = b.BasvuruTarihi,
                                  GerceklestirilenIslemler = b.GerceklestirilenIslemler,
                                  HastaEkipNo = b.HastaEkipNo,
                                  HastaSoyadi = b.HastaSoyadi,
                                  HastaTCKimlikNo = b.HastaTCKimlikNo,
                                  TaburculukNotlari = b.TaburculukNotlari,
                                  TaburculukTarihi = b.TaburculukTarihi,
                                  KurumAdi = _db.Kurumlar.Where(x => x.KurumKodu == b.KurumKodu).Select(x => x.KurumAdi).FirstOrDefault(),
                                  Tanilar = (from t in _db.AcilServisVakaTanilari
                                             where t.AcilServisVakaID == b.ID
                                             select new IAcilServisVakaTanilari
                                             {
                                                 ID = t.ID,
                                                 AcilServisVakaID = t.ID,
                                                 TaniKoduID = (int)t.TaniKoduID,
                                                 Tani = _db.TaniKodlari.Where(x => x.ID == t.TaniKoduID).Select(x => x.Tani).FirstOrDefault(),
                                                 TaniKodu = _db.TaniKodlari.Where(x => x.ID == t.TaniKoduID).Select(x => x.ICDKodu).FirstOrDefault()
                                             }).ToList()

                              }).ToList();

                return basvurular;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public List<IAcilServisVakaBilgileri> KurumdakiHastalarBasvuru(int UserID)
        {
            List<IAcilServisVakaBilgileri> basvurular = new List<IAcilServisVakaBilgileri>();
            try
            {

                basvurular = (from b in _db.AcilServisVakaBilgileri
                              join h in _db.Hastalar on b.HastaEkipNo equals h.HastaEkipNo
                              join k in _db.Kullanicilar on UserID equals k.ID
                              join kr in _db.Kurumlar on h.KurumKodu equals kr.KurumKodu
                              where k.KurumID == kr.KurumID
                              select new IAcilServisVakaBilgileri
                              {
                                  ID = b.ID,
                                  KurumKodu = b.KurumKodu,
                                  HastaAdi = b.HastaAdi,
                                  HekimAdi = b.HekimAdi,
                                  BasvuruTarihi = b.BasvuruTarihi,
                                  GerceklestirilenIslemler = b.GerceklestirilenIslemler,
                                  HastaEkipNo = b.HastaEkipNo,
                                  HastaSoyadi = b.HastaSoyadi,
                                  HastaTCKimlikNo = b.HastaTCKimlikNo,
                                  TaburculukNotlari = b.TaburculukNotlari,
                                  TaburculukTarihi = b.TaburculukTarihi,
                                  KurumAdi = _db.Kurumlar.Where(x => x.KurumKodu == b.KurumKodu).Select(x => x.KurumAdi).FirstOrDefault(),
                                  Tanilar = (from t in _db.AcilServisVakaTanilari
                                             where t.AcilServisVakaID == b.ID
                                             select new IAcilServisVakaTanilari
                                             {
                                                 ID = t.ID,
                                                 AcilServisVakaID = t.ID,
                                                 TaniKoduID = (int)t.TaniKoduID,
                                                 Tani = _db.TaniKodlari.Where(x => x.ID == t.TaniKoduID).Select(x => x.Tani).FirstOrDefault(),
                                                 TaniKodu = _db.TaniKodlari.Where(x => x.ID == t.TaniKoduID).Select(x => x.ICDKodu).FirstOrDefault()
                                             }).ToList()
                              }).ToList();

                return basvurular;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        #endregion

        #region 112 Vakaları
        public List<IVakaBilgileri112> VakaBilgileri112()
        {
            List<IVakaBilgileri112> bilgiler = new List<IVakaBilgileri112>();
            try
            {
                bilgiler = (from b in _db.VakaBilgileri112
                            select new IVakaBilgileri112
                            {
                                ID = b.ID,
                                KurumKodu = b.KurumKodu,
                                HastaAdi = b.HastaAdi,
                                HastaEkipNo = b.HastaEkipNo,
                                HastaSoyadi = b.HastaSoyadi,
                                HastaTCKimlikNo = b.HastaTCKimlikNo,
                                CagriTarihSaati = b.CagriTarihSaati,
                                OnTani = b.OnTani,
                                YapilanIslem = b.YapilanIslem,
                                AlindigiYer = b.AlindigiYer,
                                BirakildigiKurum = b.BirakildigiKurum,
                                KurumAdi = _db.Kurumlar.Where(x => x.KurumKodu == b.KurumKodu).Select(x => x.KurumAdi).FirstOrDefault(),
                                Tanilar = (from t in _db.VakaTanilari112
                                           where t.Vaka112ID == b.ID
                                           select new IVakaTanilari112
                                           {
                                               ID = t.ID,
                                               Vaka112ID = t.ID,
                                               TaniKoduID = (int)t.TaniKoduID,
                                               Tani = _db.TaniKodlari.Where(x => x.ID == t.TaniKoduID).Select(x => x.Tani).FirstOrDefault(),
                                               TaniKodu = _db.TaniKodlari.Where(x => x.ID == t.TaniKoduID).Select(x => x.ICDKodu).FirstOrDefault()
                                           }).ToList()

                            }).ToList();

                return bilgiler;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public IVakaBilgileri112 VakaBilgileri112Detay(int id)
        {
            IVakaBilgileri112 model = new IVakaBilgileri112();
            try
            {
                model = (from b in _db.VakaBilgileri112
                         where b.ID == id
                         select new IVakaBilgileri112
                         {
                             ID = b.ID,
                             KurumKodu = b.KurumKodu,
                             HastaAdi = b.HastaAdi,
                             HastaEkipNo = b.HastaEkipNo,
                             HastaSoyadi = b.HastaSoyadi,
                             HastaTCKimlikNo = b.HastaTCKimlikNo,
                             CagriTarihSaati = b.CagriTarihSaati,
                             OnTani = b.OnTani,
                             YapilanIslem = b.YapilanIslem,
                             AlindigiYer = b.AlindigiYer,
                             BirakildigiKurum = b.BirakildigiKurum,
                             KurumAdi = _db.Kurumlar.Where(x => x.KurumKodu == b.KurumKodu).Select(x => x.KurumAdi).FirstOrDefault(),
                             Tanilar = (from t in _db.VakaTanilari112
                                        where t.Vaka112ID == b.ID
                                        select new IVakaTanilari112
                                        {
                                            ID = t.ID,
                                            Vaka112ID = t.ID,
                                            TaniKoduID = (int)t.TaniKoduID,
                                            Tani = _db.TaniKodlari.Where(x => x.ID == t.TaniKoduID).Select(x => x.Tani).FirstOrDefault(),
                                            TaniKodu = _db.TaniKodlari.Where(x => x.ID == t.TaniKoduID).Select(x => x.ICDKodu).FirstOrDefault()
                                        }).ToList()

                         }).FirstOrDefault();
                return model;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return model;
            }
        }
        public List<IVakaBilgileri112> KendiHastalariBasvuru112(int UserID)
        {
            List<IVakaBilgileri112> basvurular = new List<IVakaBilgileri112>();
            try
            {

                basvurular = (from b in _db.VakaBilgileri112
                              join h in _db.Hastalar on b.HastaEkipNo equals h.HastaEkipNo
                              where h.KaydedenKullaniciID == UserID
                              select new IVakaBilgileri112
                              {
                                  ID = b.ID,
                                  KurumKodu = b.KurumKodu,
                                  HastaAdi = b.HastaAdi,
                                  HastaEkipNo = b.HastaEkipNo,
                                  HastaSoyadi = b.HastaSoyadi,
                                  HastaTCKimlikNo = b.HastaTCKimlikNo,
                                  CagriTarihSaati = b.CagriTarihSaati,
                                  OnTani = b.OnTani,
                                  YapilanIslem = b.YapilanIslem,
                                  AlindigiYer = b.AlindigiYer,
                                  BirakildigiKurum = b.BirakildigiKurum,
                                  KurumAdi = _db.Kurumlar.Where(x => x.KurumKodu == b.KurumKodu).Select(x => x.KurumAdi).FirstOrDefault(),
                                  Tanilar = (from t in _db.VakaTanilari112
                                             where t.Vaka112ID == b.ID
                                             select new IVakaTanilari112
                                             {
                                                 ID = t.ID,
                                                 Vaka112ID = t.ID,
                                                 TaniKoduID = (int)t.TaniKoduID,
                                                 Tani = _db.TaniKodlari.Where(x => x.ID == t.TaniKoduID).Select(x => x.Tani).FirstOrDefault(),
                                                 TaniKodu = _db.TaniKodlari.Where(x => x.ID == t.TaniKoduID).Select(x => x.ICDKodu).FirstOrDefault()
                                             }).ToList()

                              }).ToList();

                return basvurular;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public List<IVakaBilgileri112> KurumdakiHastalarBasvuru112(int UserID)
        {
            List<IVakaBilgileri112> basvurular = new List<IVakaBilgileri112>();
            try
            {

                basvurular = (from b in _db.VakaBilgileri112
                              join h in _db.Hastalar on b.HastaEkipNo equals h.HastaEkipNo
                              join k in _db.Kullanicilar on UserID equals k.ID
                              join kr in _db.Kurumlar on h.KurumKodu equals kr.KurumKodu
                              where k.KurumID == kr.KurumID
                              select new IVakaBilgileri112
                              {
                                  ID = b.ID,
                                  KurumKodu = b.KurumKodu,
                                  HastaAdi = b.HastaAdi,
                                  HastaEkipNo = b.HastaEkipNo,
                                  HastaSoyadi = b.HastaSoyadi,
                                  HastaTCKimlikNo = b.HastaTCKimlikNo,
                                  CagriTarihSaati = b.CagriTarihSaati,
                                  OnTani = b.OnTani,
                                  YapilanIslem = b.YapilanIslem,
                                  AlindigiYer = b.AlindigiYer,
                                  BirakildigiKurum = b.BirakildigiKurum,
                                  KurumAdi = _db.Kurumlar.Where(x => x.KurumKodu == b.KurumKodu).Select(x => x.KurumAdi).FirstOrDefault(),
                                  Tanilar = (from t in _db.VakaTanilari112
                                             where t.Vaka112ID == b.ID
                                             select new IVakaTanilari112
                                             {
                                                 ID = t.ID,
                                                 Vaka112ID = t.ID,
                                                 TaniKoduID = (int)t.TaniKoduID,
                                                 Tani = _db.TaniKodlari.Where(x => x.ID == t.TaniKoduID).Select(x => x.Tani).FirstOrDefault(),
                                                 TaniKodu = _db.TaniKodlari.Where(x => x.ID == t.TaniKoduID).Select(x => x.ICDKodu).FirstOrDefault()
                                             }).ToList()
                              }).ToList();

                return basvurular;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        #endregion
    }
}