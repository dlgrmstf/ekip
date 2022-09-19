using EkipProjesi.Core.KurumlarArasiSevkVeTakip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkipProjesi.Data.Repositories
{
    public class SosyalHizmetRepository
    {
        #region Const
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private EKIPEntities _db;

        public SosyalHizmetRepository()
        {
            _db = new EKIPEntities();
        }
        #endregion

        public List<IRandevuTalepleri> AktifRandevuTalepleri()
        {
            List<IRandevuTalepleri> talepler = new List<IRandevuTalepleri>();
            try
            {

                talepler = (from i in _db.RandevuTalepleri
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
                                RandevuTalebiNotu = i.RandevuTalebiNotu,
                                RandevuVerildiMi = (bool)i.RandevuVerildiMi,
                                TalepOlusturanKullaniciID = (int)i.TalepOlusturanKullaniciID,
                                KayitTarihi = i.KayitTarihi,
                                HizmetMerkeziAdi = _db.KurumHizmetMerkezleri.Where(x => x.KurumID == 7 && x.ID == i.HizmetMerkeziID).Select(x => x.Ad).FirstOrDefault()

                            }).Where(x => x.RandevuVerildiMi == false).ToList();

                return talepler;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public List<IRandevuTalepleri> VerilenRandevular()
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
                                  RandevuTalebiNotu = i.RandevuTalebiNotu,
                                  RandevuVerildiMi = (bool)i.RandevuVerildiMi,
                                  TalepOlusturanKullaniciID = (int)i.TalepOlusturanKullaniciID,
                                  KayitTarihi = i.KayitTarihi,
                                  RandevuTarihi = (DateTime)i.RandevuTarihi,
                                  RandevuVerenKullaniciID = (int)i.RandevuVerenKullaniciID,
                                  RandevuDurumu = i.RandevuDurumu,
                                  HizmetMerkeziAdi = _db.KurumHizmetMerkezleri.Where(x => x.KurumID == 7 && x.ID == i.HizmetMerkeziID).Select(x => x.Ad).FirstOrDefault()

                              }).Where(x => x.RandevuVerildiMi == true).ToList();

                return randevular;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public IRandevuTalepleri TalepDetay(int talepId)
        {
            IRandevuTalepleri talep = new IRandevuTalepleri();
            try
            {

                talep = (from i in _db.RandevuTalepleri
                         where i.ID == talepId
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
                             RandevuTalebiNotu = i.RandevuTalebiNotu,
                             RandevuVerildiMi = (bool)i.RandevuVerildiMi,
                             TalepOlusturanKullaniciID = (int)i.TalepOlusturanKullaniciID,
                             KayitTarihi = i.KayitTarihi,
                             HizmetMerkeziAdi = _db.KurumHizmetMerkezleri.Where(x => x.KurumID == 7 && x.ID == i.HizmetMerkeziID).Select(x => x.Ad).FirstOrDefault()

                         }).FirstOrDefault();

                return talep;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public bool RandevuEkle(int ID, DateTime RandevuTarihi, int userId)
        {
            try
            {
                RandevuTalepleri i = _db.RandevuTalepleri.FirstOrDefault(x => x.ID == ID);

                i.RandevuTarihi = RandevuTarihi;
                i.RandevuVerildiMi = true;
                i.RandevuVerenKullaniciID = userId;

                _db.SaveChanges();

                if (i.ID > 0)
                {
                    #region İzlem 
                    HastaIzlemBilgileri hi = new HastaIzlemBilgileri();
                    hi.HastaID = i.HastaID;
                    hi.IzlemTarihi = DateTime.Now;
                    hi.IzlemYapanKurum = _db.KullaniciBirimBilgileri.Where(x => x.KullaniciID == userId).Select(x => x.Kurum).FirstOrDefault();
                    hi.IzlemBasligi = "Randevu oluşturuldu.";
                    hi.IzlemAciklama = "Hastaya sosyal hizmet merkezinde randevu oluşturulmuştur.";

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
        public bool RandevuDurumGuncelle(int ID, string RandevuDurumu)
        {
            try
            {
                RandevuTalepleri i = _db.RandevuTalepleri.FirstOrDefault(x => x.ID == ID);

                i.RandevuDurumu = RandevuDurumu;

                _db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }
    }
}