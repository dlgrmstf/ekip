using EkipProjesi.Core.AcilServis;
using System;
using System.Collections.Generic;

namespace EkipProjesi.Core.AcilServis
{
    public class IAcilServisVakaBilgileri
    {
        public int ID { get; set; }
        public int KurumKodu { get; set; }
        public string HastaTCKimlikNo { get; set; }
        public string HastaEkipNo { get; set; }
        public string HastaAdi { get; set; }
        public string HastaSoyadi { get; set; }
        public DateTime? BasvuruTarihi { get; set; }
        public string HekimAdi { get; set; }
        public DateTime? TaburculukTarihi { get; set; }
        public string GerceklestirilenIslemler { get; set; }
        public string TaburculukNotlari { get; set; }
        public string KurumAdi { get; set; }
        public List<IAcilServisVakaTanilari> Tanilar { get; set; }
    }
}