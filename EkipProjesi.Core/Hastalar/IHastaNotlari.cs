using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkipProjesi.Core.Hastalar
{
    public class IHastaNotlari
    {
        public int ID { get; set; }
        public int HastaID { get; set; }
        public string NotBilgisi { get; set; }
        public DateTime KayitTarihi { get; set; }
        public int KaydedenKullaniciID { get; set; }
        public string KullaniciAdi { get; set; }
    }
}