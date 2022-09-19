using EkipProjesi.Core.IstihdamModulu;
using EkipProjesi.Core;
using EkipProjesi.Core.KurumlarArasiSevkVeTakip;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EkipProjesi.Core.Hastalar;

namespace EkipProjesi.Data.Repositories
{
    public class IstihdamRepository
    {
        #region Const
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private EKIPEntities _db;

        public IstihdamRepository()
        {
            _db = new EKIPEntities();
        }
        #endregion

        #region Istihdam Isyerleri   
        public bool IstihdamIsyeriEkle(IIstihdamIsyerleri model, int UserID)
        {
            try
            {
                IstihdamIsyerleri i = new IstihdamIsyerleri();

                i.IsyeriAdi = model.IsyeriAdi;
                i.SektorID = model.SektorID;
                i.ToplamCalisanSayisi = model.ToplamCalisanSayisi;
                i.Notlar = model.Notlar;
                i.KayitTarihi = DateTime.Now;
                i.KaydedenKullaniciID = UserID;

                _db.IstihdamIsyerleri.Add(i);
                _db.SaveChanges();

                if (i.ID > 0)
                {
                    IstihdamIsyeriAdresBilgisi ab = new IstihdamIsyeriAdresBilgisi();
                    ab.IsyeriID = i.ID;
                    ab.Sokak = model.IsyeriAdresBilgisi.Sokak;
                    ab.Cadde = model.IsyeriAdresBilgisi.Cadde;
                    ab.Bina_No = model.IsyeriAdresBilgisi.BinaNo;
                    ab.Daire_No = model.IsyeriAdresBilgisi.DaireNo;
                    if (model.IsyeriAdresBilgisi.MahalleID > 0)
                    {
                        ab.MahalleID = _db.Mahalle.Where(x => x.ID == model.IsyeriAdresBilgisi.MahalleID).FirstOrDefault().ID;
                    }
                    if (model.IsyeriAdresBilgisi.IlceID > 0)
                    {
                        ab.IlceID = _db.Ilce.Where(x => x.ID == model.IsyeriAdresBilgisi.IlceID).FirstOrDefault().ID;
                    }
                    if (model.IsyeriAdresBilgisi.IlID > 0)
                    {
                        ab.IlID = _db.Il.Where(x => x.ID == model.IsyeriAdresBilgisi.IlID).FirstOrDefault().ID;
                    }
                    _db.IstihdamIsyeriAdresBilgisi.Add(ab);
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
        public List<IIstihdamIsyerleri> IstihdamIsyerleri()
        {
            List<IIstihdamIsyerleri> isyerleri = new List<IIstihdamIsyerleri>();
            try
            {

                isyerleri = (from i in _db.IstihdamIsyerleri
                             orderby i.IsyeriAdi descending
                             select new IIstihdamIsyerleri
                             {
                                 ID = i.ID,
                                 IsyeriAdi = i.IsyeriAdi,
                                 SektorID = i.SektorID,
                                 ToplamCalisanSayisi = (int)i.ToplamCalisanSayisi,
                                 Notlar = i.Notlar,
                                 IlceBilgisi = (from a in _db.IstihdamIsyeriAdresBilgisi
                                                where i.ID == a.IsyeriID
                                                select new IIstihdamIsyeriAdresBilgisi
                                                {
                                                    IlceID = (int)a.IlceID,
                                                    IlceAd = _db.Ilce.Where(x => x.ID == a.IlceID).Select(x => x.ILCEAdi).FirstOrDefault().ToString()
                                                }).FirstOrDefault(),
                                 SektorAdi = _db.IstihdamIsyeriSektorler.Where(x => x.ID == i.SektorID).Select(x => x.SektorAdi).FirstOrDefault()
                             }).ToList();

                return isyerleri;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public List<IIstihdamIsyeriSektorler> IstihdamIsyeriSektorler()
        {
            List<IIstihdamIsyeriSektorler> sektorler = new List<IIstihdamIsyeriSektorler>();
            try
            {

                sektorler = (from i in _db.IstihdamIsyeriSektorler
                             orderby i.ID ascending
                             select new IIstihdamIsyeriSektorler
                             {
                                 ID = i.ID,
                                 SektorAdi = i.SektorAdi
                             }).ToList();

                return sektorler;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public IIstihdamIsyerleri IstihdamIsyeriDetay(int id)
        {
            IIstihdamIsyerleri isyeri = new IIstihdamIsyerleri();
            try
            {

                isyeri = (from i in _db.IstihdamIsyerleri
                          where i.ID == id
                          select new IIstihdamIsyerleri
                          {
                              ID = i.ID,
                              IsyeriAdi = i.IsyeriAdi,
                              SektorID = i.SektorID,
                              ToplamCalisanSayisi = (int)i.ToplamCalisanSayisi,
                              Notlar = i.Notlar,
                              SektorAdi = _db.IstihdamIsyeriSektorler.Where(x => x.ID == i.SektorID).Select(x => x.SektorAdi).FirstOrDefault()

                          }).FirstOrDefault();

                return isyeri;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public IIstihdamIsyeriAdresBilgisi IstihdamIsyeriAdresBilgisiDetay(int id)
        {
            try
            {
                IIstihdamIsyeriAdresBilgisi model = new IIstihdamIsyeriAdresBilgisi();
                model = (from b in _db.IstihdamIsyeriAdresBilgisi
                         where b.IsyeriID == id
                         select new IIstihdamIsyeriAdresBilgisi
                         {
                             ID = b.ID,
                             IsyeriID = b.IsyeriID,
                             BinaNo = b.Bina_No,
                             Cadde = b.Cadde,
                             DaireNo = b.Daire_No,
                             MahalleID = b.MahalleID,
                             Sokak = b.Sokak,
                             IlID = (int)b.IlID,
                             IlceID = (int)b.IlceID,

                         }).FirstOrDefault();

                model.IlAd = _db.Il.Where(x => x.KOD == model.IlID).Select(x => x.ILAdi).FirstOrDefault().ToString();
                model.IlceAd = _db.Ilce.Where(x => x.ILID == model.IlID && x.ID == model.IlceID).Select(x => x.ILCEAdi).FirstOrDefault().ToString();
                model.MahalleAd = _db.Mahalle.Where(x => x.IlceID == model.IlceID && x.ID == model.MahalleID).Select(x => x.MahalleAdi).FirstOrDefault().ToString();

                return model;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public bool IstihdamIsyeriDuzenle(IIstihdamIsyerleri model, out string hata)
        {
            hata = "";
            try
            {
                IstihdamIsyerleri i = _db.IstihdamIsyerleri.FirstOrDefault(x => x.ID == model.ID);

                i.IsyeriAdi = model.IsyeriAdi;
                i.SektorID = model.SektorID;
                i.ToplamCalisanSayisi = model.ToplamCalisanSayisi;
                i.GuncellemeTarihi = DateTime.Now;

                _db.SaveChanges();

                if (i.ID > 0)
                {
                    IstihdamIsyeriAdresBilgisi ab = _db.IstihdamIsyeriAdresBilgisi.Where(x => x.IsyeriID == i.ID).FirstOrDefault();
                    ab.Sokak = model.IsyeriAdresBilgisi.Sokak;
                    ab.Cadde = model.IsyeriAdresBilgisi.Cadde;
                    ab.Bina_No = model.IsyeriAdresBilgisi.BinaNo;
                    ab.Daire_No = model.IsyeriAdresBilgisi.DaireNo;
                    if (model.IsyeriAdresBilgisi.MahalleID > 0)
                    {
                        ab.MahalleID = _db.Mahalle.Where(x => x.ID == model.IsyeriAdresBilgisi.MahalleID).FirstOrDefault().ID;
                    }
                    if (model.IsyeriAdresBilgisi.IlceID > 0)
                    {
                        ab.IlceID = _db.Ilce.Where(x => x.ID == model.IsyeriAdresBilgisi.IlceID).FirstOrDefault().ID;
                    }
                    if (model.IsyeriAdresBilgisi.IlID > 0)
                    {
                        ab.IlID = _db.Il.Where(x => x.ID == model.IsyeriAdresBilgisi.IlID).FirstOrDefault().ID;
                    }
                    _db.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                hata = "Güncelleme İşlemi Sırasında Bir Hata Meydana Geldi." + ex.Message + " - " + ex.InnerException;
                return false;
            }
        }
        public bool IsyeriIletisimKisiEkle(IIstihdamIsyeriIletisimKisileri model, int UserID)
        {
            try
            {
                IstihdamIsyeriIletisimKisileri i = new IstihdamIsyeriIletisimKisileri();

                i.IsyeriID = model.IsyeriID;
                i.Ad = model.Ad;
                i.Soyad = model.Soyad;
                i.Unvan = model.Unvan;
                i.Adres = model.Adres;
                i.Telefon = model.Telefon;
                i.EPosta = model.EPosta;
                i.KaydedenKullaniciID = UserID;

                _db.IstihdamIsyeriIletisimKisileri.Add(i);
                _db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }
        public List<IIstihdamIsyeriIletisimKisileri> IsyeriIletisimKisileri(int id)
        {
            List<IIstihdamIsyeriIletisimKisileri> kisiler = new List<IIstihdamIsyeriIletisimKisileri>();
            try
            {

                kisiler = (from i in _db.IstihdamIsyeriIletisimKisileri
                           where i.IsyeriID == id
                           orderby i.ID descending
                           select new IIstihdamIsyeriIletisimKisileri
                           {
                               ID = i.ID,
                               IsyeriID = i.IsyeriID,
                               Ad = i.Ad,
                               Soyad = i.Soyad,
                               Unvan = i.Unvan,
                               Adres = i.Adres,
                               Telefon = i.Telefon,
                               EPosta = i.EPosta

                           }).ToList();

                return kisiler;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public bool IsyeriIletisimKisiSil(int id)
        {
            try
            {
                using (var trans = _db.Database.BeginTransaction())
                {
                    try
                    {
                        IstihdamIsyeriIletisimKisileri kisi = _db.IstihdamIsyeriIletisimKisileri.FirstOrDefault(x => x.ID == id);
                        _db.IstihdamIsyeriIletisimKisileri.Remove(kisi);
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
        public bool IsyeriGorusmeEkle(IIstihdamIsyeriGorusmeleri model, int UserID)
        {
            try
            {
                IstihdamIsyeriGorusmeleri i = new IstihdamIsyeriGorusmeleri();

                i.IsyeriID = model.IsyeriID;
                i.GorusmeyiYapanKisi = model.GorusmeyiYapanKisi;
                i.GorusulenKisi = model.GorusulenKisi;
                i.GorusmedeEleAlinanKonular = model.GorusmedeEleAlinanKonular;
                i.GorusmeTarihi = model.GorusmeTarihi;
                i.KaydedenKullaniciID = UserID;

                _db.IstihdamIsyeriGorusmeleri.Add(i);
                _db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }
        public IIstihdamIsyeriGorusmeleri GorusmeDetay(int id)
        {
            IIstihdamIsyeriGorusmeleri gorusme = new IIstihdamIsyeriGorusmeleri();
            try
            {

                gorusme = (from i in _db.IstihdamIsyeriGorusmeleri
                           where i.ID == id
                           select new IIstihdamIsyeriGorusmeleri
                           {
                               ID = i.ID,
                               GorusmedeEleAlinanKonular = i.GorusmedeEleAlinanKonular,
                               GorusmeTarihi = i.GorusmeTarihi,
                               GorusmeyiYapanKisi = i.GorusmeyiYapanKisi,
                               GorusulenKisi = i.GorusulenKisi,
                               IsyeriID = i.IsyeriID

                           }).FirstOrDefault();

                return gorusme;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public bool GorusmeGuncelle(IIstihdamIsyeriGorusmeleri model)
        {
            try
            {
                IstihdamIsyeriGorusmeleri g = _db.IstihdamIsyeriGorusmeleri.Where(x => x.ID == model.ID).FirstOrDefault();

                g.GorusulenKisi = model.GorusulenKisi;
                g.GorusmeyiYapanKisi = model.GorusmeyiYapanKisi;
                g.GorusmeTarihi = model.GorusmeTarihi;
                g.GorusmedeEleAlinanKonular = model.GorusmedeEleAlinanKonular;

                _db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }
        public bool GorusmeSil(int id)
        {
            try
            {
                using (var trans = _db.Database.BeginTransaction())
                {
                    try
                    {
                        IstihdamIsyeriGorusmeleri gorusme = _db.IstihdamIsyeriGorusmeleri.FirstOrDefault(x => x.ID == id);
                        _db.IstihdamIsyeriGorusmeleri.Remove(gorusme);
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
        public List<IIstihdamIsyeriGorusmeleri> IsyeriGorusmeleri(int id)
        {
            List<IIstihdamIsyeriGorusmeleri> gorusmeler = new List<IIstihdamIsyeriGorusmeleri>();
            try
            {

                gorusmeler = (from i in _db.IstihdamIsyeriGorusmeleri
                              where i.IsyeriID == id
                              orderby i.ID descending
                              select new IIstihdamIsyeriGorusmeleri
                              {
                                  ID = i.ID,
                                  IsyeriID = i.IsyeriID,
                                  GorusmeTarihi = i.GorusmeTarihi,
                                  GorusmedeEleAlinanKonular = i.GorusmedeEleAlinanKonular,
                                  GorusulenKisi = i.GorusulenKisi,
                                  GorusmeyiYapanKisi = i.GorusmeyiYapanKisi

                              }).ToList();

                return gorusmeler;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        #endregion

        #region Iskur Islemleri
        public List<IRandevuTalepleri> AktifRandevuTalepleri()
        {
            List<IRandevuTalepleri> talepler = new List<IRandevuTalepleri>();
            try
            {

                talepler = (from i in _db.RandevuTalepleri
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
                                RandevuTalebiNotu = i.RandevuTalebiNotu,
                                RandevuVerildiMi = (bool)i.RandevuVerildiMi,
                                TalepOlusturanKullaniciID = (int)i.TalepOlusturanKullaniciID,
                                KayitTarihi = i.KayitTarihi,
                                HizmetMerkeziAdi = _db.KurumHizmetMerkezleri.Where(x => x.KurumID == 5 && x.ID == i.HizmetMerkeziID).Select(x => x.Ad).FirstOrDefault()

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
                                  RandevuTalebiNotu = i.RandevuTalebiNotu,
                                  RandevuVerildiMi = (bool)i.RandevuVerildiMi,
                                  TalepOlusturanKullaniciID = (int)i.TalepOlusturanKullaniciID,
                                  KayitTarihi = i.KayitTarihi,
                                  RandevuTarihi = (DateTime)i.RandevuTarihi,
                                  RandevuVerenKullaniciID = (int)i.RandevuVerenKullaniciID,
                                  RandevuDurumu = i.RandevuDurumu,
                                  HizmetMerkeziAdi = _db.KurumHizmetMerkezleri.Where(x => x.KurumID == 5 && x.ID == i.HizmetMerkeziID).Select(x => x.Ad).FirstOrDefault()

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
                             HizmetMerkeziAdi = _db.KurumHizmetMerkezleri.Where(x => x.KurumID == 5 && x.ID == i.HizmetMerkeziID).Select(x => x.Ad).FirstOrDefault()

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
                    hi.IzlemAciklama = "Hastaya İŞKUR hizmet merkezinde randevu oluşturulmuştur.";

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
        public bool KayitEkle(IHastalar model, int UserID)
        {
            try
            {
                var kontrol = _db.Hastalar.Where(x => x.HastaTCKimlikNo == model.HastaTCKimlikNo).Any();

                if (!kontrol)
                {
                    Hastalar i = new Hastalar();

                    i.HastaAdi = model.HastaAdi;
                    i.HastaSoyadi = model.HastaSoyadi;
                    i.HastaTCKimlikNo = model.HastaTCKimlikNo;
                    i.DogumTarihi = model.DogumTarihi;
                    i.Telefon = model.Telefon;
                    i.Adres = model.Adres;
                    i.Email = model.Email;
                    i.KurumKodu = model.KurumKodu;
                    i.KayitTarihi = DateTime.Now;
                    i.KaydedenKullaniciID = UserID;

                    _db.Hastalar.Add(i);
                    _db.SaveChanges();

                    model.HastaID = i.HastaID;

                    i.HastaEkipNo = model.HastaID.ToString();
                    _db.SaveChanges();

                    if (i.HastaID > 0)
                    {
                        HastaEgitimBilgileri he = new HastaEgitimBilgileri();

                        he.HastaID = i.HastaID;
                        he.OgrenimDurumu = model.EgitimBilgileri.OgrenimDurumu;
                        he.Lise = model.EgitimBilgileri.Lise;
                        he.LiseTuru = model.EgitimBilgileri.LiseTuru;
                        he.Ilkokul = model.EgitimBilgileri.Ilkokul;
                        he.Ortaokul = model.EgitimBilgileri.Ortaokul;
                        he.Universite = model.EgitimBilgileri.Universite;
                        he.Bolum = model.EgitimBilgileri.Bolum;
                        he.Fakulte = model.EgitimBilgileri.Fakulte;
                        he.IsGecmisi = model.EgitimBilgileri.IsGecmisi;
                        he.SertifikaVeYeterlilikler = model.EgitimBilgileri.SertifikaVeYeterlilikler;
                        he.KayitTarihi = DateTime.Now;
                        he.KaydedenKullaniciID = UserID;

                        _db.HastaEgitimBilgileri.Add(he);
                        _db.SaveChanges();

                        HastaIlkKayitBilgileri hi = new HastaIlkKayitBilgileri();

                        hi.HastaID = i.HastaID;
                        hi.KaydedenKurum = (from x in _db.Kurumlar where x.KurumKodu == i.KurumKodu select x.KurumAdi).FirstOrDefault();
                        hi.KayitTarihi = DateTime.Now;

                        _db.HastaIlkKayitBilgileri.Add(hi);
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
        public bool IskurIslemleriKullaniciGorusmeEkle(IHastaIskurGorusmeleri model, int UserID, int? KayitID)
        {
            try
            {
                HastaIskurGorusmeleri i = new HastaIskurGorusmeleri();

                i.GorusmeyiYapanDanisman = model.GorusmeyiYapanDanisman;
                i.HastaID = (int)KayitID;
                i.GorusmeTarihi = model.GorusmeTarihi;
                i.GorusmeSonucu = model.GorusmeSonucu;
                i.YonlendirilenIsyeri = model.YonlendirilenIsyeri;
                i.YonlendirmeBasvuruTarihi = model.YonlendirmeBasvuruTarihi;
                i.IseBasladigiYer = model.IseBasladigiYer;
                i.IseBaslamaTarihi = model.IseBaslamaTarihi;
                i.IsyeriYonlendirmeSonuc = model.IsyeriYonlendirmeSonuc;
                i.IsKulubuYonlendirmeSonuc = model.IsKulubuYonlendirmeSonuc;
                i.PlanlananProgramBaslangicTarihi = model.PlanlananProgramBaslangicTarihi;
                i.PlanlananSurecBaslangicTarihi = model.PlanlananSurecBaslangicTarihi;
                i.PlanlananProgramTuru = model.PlanlananProgramTuru;
                i.SurecBaslamaTarihi = model.SurecBaslamaTarihi;
                i.SurecTamamlanmaTarihi = model.SurecTamamlanmaTarihi;
                i.SertifikaDosyaYolu = model.SertifikaDosyaYolu;
                i.YonlendirilenKursTuru = model.YonlendirilenKursTuru;
                i.YonlendirilenKursBilgisiTuru = model.YonlendirilenKursBilgisiTuru;
                i.GorusmeSonucuDiger = model.GorusmeSonucuDiger;
                i.GorusmeKayitTarihi = DateTime.Now;
                i.KaydedenKullaniciID = UserID;
                i.IseBaslamaNedeni = model.IseBaslamaNedeni;
                i.PlanlananBaslangicTarihi = model.PlanlananBaslangicTarihi;
                i.IsKulubuYonlendirmeDurum = model.IsKulubuYonlendirmeDurum;
                i.Tur = model.Tur;
                i.ProgramDurum = model.ProgramDurum;
                i.PlanlananProgramBaslangici = model.PlanlananProgramBaslangici;
                i.ProgramTuru = model.ProgramTuru;
                i.ProgramTuruTamamlanmaTarihi = model.ProgramTuruTamamlanmaTarihi;

                _db.HastaIskurGorusmeleri.Add(i);
                _db.SaveChanges();

                if (i.ID > 0)
                {
                    #region İzlem 
                    HastaIzlemBilgileri hi = new HastaIzlemBilgileri();
                    hi.HastaID = i.HastaID;
                    hi.IzlemTarihi = DateTime.Now;
                    hi.IzlemYapanKurum = _db.KullaniciBirimBilgileri.Where(x => x.KullaniciID == UserID).Select(x => x.Kurum).FirstOrDefault();
                    hi.IzlemBasligi = "İşkur görüşmesi eklendi.";
                    hi.IzlemAciklama = "Hasta ile İŞKUR görüşmesi yapılmıştır.";

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
        public List<IHastaIskurGorusmeleri> IskurIslemleriKullaniciGorusmeleri(int id)
        {
            List<IHastaIskurGorusmeleri> gorusmeler = new List<IHastaIskurGorusmeleri>();
            try
            {

                gorusmeler = (from i in _db.HastaIskurGorusmeleri
                              where i.HastaID == id
                              orderby i.GorusmeTarihi descending
                              select new IHastaIskurGorusmeleri
                              {
                                  ID = i.ID,
                                  GorusmeKayitTarihi = (DateTime)i.GorusmeKayitTarihi,
                                  GorusmeTarihi = i.GorusmeTarihi,
                                  HastaID = i.HastaID,
                                  GorusmeyiYapanDanisman = i.GorusmeyiYapanDanisman

                              }).ToList();

                return gorusmeler;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public IHastaIskurGorusmeleri IskurIslemleriKullaniciGorusmeDetay(int id)
        {
            IHastaIskurGorusmeleri gorusme = new IHastaIskurGorusmeleri();
            try
            {

                gorusme = (from i in _db.HastaIskurGorusmeleri
                           where i.ID == id
                           select new IHastaIskurGorusmeleri
                           {
                               ID = i.ID,
                               GorusmeKayitTarihi = (DateTime)i.GorusmeKayitTarihi,
                               HastaID = i.HastaID,
                               GorusmeyiYapanDanisman = i.GorusmeyiYapanDanisman,
                               GorusmeSonucu = i.GorusmeSonucu,
                               GorusmeSonucuDiger = i.GorusmeSonucuDiger,
                               GorusmeTarihi = i.GorusmeTarihi,
                               IseBasladigiYer = i.IseBasladigiYer,
                               IseBaslamaNedeni = i.IseBaslamaNedeni,
                               IseBaslamaTarihi = i.IseBaslamaTarihi,
                               IsKulubuYonlendirmeDurum = (int)i.IsKulubuYonlendirmeDurum,
                               IsKulubuYonlendirmeSonuc = i.IsKulubuYonlendirmeSonuc,
                               IsyeriYonlendirmeSonuc = i.IsyeriYonlendirmeSonuc,
                               KaydedenKullaniciID = (int)i.KaydedenKullaniciID,
                               PlanlananBaslangicTarihi = (DateTime)i.PlanlananBaslangicTarihi,
                               PlanlananProgramBaslangici = (DateTime)i.PlanlananProgramBaslangici,
                               PlanlananProgramBaslangicTarihi = i.PlanlananProgramBaslangicTarihi,
                               PlanlananProgramTuru = i.PlanlananProgramTuru,
                               PlanlananSurecBaslangicTarihi = i.PlanlananSurecBaslangicTarihi,
                               ProgramDurum = (int)i.ProgramDurum,
                               ProgramTuru = (int)i.ProgramTuru,
                               ProgramTuruTamamlanmaTarihi = (DateTime)i.ProgramTuruTamamlanmaTarihi,
                               SertifikaDosyaYolu = i.SertifikaDosyaYolu,
                               SurecBaslamaTarihi = i.SurecBaslamaTarihi,
                               SurecTamamlanmaTarihi = i.SurecTamamlanmaTarihi,
                               Tur = (int)i.Tur,
                               YonlendirilenIsyeri = i.YonlendirilenIsyeri,
                               YonlendirilenKursBilgisiTuru = i.YonlendirilenKursBilgisiTuru,
                               YonlendirilenKursTuru = i.YonlendirilenKursTuru,
                               YonlendirmeBasvuruTarihi = i.YonlendirmeBasvuruTarihi

                           }).FirstOrDefault();

                return gorusme;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public IHastaEgitimBilgileri HastaEgitimBilgileri(int id)
        {
            IHastaEgitimBilgileri bilgiler = new IHastaEgitimBilgileri();
            try
            {

                bilgiler = (from i in _db.HastaEgitimBilgileri
                            where i.HastaID == id
                            select new IHastaEgitimBilgileri
                            {
                                ID = i.ID,
                                HastaID = i.HastaID,
                                Bolum = i.Bolum,
                                Fakulte = i.Fakulte,
                                GuncelleyenKullaniciID = i.GuncelleyenKullaniciID,
                                Ilkokul = i.Ilkokul,
                                IsGecmisi = i.IsGecmisi,
                                KaydedenKullaniciID = i.KaydedenKullaniciID,
                                KayitTarihi = (DateTime)i.KayitTarihi,
                                Lise = i.Lise,
                                LiseTuru = i.LiseTuru,
                                OgrenimDurumu = i.OgrenimDurumu,
                                Ortaokul = i.Ortaokul,
                                SertifikaVeYeterlilikler = i.SertifikaVeYeterlilikler,
                                Universite = i.Universite

                            }).FirstOrDefault();

                return bilgiler;
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