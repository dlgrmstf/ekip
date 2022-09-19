using EkipProjesi.Core.LLog;
using EkipProjesi.Core.Personel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EkipProjesi.Core.Kullanici
{
    public class IKullanici
    {
        public int ID { get; set; }
        public int KurumID { get; set; }
        public int HizmetMerkeziID { get; set; }
        public string KullaniciAdi { get; set; }
        public string Sifre { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public string KurumTelefonu { get; set; }
        public string KurumEposta { get; set; }
        public string Bolge { get; set; }
        public string SifreYenilemeSMSOnayKodu { get; set; }
        public int? TipID { get; set; }

        [DataType(DataType.Password)]
        public string EskiSifre { get; set; }
        [DataType(DataType.Password)]
        public string SifreTekrar { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string DogumTarihi { get; set; }
        public string Cinsiyet { get; set; }
        public int? KanGrubu { get; set; }
        public string MedeniDurum { get; set; }
        public string BolgeAdi { get; set; }
        public string Meslek { get; set; }
        public string KurumAdi { get; set; }
        public string HizmetMerkeziAdi { get; set; }
        public string ExcelID { get; set; }
        public DateTime IseBaslamaTarihi { get; set; }
        public Nullable<int> PersonelId { get; set; }
        public Nullable<int> RolID { get; set; }
        public Nullable<int> FaaliyetId { get; set; }
        public string PersonelSlug { get; set; }
        public bool? SifreKontrol { get; set; }
        public IKullaniciBirimBilgileri BirimBilgisi { get; set; }
        public IKullaniciIletisimBilgileri IletisimBilgisi { get; set; }

        [StringLength(11)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "T.C. Kimlik Numarası Sayılardan Oluşmalıdır.")]
        [Description("T.C. Kimlik Numarası.")]
        public string TC { get; set; }
        public Nullable<bool> KullaniciDurumu { get; set; }
        public List<IKullanici> Kullanicilar { get; set; }
        public IEnumerable<int> RolIDs { get; set; }
        public List<IRol> RolList { get; set; }
        public List<IKullaniciRol> Roller { get; set; }
        public List<int> Roller2 { get; set; }
        public List<IRolYetkileri> RolModulYetkileri { get; set; }
        public List<List<IRolYetkileri>> Yetkiler { get; set; } //Nested List
        public ILogLogin Log { get; set; }
        public byte[] FotoByte { get; set; }
        public string FotoContentType { get; set; }
        public IPersonelFoto PersonelFoto { get; set; }
        public LogSifreDegistirmeModel LogSifreDegistirme { get; set; }
        public byte[] PersonelFotoArray { get; set; }
        public string ContentType { get; set; }

        public string access_token { get; set; }
        public string token_type { get; set; }
        [JsonProperty(".expires")]
        public string expires { get; set; }
        [JsonProperty(".issued")]
        public string issued { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string grant_type = "password";
        public string error { get; set; }
        public string error_description { get; set; }
        public int KaydedenKullaniciID { get; set; }
        public DateTime KayitTarihi { get; set; }
        public IKullanici()
        {
            RolModulYetkileri = new List<IRolYetkileri>();
            Roller = new List<IKullaniciRol>();
            //YetkiliWidgetlar = new List<WidgetDBO>();
        }
    }
}