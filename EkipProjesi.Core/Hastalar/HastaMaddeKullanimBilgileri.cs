using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkipProjesi.Core.Hastalar
{
    public class HastaMaddeKullanimBilgileri
    {
        public int ID;
        public int HastaID;
        public int KurumKodu;
        public string HastaEkipNo;
        public string HastaProtokolNo;
        public string MaddeTuru;
        public string Sure;
        public bool KullanimSuresiAy;
        public bool KullanimSuresiYil;
        public string Miktar;
        public string Yontem;
        public string Siklik;
        public string EnSonKullanmaZamani;
        public bool EnSonKullanmaZamaniGun;
        public bool EnSonKullanmaZamaniAy;
        public bool EnSonKullanmaZamaniYil;
    }
}