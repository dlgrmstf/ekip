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
    
    public partial class AnketKullanicilari
    {
        public long ID { get; set; }
        public long AnketID { get; set; }
        public int PersonelID { get; set; }
        public System.DateTime Tarih { get; set; }
        public bool TamamlandiMi { get; set; }
    }
}
