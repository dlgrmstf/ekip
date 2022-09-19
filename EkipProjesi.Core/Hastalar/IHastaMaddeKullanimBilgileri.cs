using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkipProjesi.Core.Hastalar
{
    public class IHastaMaddeKullanimBilgileri
    {
        public int ID { get; set; }
        public int KurumKodu { get; set; }
        public string HastaEkipNo { get; set; }
        public string HastaProtokolNo { get; set; }
        public string MaddeTuru { get; set; }
        public string Sure { get; set; }
        public bool? KullanimSuresiAy { get; set; }
        public bool? KullanimSuresiYil { get; set; }
        public string Miktar { get; set; }
        public string Yontem { get; set; }
        public string Siklik { get; set; }
        public string EnSonKullanmaZamani { get; set; }
        public bool? EnSonKullanmaZamaniGun { get; set; }
        public bool? EnSonKullanmaZamaniAy { get; set; }
        public bool? EnSonKullanmaZamaniYil { get; set; }
    }
}