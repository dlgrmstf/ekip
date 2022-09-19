using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkipProjesi.Core.IstihdamModulu
{
    public class IIstihdamIsyeriIletisimKisileri
    {
        public int ID { get; set; }
        public int IsyeriID { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Unvan { get; set; }
        public string Telefon { get; set; }
        public string EPosta { get; set; }
        public string Adres { get; set; }
        public int KaydedenKullaniciID { get; set; }
    }
}