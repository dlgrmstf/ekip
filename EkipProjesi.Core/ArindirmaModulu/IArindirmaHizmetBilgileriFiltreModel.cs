using EkipProjesi.Core.ArindirmaModulu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkipProjesi.Core.ArindirmaModulu
{
    public class IArindirmaHizmetBilgileriFiltreModel
    {
        public string KurumAdi { get; set; }
        public DateTime BaslangicTarihi { get; set; }
        public DateTime BitisTarihi { get; set; }
        public List<IArindirmaHizmetBilgileri> HizmetBilgileri { get; set; }
    }
}