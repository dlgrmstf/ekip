using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkipProjesi.Core.IstihdamModulu
{
    public class IIstihdamIsyeriAdresBilgisi
    {
        public int ID { get; set; }
        public int IsyeriID { get; set; }
        public int? MahalleID { get; set; }
        public string Sokak { get; set; }
        public string Cadde { get; set; }
        public string BinaNo { get; set; }
        public string DaireNo { get; set; }
        public int IlID { get; set; }
        public int IlceID { get; set; }
        public string IlAd { get; set; }
        public string IlceAd { get; set; }
        public string MahalleAd { get; set; }
    }
}