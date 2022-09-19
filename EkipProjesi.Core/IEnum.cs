using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace EkipProjesi.Core
{
    public class IEnum
    {
        public static string GetEnumDisplayNameFound(Enum enumValue)
        {
            string display = "";

            try
            {
                display = enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            .GetName();
            }
            catch
            {

            }
            return display;
        }

        public static string GetEnumDisplayDescriptionFound(Enum enumValue)
        {
            string display = "";

            try
            {
                display = enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            .GetDescription();
            }
            catch
            {

            }
            return display;
        }

        public static string GetEnumDisplayNameByIndex(int index, Type enumType)
        {
            string display = "";
            try
            {
                var enumValue = Enum.ToObject(enumType, index);
                display = enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            .GetName();
            }
            catch (Exception ex)
            {

                display = "";
            }
            return display;
        }

        public static int GetEnumDisplayOrderFound(Enum enumValue)
        {
            int order = 0;

            try
            {
                order = enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            .GetOrder().Value;
            }
            catch
            {
                order = 0;
            }
            return order;
        }
    }

    public enum AnketSoruTipi
    {
        [Display(Name = "Seçiniz")]
        Yok = 0,
        [Display(Name = "Radio Buton")]
        Radio = 1,
        [Display(Name = "Check Box")]
        Checkbox = 2,
        [Display(Name = "Input")]
        Input = 3,
        [Display(Name = "Text Area")]
        TextArea = 4
    }
    public enum LogHareketi
    {
        [Display(Name = "Kayıt")]
        Kayit = 0,
        [Display(Name = "Güncelleme")]
        Guncelleme = 1,
        [Display(Name = "Silme")]
        Silme = 2
    }
    public enum LogTipi // Elleme!
    {
        Giris = 1,
        Cikis = 2,
        HataliGiris = 3,
        SifreDegistirme = 4,
        YeniSifre = 5,
        YetkisizGiris = 6,
        AktifKullanici = 7,
        PasifKullanici = 8
    }
    public enum KanGrubu
    {
        [Display(Name = "A Rh+")]
        APozitif = 1,
        [Display(Name = "A Rh-")]
        ANegatif = 2,
        [Display(Name = "B Rh+")]
        BPozitif = 3,
        [Display(Name = "B Rh-")]
        BNegatif = 4,
        [Display(Name = "AB Rh+")]
        ABPozitif = 5,
        [Display(Name = "AB R-")]
        ABNegatif = 6,
        [Display(Name = "0 Rh+")]
        SifirPozitif = 7,
        [Display(Name = "0 Rh-")]
        SifirNegatif = 8,
    }
    public enum OgrenimDurumu
    {
        [Display(Name = "Okuryazar değil")]
        OkuryazarDegil = 1,
        [Display(Name = "İlkokul")]
        IlkOkul = 2,
        [Display(Name = "Ortaokul")]
        Ortaokul = 3,
        [Display(Name = "Lise")]
        Lise = 4,
        [Display(Name = "Önlisans")]
        Onlisans = 5,
        [Display(Name = "Lisans")]
        Lisans = 6,
        [Display(Name = "Lisansüstü")]
        Lisansustu = 7
    }
    public enum LiseTuru
    {
        [Display(Name = "Fen Lisesi")]
        Fen = 1,
        [Display(Name = "Anadolu Lisesi")]
        Anadolu = 2,
        [Display(Name = "Teknik Lise")]
        Teknik = 3,
        [Display(Name = "Genel Lise")]
        Genel = 4
    }
    public enum EgitimSekli
    {
        [Display(Name = "Bilgi Yok")]
        BilgiYok = 0,
        [Display(Name = "Lise")]
        Lise = 1,
        [Display(Name = "Ön Lisans")]
        OnLisans = 2,
        [Display(Name = "Lisans")]
        Lisans = 3,
        [Display(Name = "Yüksek Lisans")]
        YuksekLisans = 4,
        [Display(Name = "Doktora")]
        Doktora = 5,
    }
    public enum ModulYetkiler
    {
        Genel = 0,
        Goruntuleme = 1,
        Ekleme = 2,
        Duzenleme = 3,
        Silme = 4
    }
    public enum Moduller
    {
        [Display(Name = "Personel Yönetim Modülü")]
        Personel = 1,
        [Display(Name = "Faaliyet Yönetim Modülü")]
        Faaliyet = 2,
        [Display(Name = "Müşteri Yönetimi Modülü")]
        Musteri = 3,
        [Display(Name = "Hatırlatma Uyarı Duyuru Öneri Modülü")]
        HUDO = 4,
        //[Display(Name = "Talep Modülü")]
        //Talep = 5,
        [Display(Name = "Anlık İzlem Yönetim Modülü")]
        AnlikIzleme = 6,
        [Display(Name = "Vaka Yönetimi Modülü")]
        Vaka = 7,
        //[Display(Name = "Performans")]
        //Performans = 8,
        [Display(Name = "Eğitim Yönetim Modülü")]
        Egitim = 9,
        [Display(Name = "Tutanak Yönetimi Modülü")]
        Tutanak = 10,
        [Display(Name = "Toplantı Yönetimi Modülü")]
        Toplanti = 11,
        //[Display(Name = "Olay ve Risk")]
        //OlayveRisk = 12,
        [Display(Name = "Sürüm Yönetimi Modülü")]
        SurumYonetimi = 13,
        [Display(Name = "Hakediş Yönetimi Modülü")]
        Hakedis = 14,
        //[Display(Name = "Sistem Yöneticisi")]
        //SistemYoneticisi = 15,
        //[Display(Name = "Entegrasyonlar")]
        //Entegrasyonlar = 16,
        [Display(Name = "Vardiya Yönetimi Modülü")]
        Vardiya = 17,
        [Display(Name = "Anket Modülü")]
        Anket = 18,
        [Display(Name = "Kıyafet Modülü")]
        Kiyafet = 19
    }
    public enum UyariIslemTipi
    {
        [Display(Name = "Personel Oluşturulunca", Order = 1)]
        PersonelOlusturma = 1,

        [Display(Name = "Faaliyet Güncelleme", Order = 2)]
        FaaliyetGuncelleme = 2,

        [Display(Name = "Faaliyet Atama", Order = 2)]
        FaaliyetAtama = 3,

        [Display(Name = "Faaliyet Günlük Rapor", Order = 2)]
        FaaliyetGunlukRapor = 4,

        [Display(Name = "Tutanak Atama", Order = 10)]
        TutanakAtama = 5,

        [Display(Name = "Tutanak Cevaplama", Order = 10)]
        TutanakCevaplama = 6,

        [Display(Name = "Tutanak Onaylama", Order = 10)]
        TutanakOnaylama = 7,

        [Display(Name = "Tutanak Gönderme", Order = 10)]
        TutanakGonderme = 8,

        [Display(Name = "Tutanak Haftalık Değerlendirme", Order = 10)]
        TutanakHaftalikDegerlendirme = 9,

        [Display(Name = "Hakediş Ön Onay", Order = 14)]
        HakedisOnOnay = 10,

        [Display(Name = "Hakediş Ön Onay İptal", Order = 14)]
        HakedisOnOnayIptal = 11,

        [Display(Name = "Anket Yayınlama", Order = 18)]
        AnketYayinlama = 12,

        [Display(Name = "Personel İzin Girişi", Order = 1)]
        PersonelIzinGirisi = 13,

        [Display(Name = "Eğitim Atama", Order = 9)]
        EgitimAtama = 14,

        [Display(Name = "Vardiya Kesinleştirme", Order = 17)]
        VardiyaKesinlestirme = 15,

        [Display(Name = "Öneri Değerlendirildiğinde", Order = 4)]
        OneriDegerlendirildiginde = 16,

        [Display(Name = "Günlük Faaliyet Oluşturma", Order = 2)]
        GunlukFaaliyetOlusturma = 17,

        [Display(Name = "Haftalık Faaliyet Oluşturma", Order = 2)]
        HaftalikFaaliyetOlusturma = 18,

        [Display(Name = "Öneri Maili Oluştuğunda", Order = 4)]
        OneriMailOluştugunda = 19,

        [Display(Name = "Personel İzin Talebi", Order = 1)]
        PersonelIzinTalebi = 20,
    }
    public enum BildirimDegiskenler
    {
        [Display(Name = "Personel Adı", Description = "1")]
        PersonelAd = 1,
        [Display(Name = "Personel Soyadı", Description = "1")]
        PersonelSoyad = 2,
        [Display(Name = "Faaliyet Konusu", Description = "2,10")]
        FaaliyetKonusu = 3,
        [Display(Name = "Açık Tutanak Sayısı", Description = "10")]
        AcikTutanakSayisi = 4,
        [Display(Name = "Tutanak Atanan Kişi", Description = "10")]
        AtananKisi = 5,
        [Display(Name = "Hakediş Firma", Description = "14")]
        HakedisFirma = 6,
        [Display(Name = "Hakediş Adı", Description = "14")]
        HakedisAdi = 7,
        [Display(Name = "Hakediş Dönem", Description = "14")]
        HakedisDonem = 8,
        [Display(Name = "Anket Adı", Description = "18")]
        AnketAdi = 9,
        [Display(Name = "Anket Son Giriş Zamanı", Description = "18")]
        AnketSonGirisZamani = 10,
        [Display(Name = "İzin Başlangıç Zamanı", Description = "1")]
        IzinBaslangic = 11,
        [Display(Name = "İzin Bitis Zamanı", Description = "1")]
        IzinBitis = 12,
        [Display(Name = "İzin Türü", Description = "1")]
        IzinTuru = 13,
        [Display(Name = "Eğitim Başlangıç Zamanı", Description = "9")]
        EgitimBaslangicZamani = 14,
        [Display(Name = "Eğitim Adi", Description = "9")]
        EgitimAdi = 15,
        [Display(Name = "Vardiya Zamanı", Description = "17")]
        VardiyaZamani = 16,
        [Display(Name = "Öneri Konusu", Description = "4")]
        OneriKonusu = 18,
        [Display(Name = "Öneri Açıklama", Description = "4")]
        OneriAciklama = 19,
        [Display(Name = "Öneri Durumu", Description = "4")]
        OneriDurumu = 20,
    }
    public enum Roller
    {
        Hata = 0,
        Admin = 1,
        GenelMudur = 2,
        Direktor = 3,
        Operasyon = 4,
        SahaYoneticisi = 5,
        AltYuklenici = 6,
        MerkezOperasyon = 7,
        VeriGirisElemani = 8,
        MusteriIliskileri = 9,
        OperasyonUzmani = 10,
        SahaTeknikPersonel = 11,
        YardimMasasi = 12,
        Sistem = 13,
    }
    public enum Cinsiyet
    {
        [Display(Name = "Erkek")]
        Erkek = 1,
        [Display(Name = "Kadın")]
        Kadın = 2,
    }
    public enum EgitimPeriyotlari
    {
        [Display(Name = "İki Haftada Bir")]
        IkiHaftadaBir = 1,
        [Display(Name = "Üç Ayda Bir")]
        UcAydaBir = 2,
        [Display(Name = "Altı Ayda Bir")]
        AltiAydaBir = 3,
        [Display(Name = "Yılda Bir")]
        YildaBir = 4,
        [Display(Name = "İşe Giriş Esnasında")]
        IseGirisEsnasinda = 5
    }
    public enum TopluEgitimDosyaTipi
    {
        Form = 1,
        Dosya = 2,
    }
    public enum ToplantiTuru
    {
        Aciklama = 1,
        Karar = 2,
    }
    public enum Kurumlar
    {
        [Display(Name = "Bakırköy")]
        Bakırkoy = 1,
        [Display(Name = "Erenköy")]
        Erenkoy = 2,
    }
    public enum Gunler
    {
        [Display(Name = "Pazartesi")]
        Pazartesi = 1,
        [Display(Name = "Salı")]
        Sali = 2,
        [Display(Name = "Çarşamba")]
        Carsamba = 3,
        [Display(Name = "Persembe")]
        Persembe = 4,
        [Display(Name = "Cuma")]
        Cuma = 5,
    }

    #region Mobil Ekip Vaka Form
    public enum MaddeKullanimSikligi
    {
        [Display(Name = "Gün Aşırı")]
        GunAsiri = 1,
        [Display(Name = "Günde 1")]
        GundeBir = 2,
        [Display(Name = "HaftadaBir")]
        HaftadaBir = 3,
        [Display(Name = "AydaBir")]
        AydaBir = 4,
        [Display(Name = "Yılda Bir")]
        YildaBir = 5,
    }
    public enum BagimlilikEvresi
    {
        [Display(Name = "Farkındalık öncesi")]
        FOncesi = 1,
        [Display(Name = "Farkındalık")]
        Farkindalik = 2,
        [Display(Name = "Karar verme")]
        Karar = 3,
        [Display(Name = "Hazırlık")]
        Hazirlik = 4,
        [Display(Name = "Bırakma")]
        Birakma = 5,
        [Display(Name = "Sürdürme")]
        Surdurme = 6
    }
    public enum TedaviyeYaklasim
    {
        [Display(Name = "İstekli – Tedaviye inanıyor")]
        Istekli1 = 1,
        [Display(Name = "İstekli – Tedaviye inanmıyor")]
        Istekli2 = 2,
        [Display(Name = "Kararsız")]
        Kararsiz = 3,
        [Display(Name = "İsteksiz")]
        Isteksiz = 4
    }
    public enum MotivasyonDuzeyi
    {
        [Display(Name = "Yüksek")]
        Yuksek = 1,
        [Display(Name = "Orta")]
        Orta = 2,
        [Display(Name = "Düşük")]
        Dusuk = 3
    }
    public enum MLiseDuzeyi
    {
        [Display(Name = "Genel Lise")]
        Genel = 1,
        [Display(Name = "Meslek Lisesi")]
        Meslek = 2
    }
    public enum GecimKaynagi
    {
        [Display(Name = "Düzenli Çalışıyor")]
        Duzenli = 1,
        [Display(Name = "Düzensiz Çalışıyor")]
        Duzensiz = 2,
        [Display(Name = "Aile")]
        Aile = 3,
        [Display(Name = "Yakın")]
        Yakin = 4,
        [Display(Name = "Sosyal Yardım vb.")]
        SosyalYardim = 5,
        [Display(Name = "Geçim Kaynağı Yok")]
        Yok = 6,
        [Display(Name = "Diğer")]
        Diger = 7
    }
    public enum IkametDurumu
    {
        [Display(Name = "Kendi evi")]
        Kendi = 1,
        [Display(Name = "Kira")]
        Kira = 2,
        [Display(Name = "Ailesi ile yaşıyor")]
        Aile = 3,
        [Display(Name = "Toplu ortamda yaşıyor")]
        Toplu = 4,
        [Display(Name = "Düzenli ikameti yok")]
        Yok = 5,
        [Display(Name = "Diğer")]
        Diger = 6
    }
    public enum CalismaSuresi
    {
        [Display(Name = "0 - 3")]
        Bir = 1,
        [Display(Name = "3 - 9")]
        Iki = 2,
        [Display(Name = "9 +")]
        Uc = 3
    }
    public enum AdliDurum
    {
        [Display(Name = "Adli geçmişi yok")]
        Yok = 1,
        [Display(Name = "Adli geçmişi var sabıkası yok")]
        SabikaYok = 2,
        [Display(Name = "Sabıkası var")]
        Var = 3,
        [Display(Name = "Şu an denetimli serbestlikte")]
        Denetimli = 4,
        [Display(Name = "Diğer")]
        Diger = 5
    }
    public enum DevamEtmemeNedeni
    {
        [Display(Name = "İkamet değişikliği")]
        Degisiklik = 1,
        [Display(Name = "Ulaşım sorunları")]
        Ulasim = 2,
        [Display(Name = "Erişim sorunları")]
        Erisim = 3,
        [Display(Name = "Ekonomik nedenler")]
        Ekonomik = 4,
        [Display(Name = "İş / Çalışma")]
        Is = 5,
        [Display(Name = "Eğitim")]
        Egitim = 6,
        [Display(Name = "Ailevi nedenler")]
        Ailevi = 7,
        [Display(Name = "Eşlik eden sağlık sorunları")]
        Saglik = 8,
        [Display(Name = "Sosyal yaşam")]
        Sosyal = 9,
        [Display(Name = "Adli durum")]
        Adli = 10,
        [Display(Name = "Barınma sorunları")]
        Barinma = 11,
        [Display(Name = "Tedavi kurumuna ilişkin olumsuz görüş")]
        Kurum = 12,
        [Display(Name = "Tedaviye ilişkin olumsuz görüş")]
        Olumsuz = 13,
        [Display(Name = "Sigorta vb. nedenler")]
        Sigorta = 14,
        [Display(Name = "Diğer")]
        Diger = 15
    }
    public enum MKarar
    {
        [Display(Name = "Danışma birimine yönlendirme")]
        Danisma = 1,
        [Display(Name = "Tedavi kurumuna yönlendirme")]
        Tedavi = 2,
        [Display(Name = "Rehabilitasyona yönlendirme")]
        Rehabilitasyon = 3,
        [Display(Name = "Sosyal hizmetlere yönlendirme")]
        Sosyal = 4,
        [Display(Name = "Tekrar değerlendirme")]
        Tekrar = 5
    }
    #endregion

    #region Iskur Islemleri
    public enum GorusmeSonucu
    {
        [Display(Name = "İşyerine yönlendirildi")]
        Bir = 1,
        [Display(Name = "İşyeri yönlendirme sonucu bilgisi edinildi")]
        Iki = 2,
        [Display(Name = "İş kulübüne yönlendirildi")]
        Uc = 3,
        [Display(Name = "İş kulübü yönlendirme sonucu bilgisi edinildi")]
        Dort = 4,
        [Display(Name = "Kurs ve programlara yönlendirildi")]
        Bes = 5,
        [Display(Name = "Kurs ve programlara yönlendirme sonucu bilgisi edinildi")]
        Alti = 6,
        [Display(Name = "Danışma ve/veya tedavi merkezinde yeniden değerlendirmek üzere yönlendirildi")]
        Yedi = 7,
        [Display(Name = "Diğer")]
        Sekiz = 8
    }
    public enum IDurum
    {
        [Display(Name = "Olumlu")]
        Olumlu = 1,
        [Display(Name = "Olumsuz")]
        Olumsuz = 2
    }
    public enum IsKulubu
    {
        [Display(Name = "Süreç programlandı")]
        Programlandi = 1,
        [Display(Name = "Sürece başlandı")]
        Baslandi = 2,
        [Display(Name = "Süreç devam ediyor")]
        Devam = 3,
        [Display(Name = "Süreci tamamlandı")]
        Tamamlandi = 4
    }
    public enum IYonlendirmeDurum
    {
        [Display(Name = "Başarılı")]
        Basarili = 1,
        [Display(Name = "Başarısız")]
        Basarisiz = 2
    }
    public enum Tur
    {
        [Display(Name = "TYP")]
        TYP = 1,
        [Display(Name = "İşbaşı eğitim programı")]
        Isbasi = 2,
        [Display(Name = "Mesleki eğitim")]
        Mesleki = 3
    }
    #endregion

    #region Rehabilitasyon Bahar Formu
    public enum BaharCalismaDurumu
    {
        [Display(Name = "Hayır")]
        Hayir = 1,
        [Display(Name = "Düzensiz olarak çalışıyor")]
        Duzensiz = 2,
        [Display(Name = "Düzenli olarak çalışıyor")]
        Duzenli = 3,
        [Display(Name = "Emekli")]
        Emekli = 4
    }
    public enum BaharEkonomikDurum
    {
        [Display(Name = "İyi")]
        Iyi = 1,
        [Display(Name = "Orta")]
        Orta = 2,
        [Display(Name = "Kötü")]
        Kotu = 3
    }
    public enum GorusmeMerkezi
    {
        [Display(Name = "Erenköy BAHAR")]
        Erenkoy = 1,
        [Display(Name = "Üsküdar BAHAR")]
        Uskudar = 2
    }
    public enum GelisSekli
    {
        [Display(Name = "Poliklinik")]
        Poliklinik = 1,
        [Display(Name = "AMATEM Servisi")]
        Amatem = 2,
        [Display(Name = "Kurum Dışı")]
        KurumDisi = 3
    }
    public enum TedaviBasvuruNedeni
    {
        [Display(Name = "Kendi isteği")]
        Kendi = 1,
        [Display(Name = "Ailesinin isteği")]
        Aile = 2,
        [Display(Name = "Her ikisi")]
        Her = 3,
        [Display(Name = "Diğer (adli, psikolojik vb.)")]
        Diger = 4
    }
    public enum EvetHayir
    {
        [Display(Name = "Hayır")]
        Hayir = 0,
        [Display(Name = "Evet")]
        Evet = 1
    }
    public enum SoruCevap
    {
        [Display(Name = "Hiç")]
        Hic = 1,
        [Display(Name = "Bir ya da İki kez")]
        Bir = 2,
        [Display(Name = "Aylık")]
        Aylik = 3,
        [Display(Name = "Haftalık")]
        Haftalik = 4,
        [Display(Name = "Günlük veya neredeyse her gün")]
        Gunluk = 5
    }
    public enum TamYari
    {
        [Display(Name = "Tam Zamanlı")]
        Tam = 1,
        [Display(Name = "Yarı Zamanlı")]
        Yari = 2
    }
    public enum ProgramSuresi
    {
        [Display(Name = "3 ay")]
        Bir = 1,
        [Display(Name = "6 ay")]
        Iki = 2,
        [Display(Name = "9 ay ve üzeri")]
        Uc = 3
    }
    #endregion
    #region Olcekler Formu
    public enum VarYok
    {
        [Display(Name = "Var")]
        Var = 1,
        [Display(Name = "Yok")]
        Yok = 2
    }
    public enum Uyku
    {
        [Display(Name = "Hiç")]
        Hic = 1,
        [Display(Name = "Birden az")]
        Bir = 2,
        [Display(Name = "1-2 kez")]
        BirIki = 3,
        [Display(Name = "3'ten çok")]
        Uc = 4
    }
    public enum KadinErkek1
    {
        [Display(Name = "Oldukça İstekli")]
        Bir = 1,
        [Display(Name = "Çok İstekli")]
        Iki = 2,
        [Display(Name = "Biraz İstekli")]
        Uc = 3,
        [Display(Name = "Biraz İsteksiz")]
        Dort = 4,
        [Display(Name = "Çok İsteksiz")]
        Bes = 5,
        [Display(Name = "Tamamen İsteksiz")]
        Alti = 6,
    }
    public enum KadinErkek2
    {
        [Display(Name = "Oldukça Kolay")]
        Bir = 1,
        [Display(Name = "Çok Kolay")]
        Iki = 2,
        [Display(Name = "Biraz Kolay")]
        Uc = 3,
        [Display(Name = "Biraz Zor")]
        Dort = 4,
        [Display(Name = "Çok Zor")]
        Bes = 5,
        [Display(Name = "Oldukça Zor")]
        Alti = 6,
    }
    public enum KadinErkek3
    {
        [Display(Name = "Oldukça Tatmin Edici")]
        Bir = 1,
        [Display(Name = "Çok Tatmin Edici")]
        Iki = 2,
        [Display(Name = "Biraz Tatmin Edici")]
        Uc = 3,
        [Display(Name = "Pek Tatmin Etmiyor")]
        Dort = 4,
        [Display(Name = "Çok Tatmin Etmiyor")]
        Bes = 5,
        [Display(Name = "Doyuma Ulaşamam")]
        Alti = 6,
    }
    public enum Appendix
    {
        [Display(Name = "Hiç")]
        Bir = 1,
        [Display(Name = "Çok Az")]
        Iki = 2,
        [Display(Name = "Orta Derecede")]
        Uc = 3,
        [Display(Name = "Oldukça Fazla")]
        Dort = 4,
        [Display(Name = "Aşırı")]
        Bes = 5
    }
    #endregion
    #region BSİ
    public enum HastaDegerlendirmeOlcegi
    {
        [Display(Name = "Hiç Değil")]
        Sifir = 0,
        [Display(Name = "Biraz")]
        Bir = 1,
        [Display(Name = "Orta derecede")]
        Iki = 2,
        [Display(Name = "Önemli ölçüde")]
        Uc = 3,
        [Display(Name = "Son derece")]
        Dort = 4
    }
    public enum G2cTedaviKodlari
    {
        [Display(Name = "Ayaktan tedavi (haftada<5 saat)")]
        Bir = 1,
        [Display(Name = "Yoğun ayaktan tedavi (haftada ≥ 5 saat)")]
        Iki = 2,
        [Display(Name = "Yatılı/Serviste yatırılarak")]
        Uc = 3,
        [Display(Name = "Grup terapisi")]
        Dort = 4,
        [Display(Name = "Rehabilitasyon merkezi")]
        Bes = 5,
        [Display(Name = "Detoks – Serviste yatırılarak (tipik olarak 3-7 gün)")]
        Alti = 6,
        [Display(Name = "Detoks Ayaktan izlem/Ambulatuvar")]
        Yedi = 7,
        [Display(Name = "Opioid replasmanı, ayaktan (Metadon, Buprenorfin, vb.)")]
        Sekiz = 8,
        [Display(Name = "Diğer (düşük eşikli, aile hekimi , ruhani destek, vb.)")]
        Dokuz = 9
    }
    public enum BSISinif
    {
        [Display(Name = "İlk Başvuru")]
        Ilk = 1,
        [Display(Name = "Takip")]
        Takip = 2,
    }
    public enum BSIIletisimSekli
    {
        [Display(Name = "Yüz yüze")]
        Yuz = 1,
        [Display(Name = "Telefon")]
        Telefon = 2,
    }
    public enum G19
    {
        [Display(Name = "Hayır")]
        Bir = 1,
        [Display(Name = "Cezaevi")]
        Iki = 2,
        [Display(Name = "Alkol/Madde tedavisi")]
        Uc = 3,
        [Display(Name = "Tıbbi tedavi")]
        Dort = 4,
        [Display(Name = "Psikiyatrik tedavi")]
        Bes = 5,
        [Display(Name = "Diğer")]
        Alti = 6
    }
    public enum TestSonuc
    {
        [Display(Name = "Negatif (enfekte değil)")]
        Bir = 1,
        [Display(Name = "Pozitif (enfekte)")]
        Iki = 2,
        [Display(Name = "Bilmiyor")]
        Uc = 3,
        [Display(Name = "Hayır")]
        Dort = 4
    }
    public enum EHE
    {
        [Display(Name = "Evet")]
        Evet = 1,
        [Display(Name = "Hayır")]
        Hayir = 2,
        [Display(Name = "Emin Değil")]
        EminDegil = 3
    }
    public enum BSIEgitimDuzeyi
    {        
        [Display(Name = "Eğitim yok")]
        Bir = 1,
        [Display(Name = "İlkokul 1-6 yıl")]
        Iki = 2,
        [Display(Name = "Ortaokul 7-9 yıl")]
        Uc = 3,
        [Display(Name = "Lise 10-12 yıl")]
        Dort = 4,
        [Display(Name = "Lise sonrası, üniversite değil")]
        Bes = 5,
        [Display(Name = "İlk basamak üniversite")]
        Alti = 6,
        [Display(Name = "İleri basamak üniversite (doktora, vb.)")]
        Yedi = 7
    }
    public enum E7
    {
        [Display(Name = "1.	Yasa koyucular, üst düzey yetkililer – Ana görevleri hükümet politikalarını, yasaları, düzenlemeleri oluşturmak ve uygulamayı denetlemek.")]
        Bir = 1,
        [Display(Name = "2.	Profesyoneller – Fiziki bilimler / yaşam bilimleri veya sosyal bilimler / beşeri bilimler alanlarında yüksek düzeyde profesyonel bilgi gerektirir.")]
        Iki = 2,
        [Display(Name = "3. Teknisyenler / Yardımcı profesyoneller – Fiziki bilimler / yaşam bilimleri veya sosyal bilimler / beşeri bilimler alanlarında teknik bilgi, deneyim gerektirir.")]
        Uc = 3,
        [Display(Name = "4.	Memurlar - Sekreterlik görevleri, yazı işleri ve diğermüşteri odaklı büro görevlerini yerine getirirler.")]
        Dort = 4,
        [Display(Name = "5.	Hizmet ve Satış - Seyahat, yiyecek-içecek hizmeti, mağaza satışları, bakım ve temizlik, yasalara uyum ve düzeni sürdürme ile ilgili hizmetleri içerir.")]
        Bes = 5,
        [Display(Name = "6.	Nitelikli tarım ve balıkçılık işçileri - Mahsul yetiştirme,hayvan yetiştirme veya avlama, balık yakalama veya yetiştirme vb.")]
        Alti = 6,
        [Display(Name = "7.	Zanaat ve Ticaret - Ana görevleri bina ve diğer yapıları inşa etmek, çeşitli ürünler üretmektir. El sanatlarını da içerir.")]
        Yedi = 7,
        [Display(Name = "8.	Tesis ve makine operatörleri - Ana görevleri araç sürmek, makine kullanmak veya ürünlerin montajıdır.")]
        Sekiz = 8,
        [Display(Name = "9.	Basit Meslekler – Sokaklarda mal satmak, kapıcılık, temizlik ve emekçilik gibi basit ve rutin görevleri içerir.")]
        Dokuz = 9,
        [Display(Name = "10. Ordu, donanma, hava kuvvetleri çalışanları vb hariçtir.")]
        On = 10
    }
    public enum E10
    {
        [Display(Name = "Tam zamanlı (40+ saat)")]
        Bir = 1,
        [Display(Name = "Yarı zamanlı (düzenli saatler)")]
        Iki = 2,
        [Display(Name = "Yarı zamanlı (düzensiz saatler)")]
        Uc = 3,
        [Display(Name = "Öğrenci")]
        Dort = 4,
        [Display(Name = "Asker")]
        Bes = 5,
        [Display(Name = "Emekli/Engelli")]
        Alti = 6,
        [Display(Name = "İşsiz")]
        Yedi = 7,
        [Display(Name = "Denetimli çevre (denetimli serbestlik, vb)")]
        Sekiz = 8,
        [Display(Name = "Ev kadını")]
        Dokuz = 9
    }
    public enum MaddeKullanmaYolu
    {
        [Display(Name = "Oral (Ağızdan herhangi bir şey)")]
        Bir = 1,
        [Display(Name = "Nazal (veya başka herhangi bir mukoza zarı uygulaması)")]
        Iki = 2,
        [Display(Name = "Sigara şeklinde")]
        Uc = 3,
        [Display(Name = "Enjeksiyon – İV dışı (İM veya “deriden alma” gibi)")]
        Dort = 4,
        [Display(Name = "İV (doğrudan damara vererek)")]
        Bes = 5
    }
    public enum SonYasam
    {
        [Display(Name = "Son 30 Gün")]
        Bir = 1,
        [Display(Name = "Yaşam Boyu")]
        Iki = 2
    }
    public enum MedeniHal
    {
        [Display(Name = "Evli")]
        Bir = 1,
        [Display(Name = "Yeniden evlenmiş")]
        Iki = 2,
        [Display(Name = "Dul")]
        Uc = 3,
        [Display(Name = "Ayrılmış")]
        Dort = 4,
        [Display(Name = "Boşanmış")]
        Bes = 5,
        [Display(Name = "Hiç evlenmemiş")]
        Alti = 6
    }
    public enum Memnun
    {
        [Display(Name = "Evet")]
        Bir = 1,
        [Display(Name = "Kayıtsız")]
        Iki = 2,
        [Display(Name = "Hayır")]
        Uc = 3
    }
    public enum YasamDuzenlemeleri
    {
        [Display(Name = "Eş ve çocuklarla")]
        Bir = 1,
        [Display(Name = "Sadece eşle")]
        Iki = 2,
        [Display(Name = "Sadece çocuklarla")]
        Uc = 3,
        [Display(Name = "Ebeveynlerle")]
        Dort = 4,
        [Display(Name = "Aileyle")]
        Bes = 5,
        [Display(Name = "Arkadaşlarla")]
        Alti = 6,
        [Display(Name = "Yalnız")]
        Yedi = 7,
        [Display(Name = "Denetimli çevre")]
        Sekiz = 8,
        [Display(Name = "Sabit bir düzenleme yok")]
        Dokuz = 9
    }
    public enum BosZaman
    {
        [Display(Name = "Aile")]
        Bir = 1,
        [Display(Name = "Arkadaş")]
        Iki = 2,
        [Display(Name = "Yalnız")]
        Uc = 3
    }
    public enum G12
    {
        [Display(Name = "Görüşme görüşmeci tarafından sonlandırıldı.")]
        Bir = 1,
        [Display(Name = "Hasta görüşmeyi tamamlamayı reddetti.")]
        Iki = 2,
        [Display(Name = "Hasta yanıt veremiyor (dil veya entellektüel engel, etki altında bulunma, vb.)")]
        Uc = 3,
        [Display(Name = "Görüşme tamamlandı.")]
        Dort = 4
    }
    public enum G50
    {
        [Display(Name = "Ayaktan tedavi (haftada<5 saat)")]
        Bir = 1,
        [Display(Name = "Yoğun ayaktan tedavi (haftada ≥ 5 saat)")]
        Iki = 2,
        [Display(Name = "Yatılı/Serviste yatırılarak")]
        Uc = 3,
        [Display(Name = "Grup terapisi")]
        Dort = 4,
        [Display(Name = "Rehabilitasyon merkezi")]
        Bes = 5,
        [Display(Name = "Detoks – Serviste yatırılarak (tipik olarak 3-7 gün)")]
        Alti = 6,
        [Display(Name = "Detoks Ayaktan izlem/Ambulatuvar")]
        Yedi = 7,
        [Display(Name = "Opioid replasmanı, ayaktan (Metadon, Buprenorfin, vb.)")]
        Sekiz = 8,
        [Display(Name = "Diğer (düşük eşikli, aile hekimi , ruhani destek, vb.)")]
        Dokuz = 9
    }
    #endregion
}