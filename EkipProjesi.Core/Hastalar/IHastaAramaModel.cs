using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkipProjesi.Core.Hastalar
{
    public class IHastaAramaModel
    {
        public string HastaTC { get; set; }
        public string HastaEkipNo { get; set; }
        public List<IHastalar> Hastalar { get; set; }
    }
}