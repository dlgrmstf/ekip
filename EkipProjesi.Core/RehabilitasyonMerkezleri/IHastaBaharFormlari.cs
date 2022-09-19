using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkipProjesi.Core.RehabilitasyonMerkezleri
{
    public class IHastaBaharFormlari
    {
        public int ID { get; set; }
        public int HastaID { get; set; }
        public int CalismaDurumu { get; set; }
        public int EkonomikDurumu { get; set; }
        public string YasadigiIl { get; set; }
        public string YasadigiIlce { get; set; }
        public int GorusmeninYapildigiMerkez { get; set; }
        public string Adres { get; set; }
        public string Telefon { get; set; }
        public string YakinAdiSoyadi { get; set; }
        public string YakinTelefonu { get; set; }
        public int GelisSekli { get; set; }
        public int TedaviBasvuruNedeni { get; set; }
        public string SonTedaviBasvuruNedeni { get; set; }
        public int S1TutunUrunleri { get; set; }
        public int S1AlkolluIcecekler { get; set; }
        public int S1Esrar { get; set; }
        public int S1Kokain { get; set; }
        public int S1AmfctaminTipiUyaricilar { get; set; }
        public int S1Inhalanlar { get; set; }
        public int S1Sakinlestiriciler { get; set; }
        public int S1Halüsinojenler { get; set; }
        public int S1Opioidler { get; set; }
        public int S1Diger { get; set; }
        public string S1DigerAciklama { get; set; }
        public int S2TutunUrunleri { get; set; }
        public int S2AlkolluIcecekler { get; set; }
        public int S2Esrar { get; set; }
        public int S2Kokain { get; set; }
        public int S2AmfctaminTipiUyaricilar { get; set; }
        public int S2Inhalanlar { get; set; }
        public int S2Sakinlestiriciler { get; set; }
        public int S2Halüsinojenler { get; set; }
        public int S2Opioidler { get; set; }
        public int S2Diger { get; set; }
        public string S2DigerAciklama { get; set; }
        public int S3TutunUrunleri { get; set; }
        public int S3AlkolluIcecekler { get; set; }
        public int S3Esrar { get; set; }
        public int S3Kokain { get; set; }
        public int S3AmfctaminTipiUyaricilar { get; set; }
        public int S3Inhalanlar { get; set; }
        public int S3Sakinlestiriciler { get; set; }
        public int S3Halüsinojenler { get; set; }
        public int S3Opioidler { get; set; }
        public int S3Diger { get; set; }
        public string S3DigerAciklama { get; set; }
        public int S4TutunUrunleri { get; set; }
        public int S4AlkolluIcecekler { get; set; }
        public int S4Esrar { get; set; }
        public int S4Kokain { get; set; }
        public int S4AmfctaminTipiUyaricilar { get; set; }
        public int S4Inhalanlar { get; set; }
        public int S4Sakinlestiriciler { get; set; }
        public int S4Halüsinojenler { get; set; }
        public int S4Opioidler { get; set; }
        public int S4Diger { get; set; }
        public string S4DigerAciklama { get; set; }
        public int S5TutunUrunleri { get; set; }
        public int S5AlkolluIcecekler { get; set; }
        public int S5Esrar { get; set; }
        public int S5Kokain { get; set; }
        public int S5AmfctaminTipiUyaricilar { get; set; }
        public int S5Inhalanlar { get; set; }
        public int S5Sakinlestiriciler { get; set; }
        public int S5Halüsinojenler { get; set; }
        public int S5Opioidler { get; set; }
        public int S5Diger { get; set; }
        public string S5DigerAciklama { get; set; }
        public int S6TutunUrunleri { get; set; }
        public int S6AlkolluIcecekler { get; set; }
        public int S6Esrar { get; set; }
        public int S6Kokain { get; set; }
        public int S6AmfctaminTipiUyaricilar { get; set; }
        public int S6Inhalanlar { get; set; }
        public int S6Sakinlestiriciler { get; set; }
        public int S6Halüsinojenler { get; set; }
        public int S6Opioidler { get; set; }
        public int S6Diger { get; set; }
        public string S6DigerAciklama { get; set; }
        public string TamAyiklikBaslamaZamani { get; set; }
        public bool? Suboxone { get; set; }
        public string SuboxoneAciklama { get; set; }
        public bool? NaltreksonImplant { get; set; }
        public string NaltreksonImplantAciklama { get; set; }
        public bool? Ethylex { get; set; }
        public string EthylexAciklama { get; set; }
        public bool? Campral { get; set; }
        public string CampralAciklama { get; set; }
        public bool? Antabus { get; set; }
        public string AntabusAciklama { get; set; }
        public string TedaviAciklama { get; set; }
        public int PlanlananRehabilitasyonProgrami { get; set; }
        public int PlanlananProgramSuresi { get; set; }
        public int ProgramGunleri { get; set; }
        public string BelirlenenTerapist { get; set; }
        public string Hedefler { get; set; }
        public int KaydedenKullaniciID { get; set; }
        public DateTime KayitTarihi { get; set; }
        public List<int> FormSonuc { get; set; }
    }
}