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
    
    public partial class HastaBeyanBilgileri
    {
        public int ID { get; set; }
        public int BasvuruID { get; set; }
        public string BeyanAdresi { get; set; }
        public string BeyanTelefonu { get; set; }
        public string BeyanAldigiKurum { get; set; }
        public Nullable<System.DateTime> BeyanTarihi { get; set; }
        public Nullable<System.DateTime> GuncellemeTarihi { get; set; }
    
        public virtual ArindirmaBasvuruBilgileri ArindirmaBasvuruBilgileri { get; set; }
    }
}