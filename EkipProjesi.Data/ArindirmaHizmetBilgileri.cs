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
    
    public partial class ArindirmaHizmetBilgileri
    {
        public int ID { get; set; }
        public int KurumKodu { get; set; }
        public System.DateTime KayitTarihi { get; set; }
        public Nullable<int> AktifAmatemPoliklinikSayisi { get; set; }
        public Nullable<int> AktifCematemPoliklinikSayisi { get; set; }
        public Nullable<int> AmatemYeniBasvuruSayisi { get; set; }
        public Nullable<int> CematemYeniBasvuruSayisi { get; set; }
        public Nullable<int> AmatemTakipBasvurusuSayisi { get; set; }
        public Nullable<int> CematemTakipBasvurusuSayisi { get; set; }
        public Nullable<int> AktifAmatemYatakSayisi { get; set; }
        public Nullable<int> AktifCematemYatakSayisi { get; set; }
        public Nullable<int> AmatemBosYatakSayisi { get; set; }
        public Nullable<int> AmatemDoluYatakSayisi { get; set; }
        public Nullable<int> CematemBosYatakSayisi { get; set; }
        public Nullable<int> CematemDoluYatakSayisi { get; set; }
        public Nullable<System.DateTime> LogTarihi { get; set; }
        public Nullable<System.DateTime> GuncellemeTarihi { get; set; }
    }
}