using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkipProjesi.Core.Randevu
{
    public class ILogSilinenRandevular
    {
        public int ID { get; set; }
        public int RandevuID { get; set; }
        public int KurumID { get; set; }
        public int PoliklinikTuruID { get; set; }
        public int HastaID { get; set; }
        public string Aciklama { get; set; }
        public bool TumGun { get; set; }
        public string Renk { get; set; }
        public string HastaTCKimlikNo { get; set; }
        public string HastaAdi { get; set; }
        public string HastaSoyadi { get; set; }
        public string Telefon { get; set; }
        public string YakinTelefonu { get; set; }
        public DateTime RandevuBaslangicTarihi { get; set; }
        public DateTime RandevuBitisTarihi { get; set; }
        public string LogRandevuIptalNedeni { get; set; }
    }
}