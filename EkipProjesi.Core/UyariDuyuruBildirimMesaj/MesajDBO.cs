using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkipProjesi.Core.UyariDuyuruBildirimMesaj
{
    public class MesajDBO
    {
        public int ID { get; set; }
        public string Baslik { get; set; }
        public string Icerik { get; set; }
        public Nullable<System.DateTime> BaslangicTarihi { get; set; }
        public Nullable<System.DateTime> BitisTarihi { get; set; }
        public Nullable<long> IliskiliTabloId { get; set; }
        public Nullable<int> IliskiliModulId { get; set; }
        public Nullable<int> MesajDurum { get; set; }
        public Nullable<System.DateTime> KayitTarihi { get; set; }
        public Nullable<int> KaydedenKullaniciId { get; set; }

        public string KaydedenKullaniciAdi { get; set; }
        public string Url { get; set; }
        public List<int> Kullanicilar { get; set; }
    }

    public enum MesajDurum
    {
        Aktif = 1,
        Okundu = 2,
        Pasif = 3,
    }
   
}