using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkipProjesi.Core.KurumlarArasiSevkVeTakip
{
    public class IRandevuTalepleri
    {
        public int ID { get; set; }
        public int KurumID { get; set; }
        public int HizmetMerkeziID { get; set; }
        public int HastaID { get; set; }
        public string HastaAdi { get; set; }
        public string HastaSoyadi { get; set; }
        public string HastaTCKimlikNo { get; set; }
        public string Telefon { get; set; }
        public DateTime? RandevuTarihi { get; set; }
        public string RandevuTalebiNotu { get; set; }
        public bool? RandevuVerildiMi { get; set; }
        public int TalepOlusturanKullaniciID { get; set; }
        public int? RandevuVerenKullaniciID { get; set; }
        public string RandevuDurumu { get; set; }
        public DateTime KayitTarihi { get; set; }
        public string HizmetMerkeziAdi { get; set; }
    }
}