using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkipProjesi.Core.KurumlarArasiSevkVeTakip
{
    public class IKurumHizmetMerkezleri
    {
        public int ID { get; set; }
        public int KurumID { get; set; }
        public string Ad { get; set; }
        public string Adres { get; set; }
        public string Telefon { get; set; }
    }
}