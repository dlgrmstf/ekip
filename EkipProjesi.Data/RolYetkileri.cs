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
    
    public partial class RolYetkileri
    {
        public int ID { get; set; }
        public int RolId { get; set; }
        public int YetkiId { get; set; }
        public bool Goruntuleme { get; set; }
        public bool Ekleme { get; set; }
        public bool Duzenleme { get; set; }
        public bool Silme { get; set; }
    }
}
