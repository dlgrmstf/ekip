using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkipProjesi.Core.ArindirmaModulu
{
    public class IRandevuTakvimJson
    {
        public int id { get; set; }
        public string aciklama { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string color { get; set; }
        public bool isFullDay { get; set; }
        public string telefon { get; set; }
        public string hastaAdi { get; set; }
        public string hastaSoyadi { get; set; }
        public string hastaTCKimlikNo { get; set; }
        public int kurumId { get; set; }
        public int poliklinikTuruId { get; set; }
    }
}