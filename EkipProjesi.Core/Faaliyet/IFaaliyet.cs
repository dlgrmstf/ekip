using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkipProjesi.Core.Faaliyet
{
    public class IFaaliyet
    {
        public int ID { get; set; }
        public string FaaliyetAdi { get; set; }
        public string FaaliyetKonusu { get; set; }
        public string FaaliyetTuru { get; set; }
        public DateTime FaaliyetTarihi { get; set; }
        public string GerceklestigiYer { get; set; }
        public string GerceklestirenKurum { get; set; }
        public string GerceklestirenKisi { get; set; }
        public int UlasilanKisiSayisi { get; set; }
        public string FaaliyetRaporu { get; set; }
    }
}