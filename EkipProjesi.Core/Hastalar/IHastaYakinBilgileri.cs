using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkipProjesi.Core.Hastalar
{
    public class IHastaYakinBilgileri
    {
        public int ID { get; set; }
        public int HastaID { get; set; }
        public string YakinAdi { get; set; }
        public string YakinSoyadi { get; set; }
        public string YakinlikDerecesi { get; set; }
        public string Telefon { get; set; }
        public string Adres { get; set; }
    }
}