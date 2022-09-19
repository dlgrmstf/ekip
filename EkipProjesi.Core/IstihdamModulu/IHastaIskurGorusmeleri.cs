using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkipProjesi.Core.IstihdamModulu
{
    public class IHastaIskurGorusmeleri
    {
        public int ID { get; set; }
        public int HastaID { get; set; }
        public string GorusmeyiYapanDanisman { get; set; }
        public DateTime? GorusmeTarihi { get; set; }
        public int? GorusmeSonucu { get; set; }
        public string YonlendirilenIsyeri { get; set; }
        public DateTime? YonlendirmeBasvuruTarihi { get; set; }
        public string IseBasladigiYer { get; set; }
        public DateTime? IseBaslamaTarihi { get; set; }
        public int? IsyeriYonlendirmeSonuc { get; set; }
        public int? IsKulubuYonlendirmeSonuc { get; set; }
        public DateTime? PlanlananSurecBaslangicTarihi { get; set; }
        public DateTime? SurecBaslamaTarihi { get; set; }
        public DateTime? SurecTamamlanmaTarihi { get; set; }
        public string SertifikaDosyaYolu { get; set; }
        public string PlanlananProgramTuru { get; set; }
        public DateTime? PlanlananProgramBaslangicTarihi { get; set; }
        public string YonlendirilenKursTuru { get; set; }
        public string YonlendirilenKursBilgisiTuru { get; set; }
        public string GorusmeSonucuDiger { get; set; }
        public DateTime GorusmeKayitTarihi { get; set; }
        public int KaydedenKullaniciID { get; set; }
        public string IseBaslamaNedeni { get; set; }
        public DateTime PlanlananBaslangicTarihi { get; set; }
        public int IsKulubuYonlendirmeDurum { get; set; }
        public int Tur { get; set; }
        public int ProgramDurum { get; set; }
        public DateTime PlanlananProgramBaslangici { get; set; }
        public int ProgramTuru { get; set; }
        public DateTime ProgramTuruTamamlanmaTarihi { get; set; }
    }
}