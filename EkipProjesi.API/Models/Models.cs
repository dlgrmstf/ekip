using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EkipProjesi.API.Models
{
    public class Models
    {

        #region Kayıt
        public class HastaEkleModel
        {
            public int KurumKodu;
            public string HastaAdi;
            public string HastaSoyadi;
            public string HastaTCKimlikNo;
            public string Telefon;
            public string HastaEkipNo;
            public DateTime KayitTarihi;
        }
        public class HastaBasvuruBilgisiEkleModel
        {
            public string HastaEkipNo;
            public int KurumKodu;
            public DateTime KayitTarihi;
            public string HastaProtokolNo;
            public string BeyanAdresi;
            public string BeyanTelefonu;
            public DateTime PoliklinikMuayeneTarihSaati;
            public int PoliklinikTuruID;
            public string MuayeneyiGerceklestirenHekim;
            public string MuayeneyiGerceklestirenHekimTuru;
            public string MaddeBilgisi;
            public string EslikEdenHastalikOykusu;
            public bool PsikiyatrikHastalikOykusu;
            public string PsikiyatrikHastalikOykusuAciklama;
            public string KullanmaktaOlduguDigerIlacBilgisi;
            public string SonBasvurudanSonraAlkolMaddeKullanimiOlmusMu;
            public string SonBasvurudanSonraOnerilenIlaclariDuzenliKullanmisMi;
            public bool YoksunlukBulgusu;
            public bool IntoksikasyonBulgusu;
            public bool IdrarToksikolojiBulgusu;
            public int KararID;           
        }
        public class HizmetBilgisiEkleModel
        {
            public int KurumKodu;
            public DateTime KayitTarihi;
            public int AktifAmatemPoliklinikSayisi;
            public int AktifCematemPoliklinikSayisi;
            public int AmatemYeniBasvuruSayisi;
            public int CematemYeniBasvuruSayisi;
            public int AmatemTakipBasvurusuSayisi;
            public int CematemTakipBasvurusuSayisi;
            public int AktifAmatemYatakSayisi;
            public int AktifCematemYatakSayisi;
            public int AmatemBosYatakSayisi;
            public int AmatemDoluYatakSayisi;
            public int CematemBosYatakSayisi;
            public int CematemDoluYatakSayisi;
        }
        public class PersonelBilgisiEkleModel
        {
            public int KurumKodu;
            public DateTime KayitTarihi;
            public int TumUzmanHekimSayisi;
            public int TumAsistanHekimSayisi;
            public int TumPratisyenHekimSayisi;
            public int HastaneAktifPsikiyatriUzmaniSayisi;
            public int HastaneAktifCocukPsikiyatriUzmaniSayisi;
            public int HastaneAktifPsikiyatriAsistaniSayisi;
            public int HastaneAktifCocukPsikiyatriAsistaniSayisi;
            public int HastaneAktifPsikologSayisi;
            public int HastaneAktifHemsireSayisi;
            public int AmatemAktifPsikiyatriUzmaniSayisi;
            public int CematemAktifCocukPsikiyatriUzmaniSayisi;
            public int AmatemAktifPsikiyatriAsistaniSayisi;
            public int CematemAktifCocukPsikiyatriAsistaniSayisi;
            public int AmatemAktifPsikologSayisi;
            public int CematemAktifPsikologSayisi;
            public int AmatemAktifHemsireSayisi;
            public int CematemAktifHemsireSayisi;
            public int MaddeBagimliligiEgitimiAlmisTumHekimSayisi;
            public int MaddeBagimliligiEgitimiAlmisPsikiyatriUzmaniSayisi;
            public int MaddeBagimliligiEgitimiAlmisCocukPsikiyatriUzmaniSayisi;
            public int MaddeBagimliligiEgitimiAlmisPsikologSayisi;
            public int MaddeBagimliligiEgitimiAlmisHemsireSayisi;
        }
        public class HastaYatisEkleModel
        {
            public string HastaEkipNo;
            public int KurumKodu;
            public DateTime YatisTarihi;
            public DateTime YatisSonlanmaTarihi;
            public int YatisSonlanmaID;
            public string YatisSonlanmaAciklama;
            public int KararID;
        }
        public class HastaFizikselHastalikOykusuEkleModel
        {
            public string HastaEkipNo;
            public int KurumKodu;
            public string HastaProtokolNo;
            public bool Yok;
            public bool DM;
            public bool Hipertansiyon;
            public bool Astim;
            public bool Epilepsi;
            public bool HIV;
            public bool BobrekYetmezligi;
            public bool Siroz;
            public bool Hepatit;
            public string HepatitAciklama;
            public bool KalpHastaligi;
            public string KalpHastaligiAciklama;
            public bool IlacAlerjisi;
            public string IlacAlerjisiAciklama;
            public bool Diger;
            public string DigerAciklama;
        }
        public class HastanePoliklinikBilgisiEkleModel
        {
            public int KurumKodu;
            public string PoliklinikAdi;
            public int AktifOdaSayisi;
            public DateTime KayitTarihi;
        }
        public class AcilServisVakaBilgisiEkleModel
        {
            public int KurumKodu;
            public string HastaAdi;
            public string HastaSoyadi;
            public string HastaTCKimlikNo;
            public string HastaEkipNo;
            public DateTime BasvuruTarihi;
            public string HekimAdi;
            public DateTime TaburculukTarihi;
            public string GerceklestirilenIslemler;
            public string TaburculukNotlari;
            public string ICDTaniKod;
        }
        public class KurumRandevuDurumEkleModel
        {
            public string HastaEkipNo;
            public int KurumKodu;
            public string RandevuTipi;
            public DateTime RandevuTarihSaati;
            public bool GeldiGelmedi;
        }
        #endregion

        #region Güncelleme
        public class HastaGuncelleModel
        {
            public string HastaEkipNo;
            public int KurumKodu;
            public string HastaAdi;
            public string HastaSoyadi;
            public string HastaTCKimlikNo;
            public string Telefon;
            public bool OnamDurumu;
            public DateTime KayitTarihi;
        }
        public class HastaBasvuruBilgisiGuncelleModel
        {
            public int BasvuruID;
            public int KurumKodu;
            public DateTime KayitTarihi;
            public string HastaProtokolNo;
            public string BeyanAdresi;
            public string BeyanTelefonu;
            public DateTime PoliklinikMuayeneTarihSaati;
            public int PoliklinikTuruID;
            public string MuayeneyiGerceklestirenHekim;
            public string MuayeneyiGerceklestirenHekimTuru;
            public string MaddeBilgisi;
            public string EslikEdenHastalikOykusu;
            public bool PsikiyatrikHastalikOykusu;
            public string PsikiyatrikHastalikOykusuAciklama;
            public string KullanmaktaOlduguDigerIlacBilgisi;
            public string SonBasvurudanSonraAlkolMaddeKullanimiOlmusMu;
            public string SonBasvurudanSonraOnerilenIlaclariDuzenliKullanmisMi;
            public bool YoksunlukBulgusu;
            public bool IntoksikasyonBulgusu;
            public bool IdrarToksikolojiBulgusu;
            public int KararID; 
        }
        public class HizmetBilgisiGuncelleModel
        {
            public int ID;
            public int KurumKodu;
            public DateTime KayitTarihi;
            public int AktifAmatemPoliklinikSayisi;
            public int AktifCematemPoliklinikSayisi;
            public int AmatemYeniBasvuruSayisi;
            public int CematemYeniBasvuruSayisi;
            public int AmatemTakipBasvurusuSayisi;
            public int CematemTakipBasvurusuSayisi;
            public int AktifAmatemYatakSayisi;
            public int AktifCematemYatakSayisi;
            public int AmatemBosYatakSayisi;
            public int AmatemDoluYatakSayisi;
            public int CematemBosYatakSayisi;
            public int CematemDoluYatakSayisi;
        }
        public class PersonelBilgisiGuncelleModel
        {
            public int ID;
            public int KurumKodu;
            public DateTime KayitTarihi;
            public int TumUzmanHekimSayisi;
            public int TumAsistanHekimSayisi;
            public int TumPratisyenHekimSayisi;
            public int HastaneAktifPsikiyatriUzmaniSayisi;
            public int HastaneAktifCocukPsikiyatriUzmaniSayisi;
            public int HastaneAktifPsikiyatriAsistaniSayisi;
            public int HastaneAktifCocukPsikiyatriAsistaniSayisi;
            public int HastaneAktifPsikologSayisi;
            public int HastaneAktifHemsireSayisi;
            public int AmatemAktifPsikiyatriUzmaniSayisi;
            public int CematemAktifCocukPsikiyatriUzmaniSayisi;
            public int AmatemAktifPsikiyatriAsistaniSayisi;
            public int CematemAktifCocukPsikiyatriAsistaniSayisi;
            public int AmatemAktifPsikologSayisi;
            public int CematemAktifPsikologSayisi;
            public int AmatemAktifHemsireSayisi;
            public int CematemAktifHemsireSayisi;
            public int MaddeBagimliligiEgitimiAlmisTumHekimSayisi;
            public int MaddeBagimliligiEgitimiAlmisPsikiyatriUzmaniSayisi;
            public int MaddeBagimliligiEgitimiAlmisCocukPsikiyatriUzmaniSayisi;
            public int MaddeBagimliligiEgitimiAlmisPsikologSayisi;
            public int MaddeBagimliligiEgitimiAlmisHemsireSayisi;
        }
        public class RandevuDurumuGuncelleModel
        {
            public int RandevuID;
            public string RandevuDurumu;
        }
        public class AcilServisVakaBilgisiGuncelleModel
        {
            public int BasvuruID;
            public int KurumKodu;
            public string HastaAdi;
            public string HastaSoyadi;
            public string HastaTCKimlikNo;
            public string HastaEkipNo;
            public DateTime BasvuruTarihi;
            public string HekimAdi;
            public DateTime TaburculukTarihi;
            public string GerceklestirilenIslemler;
            public string TaburculukNotlari;
            public string ICDTaniKod;
            public int ICDTaniKodID;
        }
        public class HastaYatisGuncelleModel
        {
            public int YatisID;
            public string HastaEkipNo;
            public int KurumKodu;
            public DateTime YatisTarihi;
            public string GunlukIzlemBilgisi;
            public DateTime YatisSonlanmaTarihi;
            public int YatisSonlanmaID;
            public string YatisSonlanmaAciklama;
            public int KararID;
        }
        public class KurumRandevuDurumGuncelleModel
        {
            public int RandevuID;
            public string HastaEkipNo;
            public int KurumKodu;
            public string RandevuTipi;
            public DateTime RandevuTarihSaati;
            public bool GeldiGelmedi;
        }
        #endregion

        #region Kontrol
        public class KurumKoduKontrolModel
        {
            public int KurumKodu;
        }
        public class KararKodlariKontrolModel
        {
            public int KararKodu;
        }
        public class YatisSonlanmaNedenleriKontrolModel
        {
            public int ID;
        }
        public class PoliklinikTuruKontrolModel
        {
            public int KurumID;
            public string PoliklinikTuruAdi;
            public int PoliklinikTuruID;
        }
        public class TaniKoduSorgulaModel
        {
            public int ID;
            public string ICDKodu;
            public string Tani;
        }
        #endregion

        #region Randevu
        public class RandevuSorgulaModel
        {
            public int KurumKodu;
            public int PoliklinikTuruID;
            public string PoliklinikTuru;
            public DateTime RandevuBaslangicTarihi;
            public DateTime RandevuBitisTarihi;
            public int ID;
            public int KurumID;
            public string KurumAdi;
            public string Aciklama;
            public string HastaTCKimlikNo;
            public string HastaAdi;
            public string HastaSoyadi;
            public string Telefon;
            public string YakinTelefonu;
            public string HastaEkipNo;
        }
        #endregion

    }
}