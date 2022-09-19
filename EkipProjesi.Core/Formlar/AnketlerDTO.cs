using EkipProjesi.Core.Hastalar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EkipProjesi.Core.Formlar
{
    public class AnketDTO
    {
        public long ID { get; set; }
        [Required]
        public int IsletmeID { get; set; }
        [Required]
        public int AnketTipID { get; set; }
        public string AnketTipAdi { get; set; }
        [Required]
        public string Baslik { get; set; }
        public string Aciklama { get; set; }
        [Required]
        public System.DateTime BaslangicTarihi { get; set; }
        [Required]
        public System.DateTime BitisTarihi { get; set; }
        public System.DateTime KayitTarihi { get; set; }
        public int UserID { get; set; }
        public bool Status { get; set; }

        public int KullaniciSayisi { get; set; }
        public int TamamlayanSayisi { get; set; }
        public int SoruSayisi { get; set; }
        public List<AnketDTO> Anketler { get; set; }
        public List<AnketTipleriDTO> AnketTipleri { get; set; }
        public List<AnketKullanicilariDTO> AnketKullanicilari { get; set; }
        //public List<IUstBirim> UstBirimler { get; set; }
        public List<IHastalar> Personeller { get; set; }
        public List<AnketGruplariDTO> AnketGruplari { get; set; }
        public List<AnketSorulariDTO> Sorular { get; set; }
        public List<AnketSoruBankasiDTO> SoruBankasi { get; set; }
        public AnketDTO()
        {
            Anketler = new List<AnketDTO>();
            AnketTipleri = new List<AnketTipleriDTO>();
            AnketKullanicilari = new List<AnketKullanicilariDTO>();
            //UstBirimler = new List<IUstBirim>();
            Personeller = new List<IHastalar>();
            AnketGruplari = new List<AnketGruplariDTO>();
            Sorular = new List<AnketSorulariDTO>();
            SoruBankasi = new List<AnketSoruBankasiDTO>();
        }
    }

    public class AnketTipleriDTO
    {
        public int ID { get; set; }
        [Required]
        public string TipAdi { get; set; }
        [Required]
        public string Aciklama { get; set; }
        public bool Status { get; set; }
        public DateTime KayitTarihi { get; set; }
        public int UserID { get; set; }
    }

    public class AnketKullanicilariDTO
    {
        public long ID { get; set; }
        public long AnketID { get; set; }
        public int HastaID { get; set; }
        public int KullaniciID { get; set; }
        public DateTime Tarih { get; set; }
        public bool TamamlandiMi { get; set; }

        public string HastaAdi { get; set; }
        public string HastaSoyadi { get; set; }
        public string HastaTCKimlikNo { get; set; }
        public string UstBirimAdi { get; set; }
        public int UstBirimID { get; set; }
        public string BirimAdi { get; set; }
        public int BirimID { get; set; }
        public string AltBirimAdi { get; set; }
        public int AltBirimID { get; set; }

        public int? Personel { get; set; }
        public string KullaniciMail { get; set; }
        public DateTime? DogumTarihi { get; set; }
        public string Cinsiyet { get; set; }
        public int? Egitim { get; set; }
        public EgitimSekli EgitimSekli { get; set; }
        public int _EgitimSekli
        {
            get { return (short)EgitimSekli; }
            set { EgitimSekli = (EgitimSekli)value; }
        }
        public List<AnketKullaniciCevapDTO> Cevaplar { get; set; }
    }

    public class AnketKullaniciCevapDTO
    {
        public long ID { get; set; }
        public long AnketSoruID { get; set; }
        public int UserID { get; set; }
        public System.DateTime Tarih { get; set; }
        public long CevapID { get; set; }
        public string Cevap { get; set; }

        public string KullaniciAdi { get; set; }
        public List<AnketKullaniciCevapDTO> Cevaplar { get; set; }
    }

    public class AnketSoruBankasiDTO
    {
        public long ID { get; set; }
        [Required]
        public string Soru { get; set; }
        public string SoruKodu { get; set; }
        public string Aciklama { get; set; }
        public int UserID { get; set; }
        public System.DateTime KayitTarihi { get; set; }
        public bool Status { get; set; }
    }

    public class AnketGruplariDTO
    {
        public long ID { get; set; }
        [Required]
        public string GrupAdi { get; set; }
        [Required]
        public long AnketID { get; set; }
        public string Aciklama { get; set; }
        [Required]
        public int Sira { get; set; }
        public bool Status { get; set; }
        public int UserID { get; set; }
        public DateTime KayitTarihi { get; set; }
        public List<AnketSorulariDTO> Sorular { get; set; }

        public AnketGruplariDTO()
        {
            Sorular = new List<AnketSorulariDTO>();
        }
    }

    public class AnketCevaplariDTO
    {
        public long ID { get; set; }
        [Required]
        public long SoruID { get; set; }
        [Required]
        public string CevapAdi { get; set; }
        [Required]
        public int Puan { get; set; }
    }

    public class AnketSorulariDTO
    {
        public AnketSorulariDTO()
        {
            Cevaplar = new List<AnketCevaplariDTO>();
            KullaniciCevaplari = new List<AnketKullaniciCevapDTO>();
        }
        public long ID { get; set; }
        [Required]
        public long GrupID { get; set; }
        [Required]
        public long AnketID { get; set; }
        [Required]
        public long SoruID { get; set; }
        [Required]
        public bool ZorunluMu { get; set; }
        [Required]
        public int SoruTipi { get; set; }
        [Required]
        public double Agirlik { get; set; }
        [Required]
        public int CevapSayisi { get; set; }

        public string SoruAdi { get; set; }
        public string SoruAciklama { get; set; }
        public string GrupAdi { get; set; }

        public AnketSoruTipi SoruTipleri { get; set; }
        public int _SoruTipi
        {
            get { return (short)SoruTipleri; }
            set { SoruTipleri = (AnketSoruTipi)value; }
        }

        public List<AnketCevaplariDTO> Cevaplar { get; set; }
        public List<AnketKullaniciCevapDTO> KullaniciCevaplari { get; set; }

    }
}