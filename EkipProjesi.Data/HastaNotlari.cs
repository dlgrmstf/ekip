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
    
    public partial class HastaNotlari
    {
        public int ID { get; set; }
        public string NotBilgisi { get; set; }
        public System.DateTime KayitTarihi { get; set; }
        public int KaydedenKullaniciID { get; set; }
        public int HastaID { get; set; }
    
        public virtual Kullanicilar Kullanicilar { get; set; }
        public virtual Hastalar Hastalar { get; set; }
    }
}