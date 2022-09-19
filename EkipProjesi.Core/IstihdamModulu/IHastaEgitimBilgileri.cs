using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkipProjesi.Core.IstihdamModulu
{
    public class IHastaEgitimBilgileri
    {
        public int ID { get; set; }
        public int HastaID { get; set; }
        public int? OgrenimDurumu { get; set; }
        public int? LiseTuru { get; set; }
        public string Ilkokul { get; set; }
        public string Ortaokul { get; set; }
        public string Lise { get; set; }
        public string Universite { get; set; }
        public string Fakulte { get; set; }
        public string Bolum { get; set; }
        public string IsGecmisi { get; set; }
        public string SertifikaVeYeterlilikler { get; set; }
        public DateTime KayitTarihi { get; set; }
        public int? KaydedenKullaniciID { get; set; }
        public int? GuncelleyenKullaniciID { get; set; }
    }
}