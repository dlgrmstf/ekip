using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkipProjesi.Core.MobilEkip
{
    public class IMobilEkipVakaFormlari
    {
        public int ID { get; set; }
        public int HastaID { get; set; }
        public string Adres { get; set; }
        public string Nitelik { get; set; }
        public bool? BilgiKaynagiKendisi { get; set; }
        public bool? BilgiKaynagiAnne { get; set; }
        public bool? BilgiKaynagiBaba { get; set; }
        public bool? BilgiKaynagiEs { get; set; }
        public bool? BilgiKaynagiCocuk { get; set; }
        public bool? BilgiKaynagiKardes { get; set; }
        public bool? BilgiKaynagiArkadas { get; set; }
        public bool? BilgiKaynagiAkraba { get; set; }
        public bool? BilgiKaynagiDiger { get; set; }
        public string BilgiKaynagiAkrabaAciklama { get; set; }
        public string BilgiKaynagiDigerAciklama { get; set; }
        public string GenelTanitim { get; set; }
        public string MaddeyeBaslamaYasi { get; set; }
        public string IlkKullandigiMadde { get; set; }
        public string SuAndaKullandigiMaddeler { get; set; }
        public int MaddeKullanimSikligi { get; set; }
        public string MaddeyeBaslamaNedeni { get; set; }
        public string EnSonMaddeKullanimZamani { get; set; }
        public string KurumBilgisiVarMi { get; set; }
        public bool? MotivasyonKaynagiAile { get; set; }
        public bool? MotivasyonKaynagiEkonomi { get; set; }
        public bool? MotivasyonKaynagiSaglik { get; set; }
        public bool? MotivasyonKaynagiDiger { get; set; }
        public string MotivasyonKaynagiDigerAciklama { get; set; }
        public int BagimlilikEvresi { get; set; }
        public int TedaviyeYaklasimi { get; set; }
        public int MotivasyonDuzeyi { get; set; }
        public string MaddeDigerTespitler { get; set; }
        public int OgrenimDurumu { get; set; }
        public int LiseTuru { get; set; }
        public string Fakulte { get; set; }
        public string Bolum { get; set; }
        public string EgitimDigerTespitler { get; set; }
        public int GecimKaynagi { get; set; }
        public string YaptigiIs { get; set; }
        public string CalistigiYer { get; set; }
        public string CalismaSuresi { get; set; }
        public int Maas { get; set; }
        public string CalismaSahasi { get; set; }
        public int AylikOrtCalistigiGunSayisi { get; set; }
        public int AylikOrtGelir { get; set; }
        public string CalismaBilgisiDiger { get; set; }
        public string AylikOrtGelirDuzeyi { get; set; }
        public string AileAylikOrtGelirDuzeyi { get; set; }
        public int IkametDurumu { get; set; }
        public string TopluIkametDetay { get; set; }
        public string IkametDiger { get; set; }
        public int EnUzunCalismaSuresi { get; set; }
        public string EkonomikDurumDiger { get; set; }
        public string EslikEdenBedenselHastaliklar { get; set; }
        public string EslikEdenRuhsalHastaliklar { get; set; }
        public string PlanliTedaviPlani { get; set; }
        public string DigerBagimlilikTuru { get; set; }
        public string SaglikDigerTespitler { get; set; }
        public int AdliDurumu { get; set; }
        public string AdliDurumDiger { get; set; }
        public string AileSosyalYasantisi { get; set; }
        public string DegerlendirmeVeSonuc { get; set; }
        public int DevamEtmemeNedeni { get; set; }
        public string DevamEtmemeNedeniAciklama { get; set; }
        public int Karar { get; set; }
        public DateTime KayitTarihi { get; set; }
        public int KaydedenKullaniciID { get; set; }
        
    }
}