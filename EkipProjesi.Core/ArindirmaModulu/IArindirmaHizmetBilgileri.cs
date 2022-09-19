using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkipProjesi.Core.ArindirmaModulu
{
    public class IArindirmaHizmetBilgileri
    {
        public int ID { get; set; }
        public int KurumKodu { get; set; }
        public DateTime KayitTarihi { get; set; }
        public int AktifAmatemPoliklinikSayisi { get; set; }
        public int AktifCematemPoliklinikSayisi { get; set; }
        public int AmatemYeniBasvuruSayisi { get; set; }
        public int CematemYeniBasvuruSayisi { get; set; }
        public int AmatemTakipBasvurusuSayisi { get; set; }
        public int CematemTakipBasvurusuSayisi { get; set; }
        public int AktifAmatemYatakSayisi { get; set; }
        public int AktifCematemYatakSayisi { get; set; }
        public int AmatemBosYatakSayisi { get; set; }
        public int AmatemDoluYatakSayisi { get; set; }
        public int CematemBosYatakSayisi { get; set; }
        public int CematemDoluYatakSayisi { get; set; }
        public double AmatemYatakDolulukOrani { get; set; }
        public double CematemYatakDolulukOrani { get; set; }
    }
}