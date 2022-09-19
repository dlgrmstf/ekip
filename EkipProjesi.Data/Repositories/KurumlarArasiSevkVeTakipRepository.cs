using EkipProjesi.Core.Hastalar;
using EkipProjesi.Core.KurumlarArasiSevkVeTakip;
using EkipProjesi.Core.Randevu;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EkipProjesi.Data.Repositories
{
    public class KurumlarArasiSevkVeTakipRepository
    {
        #region const
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private EKIPEntities _db;
        public KurumlarArasiSevkVeTakipRepository()
        {
            _db = new EKIPEntities();
        }
        #endregion

        #region Bilgileri Getir
        public List<IHastalar> HastaBilgileriGetir(IHastaAramaModel model)
        {
            List<IHastalar> hastalar = new List<IHastalar>();

            try
            {
                hastalar = (from b in _db.Hastalar
                            where b.HastaTCKimlikNo == model.HastaTC || b.HastaEkipNo == model.HastaEkipNo
                            select new IHastalar
                            {
                                HastaID = b.HastaID,
                                HastaAdi = b.HastaAdi,
                                HastaSoyadi = b.HastaSoyadi,
                                HastaTCKimlikNo = b.HastaTCKimlikNo
                            }).ToList();

                return hastalar;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public IHastalar HastaBilgiGetir(int? hastaId)
        {
            IHastalar hasta = new IHastalar();
            try
            {

                hasta = (from b in _db.Hastalar
                         select new IHastalar
                         {
                             HastaID = b.HastaID,
                             HastaAdi = b.HastaAdi,
                             HastaSoyadi = b.HastaSoyadi,
                             HastaTCKimlikNo = b.HastaTCKimlikNo

                         }).Where(x => x.HastaID == hastaId).FirstOrDefault();

                return hasta;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public List<IKurumHizmetMerkezleri> HizmetMerkezleriGetir()
        {
            List<IKurumHizmetMerkezleri> merkezler = new List<IKurumHizmetMerkezleri>();
            try
            {
                merkezler = (from k in _db.KurumHizmetMerkezleri
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
        #endregion

        #region Randevu Talepleri
        public bool RandevuTalebiKaydet(IRandevuTalepleri model, int userId)
        {
            try
            {
                RandevuTalepleri h = new RandevuTalepleri();

                h.HizmetMerkeziID = model.HizmetMerkeziID;
                h.KurumID = _db.KurumHizmetMerkezleri.Where(x => x.ID == model.HizmetMerkeziID).Select(x => x.KurumID).FirstOrDefault();
                h.HastaID = model.HastaID;
                h.HastaAdi = model.HastaAdi;
                h.HastaSoyadi = model.HastaSoyadi;
                h.HastaTCKimlikNo = model.HastaTCKimlikNo;
                h.Telefon = model.Telefon;
                h.RandevuTalebiNotu = model.RandevuTalebiNotu;
                h.RandevuVerildiMi = false;
                h.TalepOlusturanKullaniciID = userId;
                h.KayitTarihi = DateTime.Now;
                _db.RandevuTalepleri.Add(h);
                _db.SaveChanges();

                if (h.ID > 0)
                {
                    #region İzlem 
                    HastaIzlemBilgileri hi = new HastaIzlemBilgileri();
                    hi.HastaID = model.HastaID;
                    hi.IzlemTarihi = DateTime.Now;
                    hi.IzlemYapanKurum = _db.KullaniciBirimBilgileri.Where(x => x.KullaniciID == h.TalepOlusturanKullaniciID).Select(x => x.Kurum).FirstOrDefault();
                    hi.IzlemBasligi = "Randevu talebi oluşturuldu.";
                    hi.IzlemAciklama = "Hasta için ilgili kurumda randevu talebi oluşturulmuştur.";

                    _db.HastaIzlemBilgileri.Add(hi);
                    _db.SaveChanges();
                    #endregion
                }

                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }
        public List<IRandevuTalepleri> IskurRandevular()
        {
            List<IRandevuTalepleri> randevular = new List<IRandevuTalepleri>();
            try
            {

                randevular = (from i in _db.RandevuTalepleri
                              where i.KurumID == 5
                              orderby i.KayitTarihi ascending
                              select new IRandevuTalepleri
                              {
                                  ID = i.ID,
                                  KurumID = i.KurumID,
                                  HizmetMerkeziID = i.HizmetMerkeziID,
                                  HastaID = i.HastaID,
                                  HastaAdi = i.HastaAdi,
                                  HastaSoyadi = i.HastaSoyadi,
                                  HastaTCKimlikNo = i.HastaTCKimlikNo,
                                  Telefon = i.Telefon,
                                  RandevuVerildiMi = (bool)i.RandevuVerildiMi,
                                  TalepOlusturanKullaniciID = (int)i.TalepOlusturanKullaniciID,
                                  KayitTarihi = i.KayitTarihi,
                                  RandevuTarihi = (DateTime)i.RandevuTarihi,
                                  RandevuVerenKullaniciID = (int)i.RandevuVerenKullaniciID,
                                  RandevuDurumu = i.RandevuDurumu,
                                  HizmetMerkeziAdi = _db.KurumHizmetMerkezleri.Where(x => x.KurumID == 5 && x.ID == i.HizmetMerkeziID).Select(x => x.Ad).FirstOrDefault(),
                                  RandevuTalebiNotu = i.RandevuTalebiNotu

                              }).ToList();

                return randevular;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public List<IRandevuTalepleri> YesilayRandevular()
        {
            List<IRandevuTalepleri> randevular = new List<IRandevuTalepleri>();
            try
            {

                randevular = (from i in _db.RandevuTalepleri
                              where i.KurumID == 6
                              orderby i.KayitTarihi ascending
                              select new IRandevuTalepleri
                              {
                                  ID = i.ID,
                                  KurumID = i.KurumID,
                                  HizmetMerkeziID = i.HizmetMerkeziID,
                                  HastaID = i.HastaID,
                                  HastaAdi = i.HastaAdi,
                                  HastaSoyadi = i.HastaSoyadi,
                                  HastaTCKimlikNo = i.HastaTCKimlikNo,
                                  Telefon = i.Telefon,
                                  RandevuVerildiMi = (bool)i.RandevuVerildiMi,
                                  TalepOlusturanKullaniciID = (int)i.TalepOlusturanKullaniciID,
                                  KayitTarihi = i.KayitTarihi,
                                  RandevuTarihi = (DateTime)i.RandevuTarihi,
                                  RandevuVerenKullaniciID = (int)i.RandevuVerenKullaniciID,
                                  RandevuDurumu = i.RandevuDurumu,
                                  HizmetMerkeziAdi = _db.KurumHizmetMerkezleri.Where(x => x.KurumID == 6 && x.ID == i.HizmetMerkeziID).Select(x => x.Ad).FirstOrDefault(),
                                  RandevuTalebiNotu = i.RandevuTalebiNotu

                              }).ToList();

                return randevular;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public List<IRandevuTalepleri> SHMRandevular()
        {
            List<IRandevuTalepleri> randevular = new List<IRandevuTalepleri>();
            try
            {

                randevular = (from i in _db.RandevuTalepleri
                              where i.KurumID == 7
                              orderby i.KayitTarihi ascending
                              select new IRandevuTalepleri
                              {
                                  ID = i.ID,
                                  KurumID = i.KurumID,
                                  HizmetMerkeziID = i.HizmetMerkeziID,
                                  HastaID = i.HastaID,
                                  HastaAdi = i.HastaAdi,
                                  HastaSoyadi = i.HastaSoyadi,
                                  HastaTCKimlikNo = i.HastaTCKimlikNo,
                                  Telefon = i.Telefon,
                                  RandevuVerildiMi = (bool)i.RandevuVerildiMi,
                                  TalepOlusturanKullaniciID = (int)i.TalepOlusturanKullaniciID,
                                  KayitTarihi = i.KayitTarihi,
                                  RandevuTarihi = (DateTime)i.RandevuTarihi,
                                  RandevuVerenKullaniciID = (int)i.RandevuVerenKullaniciID,
                                  RandevuDurumu = i.RandevuDurumu,
                                  HizmetMerkeziAdi = _db.KurumHizmetMerkezleri.Where(x => x.KurumID == 7 && x.ID == i.HizmetMerkeziID).Select(x => x.Ad).FirstOrDefault(),
                                  RandevuTalebiNotu = i.RandevuTalebiNotu

                              }).ToList();

                return randevular;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public List<IRandevuTalepleri> ISMRandevular()
        {
            List<IRandevuTalepleri> randevular = new List<IRandevuTalepleri>();
            try
            {

                randevular = (from i in _db.RandevuTalepleri
                              where i.KurumID == 8
                              orderby i.KayitTarihi ascending
                              select new IRandevuTalepleri
                              {
                                  ID = i.ID,
                                  KurumID = i.KurumID,
                                  HizmetMerkeziID = i.HizmetMerkeziID,
                                  HastaID = i.HastaID,
                                  HastaAdi = i.HastaAdi,
                                  HastaSoyadi = i.HastaSoyadi,
                                  HastaTCKimlikNo = i.HastaTCKimlikNo,
                                  Telefon = i.Telefon,
                                  RandevuVerildiMi = (bool)i.RandevuVerildiMi,
                                  TalepOlusturanKullaniciID = (int)i.TalepOlusturanKullaniciID,
                                  KayitTarihi = i.KayitTarihi,
                                  RandevuTarihi = (DateTime)i.RandevuTarihi,
                                  RandevuVerenKullaniciID = (int)i.RandevuVerenKullaniciID,
                                  RandevuDurumu = i.RandevuDurumu,
                                  HizmetMerkeziAdi = _db.KurumHizmetMerkezleri.Where(x => x.KurumID == 8 && x.ID == i.HizmetMerkeziID).Select(x => x.Ad).FirstOrDefault(),
                                  RandevuTalebiNotu = i.RandevuTalebiNotu

                              }).ToList();

                return randevular;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public List<IRandevuBilgileri> BakirkoyRandevular()
        {
            List<IRandevuBilgileri> randevular = new List<IRandevuBilgileri>();
            try
            {

                randevular = (from i in _db.RandevuBilgileri
                              where i.KurumID == 2
                              orderby i.RandevuBaslangicTarihi ascending
                              select new IRandevuBilgileri
                              {
                                  ID = i.ID,
                                  KurumID = i.KurumID,
                                  PoliklinikTuruID = i.PoliklinikTuruID,
                                  HastaAdi = i.HastaAdi,
                                  HastaSoyadi = i.HastaSoyadi,
                                  HastaTCKimlikNo = i.HastaTCKimlikNo,
                                  Telefon = i.Telefon,
                                  Aciklama = i.Aciklama,
                                  RandevuBaslangicSaati = i.RandevuBaslangicSaati,
                                  RandevuBaslangicTarihi = i.RandevuBaslangicTarihi,
                                  RandevuDurumu = i.RandevuDurumu,
                                  PoliklinikAdi = _db.KurumPoliklinikTurleri.Where(x => x.KurumID == 2 && x.ID == i.PoliklinikTuruID).Select(x => x.PoliklinikTuru).FirstOrDefault()

                              }).ToList();

                return randevular;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public List<IRandevuBilgileri> ErenkoyRandevular()
        {
            List<IRandevuBilgileri> randevular = new List<IRandevuBilgileri>();
            try
            {

                randevular = (from i in _db.RandevuBilgileri
                              where i.KurumID == 3
                              orderby i.RandevuBaslangicTarihi ascending
                              select new IRandevuBilgileri
                              {
                                  ID = i.ID,
                                  KurumID = i.KurumID,
                                  PoliklinikTuruID = i.PoliklinikTuruID,
                                  HastaAdi = i.HastaAdi,
                                  HastaSoyadi = i.HastaSoyadi,
                                  HastaTCKimlikNo = i.HastaTCKimlikNo,
                                  Telefon = i.Telefon,
                                  Aciklama = i.Aciklama,
                                  RandevuBaslangicSaati = i.RandevuBaslangicSaati,
                                  RandevuBaslangicTarihi = i.RandevuBaslangicTarihi,
                                  RandevuDurumu = i.RandevuDurumu,
                                  PoliklinikAdi = _db.KurumPoliklinikTurleri.Where(x => x.KurumID == 3 && x.ID == i.PoliklinikTuruID).Select(x => x.PoliklinikTuru).FirstOrDefault()

                              }).ToList();

                return randevular;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public List<IRandevuBilgileri> OrdRandevular()
        {
            List<IRandevuBilgileri> randevular = new List<IRandevuBilgileri>();
            try
            {

                randevular = (from i in _db.RandevuBilgileri
                              where i.KurumID == 4
                              orderby i.RandevuBaslangicTarihi ascending
                              select new IRandevuBilgileri
                              {
                                  ID = i.ID,
                                  KurumID = i.KurumID,
                                  PoliklinikTuruID = i.PoliklinikTuruID,
                                  HastaAdi = i.HastaAdi,
                                  HastaSoyadi = i.HastaSoyadi,
                                  HastaTCKimlikNo = i.HastaTCKimlikNo,
                                  Telefon = i.Telefon,
                                  Aciklama = i.Aciklama,
                                  RandevuBaslangicSaati = i.RandevuBaslangicSaati,
                                  RandevuBaslangicTarihi = i.RandevuBaslangicTarihi,
                                  RandevuDurumu = i.RandevuDurumu,
                                  PoliklinikAdi = _db.KurumPoliklinikTurleri.Where(x => x.KurumID == 4 && x.ID == i.PoliklinikTuruID).Select(x => x.PoliklinikTuru).FirstOrDefault()

                              }).ToList();

                return randevular;
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