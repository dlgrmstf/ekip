using System;

namespace EkipProjesi.Core.IstihdamModulu
{
    public class IIstihdamIsyerleri
    {
        public int ID { get; set; }
        public string IsyeriAdi { get; set; }
        public int? SektorID { get; set; }
        public IIstihdamIsyeriAdresBilgisi IlceBilgisi { get; set; }
        public int ToplamCalisanSayisi { get; set; }
        public string Notlar { get; set; }
        public DateTime KayitTarihi { get; set; }
        public DateTime GuncellemeTarihi { get; set; }
        public int KaydedenKullaniciID { get; set; }
        public IIstihdamIsyeriAdresBilgisi IsyeriAdresBilgisi { get; set; }
        public string SektorAdi { get; set; }
    }
}