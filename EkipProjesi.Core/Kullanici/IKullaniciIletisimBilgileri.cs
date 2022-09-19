using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkipProjesi.Core.Kullanici
{
    public class IKullaniciIletisimBilgileri
    {
        public int ID { get; set; }
        public int KullaniciID { get; set; }
        public string Telefon { get; set; }
        public string KurumTelefonu { get; set; }
        public string Mahalle { get; set; }
        public string Sokak { get; set; }
        public string Cadde { get; set; }
        public string BinaNo { get; set; }
        public string DaireNo { get; set; }
        public int IlID { get; set; }
        public int IlceID { get; set; }
        public string IlAd { get; set; }
        public string IlceAd { get; set; }
    }
}