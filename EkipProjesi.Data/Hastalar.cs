//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EkipProjesi.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Hastalar
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Hastalar()
        {
            this.HastaYakinBilgileri = new HashSet<HastaYakinBilgileri>();
            this.HastaNotlari = new HashSet<HastaNotlari>();
            this.HastaIlkKayitBilgileri = new HashSet<HastaIlkKayitBilgileri>();
            this.HastaIzlemBilgileri = new HashSet<HastaIzlemBilgileri>();
            this.HastaTimeline = new HashSet<HastaTimeline>();
            this.RandevuTalepleri = new HashSet<RandevuTalepleri>();
            this.MobilEkipVakaFormlari = new HashSet<MobilEkipVakaFormlari>();
            this.HastaIskurGorusmeleri = new HashSet<HastaIskurGorusmeleri>();
            this.HastaBaharFormlari = new HashSet<HastaBaharFormlari>();
            this.HastaOlceklerFormu = new HashSet<HastaOlceklerFormu>();
            this.HastaBagimlilikSiddetiFormlari = new HashSet<HastaBagimlilikSiddetiFormlari>();
        }
    
        public int HastaID { get; set; }
        public int KurumKodu { get; set; }
        public string HastaAdi { get; set; }
        public string HastaSoyadi { get; set; }
        public string HastaTCKimlikNo { get; set; }
        public string Telefon { get; set; }
        public System.DateTime KayitTarihi { get; set; }
        public Nullable<System.DateTime> GuncellemeTarihi { get; set; }
        public string Cinsiyet { get; set; }
        public Nullable<System.DateTime> DogumTarihi { get; set; }
        public string Email { get; set; }
        public string Adres { get; set; }
        public string HastaEkipNo { get; set; }
        public Nullable<bool> OnamDurumu { get; set; }
        public Nullable<int> KaydedenKullaniciID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HastaYakinBilgileri> HastaYakinBilgileri { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HastaNotlari> HastaNotlari { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HastaIlkKayitBilgileri> HastaIlkKayitBilgileri { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HastaIzlemBilgileri> HastaIzlemBilgileri { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HastaTimeline> HastaTimeline { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RandevuTalepleri> RandevuTalepleri { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MobilEkipVakaFormlari> MobilEkipVakaFormlari { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HastaIskurGorusmeleri> HastaIskurGorusmeleri { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HastaBaharFormlari> HastaBaharFormlari { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HastaOlceklerFormu> HastaOlceklerFormu { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HastaBagimlilikSiddetiFormlari> HastaBagimlilikSiddetiFormlari { get; set; }
    }
}