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
    
    public partial class Faaliyetler
    {
        public int ID { get; set; }
        public string FaaliyetAdi { get; set; }
        public string FaaliyetKonusu { get; set; }
        public string FaaliyetTuru { get; set; }
        public Nullable<System.DateTime> FaaliyetTarihi { get; set; }
        public string GerceklestigiYer { get; set; }
        public string GerceklestirenKurum { get; set; }
        public string GerceklestirenKisi { get; set; }
        public Nullable<int> UlasilanKisiSayisi { get; set; }
        public string FaaliyetRaporu { get; set; }
        public System.DateTime KayitTarihi { get; set; }
    }
}
