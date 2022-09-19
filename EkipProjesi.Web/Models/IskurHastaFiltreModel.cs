using EkipProjesi.Core.Hastalar;
using System.Collections.Generic;

namespace EkipProjesi.Web.Models
{
    public class IskurHastaFiltreModel
    {
        public string HastaAdi { get; set; }
        public string HastaSoyadi { get; set; }
        public List<IHastalar> Hastalar { get; set; }
    }
}