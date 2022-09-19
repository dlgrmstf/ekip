using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkipProjesi.Core.ArindirmaModulu
{
    public class IYatislar
    {
        public int ID { get; set; }
        public int HastaID { get; set; }
        public int KurumKodu { get; set; }
        public DateTime YatisTarihi { get; set; }
        public string GunlukIzlemBilgisi { get; set; }
        public DateTime YatisSonlanmaTarihi { get; set; }
        public int YatisSonlanmaID { get; set; }
        public string YatisSonlanmaNedeni { get; set; }
        public string YatisSonlanmaAciklama { get; set; }
    }
}