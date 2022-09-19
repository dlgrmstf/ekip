using EkipProjesi.Core;
using EkipProjesi.Core.Hastalar;
using EkipProjesi.Core.KurumlarArasiSevkVeTakip;
using EkipProjesi.Core.MobilEkip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkipProjesi.Data.Repositories
{
    public class MobilEkipRepository
    {
        #region Const
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private EKIPEntities _db;

        public MobilEkipRepository()
        {
            _db = new EKIPEntities();
        }
        #endregion

        #region Hastalar
        public IHastalar VakaDetay(int HastaID)
        {
            IHastalar model = new IHastalar();

            try
            {
                model = (from r in _db.Hastalar
                         where r.HastaID == HastaID
                         select new IHastalar
                         {
                             HastaID = r.HastaID,
                             HastaAdi = r.HastaAdi,
                             HastaSoyadi = r.HastaSoyadi,
                             HastaTCKimlikNo = r.HastaTCKimlikNo,
                             Telefon = r.Telefon,
                             KurumKodu = r.KurumKodu,
                             Cinsiyet = r.Cinsiyet,
                             DogumTarihi = (DateTime)r.DogumTarihi
                         }).FirstOrDefault();

                return model;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return model;
            }
        }
        public IMobilEkipVakaFormlari VakaFormEkle(IMobilEkipVakaFormlari model,int? HastaID, int UserID)
        {
            try
            {
                MobilEkipVakaFormlari m = new MobilEkipVakaFormlari();

                m.HastaID = (int)HastaID;
                m.AdliDurumDiger = model.AdliDurumDiger;
                m.AdliDurumu = model.AdliDurumu;
                m.Adres = model.Adres;
                m.AileAylikOrtGelirDuzeyi = model.AileAylikOrtGelirDuzeyi;
                m.AileSosyalYasantisi = model.AileSosyalYasantisi;
                m.AylikOrtCalistigiGunSayisi = model.AylikOrtCalistigiGunSayisi;
                m.AylikOrtGelir = model.AylikOrtGelir;
                m.AylikOrtGelirDuzeyi = model.AylikOrtGelirDuzeyi;
                m.BagimlilikEvresi = model.BagimlilikEvresi;
                m.BilgiKaynagiAkraba = model.BilgiKaynagiAkraba;
                m.BilgiKaynagiAkrabaAciklama = model.BilgiKaynagiAkrabaAciklama;
                m.BilgiKaynagiAnne = model.BilgiKaynagiAnne;
                m.BilgiKaynagiArkadas = model.BilgiKaynagiArkadas;
                m.BilgiKaynagiBaba = model.BilgiKaynagiBaba;
                m.BilgiKaynagiCocuk = model.BilgiKaynagiCocuk;
                m.BilgiKaynagiDiger = model.BilgiKaynagiDiger;
                m.BilgiKaynagiDigerAciklama = model.BilgiKaynagiDigerAciklama;
                m.BilgiKaynagiEs = model.BilgiKaynagiEs;
                m.BilgiKaynagiKardes = model.BilgiKaynagiKardes;
                m.BilgiKaynagiKendisi = model.BilgiKaynagiKendisi;
                m.Bolum = model.Bolum;
                m.CalismaBilgisiDiger = model.CalismaBilgisiDiger;
                m.CalismaSahasi = model.CalismaSahasi;
                m.CalismaSuresi = model.CalismaSuresi;
                m.CalistigiYer = model.CalistigiYer;
                m.DegerlendirmeVeSonuc = model.DegerlendirmeVeSonuc;
                m.DevamEtmemeNedeni = model.DevamEtmemeNedeni;
                m.DevamEtmemeNedeniAciklama = model.DevamEtmemeNedeniAciklama;
                m.DigerBagimlilikTuru = model.DigerBagimlilikTuru;
                m.EgitimDigerTespitler = model.EgitimDigerTespitler;
                m.EkonomikDurumDiger = model.EkonomikDurumDiger;
                m.EnSonMaddeKullanimZamani = model.EnSonMaddeKullanimZamani;
                m.EnUzunCalismaSuresi = model.EnUzunCalismaSuresi;
                m.EslikEdenBedenselHastaliklar = model.EslikEdenBedenselHastaliklar;
                m.EslikEdenRuhsalHastaliklar = model.EslikEdenRuhsalHastaliklar;
                m.Fakulte = model.Fakulte;
                m.GecimKaynagi = model.GecimKaynagi;
                m.GenelTanitim = model.GenelTanitim;
                m.IkametDiger = model.IkametDiger;
                m.IkametDurumu = model.IkametDurumu;
                m.IlkKullandigiMadde = model.IlkKullandigiMadde;
                m.Karar = model.Karar;
                m.KaydedenKullaniciID = UserID;
                m.KayitTarihi = DateTime.Now;
                m.KurumBilgisiVarMi = model.KurumBilgisiVarMi;
                m.LiseTuru = model.LiseTuru;
                m.Maas = model.Maas;
                m.MaddeDigerTespitler = model.MaddeDigerTespitler;
                m.MaddeKullanimSikligi = model.MaddeKullanimSikligi;
                m.MaddeyeBaslamaNedeni = model.MaddeyeBaslamaNedeni;
                m.MaddeyeBaslamaYasi = model.MaddeyeBaslamaYasi;
                m.MotivasyonDuzeyi = model.MotivasyonDuzeyi;
                m.MotivasyonKaynagiAile = model.MotivasyonKaynagiAile;
                m.MotivasyonKaynagiDiger = model.MotivasyonKaynagiDiger;
                m.MotivasyonKaynagiDigerAciklama = model.MotivasyonKaynagiDigerAciklama;
                m.MotivasyonKaynagiEkonomi = model.MotivasyonKaynagiEkonomi;
                m.MotivasyonKaynagiSaglik = model.MotivasyonKaynagiSaglik;
                m.Nitelik = model.Nitelik;
                m.OgrenimDurumu = model.OgrenimDurumu;
                m.PlanliTedaviPlani = model.PlanliTedaviPlani;
                m.SaglikDigerTespitler = model.SaglikDigerTespitler;
                m.SuAndaKullandigiMaddeler = model.SuAndaKullandigiMaddeler;
                m.TedaviyeYaklasimi = model.TedaviyeYaklasimi;
                m.TopluIkametDetay = model.TopluIkametDetay;
                m.YaptigiIs = model.YaptigiIs;

                _db.MobilEkipVakaFormlari.Add(m);
                _db.SaveChanges();

                if (m.ID > 0)
                {
                    #region İzlem 
                    HastaIzlemBilgileri hi = new HastaIzlemBilgileri();
                    hi.HastaID = model.HastaID;
                    hi.IzlemTarihi = DateTime.Now;
                    hi.IzlemYapanKurum = _db.KullaniciBirimBilgileri.Where(x => x.KullaniciID == UserID).Select(x => x.Kurum).FirstOrDefault();
                    hi.IzlemBasligi = "Yeni Form eklendi.";
                    hi.IzlemAciklama = "Hastaya Mobil Ekip Formu eklenmiştir.";

                    _db.HastaIzlemBilgileri.Add(hi);
                    _db.SaveChanges();
                    #endregion
                }

                return model;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return model;
            }
        }
        public List<IMobilEkipVakaFormlari> Formlar(int HastaID)
        {
            List<IMobilEkipVakaFormlari> bilgiler = new List<IMobilEkipVakaFormlari>();
            try
            {

                bilgiler = (from b in _db.MobilEkipVakaFormlari
                            where b.HastaID == HastaID
                            orderby b.KayitTarihi descending
                            select new IMobilEkipVakaFormlari
                            {
                                ID = b.ID,
                                HastaID = b.HastaID,
                                KayitTarihi = (DateTime)b.KayitTarihi,
                                KaydedenKullaniciID = (int)b.KaydedenKullaniciID,
                                Karar = (int)b.Karar

                            }).ToList();

                return bilgiler;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public IMobilEkipVakaFormlari FormDetay(int ID)
        {
            IMobilEkipVakaFormlari  bilgiler = new IMobilEkipVakaFormlari();
            try
            {

                bilgiler = (from b in _db.MobilEkipVakaFormlari
                            where b.ID == ID
                            select new IMobilEkipVakaFormlari
                            {
                               AdliDurumDiger = b.AdliDurumDiger,
                               AdliDurumu = (int)b.AdliDurumu,
                               Adres = b.Adres,
                               AileAylikOrtGelirDuzeyi = b.AileAylikOrtGelirDuzeyi,
                               AileSosyalYasantisi = b.AileSosyalYasantisi,
                               AylikOrtCalistigiGunSayisi = (int)b.AylikOrtCalistigiGunSayisi,
                               AylikOrtGelir = (int)b.AylikOrtGelir,
                               AylikOrtGelirDuzeyi = b.AylikOrtGelirDuzeyi,
                               BilgiKaynagiAkraba = b.BilgiKaynagiAkraba,
                               BilgiKaynagiAkrabaAciklama = b.BilgiKaynagiAkrabaAciklama,
                               BilgiKaynagiAnne = b.BilgiKaynagiAnne,
                               BilgiKaynagiArkadas = b.BilgiKaynagiArkadas,
                               BilgiKaynagiDigerAciklama = b.BilgiKaynagiDigerAciklama,
                               DevamEtmemeNedeniAciklama = b.DevamEtmemeNedeniAciklama,
                               MotivasyonKaynagiAile = b.MotivasyonKaynagiAile,
                               MotivasyonKaynagiDigerAciklama = b.MotivasyonKaynagiDigerAciklama,
                               SuAndaKullandigiMaddeler = b.SuAndaKullandigiMaddeler,
                               BagimlilikEvresi = (int)b.BagimlilikEvresi,
                               BilgiKaynagiBaba = b.BilgiKaynagiBaba,
                               BilgiKaynagiCocuk = b.BilgiKaynagiCocuk,
                               BilgiKaynagiDiger = b.BilgiKaynagiDiger,
                               BilgiKaynagiEs = b.BilgiKaynagiEs,
                               BilgiKaynagiKardes = b.BilgiKaynagiKardes,
                               BilgiKaynagiKendisi = b.BilgiKaynagiKendisi,
                               Bolum = b.Bolum,
                               CalismaBilgisiDiger = b.CalismaBilgisiDiger,
                               CalismaSahasi = b.CalismaSahasi,
                               CalismaSuresi = b.CalismaSuresi,
                               CalistigiYer = b.CalistigiYer,
                               DegerlendirmeVeSonuc = b.DegerlendirmeVeSonuc,
                               DevamEtmemeNedeni = (int)b.DevamEtmemeNedeni,
                               DigerBagimlilikTuru = b.DigerBagimlilikTuru,
                               EgitimDigerTespitler = b.EgitimDigerTespitler,
                               EkonomikDurumDiger = b.EkonomikDurumDiger,
                               EnSonMaddeKullanimZamani = b.EnSonMaddeKullanimZamani,
                               EnUzunCalismaSuresi = (int)b.EnUzunCalismaSuresi,
                               EslikEdenBedenselHastaliklar = b.EslikEdenBedenselHastaliklar,
                               EslikEdenRuhsalHastaliklar = b.EslikEdenRuhsalHastaliklar,
                               Fakulte = b.Fakulte,
                               GecimKaynagi = (int)b.GecimKaynagi,
                               GenelTanitim = b.GenelTanitim,
                               HastaID = b.HastaID,
                               ID = b.ID,
                               IkametDiger = b.IkametDiger,
                               IkametDurumu = (int)b.IkametDurumu,
                               IlkKullandigiMadde = b.IlkKullandigiMadde,
                               Karar = (int)b.Karar,
                               KaydedenKullaniciID = (int)b.KaydedenKullaniciID,
                               KayitTarihi = (DateTime)b.KayitTarihi,
                               KurumBilgisiVarMi = b.KurumBilgisiVarMi,
                               LiseTuru = (int)b.LiseTuru,
                               Maas = (int)b.Maas,
                               MaddeDigerTespitler = b.MaddeDigerTespitler,
                               MaddeKullanimSikligi = (int)b.MaddeKullanimSikligi,
                               MaddeyeBaslamaNedeni = b.MaddeyeBaslamaNedeni,
                               MaddeyeBaslamaYasi = b.MaddeyeBaslamaYasi,
                               MotivasyonDuzeyi = (int)b.MotivasyonDuzeyi,
                               MotivasyonKaynagiDiger = b.MotivasyonKaynagiDiger,
                               MotivasyonKaynagiEkonomi = b.MotivasyonKaynagiEkonomi,
                               MotivasyonKaynagiSaglik = b.MotivasyonKaynagiSaglik,
                               Nitelik = b.Nitelik,
                               OgrenimDurumu = (int)b.OgrenimDurumu,
                               PlanliTedaviPlani = b.PlanliTedaviPlani,
                               SaglikDigerTespitler = b.SaglikDigerTespitler,
                               TedaviyeYaklasimi = (int)b.TedaviyeYaklasimi,
                               TopluIkametDetay = b.TopluIkametDetay,
                               YaptigiIs = b.YaptigiIs

                            }).FirstOrDefault();

                return bilgiler;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public IMobilEkipVakaFormlari VakaFormGuncelle(IMobilEkipVakaFormlari model)
        {
            try
            {
                MobilEkipVakaFormlari m = _db.MobilEkipVakaFormlari.Where(x => x.ID == model.ID).FirstOrDefault();

                m.AdliDurumDiger = model.AdliDurumDiger;
                m.AdliDurumu = model.AdliDurumu;
                m.Adres = model.Adres;
                m.AileAylikOrtGelirDuzeyi = model.AileAylikOrtGelirDuzeyi;
                m.AileSosyalYasantisi = model.AileSosyalYasantisi;
                m.AylikOrtCalistigiGunSayisi = model.AylikOrtCalistigiGunSayisi;
                m.AylikOrtGelir = model.AylikOrtGelir;
                m.AylikOrtGelirDuzeyi = model.AylikOrtGelirDuzeyi;
                m.BagimlilikEvresi = model.BagimlilikEvresi;
                m.BilgiKaynagiAkraba = model.BilgiKaynagiAkraba;
                m.BilgiKaynagiAkrabaAciklama = model.BilgiKaynagiAkrabaAciklama;
                m.BilgiKaynagiAnne = model.BilgiKaynagiAnne;
                m.BilgiKaynagiArkadas = model.BilgiKaynagiArkadas;
                m.BilgiKaynagiBaba = model.BilgiKaynagiBaba;
                m.BilgiKaynagiCocuk = model.BilgiKaynagiCocuk;
                m.BilgiKaynagiDiger = model.BilgiKaynagiDiger;
                m.BilgiKaynagiDigerAciklama = model.BilgiKaynagiDigerAciklama;
                m.BilgiKaynagiEs = model.BilgiKaynagiEs;
                m.BilgiKaynagiKardes = model.BilgiKaynagiKardes;
                m.BilgiKaynagiKendisi = model.BilgiKaynagiKendisi;
                m.Bolum = model.Bolum;
                m.CalismaBilgisiDiger = model.CalismaBilgisiDiger;
                m.CalismaSahasi = model.CalismaSahasi;
                m.CalismaSuresi = model.CalismaSuresi;
                m.CalistigiYer = model.CalistigiYer;
                m.DegerlendirmeVeSonuc = model.DegerlendirmeVeSonuc;
                m.DevamEtmemeNedeni = model.DevamEtmemeNedeni;
                m.DevamEtmemeNedeniAciklama = model.DevamEtmemeNedeniAciklama;
                m.DigerBagimlilikTuru = model.DigerBagimlilikTuru;
                m.EgitimDigerTespitler = model.EgitimDigerTespitler;
                m.EkonomikDurumDiger = model.EkonomikDurumDiger;
                m.EnSonMaddeKullanimZamani = model.EnSonMaddeKullanimZamani;
                m.EnUzunCalismaSuresi = model.EnUzunCalismaSuresi;
                m.EslikEdenBedenselHastaliklar = model.EslikEdenBedenselHastaliklar;
                m.EslikEdenRuhsalHastaliklar = model.EslikEdenRuhsalHastaliklar;
                m.Fakulte = model.Fakulte;
                m.GecimKaynagi = model.GecimKaynagi;
                m.GenelTanitim = model.GenelTanitim;
                m.IkametDiger = model.IkametDiger;
                m.IkametDurumu = model.IkametDurumu;
                m.IlkKullandigiMadde = model.IlkKullandigiMadde;
                m.Karar = model.Karar;
                m.KurumBilgisiVarMi = model.KurumBilgisiVarMi;
                m.LiseTuru = model.LiseTuru;
                m.Maas = model.Maas;
                m.MaddeDigerTespitler = model.MaddeDigerTespitler;
                m.MaddeKullanimSikligi = model.MaddeKullanimSikligi;
                m.MaddeyeBaslamaNedeni = model.MaddeyeBaslamaNedeni;
                m.MaddeyeBaslamaYasi = model.MaddeyeBaslamaYasi;
                m.MotivasyonDuzeyi = model.MotivasyonDuzeyi;
                m.MotivasyonKaynagiAile = model.MotivasyonKaynagiAile;
                m.MotivasyonKaynagiDiger = model.MotivasyonKaynagiDiger;
                m.MotivasyonKaynagiDigerAciklama = model.MotivasyonKaynagiDigerAciklama;
                m.MotivasyonKaynagiEkonomi = model.MotivasyonKaynagiEkonomi;
                m.MotivasyonKaynagiSaglik = model.MotivasyonKaynagiSaglik;
                m.Nitelik = model.Nitelik;
                m.OgrenimDurumu = model.OgrenimDurumu;
                m.PlanliTedaviPlani = model.PlanliTedaviPlani;
                m.SaglikDigerTespitler = model.SaglikDigerTespitler;
                m.SuAndaKullandigiMaddeler = model.SuAndaKullandigiMaddeler;
                m.TedaviyeYaklasimi = model.TedaviyeYaklasimi;
                m.TopluIkametDetay = model.TopluIkametDetay;
                m.YaptigiIs = model.YaptigiIs;

                _db.SaveChanges();

                return model;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return model;
            }
        }
        #endregion

        #region Randevular
        public List<IRandevuTalepleri> AktifRandevuTalepleri()
        {
            List<IRandevuTalepleri> talepler = new List<IRandevuTalepleri>();
            try
            {

                talepler = (from i in _db.RandevuTalepleri
                            where i.KurumID == 9
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
                                HizmetMerkeziAdi = _db.KurumHizmetMerkezleri.Where(x => x.KurumID == 9 && x.ID == i.HizmetMerkeziID).Select(x => x.Ad).FirstOrDefault()

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
                              where i.KurumID == 9
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
                                  HizmetMerkeziAdi = _db.KurumHizmetMerkezleri.Where(x => x.KurumID == 9 && x.ID == i.HizmetMerkeziID).Select(x => x.Ad).FirstOrDefault()

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
                             HizmetMerkeziAdi = _db.KurumHizmetMerkezleri.Where(x => x.KurumID == 9 && x.ID == i.HizmetMerkeziID).Select(x => x.Ad).FirstOrDefault()

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
        #endregion
    }
}