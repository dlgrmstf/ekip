using System;

namespace EkipProjesi.Core.Kullanici
{
    public class IKullaniciGorevTakip
    {
        public int ID { get; set; }
        public int KullaniciID { get; set; }
        public string Baslik { get; set; }
        public string Aciklama { get; set; }
        public DateTime Tarih { get; set; }
        public string Durum { get; set; }
    }
}