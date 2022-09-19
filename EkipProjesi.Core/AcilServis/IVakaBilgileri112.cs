using System;
using System.Collections.Generic;

namespace EkipProjesi.Core.AcilServis
{
    public class IVakaBilgileri112
    {
        public int ID { get; set; }
        public int KurumKodu { get; set; }
        public string HastaTCKimlikNo { get; set; }
        public string HastaEkipNo { get; set; }
        public string HastaAdi { get; set; }
        public string HastaSoyadi { get; set; }
        public DateTime? CagriTarihSaati { get; set; }
        public string OnTani { get; set; }
        public string YapilanIslem { get; set; }
        public string AlindigiYer { get; set; }
        public string BirakildigiKurum { get; set; }
        public string KurumAdi { get; set; }
        public List<IVakaTanilari112> Tanilar { get; set; }
    }
}