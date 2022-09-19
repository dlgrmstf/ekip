using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkipProjesi.Core.Hastalar
{
    public class IHastaIlkKayitBilgileri
    {
        public int ID { get; set; }
        public int HastaID { get; set; }
        public DateTime KayitTarihi { get; set; }
        public string KurumTuru { get; set; }
        public string KaydedenKurum { get; set; }
        public string VakaBilgiKaynagi { get; set; }
        public string EldeEdilenBilgiler { get; set; }
    }
}