using EkipProjesi.Core;
using EkipProjesi.Core.ArindirmaModulu;
using EkipProjesi.Core.Randevu;
using EkipProjesi.Core.Kullanici;
using EkipProjesi.Core.LLog;
using EkipProjesi.Core.Personel;
using EkipProjesi.Cryptography.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.SqlServer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using EkipProjesi.Core.Hastalar;

namespace EkipProjesi.Data.Repositories
{
    public class ArindirmaMerkezleriRepository
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private EKIPEntities _db;
        public ArindirmaMerkezleriRepository()
        {
            _db = new EKIPEntities();
        }

        #region Bilgiler
        public List<IArindirmaHizmetBilgileri> HizmetBilgileriGetir(IArindirmaHizmetBilgileriFiltreModel model)
        {
            List<IArindirmaHizmetBilgileri> bilgiler = new List<IArindirmaHizmetBilgileri>();
            try
            {
                var kurumkodu = 0;

                if (model.KurumAdi == "1")
                    kurumkodu = 1;
                else if (model.KurumAdi == "2")
                    kurumkodu = 2;
                else if (model.KurumAdi == "3")
                    kurumkodu = 3;

                if (!string.IsNullOrEmpty(model.KurumAdi) && model.BaslangicTarihi != null && model.BitisTarihi != null)
                {
                    bilgiler = (from b in _db.ArindirmaHizmetBilgileri
                                where b.KurumKodu == kurumkodu
                                where b.KayitTarihi >= model.BaslangicTarihi && b.KayitTarihi <= model.BitisTarihi
                                select new IArindirmaHizmetBilgileri
                                {
                                    ID = b.ID,
                                    KayitTarihi = b.KayitTarihi,
                                    AktifAmatemPoliklinikSayisi = (int)b.AktifAmatemPoliklinikSayisi,
                                    AktifCematemPoliklinikSayisi = (int)b.AktifCematemPoliklinikSayisi,
                                    AmatemYeniBasvuruSayisi = (int)b.AmatemYeniBasvuruSayisi,
                                    CematemYeniBasvuruSayisi = (int)b.CematemYeniBasvuruSayisi,
                                    AmatemTakipBasvurusuSayisi = (int)b.AmatemTakipBasvurusuSayisi,
                                    CematemTakipBasvurusuSayisi = (int)b.CematemTakipBasvurusuSayisi,
                                    AktifAmatemYatakSayisi = (int)b.AktifAmatemYatakSayisi,
                                    AktifCematemYatakSayisi = (int)b.AktifCematemYatakSayisi,
                                    AmatemBosYatakSayisi = (int)b.AmatemBosYatakSayisi,
                                    AmatemDoluYatakSayisi = (int)b.AmatemDoluYatakSayisi,
                                    CematemBosYatakSayisi = (int)b.CematemBosYatakSayisi,
                                    CematemDoluYatakSayisi = (int)b.CematemDoluYatakSayisi,
                                    AmatemYatakDolulukOrani = ((int)b.AmatemDoluYatakSayisi / ((int)b.AmatemBosYatakSayisi + (int)b.AmatemDoluYatakSayisi) * 100),
                                    CematemYatakDolulukOrani = ((int)b.CematemDoluYatakSayisi / ((int)b.CematemBosYatakSayisi + (int)b.CematemDoluYatakSayisi) * 100)
                                }).ToList();

                    return bilgiler;
                }
                else
                {
                    return null;
                }

                #region Filtreleme
                //if (bilgiler != null && bilgiler.Count() > 0 && !string.IsNullOrEmpty(model.KurumAdi))
                //{
                //    if (model.BaslangicTarihi != null && model.BitisTarihi != null)
                //        bilgiler = (from b in bilgiler where b.KurumKodu == kurumkodu where b.KayitTarihi >= model.BaslangicTarihi  && b.KayitTarihi < model.BitisTarihi select b).ToList();
                //}
                #endregion
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public List<IArindirmaPersonelBilgileri> PersonelBilgileriGetir(IArindirmaPersonelBilgileriFiltreModel model)
        {
            List<IArindirmaPersonelBilgileri> bilgiler = new List<IArindirmaPersonelBilgileri>();
            try
            {
                var kurumkodu = 0;

                if (model.KurumAdi == "1")
                    kurumkodu = 1;
                else if (model.KurumAdi == "2")
                    kurumkodu = 2;
                else if (model.KurumAdi == "3")
                    kurumkodu = 3;

                if (!string.IsNullOrEmpty(model.KurumAdi) && model.BaslangicTarihi != null && model.BitisTarihi != null)
                {
                    bilgiler = (from b in _db.ArindirmaPersonelBilgileri
                                where b.KurumKodu == kurumkodu
                                where b.KayitTarihi >= model.BaslangicTarihi && b.KayitTarihi <= model.BitisTarihi
                                select new IArindirmaPersonelBilgileri
                                {
                                    ID = b.ID,
                                    KayitTarihi = b.KayitTarihi,
                                    TumUzmanHekimSayisi = (int)b.TumUzmanHekimSayisi,
                                    TumAsistanHekimSayisi = (int)b.TumAsistanHekimSayisi,
                                    TumPratisyenHekimSayisi = (int)b.TumPratisyenHekimSayisi,
                                    HastaneAktifPsikiyatriUzmaniSayisi = (int)b.HastaneAktifPsikiyatriUzmaniSayisi,
                                    HastaneAktifCocukPsikiyatriUzmaniSayisi = (int)b.HastaneAktifCocukPsikiyatriUzmaniSayisi,
                                    HastaneAktifPsikiyatriAsistaniSayisi = (int)b.HastaneAktifPsikiyatriAsistaniSayisi,
                                    HastaneAktifCocukPsikiyatriAsistaniSayisi = (int)b.HastaneAktifCocukPsikiyatriAsistaniSayisi,
                                    HastaneAktifPsikologSayisi = (int)b.HastaneAktifPsikologSayisi,
                                    HastaneAktifHemsireSayisi = (int)b.HastaneAktifHemsireSayisi,
                                    AmatemAktifPsikiyatriUzmaniSayisi = (int)b.AmatemAktifPsikiyatriUzmaniSayisi,
                                    CematemAktifCocukPsikiyatriUzmaniSayisi = (int)b.CematemAktifCocukPsikiyatriUzmaniSayisi,
                                    AmatemAktifPsikiyatriAsistaniSayisi = (int)b.AmatemAktifPsikiyatriAsistaniSayisi,
                                    CematemAktifCocukPsikiyatriAsistaniSayisi = (int)b.CematemAktifCocukPsikiyatriAsistaniSayisi,
                                    AmatemAktifPsikologSayisi = (int)b.AmatemAktifPsikologSayisi,
                                    CematemAktifPsikologSayisi = (int)b.CematemAktifPsikologSayisi,
                                    AmatemAktifHemsireSayisi = (int)b.AmatemAktifHemsireSayisi,
                                    CematemAktifHemsireSayisi = (int)b.CematemAktifHemsireSayisi,
                                    MaddeBagimliligiEgitimiAlmisTumHekimSayisi = (int)b.MaddeBagimliligiEgitimiAlmisTumHekimSayisi,
                                    MaddeBagimliligiEgitimiAlmisPsikiyatriUzmaniSayisi = (int)b.MaddeBagimliligiEgitimiAlmisPsikiyatriUzmaniSayisi,
                                    MaddeBagimliligiEgitimiAlmisCocukPsikiyatriUzmaniSayisi = (int)b.MaddeBagimliligiEgitimiAlmisCocukPsikiyatriUzmaniSayisi,
                                    MaddeBagimliligiEgitimiAlmisPsikologSayisi = (int)b.MaddeBagimliligiEgitimiAlmisPsikologSayisi,
                                    MaddeBagimliligiEgitimiAlmisHemsireSayisi = (int)b.MaddeBagimliligiEgitimiAlmisHemsireSayisi
                                }).ToList();

                    return bilgiler;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public List<IArindirmaBasvuruBilgileri> BasvuruBilgileriGetir()
        {
            List<IArindirmaBasvuruBilgileri> bilgiler = new List<IArindirmaBasvuruBilgileri>();
            try
            {
                bilgiler = (from b in _db.ArindirmaBasvuruBilgileri
                            select new IArindirmaBasvuruBilgileri
                            {
                                ID = b.ID,
                                KurumKodu = b.KurumKodu,
                                KayitTarihi = b.KayitTarihi,
                                HastaEkipNo = b.HastaEkipNo,
                                HastaAdi = _db.Hastalar.Where(x => x.HastaEkipNo == b.HastaEkipNo).Select(x => x.HastaAdi).FirstOrDefault(),
                                HastaSoyadi = _db.Hastalar.Where(x => x.HastaEkipNo == b.HastaEkipNo).Select(x => x.HastaSoyadi).FirstOrDefault(),
                                HastaTCKimlikNo = _db.Hastalar.Where(x => x.HastaEkipNo == b.HastaEkipNo).Select(x => x.HastaTCKimlikNo).FirstOrDefault(),
                                HastaProtokolNo = b.HastaProtokolNo,
                                PoliklinikMuayeneTarihSaati = (DateTime)b.PoliklinikMuayeneTarihSaati,
                                PoliklinikTuruID = (int)b.PoliklinikTuruID,
                                PoliklinikTuru = _db.KurumPoliklinikTurleri.Where(x=>x.ID == b.PoliklinikTuruID).Select(x=>x.PoliklinikTuru).FirstOrDefault(),
                                MuayeneyiGerceklestirenHekim = b.MuayeneyiGerceklestirenHekim,
                                MuayeneyiGerceklestirenHekimTuru = b.MuayeneyiGerceklestirenHekimTuru,
                                MaddeBilgisi = b.MaddeBilgisi,
                                EslikEdenHastalikOykusu = b.EslikEdenHastalikOykusu,
                                PsikiyatrikHastalikOykusu = (bool)b.PsikiyatrikHastalikOykusu,
                                PsikiyatrikHastalikOykusuAciklama = b.PsikiyatrikHastalikOykusuAciklama,
                                KullanmaktaOlduguDigerIlacBilgisi = b.KullanmaktaOlduguDigerIlacBilgisi,
                                SonBasvurudanSonraAlkolMaddeKullanimiOlmusMu = b.SonBasvurudanSonraAlkolMaddeKullanimiOlmusMu,
                                SonBasvurudanSonraOnerilenIlaclariDuzenliKullanmisMi = b.SonBasvurudanSonraOnerilenIlaclariDuzenliKullanmisMi,
                                YoksunlukBulgusu = (bool)b.YoksunlukBulgusu,
                                IntoksikasyonBulgusu = (bool)b.IntoksikasyonBulgusu,
                                IdrarToksikolojiBulgusu = (bool)b.IdrarToksikolojiBulgusu,
                                KararID = (int)b.KararID,
                                Karar = _db.ArindirmaBasvuruKararlari.Where(x => x.KararID == b.KararID).Select(x => x.Karar).FirstOrDefault()

                            }).ToList();

                return bilgiler;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public IArindirmaBasvuruBilgileri BasvuruBilgisiDetay(int id)
        {
            IArindirmaBasvuruBilgileri model = new IArindirmaBasvuruBilgileri();
            try
            {
                model = (from b in _db.ArindirmaBasvuruBilgileri
                         where b.ID == id
                         select new IArindirmaBasvuruBilgileri
                         {
                             ID = b.ID,
                             KurumKodu = b.KurumKodu,
                             KayitTarihi = b.KayitTarihi,
                             HastaEkipNo = b.HastaEkipNo,
                             HastaAdi = _db.Hastalar.Where(x => x.HastaEkipNo == b.HastaEkipNo).Select(x => x.HastaAdi).FirstOrDefault(),
                             HastaSoyadi = _db.Hastalar.Where(x => x.HastaEkipNo == b.HastaEkipNo).Select(x => x.HastaSoyadi).FirstOrDefault(),
                             HastaTCKimlikNo = _db.Hastalar.Where(x => x.HastaEkipNo == b.HastaEkipNo).Select(x => x.HastaTCKimlikNo).FirstOrDefault(),
                             HastaProtokolNo = b.HastaProtokolNo,
                             PoliklinikMuayeneTarihSaati = (DateTime)b.PoliklinikMuayeneTarihSaati,
                             PoliklinikTuruID = (int)b.PoliklinikTuruID,
                             PoliklinikTuru = _db.KurumPoliklinikTurleri.Where(x => x.ID == b.PoliklinikTuruID).Select(x => x.PoliklinikTuru).FirstOrDefault(),
                             MuayeneyiGerceklestirenHekim = b.MuayeneyiGerceklestirenHekim,
                             MuayeneyiGerceklestirenHekimTuru = b.MuayeneyiGerceklestirenHekimTuru,
                             MaddeBilgisi = b.MaddeBilgisi,
                             EslikEdenHastalikOykusu = b.EslikEdenHastalikOykusu,
                             PsikiyatrikHastalikOykusu = (bool)b.PsikiyatrikHastalikOykusu,
                             PsikiyatrikHastalikOykusuAciklama = b.PsikiyatrikHastalikOykusuAciklama,
                             KullanmaktaOlduguDigerIlacBilgisi = b.KullanmaktaOlduguDigerIlacBilgisi,
                             SonBasvurudanSonraAlkolMaddeKullanimiOlmusMu = b.SonBasvurudanSonraAlkolMaddeKullanimiOlmusMu,
                             SonBasvurudanSonraOnerilenIlaclariDuzenliKullanmisMi = b.SonBasvurudanSonraOnerilenIlaclariDuzenliKullanmisMi,
                             YoksunlukBulgusu = (bool)b.YoksunlukBulgusu,
                             IntoksikasyonBulgusu = (bool)b.IntoksikasyonBulgusu,
                             IdrarToksikolojiBulgusu = (bool)b.IdrarToksikolojiBulgusu,
                             KararID = (int)b.KararID,
                             Karar = _db.ArindirmaBasvuruKararlari.Where(x => x.KararID == b.KararID).Select(x => x.Karar).FirstOrDefault()

                         }).FirstOrDefault();
                return model;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return model;
            }
        }
        public IYatislar YatisBilgisiDetay(int hastaid)
        {
            string ekipNo = _db.Hastalar.Where(x => x.HastaID == hastaid).Select(x => x.HastaEkipNo).FirstOrDefault();
            IYatislar model = new IYatislar();
            try
            {
                model = (from b in _db.Yatislar
                         where b.HastaEkipNo == ekipNo
                         select new IYatislar
                         {
                             ID = b.ID,
                             KurumKodu = b.KurumKodu,
                             HastaID = hastaid,
                             YatisTarihi = (DateTime)b.YatisTarihi,
                             GunlukIzlemBilgisi = b.GunlukIzlemBilgisi,
                             YatisSonlanmaTarihi = (DateTime)b.YatisSonlanmaTarihi,
                             YatisSonlanmaID = (int)b.YatisSonlanmaID,
                             YatisSonlanmaAciklama = b.YatisSonlanmaAciklama,
                             YatisSonlanmaNedeni = _db.YatisSonlanmaNedenleri.Where(x => x.ID == b.YatisSonlanmaID).Select(x => x.SonlanmaNedeni).FirstOrDefault(),
                         }).FirstOrDefault();
                return model;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return model;
            }
        }
        public List<IHastaMaddeKullanimBilgileri> MaddeBilgisiDetay(int id)
        {
            List<IHastaMaddeKullanimBilgileri> bilgiler = new List<IHastaMaddeKullanimBilgileri>();

            string basvuruNo = _db.ArindirmaBasvuruBilgileri.Where(x => x.ID == id).Select(x => x.HastaProtokolNo).FirstOrDefault();

            try
            {
                bilgiler = (from b in _db.HastaMaddeKullanimBilgileri
                            where b.HastaProtokolNo == basvuruNo
                            select new IHastaMaddeKullanimBilgileri
                            {
                                ID = b.ID,
                                HastaProtokolNo = b.HastaProtokolNo,
                                HastaEkipNo = b.HastaEkipNo,
                                KurumKodu = b.KurumKodu,
                                MaddeTuru = b.MaddeTuru,
                                Sure = b.Sure,
                                KullanimSuresiAy = b.KullanimSuresiAy,
                                KullanimSuresiYil = b.KullanimSuresiYil,
                                Miktar = b.Miktar,
                                Yontem = b.Yontem,
                                Siklik = b.Siklik,
                                EnSonKullanmaZamani = b.EnSonKullanmaZamani,
                                EnSonKullanmaZamaniAy = b.EnSonKullanmaZamaniAy,
                                EnSonKullanmaZamaniGun = b.EnSonKullanmaZamaniGun,
                                EnSonKullanmaZamaniYil = b.EnSonKullanmaZamaniYil
                            }).ToList();

                return bilgiler;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public List<IArindirmaBasvuruBilgileri> KendiHastalariBasvuru(int UserID)
        {
            List<IArindirmaBasvuruBilgileri> basvurular = new List<IArindirmaBasvuruBilgileri>();
            try
            {

                basvurular = (from b in _db.ArindirmaBasvuruBilgileri
                              join h in _db.Hastalar on b.HastaEkipNo equals h.HastaEkipNo
                            where h.KaydedenKullaniciID == UserID
                            select new IArindirmaBasvuruBilgileri
                            {
                                ID = b.ID,
                                KurumKodu = b.KurumKodu,
                                KayitTarihi = b.KayitTarihi,
                                HastaEkipNo = b.HastaEkipNo,
                                HastaAdi = _db.Hastalar.Where(x => x.HastaEkipNo == b.HastaEkipNo).Select(x => x.HastaAdi).FirstOrDefault(),
                                HastaSoyadi = _db.Hastalar.Where(x => x.HastaEkipNo == b.HastaEkipNo).Select(x => x.HastaSoyadi).FirstOrDefault(),
                                HastaTCKimlikNo = _db.Hastalar.Where(x => x.HastaEkipNo == b.HastaEkipNo).Select(x => x.HastaTCKimlikNo).FirstOrDefault(),
                                HastaProtokolNo = b.HastaProtokolNo,
                                PoliklinikMuayeneTarihSaati = (DateTime)b.PoliklinikMuayeneTarihSaati,
                                PoliklinikTuruID = (int)b.PoliklinikTuruID,
                                PoliklinikTuru = _db.KurumPoliklinikTurleri.Where(x => x.ID == b.PoliklinikTuruID).Select(x => x.PoliklinikTuru).FirstOrDefault(),
                                MuayeneyiGerceklestirenHekim = b.MuayeneyiGerceklestirenHekim,
                                MuayeneyiGerceklestirenHekimTuru = b.MuayeneyiGerceklestirenHekimTuru,
                                MaddeBilgisi = b.MaddeBilgisi,
                                EslikEdenHastalikOykusu = b.EslikEdenHastalikOykusu,
                                PsikiyatrikHastalikOykusu = (bool)b.PsikiyatrikHastalikOykusu,
                                PsikiyatrikHastalikOykusuAciklama = b.PsikiyatrikHastalikOykusuAciklama,
                                KullanmaktaOlduguDigerIlacBilgisi = b.KullanmaktaOlduguDigerIlacBilgisi,
                                SonBasvurudanSonraAlkolMaddeKullanimiOlmusMu = b.SonBasvurudanSonraAlkolMaddeKullanimiOlmusMu,
                                SonBasvurudanSonraOnerilenIlaclariDuzenliKullanmisMi = b.SonBasvurudanSonraOnerilenIlaclariDuzenliKullanmisMi,
                                YoksunlukBulgusu = (bool)b.YoksunlukBulgusu,
                                IntoksikasyonBulgusu = (bool)b.IntoksikasyonBulgusu,
                                IdrarToksikolojiBulgusu = (bool)b.IdrarToksikolojiBulgusu,
                                KararID = (int)b.KararID,
                                Karar = _db.ArindirmaBasvuruKararlari.Where(x => x.KararID == b.KararID).Select(x => x.Karar).FirstOrDefault()

                            }).ToList();

                return basvurular;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public List<IArindirmaBasvuruBilgileri> KurumdakiHastalarBasvuru(int UserID)
        {
            List<IArindirmaBasvuruBilgileri> basvurular = new List<IArindirmaBasvuruBilgileri>();
            try
            {

                basvurular = (from b in _db.ArindirmaBasvuruBilgileri
                              join h in _db.Hastalar on b.HastaEkipNo equals h.HastaEkipNo
                            join k in _db.Kullanicilar on UserID equals k.ID
                            join kr in _db.Kurumlar on h.KurumKodu equals kr.KurumKodu
                            where k.KurumID == kr.KurumID
                            select new IArindirmaBasvuruBilgileri
                            {
                                ID = b.ID,
                                KurumKodu = b.KurumKodu,
                                KayitTarihi = b.KayitTarihi,
                                HastaEkipNo = b.HastaEkipNo,
                                HastaAdi = _db.Hastalar.Where(x => x.HastaEkipNo == b.HastaEkipNo).Select(x => x.HastaAdi).FirstOrDefault(),
                                HastaSoyadi = _db.Hastalar.Where(x => x.HastaEkipNo == b.HastaEkipNo).Select(x => x.HastaSoyadi).FirstOrDefault(),
                                HastaTCKimlikNo = _db.Hastalar.Where(x => x.HastaEkipNo == b.HastaEkipNo).Select(x => x.HastaTCKimlikNo).FirstOrDefault(),
                                HastaProtokolNo = b.HastaProtokolNo,
                                PoliklinikMuayeneTarihSaati = (DateTime)b.PoliklinikMuayeneTarihSaati,
                                PoliklinikTuruID = (int)b.PoliklinikTuruID,
                                PoliklinikTuru = _db.KurumPoliklinikTurleri.Where(x => x.ID == b.PoliklinikTuruID).Select(x => x.PoliklinikTuru).FirstOrDefault(),
                                MuayeneyiGerceklestirenHekim = b.MuayeneyiGerceklestirenHekim,
                                MuayeneyiGerceklestirenHekimTuru = b.MuayeneyiGerceklestirenHekimTuru,
                                MaddeBilgisi = b.MaddeBilgisi,
                                EslikEdenHastalikOykusu = b.EslikEdenHastalikOykusu,
                                PsikiyatrikHastalikOykusu = (bool)b.PsikiyatrikHastalikOykusu,
                                PsikiyatrikHastalikOykusuAciklama = b.PsikiyatrikHastalikOykusuAciklama,
                                KullanmaktaOlduguDigerIlacBilgisi = b.KullanmaktaOlduguDigerIlacBilgisi,
                                SonBasvurudanSonraAlkolMaddeKullanimiOlmusMu = b.SonBasvurudanSonraAlkolMaddeKullanimiOlmusMu,
                                SonBasvurudanSonraOnerilenIlaclariDuzenliKullanmisMi = b.SonBasvurudanSonraOnerilenIlaclariDuzenliKullanmisMi,
                                YoksunlukBulgusu = (bool)b.YoksunlukBulgusu,
                                IntoksikasyonBulgusu = (bool)b.IntoksikasyonBulgusu,
                                IdrarToksikolojiBulgusu = (bool)b.IdrarToksikolojiBulgusu,
                                KararID = (int)b.KararID,
                                Karar = _db.ArindirmaBasvuruKararlari.Where(x => x.KararID == b.KararID).Select(x => x.Karar).FirstOrDefault()

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

        #region Randevu Ayarları
        public bool RandevuAyariKayit(int kid, int pol, int gun, int slot, TimeSpan bassaati, TimeSpan bitsaati, out int id, int UserID)
        {
            try
            { 
                var kurumkontrol = _db.Kurumlar.Where(x => x.KurumID == kid).Any();

                RandevuAyarlari r = new RandevuAyarlari();

                if (kid != null && kid != 0 && kurumkontrol)
                    r.KurumID = kid;

                r.PoliklinikTuruID = pol;
                r.Gun = gun;
                r.BaslangicSaati = bassaati;
                r.BitisSaati = bitsaati;
                r.SlotSayisi = slot;
                r.Durum = true;
                r.LogTarihi = DateTime.Now;
                r.KaydedenKullaniciID = UserID;

                _db.RandevuAyarlari.Add(r);
                _db.SaveChanges();
                id = r.ID;
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                id = 0;
                return false;
            }
        }

        public IRandevuAyarlari RandevuAyariDetay(int id)
        {
            IRandevuAyarlari model = new IRandevuAyarlari();
            try
            {
                model = (from r in _db.RandevuAyarlari
                         where r.ID == id
                         select new IRandevuAyarlari
                         {
                             KurumID = r.KurumID,
                             PoliklinikTuruID = r.PoliklinikTuruID,
                             KurumAdi = _db.Kurumlar.Where(x => x.KurumID == r.KurumID).Select(x => x.KurumAdi).FirstOrDefault(),
                             PoliklinikAdi = _db.KurumPoliklinikTurleri.Where(x => x.KurumID == r.KurumID && x.ID == r.PoliklinikTuruID).Select(x => x.PoliklinikTuru).FirstOrDefault(),
                             Gun = r.Gun,
                             BaslangicSaati = r.BaslangicSaati,
                             BitisSaati = r.BitisSaati,
                             SlotSayisi = r.SlotSayisi,
                             Durum = r.Durum,
                             PasifBaslangicTarihi =r.PasifBaslangicTarihi,
                             PasifBitisTarihi = r.PasifBitisTarihi
                         }).FirstOrDefault();
                return model;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return model;
            }
        }

        public bool RandevuAyarDuzenleme(IRandevuAyarlari model)
        {
            try
            {
                var ayarkontrol = _db.RandevuAyarlari.Where(x => x.KurumID == model.KurumID && x.PoliklinikTuruID == model.PoliklinikTuruID && x.Gun == model.Gun).Any();

                if (ayarkontrol)
                {
                    return false;
                }
                else
                {
                    RandevuAyarlari r = _db.RandevuAyarlari.Where(x => x.ID == model.ID).FirstOrDefault();

                    r.BaslangicSaati = model.BaslangicSaati;
                    r.BitisSaati = model.BitisSaati;
                    r.SlotSayisi = model.SlotSayisi;
                    r.Durum = model.Durum; 
                    if(model.Durum == false)
                    {
                        r.PasifBitisTarihi = model.PasifBitisTarihi;
                        r.PasifBaslangicTarihi = model.PasifBaslangicTarihi;
                    }                   
                    r.LogTarihi = DateTime.Now;

                    r.GuncellemeTarihi = DateTime.Now;
                    _db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }

        public List<IRandevuAyarlari> RandevuAyarlariList()
        {
            List<IRandevuAyarlari> model = new List<IRandevuAyarlari>();

            try
            {
                model = (from r in _db.RandevuAyarlari
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
        #endregion

        #region Bilgileri Getir
        public IRandevuAyarlari RandevuAyariGetir(IKurumlarFiltreModel model)
        {
            IRandevuAyarlari ayarlar = new IRandevuAyarlari();

            try
            {
                if (model.Tarih.ToString("dddd").Contains("Pazartesi"))
                {
                    ayarlar = (from r in _db.RandevuAyarlari
                               where r.KurumID == model.KurumID && r.PoliklinikTuruID == model.PoliklinikTuruID && r.Gun == 1
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
                               }).FirstOrDefault();

                }

                if (ayarlar.Durum == true)
                {
                    TimeSpan basBitisFarki = DateTime.Parse(ayarlar.BitisSaati.ToString()).Subtract(DateTime.Parse(ayarlar.BaslangicSaati.ToString()));
                    string sure = basBitisFarki.ToString();

                    decimal sure2 = Convert.ToDecimal(TimeSpan.Parse(sure).TotalMinutes);

                    var muayenesuresi = (sure2) / ayarlar.SlotSayisi;
                    model.MuayeneSuresi = muayenesuresi;
                }

                return ayarlar;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return ayarlar;
            }
        }

        public IRandevuAyarlari RandevuAyarGetir(DateTime gun, int kid, int pid)
        {
            IRandevuAyarlari ayarlar = new IRandevuAyarlari();

            try
            {
                if (gun.ToString("dddd").Contains("Pazartesi"))
                {
                    ayarlar = (from r in _db.RandevuAyarlari
                               where r.KurumID == kid && r.PoliklinikTuruID == pid && r.Gun == 1
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
                               }).FirstOrDefault();

                }

                if (gun.ToString("dddd").Contains("Salı"))
                {
                    ayarlar = (from r in _db.RandevuAyarlari
                               where r.KurumID == kid && r.PoliklinikTuruID == pid && r.Gun == 2
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
                               }).FirstOrDefault();

                }

                if (gun.ToString("dddd").Contains("Çarşamba"))
                {
                    ayarlar = (from r in _db.RandevuAyarlari
                               where r.KurumID == kid && r.PoliklinikTuruID == pid && r.Gun == 3
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
                               }).FirstOrDefault();

                }

                if (gun.ToString("dddd").Contains("Perşembe"))
                {
                    ayarlar = (from r in _db.RandevuAyarlari
                               where r.KurumID == kid && r.PoliklinikTuruID == pid && r.Gun == 4
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
                               }).FirstOrDefault();

                }

                if (gun.ToString("dddd").Contains("Cuma"))
                {
                    ayarlar = (from r in _db.RandevuAyarlari
                               where r.KurumID == kid && r.PoliklinikTuruID == pid && r.Gun == 5
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
                               }).FirstOrDefault();

                }

                TimeSpan basBitisFarki = DateTime.Parse(ayarlar.BitisSaati.ToString()).Subtract(DateTime.Parse(ayarlar.BaslangicSaati.ToString()));
                string sure = basBitisFarki.ToString();

                decimal sure2 = Convert.ToDecimal(TimeSpan.Parse(sure).TotalMinutes);

                var muayenesuresi = (sure2) / ayarlar.SlotSayisi;
                ayarlar.MuayeneSuresi = muayenesuresi;

                return ayarlar;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return ayarlar;
            }
        }

        public List<IRandevuBilgileri> RandevuSaatiKontrol(string gunbilgisi, int kid, int pid)
        {
            List<IRandevuBilgileri> bilgiler = new List<IRandevuBilgileri>();
            try
            {
                bilgiler = (from b in _db.RandevuBilgileri.AsEnumerable()
                            where b.RandevuBaslangicTarihi.ToShortDateString() == gunbilgisi
                            select new IRandevuBilgileri
                              {
                                  ID = b.ID,
                                  KurumID = b.KurumID,
                                  PoliklinikTuruID = b.PoliklinikTuruID,
                                  RandevuBaslangicSaati = b.RandevuBaslangicSaati

                              }).Where(x=>x.KurumID == kid && x.PoliklinikTuruID == pid).ToList();               

                return bilgiler;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }

        public List<IKurumlar> KurumBilgileriGetir()
        {
            List<IKurumlar> kurumlar = new List<IKurumlar>();
            try
            {

                kurumlar = (from b in _db.Kurumlar
                            select new IKurumlar
                            {
                                KurumID = b.KurumID,
                                KurumAdi = b.KurumAdi,
                                KurumKodu = b.KurumKodu

                            }).ToList();

                return kurumlar;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }

        public List<IKurumPoliklinikTurleri> PoliklinikBilgileriGetir(int kurumid)
        {
            List<IKurumPoliklinikTurleri> poliklinikler = new List<IKurumPoliklinikTurleri>();
            try
            {

                poliklinikler = (from b in _db.KurumPoliklinikTurleri
                                 select new IKurumPoliklinikTurleri
                                 {
                                     ID = b.ID,
                                     KurumID = b.KurumID,
                                     PoliklinikTuru = b.PoliklinikTuru

                                 }).Where(x => x.KurumID == kurumid).ToList();

                return poliklinikler;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }

        public List<IRandevuBilgileri> RandevuBilgileriGetir(int? kurumid, int? polid)
        {
            List<IRandevuBilgileri> randevular = new List<IRandevuBilgileri>();
            try
            {

                randevular = (from b in _db.RandevuBilgileri
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

                              }).Where(x => x.KurumID == kurumid && x.PoliklinikTuruID == polid).ToList();

                return randevular;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }

        public List<IHastalar> HastaBilgileriGetir()
        {
            List<IHastalar> hastalar = new List<IHastalar>();
            try
            {

                hastalar = (from b in _db.Hastalar
                            select new IHastalar
                            {
                                HastaID = b.HastaID,
                                HastaAdi = b.HastaAdi,
                                HastaSoyadi = b.HastaSoyadi,
                                HastaTCKimlikNo = b.HastaTCKimlikNo,
                                Telefon = b.Telefon,
                                KurumKodu = b.KurumKodu,
                                HastaEkipNo = b.HastaEkipNo

                            }).ToList();

                return hastalar;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }

        public IHastalar HastaBilgiGetir(int hastaid)
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
                             HastaTCKimlikNo = b.HastaTCKimlikNo,
                             Telefon = b.Telefon,
                             KurumKodu = b.KurumKodu,
                             DogumTarihi = (DateTime)b.DogumTarihi,
                             Adres = b.Adres,
                             HastaEkipNo = b.HastaEkipNo,
                             Cinsiyet = b.Cinsiyet,
                             Email = b.Email

                         }).Where(x => x.HastaID == hastaid).FirstOrDefault();

                return hasta;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }

        public List<IKurumPoliklinikTurleri> PoliklinikBilgisiGetir()
        {
            List<IKurumPoliklinikTurleri> poliklinikler = new List<IKurumPoliklinikTurleri>();
            try
            {

                poliklinikler = (from b in _db.KurumPoliklinikTurleri
                                 select new IKurumPoliklinikTurleri
                                 {
                                     ID = b.ID,
                                     KurumID = b.KurumID,
                                     PoliklinikTuru = b.PoliklinikTuru

                                 }).ToList();

                return poliklinikler;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }

        public List<IRandevuAyarlari> PasiflikKontrolu(int kid, int pid)
        {
            List<IRandevuAyarlari> ayarlar = new List<IRandevuAyarlari>();

            try
            {
                ayarlar = (from r in _db.RandevuAyarlari
                           where r.KurumID == kid && r.PoliklinikTuruID == pid && r.Durum == false
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
                           }).ToList();

                return ayarlar;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return ayarlar;
            }
        }

        public bool RandevuAyariEksikMiKontrolu(int kid, int pid)
        {
            List<IRandevuAyarlari> ayarlar = new List<IRandevuAyarlari>();

            try
            {
                ayarlar = (from r in _db.RandevuAyarlari
                           where r.KurumID == kid && r.PoliklinikTuruID == pid
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
                           }).ToList();

                if(ayarlar.Where(x=>x.Gun == 1).Any() && ayarlar.Where(x => x.Gun == 2).Any() && ayarlar.Where(x => x.Gun == 3).Any() && ayarlar.Where(x => x.Gun == 4).Any() && ayarlar.Where(x => x.Gun == 5).Any())
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }

        public List<IRandevuBilgileri> VerilenRandevulariGetir()
        {
            List<IRandevuBilgileri> randevular = new List<IRandevuBilgileri>();
            try
            {

                randevular = (from b in _db.RandevuBilgileri
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
                                  RandevuDurumu = b.RandevuDurumu,
                                  PoliklinikAdi = _db.KurumPoliklinikTurleri.Where(x => x.ID == b.PoliklinikTuruID).Select(x => x.PoliklinikTuru).FirstOrDefault()

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

        #region Randevu Oluşturma Ekranı
        public bool RandevuKaydet(RandevuBilgileri model, int UserID)
        {
            try
            {

                RandevuBilgileri r = new RandevuBilgileri();

                r.Aciklama = model.Aciklama;
                r.Renk = model.Renk;
                r.TumGun = false;
                r.HastaAdi = model.HastaAdi;
                r.HastaSoyadi = model.HastaSoyadi;
                r.HastaTCKimlikNo = model.HastaTCKimlikNo;
                r.KurumID = model.KurumID;
                r.PoliklinikTuruID = model.PoliklinikTuruID;
                r.Telefon = model.Telefon;
                r.YakinTelefonu = model.YakinTelefonu;
                r.RandevuBaslangicSaati = model.RandevuBaslangicSaati;
                r.RandevuBitisSaati = model.RandevuBitisSaati;
                r.RandevuBaslangicTarihi = model.RandevuBaslangicTarihi.AddHours(r.RandevuBaslangicSaati.Hours).AddMinutes(r.RandevuBaslangicSaati.Minutes);
                r.RandevuBitisTarihi = model.RandevuBitisTarihi.AddHours(r.RandevuBitisSaati.Hours).AddMinutes(r.RandevuBitisSaati.Minutes);
                r.KaydedenKullaniciID = UserID;

                _db.RandevuBilgileri.Add(r);
                _db.SaveChanges();

                if (r.ID > 0)
                {
                    #region İzlem 
                    HastaIzlemBilgileri hi = new HastaIzlemBilgileri();
                    hi.HastaID = _db.Hastalar.Where(x=>x.HastaTCKimlikNo == model.HastaTCKimlikNo).Select(x=>x.HastaID).FirstOrDefault();
                    hi.IzlemTarihi = DateTime.Now;
                    hi.IzlemYapanKurum = _db.KullaniciBirimBilgileri.Where(x => x.KullaniciID == UserID).Select(x => x.Kurum).FirstOrDefault();
                    hi.IzlemBasligi = "Randevu eklendi.";
                    hi.IzlemAciklama = "Hastaya ait arındırma tedavisi randevusu oluşturulmuştur.";

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

        public bool RandevuDuzenle(RandevuBilgileri model, int UserID)
        {
            try
            {
                RandevuBilgileri r = _db.RandevuBilgileri.Where(x => x.ID == model.ID).FirstOrDefault();

                r.Aciklama = model.Aciklama;
                r.Renk = model.Renk;
                r.TumGun = false;
                r.HastaAdi = model.HastaAdi;
                r.HastaSoyadi = model.HastaSoyadi;
                r.HastaTCKimlikNo = model.HastaTCKimlikNo;
                r.KurumID = model.KurumID;
                r.PoliklinikTuruID = model.PoliklinikTuruID;
                r.Telefon = model.Telefon;
                r.YakinTelefonu = model.YakinTelefonu;
                r.RandevuBaslangicSaati = model.RandevuBaslangicSaati;
                r.RandevuBitisSaati = model.RandevuBitisSaati;
                string dt1 = new DateTime(model.RandevuBaslangicTarihi.Year, model.RandevuBaslangicTarihi.Month, model.RandevuBaslangicTarihi.Day, 0, 0, 0).ToString();
                string dt2 = new DateTime(model.RandevuBitisTarihi.Year, model.RandevuBitisTarihi.Month, model.RandevuBitisTarihi.Day, 0, 0, 0).ToString();
                r.RandevuBaslangicTarihi = DateTime.Parse(dt1).AddHours(r.RandevuBaslangicSaati.Hours).AddMinutes(r.RandevuBaslangicSaati.Minutes).AddSeconds(0);
                r.RandevuBitisTarihi = DateTime.Parse(dt2).AddHours(r.RandevuBitisSaati.Hours).AddMinutes(r.RandevuBitisSaati.Minutes).AddSeconds(0);
                r.GuncelleyenKullaniciID = UserID;

                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }

        public bool RandevuSil(int eventID, string iptalNedeni, IRandevuBilgileri model)
        {
            try
            {
                LogSilinenRandevular sr = new LogSilinenRandevular();
                sr.RandevuID = model.ID;
                sr.Aciklama = model.Aciklama;
                sr.Renk = model.Renk;
                sr.TumGun = false;
                sr.HastaAdi = model.HastaAdi;
                sr.HastaSoyadi = model.HastaSoyadi;
                sr.HastaTCKimlikNo = model.HastaTCKimlikNo;
                sr.KurumID = model.KurumID;
                sr.PoliklinikTuruID = model.PoliklinikTuruID;
                sr.Telefon = model.Telefon;
                sr.YakinTelefonu = model.YakinTelefonu;
                sr.RandevuBaslangicTarihi = model.RandevuBaslangicTarihi;
                sr.RandevuBitisTarihi = model.RandevuBitisTarihi;
                sr.RandevuBaslangicSaati = model.RandevuBaslangicSaati;
                sr.RandevuBitisSaati = model.RandevuBitisSaati;
                sr.LogRandevuIptalNedeni = iptalNedeni;

                _db.LogSilinenRandevular.Add(sr);
                _db.SaveChanges();

                RandevuBilgileri r = _db.RandevuBilgileri.Where(a => a.ID == eventID).FirstOrDefault();

                _db.RandevuBilgileri.Remove(r);
                _db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }

        public IRandevuBilgileri SeciliRandevuyuGetir(int eventID)
        {
            IRandevuBilgileri randevu = new IRandevuBilgileri();
            try
            {

                randevu = (from b in _db.RandevuBilgileri
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
                                  RandevuBaslangicTarihi = b.RandevuBaslangicTarihi,
                                  RandevuBitisTarihi = b.RandevuBitisTarihi,
                                  RandevuBaslangicSaati = b.RandevuBaslangicSaati,
                                  RandevuBitisSaati = b.RandevuBitisSaati

                              }).Where(x => x.ID == eventID).FirstOrDefault();

                return randevu;
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