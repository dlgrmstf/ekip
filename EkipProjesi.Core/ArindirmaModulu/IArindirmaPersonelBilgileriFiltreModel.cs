using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkipProjesi.Core.ArindirmaModulu
{
    public class IArindirmaPersonelBilgileriFiltreModel
    {
        public string KurumAdi { get; set; }
        public DateTime BaslangicTarihi { get; set; }
        public DateTime BitisTarihi { get; set; }
        public List<IArindirmaPersonelBilgileri> PersonelBilgileri { get; set; }
    }
}