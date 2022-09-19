using EkipProjesi.Core.Randevu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkipProjesi.Core.ArindirmaModulu
{
    public class IKurumlarFiltreModel
    {
        public int KurumID { get; set; }
        public int PoliklinikTuruID { get; set; }
        public DateTime Tarih { get; set; }
        public IRandevuAyarlari Randevu { get; set; }
        public decimal MuayeneSuresi { get; set; }
    }
}