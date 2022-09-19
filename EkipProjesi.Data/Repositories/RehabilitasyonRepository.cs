using EkipProjesi.Core.KurumlarArasiSevkVeTakip;
using EkipProjesi.Core.Randevu;
using EkipProjesi.Core.RehabilitasyonMerkezleri;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkipProjesi.Data.Repositories
{
    public class RehabilitasyonRepository
    {
        #region Const
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private EKIPEntities _db;

        public RehabilitasyonRepository()
        {
            _db = new EKIPEntities();
        }
        #endregion
        public List<IHastaBaharFormuSonucKodlari> Kodlar()
        {
            List<IHastaBaharFormuSonucKodlari> model = new List<IHastaBaharFormuSonucKodlari>();
            try
            {
                model = (from h in _db.HastaBaharFormuSonucKodlari
                         orderby h.ID
                         select new IHastaBaharFormuSonucKodlari
                         {
                             ID = h.ID,
                             Kod = h.Kod,
                             Aciklama = h.Aciklama
                         }).ToList();
                return model;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return model;
            }
        }
        public bool BaharFormuEkle(IHastaBaharFormlari model, int UserID)
        {
            try
            {
                using (var trans = _db.Database.BeginTransaction())
                {
                    #region Form Bilgisi
                    HastaBaharFormlari h = new HastaBaharFormlari();
                    h.Adres = model.Adres;
                    h.Antabus = model.Antabus;
                    h.AntabusAciklama = model.AntabusAciklama;
                    h.BelirlenenTerapist = model.BelirlenenTerapist;
                    h.CalismaDurumu = model.CalismaDurumu;
                    h.Campral = model.Campral;
                    h.CampralAciklama = model.CampralAciklama;
                    h.EkonomikDurumu = model.EkonomikDurumu;
                    h.Ethylex = model.Ethylex;
                    h.EthylexAciklama = model.EthylexAciklama;
                    h.GelisSekli = model.GelisSekli;
                    h.GorusmeninYapildigiMerkez = model.GorusmeninYapildigiMerkez;
                    h.HastaID = model.HastaID;
                    h.Hedefler = model.Hedefler;
                    h.NaltreksonImplant = model.NaltreksonImplant;
                    h.NaltreksonImplantAciklama = model.NaltreksonImplantAciklama;
                    h.PlanlananProgramSuresi = model.PlanlananProgramSuresi;
                    h.PlanlananRehabilitasyonProgrami = model.PlanlananRehabilitasyonProgrami;
                    h.ProgramGunleri = model.ProgramGunleri;
                    #region S1
                    h.S1AlkolluIcecekler = model.S1AlkolluIcecekler;
                    h.S1AmfctaminTipiUyaricilar = model.S1AmfctaminTipiUyaricilar;
                    h.S1Diger = model.S1Diger;
                    h.S1DigerAciklama = model.S1DigerAciklama;
                    h.S1Esrar = model.S1Esrar;
                    h.S1Halüsinojenler = model.S1Halüsinojenler;
                    h.S1Inhalanlar = model.S1Inhalanlar;
                    h.S1Kokain = model.S1Kokain;
                    h.S1Opioidler = model.S1Opioidler;
                    h.S1Sakinlestiriciler = model.S1Sakinlestiriciler;
                    h.S1TutunUrunleri = model.S1TutunUrunleri;
                    #endregion
                    #region S2
                    h.S2AlkolluIcecekler = model.S2AlkolluIcecekler;
                    h.S2AmfctaminTipiUyaricilar = model.S2AmfctaminTipiUyaricilar;
                    h.S2Diger = model.S2Diger;
                    h.S2DigerAciklama = model.S2DigerAciklama;
                    h.S2Esrar = model.S2Esrar;
                    h.S2Halüsinojenler = model.S2Halüsinojenler;
                    h.S2Inhalanlar = model.S2Inhalanlar;
                    h.S2Kokain = model.S2Kokain;
                    h.S2Opioidler = model.S2Opioidler;
                    h.S2Sakinlestiriciler = model.S2Sakinlestiriciler;
                    h.S2TutunUrunleri = model.S2TutunUrunleri;
                    #endregion
                    #region S3
                    h.S3AlkolluIcecekler = model.S3AlkolluIcecekler;
                    h.S3AmfctaminTipiUyaricilar = model.S3AmfctaminTipiUyaricilar;
                    h.S3Diger = model.S3Diger;
                    h.S3DigerAciklama = model.S3DigerAciklama;
                    h.S3Esrar = model.S3Esrar;
                    h.S3Halüsinojenler = model.S3Halüsinojenler;
                    h.S3Inhalanlar = model.S3Inhalanlar;
                    h.S3Kokain = model.S3Kokain;
                    h.S3Opioidler = model.S3Opioidler;
                    h.S3Sakinlestiriciler = model.S3Sakinlestiriciler;
                    h.S3TutunUrunleri = model.S3TutunUrunleri;
                    #endregion
                    #region S4
                    h.S4AlkolluIcecekler = model.S4AlkolluIcecekler;
                    h.S4AmfctaminTipiUyaricilar = model.S4AmfctaminTipiUyaricilar;
                    h.S4Diger = model.S4Diger;
                    h.S4DigerAciklama = model.S4DigerAciklama;
                    h.S4Esrar = model.S4Esrar;
                    h.S4Halüsinojenler = model.S4Halüsinojenler;
                    h.S4Inhalanlar = model.S4Inhalanlar;
                    h.S4Kokain = model.S4Kokain;
                    h.S4Opioidler = model.S4Opioidler;
                    h.S4Sakinlestiriciler = model.S4Sakinlestiriciler;
                    h.S4TutunUrunleri = model.S4TutunUrunleri;
                    #endregion
                    #region S5
                    h.S5AlkolluIcecekler = model.S5AlkolluIcecekler;
                    h.S5AmfctaminTipiUyaricilar = model.S5AmfctaminTipiUyaricilar;
                    h.S5Diger = model.S5Diger;
                    h.S5DigerAciklama = model.S5DigerAciklama;
                    h.S5Esrar = model.S5Esrar;
                    h.S5Halüsinojenler = model.S5Halüsinojenler;
                    h.S5Inhalanlar = model.S5Inhalanlar;
                    h.S5Kokain = model.S5Kokain;
                    h.S5Opioidler = model.S5Opioidler;
                    h.S5Sakinlestiriciler = model.S5Sakinlestiriciler;
                    h.S5TutunUrunleri = model.S5TutunUrunleri;
                    #endregion
                    #region S6
                    h.S6AlkolluIcecekler = model.S6AlkolluIcecekler;
                    h.S6AmfctaminTipiUyaricilar = model.S6AmfctaminTipiUyaricilar;
                    h.S6Diger = model.S6Diger;
                    h.S6DigerAciklama = model.S6DigerAciklama;
                    h.S6Esrar = model.S6Esrar;
                    h.S6Halüsinojenler = model.S6Halüsinojenler;
                    h.S6Inhalanlar = model.S6Inhalanlar;
                    h.S6Kokain = model.S6Kokain;
                    h.S6Opioidler = model.S6Opioidler;
                    h.S6Sakinlestiriciler = model.S6Sakinlestiriciler;
                    h.S6TutunUrunleri = model.S6TutunUrunleri;
                    #endregion
                    h.SonTedaviBasvuruNedeni = model.SonTedaviBasvuruNedeni;
                    h.Suboxone = model.Suboxone;
                    h.SuboxoneAciklama = model.SuboxoneAciklama;
                    h.TamAyiklikBaslamaZamani = model.TamAyiklikBaslamaZamani;
                    h.TedaviAciklama = model.TedaviAciklama;
                    h.TedaviBasvuruNedeni = model.TedaviBasvuruNedeni;
                    h.Telefon = model.Telefon;
                    h.YakinAdiSoyadi = model.YakinAdiSoyadi;
                    h.YakinTelefonu = model.YakinTelefonu;
                    h.YasadigiIl = model.YasadigiIl;
                    h.YasadigiIlce = model.YasadigiIlce;
                    h.KaydedenKullaniciID = UserID;
                    h.KayitTarihi = DateTime.Now;

                    _db.HastaBaharFormlari.Add(h);
                    _db.SaveChanges();

                    model.ID = h.ID;
                    #endregion

                    if (h.ID > 0)
                    {
                        #region Sonuc Kodlari
                        if (model.FormSonuc != null)
                        {
                            foreach (var sonuc in model.FormSonuc)
                            {
                                HastaBaharFormuSonuc bf = new HastaBaharFormuSonuc();
                                bf.FormID = h.ID;
                                bf.KodID = sonuc;

                                _db.HastaBaharFormuSonuc.Add(bf);
                                _db.SaveChanges();
                            }
                        }
                        #endregion
                        #region İzlem 
                        HastaIzlemBilgileri hi = new HastaIzlemBilgileri();
                        hi.HastaID = model.HastaID;
                        hi.IzlemTarihi = DateTime.Now;
                        hi.IzlemYapanKurum = _db.KullaniciBirimBilgileri.Where(x => x.KullaniciID == UserID).Select(x => x.Kurum).FirstOrDefault();
                        hi.IzlemBasligi = "Form eklendi.";
                        hi.IzlemAciklama = "Hastaya BAHAR Formu eklenmiştir.";

                        _db.HastaIzlemBilgileri.Add(hi);
                        _db.SaveChanges();
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
        public List<IHastaBaharFormlari> BaharFormlari(int id)
        {
            List<IHastaBaharFormlari> model = new List<IHastaBaharFormlari>();
            try
            {
                model = (from h in _db.HastaBaharFormlari
                         where h.HastaID == id
                         orderby h.ID descending
                         select new IHastaBaharFormlari
                         {
                             ID = h.ID,
                             KayitTarihi = (DateTime)h.KayitTarihi,
                             KaydedenKullaniciID = (int)h.KaydedenKullaniciID,
                             HastaID = h.HastaID
                         }).ToList();
                return model;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return model;
            }
        }
        public IHastaBaharFormlari BaharFormDetay(int id)
        {
            IHastaBaharFormlari model = new IHastaBaharFormlari();
            try
            {
                model = (from h in _db.HastaBaharFormlari
                         where h.ID == id
                         select new IHastaBaharFormlari
                         {
                             ID = h.ID,
                             KayitTarihi = (DateTime)h.KayitTarihi,
                             KaydedenKullaniciID = (int)h.KaydedenKullaniciID,
                             Adres = h.Adres,
                             Antabus = h.Antabus,
                             AntabusAciklama = h.AntabusAciklama,
                             BelirlenenTerapist = h.BelirlenenTerapist,
                             CalismaDurumu = (int)h.CalismaDurumu,
                             Campral = h.Campral,
                             CampralAciklama = h.CampralAciklama,
                             EkonomikDurumu = (int)h.EkonomikDurumu,
                             Ethylex = h.Ethylex,
                             EthylexAciklama = h.EthylexAciklama,
                             GelisSekli = (int)h.GelisSekli,
                             GorusmeninYapildigiMerkez = (int)h.GorusmeninYapildigiMerkez,
                             HastaID = h.HastaID,
                             Hedefler = h.Hedefler,
                             NaltreksonImplant = h.NaltreksonImplant,
                             NaltreksonImplantAciklama = h.NaltreksonImplantAciklama,
                             PlanlananProgramSuresi = (int)h.PlanlananProgramSuresi,
                             PlanlananRehabilitasyonProgrami = (int)h.PlanlananRehabilitasyonProgrami,
                             ProgramGunleri = (int)h.ProgramGunleri,
                             SonTedaviBasvuruNedeni = h.SonTedaviBasvuruNedeni,
                             Suboxone = h.Suboxone,
                             SuboxoneAciklama = h.SuboxoneAciklama,
                             TamAyiklikBaslamaZamani = h.TamAyiklikBaslamaZamani,
                             TedaviAciklama = h.TedaviAciklama,
                             TedaviBasvuruNedeni = (int)h.TedaviBasvuruNedeni,
                             Telefon = h.Telefon,
                             YakinAdiSoyadi = h.YakinAdiSoyadi,
                             YakinTelefonu = h.YakinTelefonu,
                             YasadigiIl = h.YasadigiIl,
                             YasadigiIlce = h.YasadigiIlce,
                             S1AlkolluIcecekler = (int)h.S1AlkolluIcecekler,
                             S1AmfctaminTipiUyaricilar = (int)h.S1AmfctaminTipiUyaricilar,
                             S1Diger = (int)h.S1Diger,
                             S1DigerAciklama = h.S1DigerAciklama,
                             S1Esrar = (int)h.S1Esrar,
                             S1Halüsinojenler = (int)h.S1Halüsinojenler,
                             S1Inhalanlar = (int)h.S1Inhalanlar,
                             S1Kokain = (int)h.S1Kokain,
                             S1Opioidler = (int)h.S1Opioidler,
                             S1Sakinlestiriciler = (int)h.S1Sakinlestiriciler,
                             S1TutunUrunleri = (int)h.S1TutunUrunleri,
                             S2AlkolluIcecekler = (int)h.S2AlkolluIcecekler,
                             S2AmfctaminTipiUyaricilar = (int)h.S2AmfctaminTipiUyaricilar,
                             S2Diger = (int)h.S2Diger,
                             S2DigerAciklama = h.S2DigerAciklama,
                             S2Esrar = (int)h.S2Esrar,
                             S2Halüsinojenler = (int)h.S2Halüsinojenler,
                             S2Inhalanlar = (int)h.S2Inhalanlar,
                             S2Kokain = (int)h.S2Kokain,
                             S2Opioidler = (int)h.S2Opioidler,
                             S2Sakinlestiriciler = (int)h.S2Sakinlestiriciler,
                             S2TutunUrunleri = (int)h.S2TutunUrunleri,
                             S3AlkolluIcecekler = (int)h.S3AlkolluIcecekler,
                             S3AmfctaminTipiUyaricilar = (int)h.S3AmfctaminTipiUyaricilar,
                             S3Diger = (int)h.S3Diger,
                             S3DigerAciklama = h.S3DigerAciklama,
                             S3Esrar = (int)h.S3Esrar,
                             S3Halüsinojenler = (int)h.S3Halüsinojenler,
                             S3Inhalanlar = (int)h.S3Inhalanlar,
                             S3Kokain = (int)h.S3Kokain,
                             S3Opioidler = (int)h.S3Opioidler,
                             S3Sakinlestiriciler = (int)h.S3Sakinlestiriciler,
                             S3TutunUrunleri = (int)h.S3TutunUrunleri,
                             S4AlkolluIcecekler = (int)h.S4AlkolluIcecekler,
                             S4AmfctaminTipiUyaricilar = (int)h.S4AmfctaminTipiUyaricilar,
                             S4Diger = (int)h.S4Diger,
                             S4DigerAciklama = h.S4DigerAciklama,
                             S4Esrar = (int)h.S4Esrar,
                             S4Halüsinojenler = (int)h.S4Halüsinojenler,
                             S4Inhalanlar = (int)h.S4Inhalanlar,
                             S4Kokain = (int)h.S4Kokain,
                             S4Opioidler = (int)h.S4Opioidler,
                             S4Sakinlestiriciler = (int)h.S4Sakinlestiriciler,
                             S4TutunUrunleri = (int)h.S4TutunUrunleri,
                             S5AlkolluIcecekler = (int)h.S5AlkolluIcecekler,
                             S5AmfctaminTipiUyaricilar = (int)h.S5AmfctaminTipiUyaricilar,
                             S5Diger = (int)h.S5Diger,
                             S5DigerAciklama = h.S5DigerAciklama,
                             S5Esrar = (int)h.S5Esrar,
                             S5Halüsinojenler = (int)h.S5Halüsinojenler,
                             S5Inhalanlar = (int)h.S5Inhalanlar,
                             S5Kokain = (int)h.S5Kokain,
                             S5Opioidler = (int)h.S5Opioidler,
                             S5Sakinlestiriciler = (int)h.S5Sakinlestiriciler,
                             S5TutunUrunleri = (int)h.S5TutunUrunleri,
                             S6AlkolluIcecekler = (int)h.S6AlkolluIcecekler,
                             S6AmfctaminTipiUyaricilar = (int)h.S6AmfctaminTipiUyaricilar,
                             S6Diger = (int)h.S6Diger,
                             S6DigerAciklama = h.S6DigerAciklama,
                             S6Esrar = (int)h.S6Esrar,
                             S6Halüsinojenler = (int)h.S6Halüsinojenler,
                             S6Inhalanlar = (int)h.S6Inhalanlar,
                             S6Kokain = (int)h.S6Kokain,
                             S6Opioidler = (int)h.S6Opioidler,
                             S6Sakinlestiriciler = (int)h.S6Sakinlestiriciler,
                             S6TutunUrunleri = (int)h.S6TutunUrunleri

                         }).FirstOrDefault();
                return model;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return model;
            }
        }
        public List<IHastaBaharFormuSonuc> BaharFormSonucDetay(int id)
        {
            List<IHastaBaharFormuSonuc> model = new List<IHastaBaharFormuSonuc>();
            try
            {
                model = (from h in _db.HastaBaharFormuSonuc
                         where h.FormID == id
                         select new IHastaBaharFormuSonuc
                         {
                             ID = h.ID,
                             KodID = h.KodID,
                             KodAdi = _db.HastaBaharFormuSonucKodlari.Where(x => x.ID == h.KodID).Select(x => x.Aciklama).FirstOrDefault()
                         }).ToList();
                return model;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return model;
            }
        }
        public bool OlceklerFormuEkle(IHastaOlceklerFormu model, int UserID)
        {
            try
            {
                using (var trans = _db.Database.BeginTransaction())
                {
                    #region Form Bilgisi
                    HastaOlceklerFormu h = new HastaOlceklerFormu();

                    h.HastaID = model.HastaID;
                    h.DikkatEksikligi = model.DikkatEksikligi;
                    h.AsiriHareketlilik = model.AsiriHareketlilik;
                    h.UykuSoru1 = model.UykuSoru1;
                    h.UykuSoru2 = model.UykuSoru2;
                    h.UykuSoru3 = model.UykuSoru3;
                    h.UykuSoru4 = model.UykuSoru4;
                    h.UykuSoru5 = model.UykuSoru5;
                    h.UykuSoru6 = model.UykuSoru6;
                    h.UykuSoru7 = model.UykuSoru7;
                    h.UykuSoru8 = model.UykuSoru8;
                    h.UykuSoru9 = model.UykuSoru9;
                    h.UykuSoru10 = model.UykuSoru10;
                    h.UykuBozukluguTanisi = model.UykuBozukluguTanisi;
                    h.KadinFormu1 = model.KadinFormu1;
                    h.KadinFormu2 = model.KadinFormu2;
                    h.KadinFormu3 = model.KadinFormu3;
                    h.KadinFormu4 = model.KadinFormu4;
                    h.KadinFormu5 = model.KadinFormu5;
                    h.ErkekFormu1 = model.ErkekFormu1;
                    h.ErkekFormu2 = model.ErkekFormu2;
                    h.ErkekFormu3 = model.ErkekFormu3;
                    h.ErkekFormu4 = model.ErkekFormu4;
                    h.ErkekFormu5 = model.ErkekFormu5;
                    h.TravmatikYasam1 = model.TravmatikYasam1;
                    h.TravmatikYasam2 = model.TravmatikYasam2;
                    h.TravmatikYasam3 = model.TravmatikYasam3;
                    h.TravmatikYasam4 = model.TravmatikYasam4;
                    h.TravmatikYasam5 = model.TravmatikYasam5;
                    h.TravmatikYasam6 = model.TravmatikYasam6;
                    h.TravmatikYasam7 = model.TravmatikYasam7;
                    h.TravmatikYasam8 = model.TravmatikYasam8;
                    h.TravmatikYasam9 = model.TravmatikYasam9;
                    h.TravmatikYasam10 = model.TravmatikYasam10;
                    h.TravmatikYasam11 = model.TravmatikYasam11;
                    h.TravmatikYasam12 = model.TravmatikYasam12;
                    h.TravmatikYasam13 = model.TravmatikYasam13;
                    h.TravmatikYasam14 = model.TravmatikYasam14;
                    h.TravmatikYasam14Aciklama = model.TravmatikYasam14Aciklama;
                    h.Appendix1 = model.Appendix1;
                    h.Appendix2 = model.Appendix2;
                    h.Appendix3 = model.Appendix3;
                    h.Appendix4 = model.Appendix4;
                    h.Appendix5 = model.Appendix5;
                    h.Appendix6 = model.Appendix6;
                    h.Appendix7 = model.Appendix7;
                    h.Appendix8 = model.Appendix8;
                    h.Appendix9 = model.Appendix9;
                    h.Appendix10 = model.Appendix10;
                    h.Appendix11 = model.Appendix11;
                    h.Appendix12 = model.Appendix12;
                    h.Appendix13 = model.Appendix13;
                    h.Appendix14 = model.Appendix14;
                    h.Appendix15 = model.Appendix15;
                    h.Appendix16 = model.Appendix16;
                    h.Appendix17 = model.Appendix17;
                    h.Appendix18 = model.Appendix18;
                    h.Appendix19 = model.Appendix19;
                    h.Appendix20 = model.Appendix20;
                    h.KaydedenKullaniciID = UserID;
                    h.KayitTarihi = DateTime.Now;

                    _db.HastaOlceklerFormu.Add(h);
                    _db.SaveChanges();

                    model.ID = h.ID;
                    #endregion

                    if (h.ID > 0)
                    {
                        #region İzlem 
                        HastaIzlemBilgileri hi = new HastaIzlemBilgileri();
                        hi.HastaID = model.HastaID;
                        hi.IzlemTarihi = DateTime.Now;
                        hi.IzlemYapanKurum = _db.KullaniciBirimBilgileri.Where(x => x.KullaniciID == UserID).Select(x => x.Kurum).FirstOrDefault();
                        hi.IzlemBasligi = "Form eklendi.";
                        hi.IzlemAciklama = "Hastaya Ölçekler Formu eklenmiştir.";

                        _db.HastaIzlemBilgileri.Add(hi);
                        _db.SaveChanges();
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
        public List<IHastaOlceklerFormu> OlceklerFormlari(int id)
        {
            List<IHastaOlceklerFormu> model = new List<IHastaOlceklerFormu>();
            try
            {
                model = (from h in _db.HastaOlceklerFormu
                         where h.HastaID == id
                         orderby h.ID descending
                         select new IHastaOlceklerFormu
                         {
                             ID = h.ID,
                             KayitTarihi = (DateTime)h.KayitTarihi,
                             KaydedenKullaniciID = (int)h.KaydedenKullaniciID,
                             HastaID = h.HastaID
                         }).ToList();
                return model;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return model;
            }
        }
        public IHastaOlceklerFormu OlceklerFormDetay(int id)
        {
            IHastaOlceklerFormu model = new IHastaOlceklerFormu();
            try
            {
                model = (from h in _db.HastaOlceklerFormu
                         where h.ID == id
                         select new IHastaOlceklerFormu
                         {
                             ID = h.ID,
                             HastaID = h.HastaID,
                             DikkatEksikligi = (int)h.DikkatEksikligi,
                             AsiriHareketlilik = (int)h.AsiriHareketlilik,
                             UykuSoru1 = (int)h.UykuSoru1,
                             UykuSoru2 = (int)h.UykuSoru2,
                             UykuSoru3 = (int)h.UykuSoru3,
                             UykuSoru4 = (int)h.UykuSoru4,
                             UykuSoru5 = (int)h.UykuSoru5,
                             UykuSoru6 = (int)h.UykuSoru6,
                             UykuSoru7 = (int)h.UykuSoru7,
                             UykuSoru8 = (int)h.UykuSoru8,
                             UykuSoru9 = (int)h.UykuSoru9,
                             UykuSoru10 = (int)h.UykuSoru10,
                             UykuBozukluguTanisi = (int)h.UykuBozukluguTanisi,
                             KadinFormu1 = (int)h.KadinFormu1,
                             KadinFormu2 = (int)h.KadinFormu2,
                             KadinFormu3 = (int)h.KadinFormu3,
                             KadinFormu4 = (int)h.KadinFormu4,
                             KadinFormu5 = (int)h.KadinFormu5,
                             ErkekFormu1 = (int)h.ErkekFormu1,
                             ErkekFormu2 = (int)h.ErkekFormu2,
                             ErkekFormu3 = (int)h.ErkekFormu3,
                             ErkekFormu4 = (int)h.ErkekFormu4,
                             ErkekFormu5 = (int)h.ErkekFormu5,
                             TravmatikYasam1 = (int)h.TravmatikYasam1,
                             TravmatikYasam2 = (int)h.TravmatikYasam2,
                             TravmatikYasam3 = (int)h.TravmatikYasam3,
                             TravmatikYasam4 = (int)h.TravmatikYasam4,
                             TravmatikYasam5 = (int)h.TravmatikYasam5,
                             TravmatikYasam6 = (int)h.TravmatikYasam6,
                             TravmatikYasam7 = (int)h.TravmatikYasam7,
                             TravmatikYasam8 = (int)h.TravmatikYasam8,
                             TravmatikYasam9 = (int)h.TravmatikYasam9,
                             TravmatikYasam10 = (int)h.TravmatikYasam10,
                             TravmatikYasam11 = (int)h.TravmatikYasam11,
                             TravmatikYasam12 = (int)h.TravmatikYasam12,
                             TravmatikYasam13 = (int)h.TravmatikYasam13,
                             TravmatikYasam14 = (int)h.TravmatikYasam14,
                             TravmatikYasam14Aciklama = h.TravmatikYasam14Aciklama,
                             Appendix1 = (int)h.Appendix1,
                             Appendix2 = (int)h.Appendix2,
                             Appendix3 = (int)h.Appendix3,
                             Appendix4 = (int)h.Appendix4,
                             Appendix5 = (int)h.Appendix5,
                             Appendix6 = (int)h.Appendix6,
                             Appendix7 = (int)h.Appendix7,
                             Appendix8 = (int)h.Appendix8,
                             Appendix9 = (int)h.Appendix9,
                             Appendix10 = (int)h.Appendix10,
                             Appendix11 = (int)h.Appendix11,
                             Appendix12 = (int)h.Appendix12,
                             Appendix13 = (int)h.Appendix13,
                             Appendix14 = (int)h.Appendix14,
                             Appendix15 = (int)h.Appendix15,
                             Appendix16 = (int)h.Appendix16,
                             Appendix17 = (int)h.Appendix17,
                             Appendix18 = (int)h.Appendix18,
                             Appendix19 = (int)h.Appendix19,
                             Appendix20 = (int)h.Appendix20

                         }).FirstOrDefault();
                return model;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return model;
            }
        }
        public bool BSIFormuEkle(IHastaBagimlilikSiddetiFormlari model, int UserID)
        {
            try
            {
                using (var trans = _db.Database.BeginTransaction())
                {
                    #region Form Bilgisi
                    HastaBagimlilikSiddetiFormlari h = new HastaBagimlilikSiddetiFormlari();

                    h.HastaID = model.HastaID;
                    h.HastaNo = model.HastaNo;
                    h.Ulke = model.Ulke;
                    h.Merkez = model.Merkez;
                    h.Program = model.Program;
                    h.Modalite = model.Modalite;
                    h.ModaliteAciklama = model.ModaliteAciklama;
                    h.G3Soru = model.G3Soru;
                    h.BasvuruTarihi = model.BasvuruTarihi;
                    h.GorusmeTarihi = model.GorusmeTarihi;
                    h.BaslamaSaati = model.BaslamaSaati;
                    h.BitisSaati = model.BitisSaati;
                    h.Sinif = model.Sinif;
                    h.IletisimSekli = model.IletisimSekli;
                    h.Cinsiyet = model.Cinsiyet;
                    h.G11Soru = model.G11Soru;
                    h.G14Yil = model.G14Yil;
                    h.G14Ay = model.G14Ay;
                    h.DogumTarihi = model.DogumTarihi;
                    h.Yas = model.Yas;
                    h.G19Soru = model.G19Soru;
                    h.G19Aciklama = model.G19Aciklama;
                    h.G20Soru = model.G20Soru;
                    h.SevkKaynagi = model.SevkKaynagi;
                    h.GenelBilgiKaynagi = model.GenelBilgiKaynagi;
                    h.M1Soru = model.M1Soru;
                    h.M3Soru = model.M3Soru;
                    h.M3SoruAciklama = model.M3SoruAciklama;
                    h.M4Soru = model.M4Soru;
                    h.M5Soru = model.M5Soru;
                    h.M5SoruAciklama = model.M5SoruAciklama;
                    h.M6Soru = model.M6Soru;
                    h.M7Soru = model.M7Soru;
                    h.M8Soru = model.M8Soru;
                    h.M8SoruAciklama = model.M8SoruAciklama;
                    h.M10Soru = model.M10Soru;
                    h.M11Soru = model.M11Soru;
                    h.M12Soru = model.M12Soru;
                    h.M12aSoru = model.M12aSoru;
                    h.M12bSoru = model.M12bSoru;
                    h.M13Soru = model.M13Soru;
                    h.M13aSoru = model.M13aSoru;
                    h.M13bSoru = model.M13bSoru;
                    h.M14Soru = model.M14Soru;
                    h.M14aSoru = model.M14aSoru;
                    h.M14bSoru = model.M14bSoru;
                    h.TibbiDurumNotlar = model.TibbiDurumNotlar;
                    h.TamamlanmisEgitimSuresiDuzey = model.TamamlanmisEgitimSuresiDuzey;
                    h.E1aSoruAciklama = model.E1aSoruAciklama;
                    h.E2Soru = model.E2Soru;
                    h.E4aSoru = model.E4aSoru;
                    h.E6SoruYil = model.E6SoruYil;
                    h.E6SoruAy = model.E6SoruAy;
                    h.E7Soru = model.E7Soru;
                    h.E9Soru = model.E9Soru;
                    h.E10Soru = model.E10Soru;
                    h.E11Soru = model.E11Soru;
                    h.E12Soru = model.E12Soru;
                    h.E13Soru = model.E13Soru;
                    h.E14Soru = model.E14Soru;
                    h.E15Soru = model.E15Soru;
                    h.E16Soru = model.E16Soru;
                    h.E17Soru = model.E17Soru;
                    h.E18Soru = model.E18Soru;
                    h.E19Soru = model.E19Soru;
                    h.E1aSoruAciklama = model.E1aSoruAciklama;
                    h.E20Soru = model.E20Soru;
                    h.E21Soru = model.E21Soru;
                    h.E23Soru = model.E23Soru;
                    h.E24Soru = model.E24Soru;
                    h.IstihdamNotlari = model.IstihdamNotlari;
                    h.L10Soru = model.L10Soru;
                    h.L11Soru = model.L11Soru;
                    h.L12Soru = model.L12Soru;
                    h.L13Soru = model.L13Soru;
                    h.L14Soru = model.L14Soru;
                    h.L15Soru = model.L15Soru;
                    h.L16Soru = model.L16Soru;
                    h.L17Soru = model.L17Soru;
                    h.L18Soru = model.L18Soru;
                    h.L19Soru = model.L19Soru;
                    h.L20Soru = model.L20Soru;
                    h.L21Soru = model.L21Soru;
                    h.L24Soru = model.L24Soru;
                    h.L25Soru = model.L25Soru;
                    h.L26Soru = model.L26Soru;
                    h.L27Soru = model.L27Soru;
                    h.L28Soru = model.L28Soru;
                    h.L29Soru = model.L29Soru;
                    h.L31Soru = model.L31Soru;
                    h.L32Soru = model.L32Soru;
                    h.HukikiNotlar = model.HukikiNotlar;
                    h.D1Soru = model.D1Soru;
                    h.D1Yol = model.D1Yol;
                    h.D2Soru = model.D2Soru;
                    h.D2Yol = model.D2Yol;
                    h.D3Soru = model.D3Soru;
                    h.D3Yol = model.D3Yol;
                    h.D4Soru = model.D4Soru;
                    h.D4Yol = model.D4Yol;
                    h.D5Soru = model.D5Soru;
                    h.D5Yol = model.D5Yol;
                    h.D6Soru = model.D6Soru;
                    h.D6Yol = model.D6Yol;
                    h.D7Soru = model.D7Soru;
                    h.D7Yol = model.D7Yol;
                    h.D8Soru = model.D8Soru;
                    h.D8Yol = model.D8Yol;
                    h.D9Soru = model.D9Soru;
                    h.D9Yol = model.D9Yol;
                    h.D10Soru = model.D10Soru;
                    h.D10Yol = model.D10Yol;
                    h.D11Soru = model.D11Soru;
                    h.D11Yol = model.D11Yol;
                    h.D12Soru = model.D12Soru;
                    h.D12Yol = model.D12Yol;
                    h.D13Soru = model.D13Soru;
                    h.D13Yol = model.D13Yol;
                    h.D14aSoru = model.D14aSoru;
                    h.D14bSoru = model.D14bSoru;
                    h.D15Soru = model.D15Soru;
                    h.D16Soru = model.D16Soru;
                    h.D17Soru = model.D17Soru;
                    h.D19aSoru = model.D19aSoru;
                    h.D21aSoru = model.D21aSoru;
                    h.D23Soru = model.D23Soru;
                    h.D24Soru = model.D24Soru;
                    h.D25Soru = model.D25Soru;
                    h.D26Soru = model.D26Soru;
                    h.D26SoruAciklama = model.D26SoruAciklama;
                    h.D27Soru = model.D27Soru;
                    h.D27SoruAciklama = model.D27SoruAciklama;
                    h.D28Soru = model.D28Soru;
                    h.D29Soru = model.D29Soru;
                    h.D30Soru = model.D30Soru;
                    h.D31Soru = model.D31Soru;
                    h.D34Soru = model.D34Soru;
                    h.D35Soru = model.D35Soru;
                    h.D36Soru = model.D36Soru;
                    h.D37Soru = model.D37Soru;
                    h.D37SoruYol = model.D37SoruYol;
                    h.D38aSoru = model.D38aSoru;
                    h.D38Soru = model.D38Soru;
                    h.D39Soru = model.D39Soru;
                    h.D39a = model.D39a;
                    h.D39b = model.D39b;
                    h.D39c = model.D39c;
                    h.D39d = model.D39d;
                    h.D39e = model.D39e;
                    h.AlkolMaddeNotlari = model.AlkolMaddeNotlari;
                    h.F1Soru = model.F1Soru;
                    h.F1SoruAciklama = model.F1SoruAciklama;
                    h.F3Soru = model.F3Soru;
                    h.F4Soru = model.F4Soru;
                    h.F6Soru = model.F6Soru;
                    h.F4aSoru = model.F4aSoru;
                    h.F7Soru = model.F7Soru;
                    h.F8Soru = model.F8Soru;
                    h.F9Soru = model.F9Soru;
                    h.F10Soru = model.F10Soru;
                    h.F11aSoru = model.F11aSoru;
                    h.F18Soru = model.F18Soru;
                    h.F19Soru = model.F19Soru;
                    h.F20Soru = model.F20Soru;
                    h.F21Soru = model.F21Soru;
                    h.F22Soru = model.F22Soru;
                    h.F23Soru = model.F23Soru;
                    h.F23SoruAciklama = model.F23SoruAciklama;
                    h.F24Soru = model.F24Soru;
                    h.F25Soru = model.F25Soru;
                    h.F26Soru = model.F26Soru;
                    h.F28Soru = model.F28Soru;
                    h.F29Soru = model.F29Soru;
                    h.F30Soru = model.F30Soru;
                    h.F32Soru = model.F32Soru;
                    h.F34Soru = model.F34Soru;
                    h.F31Soru = model.F31Soru;
                    h.F33Soru = model.F33Soru;
                    h.F35Soru = model.F35Soru;
                    h.F37Soru = model.F37Soru;
                    h.F38Soru = model.F38Soru;
                    h.F39Soru1 = model.F39Soru1;
                    h.F39Soru2 = model.F39Soru2;
                    h.F39aSoru1 = model.F39aSoru1;
                    h.F39aSoru2 = model.F39aSoru2;
                    h.AileSosyalDurumNotlar = model.AileSosyalDurumNotlar;
                    h.P1Soru = model.P1Soru;
                    h.P2Soru = model.P2Soru;
                    h.P3Soru = model.P3Soru;
                    h.P4Soru = model.P4Soru;
                    h.P5Soru = model.P5Soru;
                    h.P6Soru = model.P6Soru;
                    h.P7Soru = model.P7Soru;
                    h.P8Soru = model.P8Soru;
                    h.P9Soru = model.P9Soru;
                    h.P10Soru = model.P10Soru;
                    h.P11Soru = model.P11Soru;
                    h.P12Soru = model.P12Soru;
                    h.P13Soru = model.P13Soru;
                    h.P14Soru = model.P14Soru;
                    h.P22Soru = model.P22Soru;
                    h.P23Soru = model.P23Soru;
                    h.PsikiyatrikNotlar = model.PsikiyatrikNotlar;
                    h.G12SonNokta = model.G12SonNokta;
                    h.G50TedaviKodu = model.G50TedaviKodu;
                    h.G50TedaviAciklama = model.G50TedaviAciklama;
                    h.GenelYorumlar = model.GenelYorumlar;
                    h.KaydedenKullaniciID = UserID;
                    h.AileSosyalDurumNotlarDevami = model.AileSosyalDurumNotlarDevami;
                    h.AlkolMaddeNotu = model.AlkolMaddeNotu;
                    h.AlkolMaddeNotuD5 = model.AlkolMaddeNotuD5;
                    h.E1Ay = model.E1Ay;
                    h.D10Yil = model.D10Yil;
                    h.D11Yil = model.D11Yil;
                    h.D12Yil = model.D12Yil;
                    h.D13Yil = model.D13Yil;
                    h.D1Yil = model.D1Yil;
                    h.D2Yil = model.D2Yil;
                    h.D37SoruYil = model.D37SoruYil;
                    h.D3Yil = model.D3Yil;
                    h.D4Yil = model.D4Yil;
                    h.D5Yil = model.D5Yil;
                    h.D6Yil = model.D6Yil;
                    h.D7Yil = model.D7Yil;
                    h.D8Yil = model.D8Yil;
                    h.D9Yil = model.D9Yil;
                    h.E1Yil = model.E1Yil;
                    h.F18SoruYasam = model.F18SoruYasam;
                    h.F19SoruYasam = model.F19SoruYasam;
                    h.F20SoruYasam = model.F20SoruYasam;
                    h.F21SoruYasam = model.F21SoruYasam;
                    h.F22SoruYasam = model.F22SoruYasam;
                    h.F23SoruYasam = model.F23SoruYasam;
                    h.F24SoruYasam = model.F24SoruYasam;
                    h.F25SoruYasam = model.F25SoruYasam;
                    h.F26SoruYasam = model.F26SoruYasam;
                    h.F28SoruYasam = model.F28SoruYasam;
                    h.F29SoruYasam = model.F29SoruYasam;
                    h.GenelBilgiKaynagi = model.GenelBilgiKaynagi;
                    h.GenelBilgiKaynagiDiger = model.GenelBilgiKaynagiDiger;
                    h.HukikiNotlarDevami = model.HukikiNotlarDevami;
                    h.IsIstihdamNotlar = model.IsIstihdamNotlar;
                    h.KonuEdilenParaBirimi = model.KonuEdilenParaBirimi;
                    h.L1Soru = model.L1Soru;
                    h.L2Soru = model.L2Soru;
                    h.L3Soru = model.L3Soru;
                    h.L4Soru = model.L4Soru;
                    h.L5Soru = model.L5Soru;
                    h.L6Soru = model.L6Soru;
                    h.L7Soru = model.L7Soru;
                    h.L8Soru = model.L8Soru;
                    h.L9Soru = model.L9Soru;
                    h.P10SoruYasam = model.P10SoruYasam;
                    h.P11SoruYasam = model.P11SoruYasam;
                    h.P4SoruYasam = model.P4SoruYasam;
                    h.P5SoruYasam = model.P5SoruYasam;
                    h.P6SoruYasam = model.P6SoruYasam;
                    h.P7SoruYasam = model.P7SoruYasam;
                    h.P8SoruYasam = model.P8SoruYasam;
                    h.P9SoruYasam = model.P9SoruYasam;
                    h.KayitTarihi = DateTime.Now;

                    _db.HastaBagimlilikSiddetiFormlari.Add(h);
                    _db.SaveChanges();

                    model.ID = h.ID;
                    #endregion

                    if (h.ID > 0)
                    {
                        #region İzlem 
                        HastaIzlemBilgileri hi = new HastaIzlemBilgileri();
                        hi.HastaID = model.HastaID;
                        hi.IzlemTarihi = DateTime.Now;
                        hi.IzlemYapanKurum = _db.KullaniciBirimBilgileri.Where(x => x.KullaniciID == UserID).Select(x => x.Kurum).FirstOrDefault();
                        hi.IzlemBasligi = "Form eklendi.";
                        hi.IzlemAciklama = "Hastaya Bağımlılık Şiddet İndeksi Formu eklenmiştir.";

                        _db.HastaIzlemBilgileri.Add(hi);
                        _db.SaveChanges();
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
        public List<IHastaBagimlilikSiddetiFormlari> BSIFormlari(int id)
        {
            List<IHastaBagimlilikSiddetiFormlari> model = new List<IHastaBagimlilikSiddetiFormlari>();
            try
            {
                model = (from h in _db.HastaBagimlilikSiddetiFormlari
                         where h.HastaID == id
                         orderby h.ID descending
                         select new IHastaBagimlilikSiddetiFormlari
                         {
                             ID = h.ID,
                             KayitTarihi = (DateTime)h.KayitTarihi,
                             KaydedenKullaniciID = (int)h.KaydedenKullaniciID,
                             HastaID = h.HastaID
                         }).ToList();
                return model;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return model;
            }
        }
        public IHastaBagimlilikSiddetiFormlari BSIFormDetay(int id)
        {
            IHastaBagimlilikSiddetiFormlari model = new IHastaBagimlilikSiddetiFormlari();
            try
            {
                model = (from h in _db.HastaBagimlilikSiddetiFormlari
                         where h.ID == id
                         select new IHastaBagimlilikSiddetiFormlari
                         {
                             ID = h.ID,
                             HastaID = h.HastaID,
                             HastaNo = h.HastaNo,
                             Ulke = (int)h.Ulke,
                             Merkez = (int)h.Merkez,
                             Program = (int)h.Program,
                             Modalite = (int)h.Modalite,
                             ModaliteAciklama = h.ModaliteAciklama,
                             G3Soru = (int)h.G3Soru,
                             BasvuruTarihi = (DateTime)h.BasvuruTarihi,
                             GorusmeTarihi = (DateTime)h.GorusmeTarihi,
                             BaslamaSaati = (TimeSpan)h.BaslamaSaati,
                             BitisSaati = (TimeSpan)h.BitisSaati,
                             Sinif = (int)h.Sinif,
                             IletisimSekli = (int)h.IletisimSekli,
                             Cinsiyet = (int)h.Cinsiyet,
                             G11Soru = h.G11Soru,
                             G14Yil = (int)h.G14Yil,
                             G14Ay = (int)h.G14Ay,
                             DogumTarihi = (DateTime)h.DogumTarihi,
                             Yas = (int)h.Yas,
                             G19Soru = (int)h.G19Soru,
                             G19Aciklama = h.G19Aciklama,
                             G20Soru = h.G20Soru,
                             SevkKaynagi = h.SevkKaynagi,
                             M1Soru = (int)h.M1Soru,
                             M3Soru = (int)h.M3Soru,
                             M3SoruAciklama = h.M3SoruAciklama,
                             M4Soru = (int)h.M4Soru,
                             M5Soru = (int)h.M5Soru,
                             M5SoruAciklama = h.M5SoruAciklama,
                             M6Soru = (int)h.M6Soru,
                             M7Soru = (int)h.M7Soru,
                             M8Soru = (int)h.M8Soru,
                             M8SoruAciklama = h.M8SoruAciklama,
                             M10Soru = (int)h.M10Soru,
                             M11Soru = (int)h.M11Soru,
                             M12Soru = (int)h.M12Soru,
                             M12aSoru = (int)h.M12aSoru,
                             M12bSoru = (int)h.M12bSoru,
                             M13Soru = (int)h.M13Soru,
                             M13aSoru = (int)h.M13aSoru,
                             M13bSoru = (int)h.M13bSoru,
                             M14Soru = (int)h.M14Soru,
                             M14aSoru = (int)h.M14aSoru,
                             M14bSoru = (int)h.M14bSoru,
                             TibbiDurumNotlar = h.TibbiDurumNotlar,
                             TamamlanmisEgitimSuresiDuzey = (int)h.TamamlanmisEgitimSuresiDuzey,
                             E1aSoruAciklama = h.E1aSoruAciklama,
                             E2Soru = (int)h.E2Soru,
                             E4aSoru = (int)h.E4aSoru,
                             E6SoruYil = (int)h.E6SoruYil,
                             E6SoruAy = (int)h.E6SoruAy,
                             E7Soru = (int)h.E7Soru,
                             E9Soru = (int)h.E9Soru,
                             E10Soru = (int)h.E10Soru,
                             E11Soru = (int)h.E11Soru,
                             E12Soru = (int)h.E12Soru,
                             E13Soru = (int)h.E13Soru,
                             E14Soru = (int)h.E14Soru,
                             E15Soru = (int)h.E15Soru,
                             E16Soru = (int)h.E16Soru,
                             E17Soru = (int)h.E17Soru,
                             E18Soru = (int)h.E18Soru,
                             E19Soru = h.E19Soru,
                             E20Soru = (int)h.E20Soru,
                             E21Soru = (int)h.E21Soru,
                             E23Soru = (int)h.E23Soru,
                             E24Soru = (int)h.E24Soru,
                             IstihdamNotlari = h.IstihdamNotlari,
                             L10Soru = (int)h.L10Soru,
                             L11Soru = (int)h.L11Soru,
                             L12Soru = (int)h.L12Soru,
                             L13Soru = (int)h.L13Soru,
                             L14Soru = (int)h.L14Soru,
                             L15Soru = (int)h.L15Soru,
                             L16Soru = h.L16Soru,
                             L17Soru = h.L17Soru,
                             L18Soru = (int)h.L18Soru,
                             L19Soru = (int)h.L19Soru,
                             L20Soru = (int)h.L20Soru,
                             L21Soru = (int)h.L21Soru,
                             L24Soru = (int)h.L24Soru,
                             L25Soru = h.L25Soru,
                             L26Soru = (int)h.L26Soru,
                             L27Soru = (int)h.L27Soru,
                             L28Soru = (int)h.L28Soru,
                             L29Soru = (int)h.L29Soru,
                             L31Soru = (int)h.L31Soru,
                             L32Soru = (int)h.L32Soru,
                             HukikiNotlar = h.HukikiNotlar,
                             D1Soru = h.D1Soru,
                             D1Yol = (int)h.D1Yol,
                             D2Soru = h.D2Soru,
                             D2Yol = (int)h.D2Yol,
                             D3Soru = h.D3Soru,
                             D3Yol = (int)h.D3Yol,
                             D4Soru = h.D4Soru,
                             D4Yol = (int)h.D4Yol,
                             D5Soru = h.D5Soru,
                             D5Yol = (int)h.D5Yol,
                             D6Soru = h.D6Soru,
                             D6Yol = (int)h.D6Yol,
                             D7Soru = h.D7Soru,
                             D7Yol = (int)h.D7Yol,
                             D8Soru = h.D8Soru,
                             D8Yol = (int)h.D8Yol,
                             D9Soru = h.D9Soru,
                             D9Yol = (int)h.D9Yol,
                             D10Soru = h.D10Soru,
                             D10Yol = (int)h.D10Yol,
                             D11Soru = h.D11Soru,
                             D11Yol = (int)h.D11Yol,
                             D12Soru = h.D12Soru,
                             D12Yol = (int)h.D12Yol,
                             D13Soru = h.D13Soru,
                             D13Yol = (int)h.D13Yol,
                             D14aSoru = h.D14aSoru,
                             D14bSoru = h.D14bSoru,
                             D15Soru = (int)h.D15Soru,
                             D16Soru = h.D16Soru,
                             D17Soru = (int)h.D17Soru,
                             D19aSoru = (int)h.D19aSoru,
                             D21aSoru = h.D21aSoru,
                             D23Soru = (int)h.D23Soru,
                             D24Soru = (int)h.D24Soru,
                             D25Soru = (int)h.D25Soru,
                             D26Soru = (int)h.D26Soru,
                             D26SoruAciklama = h.D26SoruAciklama,
                             D27Soru = (int)h.D27Soru,
                             D27SoruAciklama = h.D27SoruAciklama,
                             D28Soru = (int)h.D28Soru,
                             D29Soru = (int)h.D29Soru,
                             D30Soru = (int)h.D30Soru,
                             D31Soru = (int)h.D31Soru,
                             D34Soru = (int)h.D34Soru,
                             D35Soru = (int)h.D35Soru,
                             D36Soru = (int)h.D36Soru,
                             D37Soru = h.D37Soru,
                             D37SoruYol = (int)h.D37SoruYol,
                             D38aSoru = h.D38aSoru,
                             D38Soru = (int)h.D38Soru,
                             D39Soru = (int)h.D39Soru,
                             D39a = (int)h.D39a,
                             D39b = (int)h.D39b,
                             D39c = (int)h.D39c,
                             D39d = (int)h.D39d,
                             D39e = (int)h.D39e,
                             AlkolMaddeNotlari = h.AlkolMaddeNotlari,
                             F1Soru = (int)h.F1Soru,
                             F1SoruAciklama = h.F1SoruAciklama,
                             F3Soru = (int)h.F3Soru,
                             F4Soru = (int)h.F4Soru,
                             F6Soru = (int)h.F6Soru,
                             F4aSoru = (int)h.F4aSoru,
                             F7Soru = (int)h.F7Soru,
                             F8Soru = (int)h.F8Soru,
                             F9Soru = (int)h.F9Soru,
                             F10Soru = (int)h.F10Soru,
                             F11aSoru = h.F11aSoru,
                             F18Soru = (int)h.F18Soru,
                             F19Soru = (int)h.F19Soru,
                             F20Soru = (int)h.F20Soru,
                             F21Soru = (int)h.F21Soru,
                             F22Soru = (int)h.F22Soru,
                             F23Soru = (int)h.F23Soru,
                             F23SoruAciklama = h.F23SoruAciklama,
                             F24Soru = (int)h.F24Soru,
                             F25Soru = (int)h.F25Soru,
                             F26Soru = (int)h.F26Soru,
                             F28Soru = (int)h.F28Soru,
                             F29Soru = (int)h.F29Soru,
                             F30Soru = (int)h.F30Soru,
                             F32Soru = (int)h.F32Soru,
                             F34Soru = (int)h.F34Soru,
                             F31Soru = (int)h.F31Soru,
                             F33Soru = (int)h.F33Soru,
                             F35Soru = (int)h.F35Soru,
                             F37Soru = (int)h.F37Soru,
                             F38Soru = (int)h.F38Soru,
                             F39Soru1 = (int)h.F39Soru1,
                             F39Soru2 = (int)h.F39Soru2,
                             F39aSoru1 = (int)h.F39aSoru1,
                             F39aSoru2 = (int)h.F39aSoru2,
                             AileSosyalDurumNotlar = h.AileSosyalDurumNotlar,
                             P1Soru = (int)h.P1Soru,
                             P2Soru = (int)h.P2Soru,
                             P3Soru = (int)h.P3Soru,
                             P4Soru = (int)h.P4Soru,
                             P5Soru = (int)h.P5Soru,
                             P6Soru = (int)h.P6Soru,
                             P7Soru = (int)h.P7Soru,
                             P8Soru = (int)h.P8Soru,
                             P9Soru = (int)h.P9Soru,
                             P10Soru = (int)h.P10Soru,
                             P11Soru = (int)h.P11Soru,
                             P12Soru = (int)h.P12Soru,
                             P13Soru = (int)h.P13Soru,
                             P14Soru = (int)h.P14Soru,
                             P22Soru = (int)h.P22Soru,
                             P23Soru = (int)h.P23Soru,
                             PsikiyatrikNotlar = h.PsikiyatrikNotlar,
                             G12SonNokta = (int)h.G12SonNokta,
                             G50TedaviKodu = (int)h.G50TedaviKodu,
                             G50TedaviAciklama = h.G50TedaviAciklama,
                             GenelYorumlar = h.GenelYorumlar,
                             KaydedenKullaniciID = (int)h.KaydedenKullaniciID,
                             KayitTarihi = (DateTime)h.KayitTarihi,
                             AileSosyalDurumNotlarDevami = h.AileSosyalDurumNotlarDevami,
                             AlkolMaddeNotu = h.AlkolMaddeNotu,
                             AlkolMaddeNotuD5 =h.AlkolMaddeNotuD5,
                             E1Ay = (int)h.E1Ay,
                             D10Yil = (int)h.D10Yil,
                             D11Yil = (int)h.D11Yil,
                             D12Yil = (int)h.D12Yil,
                             D13Yil = (int)h.D13Yil,
                             D1Yil = (int)h.D1Yil,
                             D2Yil = (int)h.D2Yil,
                             D37SoruYil = (int)h.D37SoruYil,
                             D3Yil = (int)h.D3Yil,
                             D4Yil = (int)h.D4Yil,
                             D5Yil = (int)h.D5Yil,
                             D6Yil = (int)h.D6Yil,
                             D7Yil = (int)h.D7Yil,
                             D8Yil = (int)h.D8Yil,
                             D9Yil = (int)h.D9Yil,
                             E1Yil = (int)h.E1Yil,
                             F18SoruYasam = (int)h.F18SoruYasam,
                             F19SoruYasam = (int)h.F19SoruYasam,
                             F20SoruYasam = (int)h.F20SoruYasam,
                             F21SoruYasam = (int)h.F21SoruYasam,
                             F22SoruYasam = (int)h.F22SoruYasam,
                             F23SoruYasam = (int)h.F23SoruYasam,
                             F24SoruYasam = (int)h.F24SoruYasam,
                             F25SoruYasam = (int)h.F25SoruYasam,
                             F26SoruYasam = (int)h.F26SoruYasam,
                             F28SoruYasam = (int)h.F28SoruYasam,
                             F29SoruYasam = (int)h.F29SoruYasam,
                             GenelBilgiKaynagi = h.GenelBilgiKaynagi,
                             GenelBilgiKaynagiDiger = h.GenelBilgiKaynagiDiger,
                             HukikiNotlarDevami = h.HukikiNotlarDevami,
                             IsIstihdamNotlar = h.IsIstihdamNotlar,
                             KonuEdilenParaBirimi = h.KonuEdilenParaBirimi,
                             L1Soru = (int)h.L1Soru,
                             L2Soru = (int)h.L2Soru,
                             L3Soru = (int)h.L3Soru,
                             L4Soru = (int)h.L4Soru,
                             L5Soru = (int)h.L5Soru,
                             L6Soru = (int)h.L6Soru,
                             L7Soru = (int)h.L7Soru,
                             L8Soru = (int)h.L8Soru,
                             L9Soru = (int)h.L9Soru,
                             P10SoruYasam = (int)h.P10SoruYasam,
                             P11SoruYasam = (int)h.P11SoruYasam,
                             P4SoruYasam = (int)h.P4SoruYasam,
                             P5SoruYasam = (int)h.P5SoruYasam,
                             P6SoruYasam = (int)h.P6SoruYasam,
                             P7SoruYasam = (int)h.P7SoruYasam,
                             P8SoruYasam = (int)h.P8SoruYasam,
                             P9SoruYasam = (int)h.P9SoruYasam

                         }).FirstOrDefault();
                return model;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return model;
            }
        }
        public bool BSIFormGuncelle(IHastaBagimlilikSiddetiFormlari model)
        {
            try
            {
                using (var trans = _db.Database.BeginTransaction())
                {
                    #region Form Bilgisi
                    HastaBagimlilikSiddetiFormlari h = _db.HastaBagimlilikSiddetiFormlari.Where(x => x.ID == model.ID).FirstOrDefault();

                    h.HastaNo = model.HastaNo;
                    h.Ulke = model.Ulke;
                    h.Merkez = model.Merkez;
                    h.Program = model.Program;
                    h.Modalite = model.Modalite;
                    h.ModaliteAciklama = model.ModaliteAciklama;
                    h.G3Soru = model.G3Soru;
                    h.BasvuruTarihi = model.BasvuruTarihi;
                    h.GorusmeTarihi = model.GorusmeTarihi;
                    h.BaslamaSaati = model.BaslamaSaati;
                    h.BitisSaati = model.BitisSaati;
                    h.Sinif = model.Sinif;
                    h.IletisimSekli = model.IletisimSekli;
                    h.Cinsiyet = model.Cinsiyet;
                    h.G11Soru = model.G11Soru;
                    h.G14Yil = model.G14Yil;
                    h.G14Ay = model.G14Ay;
                    h.DogumTarihi = model.DogumTarihi;
                    h.Yas = model.Yas;
                    h.G19Soru = model.G19Soru;
                    h.G19Aciklama = model.G19Aciklama;
                    h.G20Soru = model.G20Soru;
                    h.SevkKaynagi = model.SevkKaynagi;
                    h.GenelBilgiKaynagi = model.GenelBilgiKaynagi;
                    h.M1Soru = model.M1Soru;
                    h.M3Soru = model.M3Soru;
                    h.M3SoruAciklama = model.M3SoruAciklama;
                    h.M4Soru = model.M4Soru;
                    h.M5Soru = model.M5Soru;
                    h.M5SoruAciklama = model.M5SoruAciklama;
                    h.M6Soru = model.M6Soru;
                    h.M7Soru = model.M7Soru;
                    h.M8Soru = model.M8Soru;
                    h.M8SoruAciklama = model.M8SoruAciklama;
                    h.M10Soru = model.M10Soru;
                    h.M11Soru = model.M11Soru;
                    h.M12Soru = model.M12Soru;
                    h.M12aSoru = model.M12aSoru;
                    h.M12bSoru = model.M12bSoru;
                    h.M13Soru = model.M13Soru;
                    h.M13aSoru = model.M13aSoru;
                    h.M13bSoru = model.M13bSoru;
                    h.M14Soru = model.M14Soru;
                    h.M14aSoru = model.M14aSoru;
                    h.M14bSoru = model.M14bSoru;
                    h.TibbiDurumNotlar = model.TibbiDurumNotlar;
                    h.TamamlanmisEgitimSuresiDuzey = model.TamamlanmisEgitimSuresiDuzey;
                    h.E1aSoruAciklama = model.E1aSoruAciklama;
                    h.E2Soru = model.E2Soru;
                    h.E4aSoru = model.E4aSoru;
                    h.E6SoruYil = model.E6SoruYil;
                    h.E6SoruAy = model.E6SoruAy;
                    h.E7Soru = model.E7Soru;
                    h.E9Soru = model.E9Soru;
                    h.E10Soru = model.E10Soru;
                    h.E11Soru = model.E11Soru;
                    h.E12Soru = model.E12Soru;
                    h.E13Soru = model.E13Soru;
                    h.E14Soru = model.E14Soru;
                    h.E15Soru = model.E15Soru;
                    h.E16Soru = model.E16Soru;
                    h.E17Soru = model.E17Soru;
                    h.E18Soru = model.E18Soru;
                    h.E19Soru = model.E19Soru;
                    h.E1aSoruAciklama = model.E1aSoruAciklama;
                    h.E20Soru = model.E20Soru;
                    h.E21Soru = model.E21Soru;
                    h.E23Soru = model.E23Soru;
                    h.E24Soru = model.E24Soru;
                    h.IstihdamNotlari = model.IstihdamNotlari;
                    h.L10Soru = model.L10Soru;
                    h.L11Soru = model.L11Soru;
                    h.L12Soru = model.L12Soru;
                    h.L13Soru = model.L13Soru;
                    h.L14Soru = model.L14Soru;
                    h.L15Soru = model.L15Soru;
                    h.L16Soru = model.L16Soru;
                    h.L17Soru = model.L17Soru;
                    h.L18Soru = model.L18Soru;
                    h.L19Soru = model.L19Soru;
                    h.L20Soru = model.L20Soru;
                    h.L21Soru = model.L21Soru;
                    h.L24Soru = model.L24Soru;
                    h.L25Soru = model.L25Soru;
                    h.L26Soru = model.L26Soru;
                    h.L27Soru = model.L27Soru;
                    h.L28Soru = model.L28Soru;
                    h.L29Soru = model.L29Soru;
                    h.L31Soru = model.L31Soru;
                    h.L32Soru = model.L32Soru;
                    h.HukikiNotlar = model.HukikiNotlar;
                    h.D1Soru = model.D1Soru;
                    h.D1Yol = model.D1Yol;
                    h.D2Soru = model.D2Soru;
                    h.D2Yol = model.D2Yol;
                    h.D3Soru = model.D3Soru;
                    h.D3Yol = model.D3Yol;
                    h.D4Soru = model.D4Soru;
                    h.D4Yol = model.D4Yol;
                    h.D5Soru = model.D5Soru;
                    h.D5Yol = model.D5Yol;
                    h.D6Soru = model.D6Soru;
                    h.D6Yol = model.D6Yol;
                    h.D7Soru = model.D7Soru;
                    h.D7Yol = model.D7Yol;
                    h.D8Soru = model.D8Soru;
                    h.D8Yol = model.D8Yol;
                    h.D9Soru = model.D9Soru;
                    h.D9Yol = model.D9Yol;
                    h.D10Soru = model.D10Soru;
                    h.D10Yol = model.D10Yol;
                    h.D11Soru = model.D11Soru;
                    h.D11Yol = model.D11Yol;
                    h.D12Soru = model.D12Soru;
                    h.D12Yol = model.D12Yol;
                    h.D13Soru = model.D13Soru;
                    h.D13Yol = model.D13Yol;
                    h.D14aSoru = model.D14aSoru;
                    h.D14bSoru = model.D14bSoru;
                    h.D15Soru = model.D15Soru;
                    h.D16Soru = model.D16Soru;
                    h.D17Soru = model.D17Soru;
                    h.D19aSoru = model.D19aSoru;
                    h.D21aSoru = model.D21aSoru;
                    h.D23Soru = model.D23Soru;
                    h.D24Soru = model.D24Soru;
                    h.D25Soru = model.D25Soru;
                    h.D26Soru = model.D26Soru;
                    h.D26SoruAciklama = model.D26SoruAciklama;
                    h.D27Soru = model.D27Soru;
                    h.D27SoruAciklama = model.D27SoruAciklama;
                    h.D28Soru = model.D28Soru;
                    h.D29Soru = model.D29Soru;
                    h.D30Soru = model.D30Soru;
                    h.D31Soru = model.D31Soru;
                    h.D34Soru = model.D34Soru;
                    h.D35Soru = model.D35Soru;
                    h.D36Soru = model.D36Soru;
                    h.D37Soru = model.D37Soru;
                    h.D37SoruYol = model.D37SoruYol;
                    h.D38aSoru = model.D38aSoru;
                    h.D38Soru = model.D38Soru;
                    h.D39Soru = model.D39Soru;
                    h.D39a = model.D39a;
                    h.D39b = model.D39b;
                    h.D39c = model.D39c;
                    h.D39d = model.D39d;
                    h.D39e = model.D39e;
                    h.AlkolMaddeNotlari = model.AlkolMaddeNotlari;
                    h.F1Soru = model.F1Soru;
                    h.F1SoruAciklama = model.F1SoruAciklama;
                    h.F3Soru = model.F3Soru;
                    h.F4Soru = model.F4Soru;
                    h.F6Soru = model.F6Soru;
                    h.F4aSoru = model.F4aSoru;
                    h.F7Soru = model.F7Soru;
                    h.F8Soru = model.F8Soru;
                    h.F9Soru = model.F9Soru;
                    h.F10Soru = model.F10Soru;
                    h.F11aSoru = model.F11aSoru;
                    h.F18Soru = model.F18Soru;
                    h.F19Soru = model.F19Soru;
                    h.F20Soru = model.F20Soru;
                    h.F21Soru = model.F21Soru;
                    h.F22Soru = model.F22Soru;
                    h.F23Soru = model.F23Soru;
                    h.F23SoruAciklama = model.F23SoruAciklama;
                    h.F24Soru = model.F24Soru;
                    h.F25Soru = model.F25Soru;
                    h.F26Soru = model.F26Soru;
                    h.F28Soru = model.F28Soru;
                    h.F29Soru = model.F29Soru;
                    h.F30Soru = model.F30Soru;
                    h.F32Soru = model.F32Soru;
                    h.F34Soru = model.F34Soru;
                    h.F31Soru = model.F31Soru;
                    h.F33Soru = model.F33Soru;
                    h.F35Soru = model.F35Soru;
                    h.F37Soru = model.F37Soru;
                    h.F38Soru = model.F38Soru;
                    h.F39Soru1 = model.F39Soru1;
                    h.F39Soru2 = model.F39Soru2;
                    h.F39aSoru1 = model.F39aSoru1;
                    h.F39aSoru2 = model.F39aSoru2;
                    h.AileSosyalDurumNotlar = model.AileSosyalDurumNotlar;
                    h.P1Soru = model.P1Soru;
                    h.P2Soru = model.P2Soru;
                    h.P3Soru = model.P3Soru;
                    h.P4Soru = model.P4Soru;
                    h.P5Soru = model.P5Soru;
                    h.P6Soru = model.P6Soru;
                    h.P7Soru = model.P7Soru;
                    h.P8Soru = model.P8Soru;
                    h.P9Soru = model.P9Soru;
                    h.P10Soru = model.P10Soru;
                    h.P11Soru = model.P11Soru;
                    h.P12Soru = model.P12Soru;
                    h.P13Soru = model.P13Soru;
                    h.P14Soru = model.P14Soru;
                    h.P22Soru = model.P22Soru;
                    h.P23Soru = model.P23Soru;
                    h.PsikiyatrikNotlar = model.PsikiyatrikNotlar;
                    h.G12SonNokta = model.G12SonNokta;
                    h.G50TedaviKodu = model.G50TedaviKodu;
                    h.G50TedaviAciklama = model.G50TedaviAciklama;
                    h.GenelYorumlar = model.GenelYorumlar;
                    h.AileSosyalDurumNotlarDevami = model.AileSosyalDurumNotlarDevami;
                    h.AlkolMaddeNotu = model.AlkolMaddeNotu;
                    h.AlkolMaddeNotuD5 = model.AlkolMaddeNotuD5;
                    h.E1Ay = model.E1Ay;
                    h.D10Yil = model.D10Yil;
                    h.D11Yil = model.D11Yil;
                    h.D12Yil = model.D12Yil;
                    h.D13Yil = model.D13Yil;
                    h.D1Yil = model.D1Yil;
                    h.D2Yil = model.D2Yil;
                    h.D37SoruYil = model.D37SoruYil;
                    h.D3Yil = model.D3Yil;
                    h.D4Yil = model.D4Yil;
                    h.D5Yil = model.D5Yil;
                    h.D6Yil = model.D6Yil;
                    h.D7Yil = model.D7Yil;
                    h.D8Yil = model.D8Yil;
                    h.D9Yil = model.D9Yil;
                    h.E1Yil = model.E1Yil;
                    h.F18SoruYasam = model.F18SoruYasam;
                    h.F19SoruYasam = model.F19SoruYasam;
                    h.F20SoruYasam = model.F20SoruYasam;
                    h.F21SoruYasam = model.F21SoruYasam;
                    h.F22SoruYasam = model.F22SoruYasam;
                    h.F23SoruYasam = model.F23SoruYasam;
                    h.F24SoruYasam = model.F24SoruYasam;
                    h.F25SoruYasam = model.F25SoruYasam;
                    h.F26SoruYasam = model.F26SoruYasam;
                    h.F28SoruYasam = model.F28SoruYasam;
                    h.F29SoruYasam = model.F29SoruYasam;
                    h.GenelBilgiKaynagi = model.GenelBilgiKaynagi;
                    h.GenelBilgiKaynagiDiger = model.GenelBilgiKaynagiDiger;
                    h.HukikiNotlarDevami = model.HukikiNotlarDevami;
                    h.IsIstihdamNotlar = model.IsIstihdamNotlar;
                    h.KonuEdilenParaBirimi = model.KonuEdilenParaBirimi;
                    h.L1Soru = model.L1Soru;
                    h.L2Soru = model.L2Soru;
                    h.L3Soru = model.L3Soru;
                    h.L4Soru = model.L4Soru;
                    h.L5Soru = model.L5Soru;
                    h.L6Soru = model.L6Soru;
                    h.L7Soru = model.L7Soru;
                    h.L8Soru = model.L8Soru;
                    h.L9Soru = model.L9Soru;
                    h.P10SoruYasam = model.P10SoruYasam;
                    h.P11SoruYasam = model.P11SoruYasam;
                    h.P4SoruYasam = model.P4SoruYasam;
                    h.P5SoruYasam = model.P5SoruYasam;
                    h.P6SoruYasam = model.P6SoruYasam;
                    h.P7SoruYasam = model.P7SoruYasam;
                    h.P8SoruYasam = model.P8SoruYasam;
                    h.P9SoruYasam = model.P9SoruYasam;

                    _db.SaveChanges();

                    model.ID = h.ID;
                    #endregion

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
        #region Randevular
        public List<IRandevuBilgileri> AktifRandevuTalepleri()
        {
            List<IRandevuBilgileri> talepler = new List<IRandevuBilgileri>();
            try
            {

                talepler = (from b in _db.RandevuBilgileri
                            where b.RandevuBaslangicTarihi > DateTime.Now
                            orderby b.RandevuBaslangicTarihi ascending
                            select new IRandevuBilgileri
                            {
                                ID = b.ID,
                                KurumID = b.KurumID,
                                PoliklinikTuruID = b.PoliklinikTuruID,
                                PoliklinikAdi = _db.KurumPoliklinikTurleri.Where(x => x.ID == b.PoliklinikTuruID).Select(x => x.PoliklinikTuru).FirstOrDefault(),
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

                return talepler;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public List<IRandevuBilgileri> VerilenRandevular()
        {
            List<IRandevuBilgileri> randevular = new List<IRandevuBilgileri>();
            try
            {

                randevular = (from b in _db.RandevuBilgileri
                              where b.RandevuBaslangicTarihi <= DateTime.Now
                              orderby b.RandevuBaslangicTarihi ascending
                              select new IRandevuBilgileri
                              {
                                  ID = b.ID,
                                  KurumID = b.KurumID,
                                  PoliklinikTuruID = b.PoliklinikTuruID,
                                  PoliklinikAdi = _db.KurumPoliklinikTurleri.Where(x => x.ID == b.PoliklinikTuruID).Select(x => x.PoliklinikTuru).FirstOrDefault(),
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
        #endregion
    }
}