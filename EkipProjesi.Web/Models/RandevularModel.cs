using System.Collections.Generic;
using EkipProjesi.Core.Hastalar;
using EkipProjesi.Core.Randevu;

namespace EkipProjesi.Web.Models
{
    public class RandevularModel
    {
        public List<IRandevuBilgileri> RandevuBilgileriList { get; set; }
        public List<IHastalar> HastalarList { get; set; }

    }
}