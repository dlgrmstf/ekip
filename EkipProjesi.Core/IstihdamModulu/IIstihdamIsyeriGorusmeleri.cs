using System;

namespace EkipProjesi.Core.IstihdamModulu
{
    public class IIstihdamIsyeriGorusmeleri
    {
        public int ID { get; set; }
        public int IsyeriID { get; set; }
        public string GorusmeyiYapanKisi { get; set; }
        public string GorusulenKisi { get; set; }
        public string GorusmedeEleAlinanKonular { get; set; }
        public DateTime GorusmeTarihi { get; set; }
        public int KaydedenKullaniciID { get; set; }
    }
}