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
    
    public partial class ArindirmaBasvuruKararlari
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ArindirmaBasvuruKararlari()
        {
            this.ArindirmaBasvuruBilgileri = new HashSet<ArindirmaBasvuruBilgileri>();
            this.Yatislar = new HashSet<Yatislar>();
        }
    
        public string Karar { get; set; }
        public string Aciklama { get; set; }
        public int KararID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ArindirmaBasvuruBilgileri> ArindirmaBasvuruBilgileri { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Yatislar> Yatislar { get; set; }
    }
}