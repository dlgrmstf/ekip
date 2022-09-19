using EkipProjesi.Core.Hastalar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkipProjesi.Core.ArindirmaModulu
{
    public class IArindirmaBasvuruBilgileri
    {
        public int ID { get; set; }
        public string HastaEkipNo { get; set; }
        public int KurumKodu { get; set; }
        public DateTime KayitTarihi { get; set; }
        public string HastaProtokolNo { get; set; }
        public string BeyanAdresi { get; set; }
        public string BeyanTelefonu { get; set; }
        public string HastaAdi { get; set; }
        public string HastaSoyadi { get; set; }
        public string HastaTCKimlikNo { get; set; }
        public DateTime PoliklinikMuayeneTarihSaati { get; set; }
        public int PoliklinikTuruID { get; set; }
        public string MuayeneyiGerceklestirenHekim { get; set; }
        public string MuayeneyiGerceklestirenHekimTuru { get; set; }
        public string MaddeBilgisi { get; set; }
        public string EslikEdenHastalikOykusu { get; set; }
        public bool PsikiyatrikHastalikOykusu { get; set; }
        public string PsikiyatrikHastalikOykusuAciklama { get; set; }
        public string KullanmaktaOlduguDigerIlacBilgisi { get; set; }
        public string SonBasvurudanSonraAlkolMaddeKullanimiOlmusMu { get; set; }
        public string SonBasvurudanSonraOnerilenIlaclariDuzenliKullanmisMi { get; set; }
        public string MuayeneBulgulari { get; set; }
        public string TedaviUyuncuDegerlendirmesi { get; set; }
        public bool YoksunlukBulgusu { get; set; }
        public bool IntoksikasyonBulgusu { get; set; }
        public bool IdrarToksikolojiBulgusu { get; set; }
        public int? KararID { get; set; }
        public string PoliklinikTuru { get; set; }
        public string Karar { get; set; }
        public List<IHastalar> Hastalar { get; set; }
        public IYatislar Yatislar { get; set; }
        public List<IHastaMaddeKullanimBilgileri> MaddeBilgileri { get; set; }
    }
}