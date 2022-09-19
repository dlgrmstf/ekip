using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkipProjesi.Core.Randevu
{
    public class IRandevuBilgileri
    {
        public int ID { get; set; }
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
        public TimeSpan RandevuBaslangicSaati { get; set; }
        public DateTime RandevuBitisTarihi { get; set; }
        public TimeSpan RandevuBitisSaati { get; set; }
        public string RandevuDurumu { get; set; }
        public string PoliklinikAdi { get; set; }
        public string KurumAdi { get; set; }
        public int KaydedenKullaniciID { get; set; }
        public int GuncelleyenKullaniciID { get; set; }
        public string KaydedenKullaniciKurum { get; set; }
        public string KaydedenKullaniciKurumTuru { get; set; }
    }
}