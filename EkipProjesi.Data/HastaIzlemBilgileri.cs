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
    
    public partial class HastaIzlemBilgileri
    {
        public int ID { get; set; }
        public int HastaID { get; set; }
        public System.DateTime IzlemTarihi { get; set; }
        public string IzlemYapanKurumTuru { get; set; }
        public string IzlemYapanKurum { get; set; }
        public string IzlemVakaKaynagi { get; set; }
        public string EldeEdilenBilgiler { get; set; }
        public string IzlemSonucu { get; set; }
        public string IzlemBasligi { get; set; }
        public string IzlemAciklama { get; set; }
    
        public virtual Hastalar Hastalar { get; set; }
    }
}
