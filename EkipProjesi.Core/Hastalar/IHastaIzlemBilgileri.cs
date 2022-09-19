using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkipProjesi.Core.Hastalar
{
    public class IHastaIzlemBilgileri
    {
        public int ID { get; set; }
        public int HastaID { get; set; }
        public DateTime IzlemTarihi { get; set; }
        public string IzlemBasligi { get; set; }
        public string IzlemAciklama { get; set; }
        public string IzlemYapanKurumTuru { get; set; }
        public string IzlemYapanKurum { get; set; }
        public string IzlemVakaKaynagi { get; set; }
        public string EldeEdilenBilgiler { get; set; }
        public string IzlemSonucu { get; set; }
    }
}