using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkipProjesi.Core.Randevu
{
    public class IRandevuAyarlari
    {
        public int ID { get; set; }
        public int KurumID { get; set; }
        public int PoliklinikTuruID { get; set; }
        public int Gun { get; set; }
        public string KurumAdi { get; set; }
        public string PoliklinikAdi { get; set; }
        public TimeSpan BaslangicSaati { get; set; }
        public TimeSpan BitisSaati { get; set; }
        public int SlotSayisi { get; set; }
        public DateTime LogTarihi { get; set; }
        public bool Durum { get; set; }
        public DateTime? PasifBaslangicTarihi { get; set; }
        public DateTime? PasifBitisTarihi { get; set; }
        public decimal MuayeneSuresi { get; set; }
        public int KaydedenKullaniciID { get; set; }

    }
}