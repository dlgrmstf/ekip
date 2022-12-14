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
    
    public partial class IstihdamIsyerleri
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public IstihdamIsyerleri()
        {
            this.IstihdamIsyeriIletisimKisileri = new HashSet<IstihdamIsyeriIletisimKisileri>();
            this.IstihdamIsyeriGorusmeleri = new HashSet<IstihdamIsyeriGorusmeleri>();
            this.IstihdamIsyeriAdresBilgisi = new HashSet<IstihdamIsyeriAdresBilgisi>();
        }
    
        public int ID { get; set; }
        public string IsyeriAdi { get; set; }
        public Nullable<int> ToplamCalisanSayisi { get; set; }
        public string Notlar { get; set; }
        public System.DateTime KayitTarihi { get; set; }
        public Nullable<System.DateTime> GuncellemeTarihi { get; set; }
        public Nullable<int> KaydedenKullaniciID { get; set; }
        public Nullable<int> SektorID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IstihdamIsyeriIletisimKisileri> IstihdamIsyeriIletisimKisileri { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IstihdamIsyeriGorusmeleri> IstihdamIsyeriGorusmeleri { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IstihdamIsyeriAdresBilgisi> IstihdamIsyeriAdresBilgisi { get; set; }
        public virtual IstihdamIsyeriSektorler IstihdamIsyeriSektorler { get; set; }
    }
}
