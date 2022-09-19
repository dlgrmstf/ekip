using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkipProjesi.Core.RehabilitasyonMerkezleri
{
    public class IHastaBaharFormuSonuc
    {
        public int ID { get; set; }
        public int FormID { get; set; }
        public int KodID { get; set; }
        public string KodAdi { get; set; }
        public List<IHastaBaharFormuSonucKodlari> SonucKodlari { get; set; }
    }
}