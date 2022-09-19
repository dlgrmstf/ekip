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
    
    public partial class RandevuTalepleri
    {
        public int ID { get; set; }
        public int KurumID { get; set; }
        public int HizmetMerkeziID { get; set; }
        public int HastaID { get; set; }
        public string HastaAdi { get; set; }
        public string HastaSoyadi { get; set; }
        public string HastaTCKimlikNo { get; set; }
        public string Telefon { get; set; }
        public Nullable<System.DateTime> RandevuTarihi { get; set; }
        public Nullable<bool> RandevuVerildiMi { get; set; }
        public int TalepOlusturanKullaniciID { get; set; }
        public Nullable<int> RandevuVerenKullaniciID { get; set; }
        public string RandevuDurumu { get; set; }
        public System.DateTime KayitTarihi { get; set; }
        public string RandevuTalebiNotu { get; set; }
    
        public virtual Hastalar Hastalar { get; set; }
        public virtual KurumHizmetMerkezleri KurumHizmetMerkezleri { get; set; }
        public virtual Kurumlar Kurumlar { get; set; }
    }
}