using EkipProjesi.Core.IstihdamModulu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkipProjesi.Core.Hastalar
{
    public class IHastalar
    {
        public int HastaID { get; set; }
        public int KurumKodu { get; set; }
        public string HastaAdi { get; set; }
        public string HastaSoyadi { get; set; }
        public string HastaTCKimlikNo { get; set; }
        public string HastaEkipNo { get; set; }
        public string Telefon { get; set; }
        public string Email { get; set; }
        public string Adres { get; set; }
        public string Cinsiyet { get; set; }
        public DateTime DogumTarihi { get; set; }
        public int KaydedenKullaniciID { get; set; }
        public IHastaEgitimBilgileri EgitimBilgileri { get; set; }
        public IHastaIskurGorusmeleri HastaIskurGorusmeleri { get; set; }
    }
}