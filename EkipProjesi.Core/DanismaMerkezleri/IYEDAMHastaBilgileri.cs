using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkipProjesi.Core.DanismaMerkezleri
{
    public class IYEDAMHastaBilgileri
    {
        public int ID { get; set; }
        public string HastaTCKimlikNo { get; set; }
        public string HastaEkipNo { get; set; }
        public string HastaAdi { get; set; }
        public string HastaSoyadi { get; set; }
        public DateTime? BasvuruTarihi { get; set; }
        public string BasvuruTuru { get; set; }
        public string BasvurudaYapilanIslemler { get; set; }
        public string BasvuruDegerlendirmeNotlari { get; set; }
    }
}