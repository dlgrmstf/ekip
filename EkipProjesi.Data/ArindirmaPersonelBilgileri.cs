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
    
    public partial class ArindirmaPersonelBilgileri
    {
        public int ID { get; set; }
        public int KurumKodu { get; set; }
        public System.DateTime KayitTarihi { get; set; }
        public Nullable<int> TumUzmanHekimSayisi { get; set; }
        public Nullable<int> TumAsistanHekimSayisi { get; set; }
        public Nullable<int> TumPratisyenHekimSayisi { get; set; }
        public Nullable<int> HastaneAktifPsikiyatriUzmaniSayisi { get; set; }
        public Nullable<int> HastaneAktifCocukPsikiyatriUzmaniSayisi { get; set; }
        public Nullable<int> HastaneAktifPsikiyatriAsistaniSayisi { get; set; }
        public Nullable<int> HastaneAktifCocukPsikiyatriAsistaniSayisi { get; set; }
        public Nullable<int> HastaneAktifPsikologSayisi { get; set; }
        public Nullable<int> HastaneAktifHemsireSayisi { get; set; }
        public Nullable<int> AmatemAktifPsikiyatriUzmaniSayisi { get; set; }
        public Nullable<int> CematemAktifCocukPsikiyatriUzmaniSayisi { get; set; }
        public Nullable<int> AmatemAktifPsikiyatriAsistaniSayisi { get; set; }
        public Nullable<int> CematemAktifCocukPsikiyatriAsistaniSayisi { get; set; }
        public Nullable<int> AmatemAktifPsikologSayisi { get; set; }
        public Nullable<int> CematemAktifPsikologSayisi { get; set; }
        public Nullable<int> AmatemAktifHemsireSayisi { get; set; }
        public Nullable<int> CematemAktifHemsireSayisi { get; set; }
        public Nullable<int> MaddeBagimliligiEgitimiAlmisTumHekimSayisi { get; set; }
        public Nullable<int> MaddeBagimliligiEgitimiAlmisPsikiyatriUzmaniSayisi { get; set; }
        public Nullable<int> MaddeBagimliligiEgitimiAlmisCocukPsikiyatriUzmaniSayisi { get; set; }
        public Nullable<int> MaddeBagimliligiEgitimiAlmisPsikologSayisi { get; set; }
        public Nullable<int> MaddeBagimliligiEgitimiAlmisHemsireSayisi { get; set; }
        public Nullable<System.DateTime> LogTarihi { get; set; }
        public Nullable<System.DateTime> GuncellemeTarihi { get; set; }
    }
}