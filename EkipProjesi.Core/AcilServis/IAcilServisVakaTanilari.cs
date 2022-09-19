using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkipProjesi.Core.AcilServis
{
    public class IAcilServisVakaTanilari
    {
         public int ID { get; set; }
         public int AcilServisVakaID { get; set; }
         public int TaniKoduID { get; set; }
         public string TaniKodu { get; set; }
         public string Tani { get; set; }
    }
}