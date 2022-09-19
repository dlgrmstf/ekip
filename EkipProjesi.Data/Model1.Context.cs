﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EkipProjesi.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class EKIPEntities : DbContext
    {
        public EKIPEntities()
            : base("name=EKIPEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AnketCevaplari> AnketCevaplari { get; set; }
        public virtual DbSet<AnketGruplari> AnketGruplari { get; set; }
        public virtual DbSet<AnketKullaniciCevap> AnketKullaniciCevap { get; set; }
        public virtual DbSet<AnketKullanicilari> AnketKullanicilari { get; set; }
        public virtual DbSet<Anketler> Anketler { get; set; }
        public virtual DbSet<AnketSoruBankasi> AnketSoruBankasi { get; set; }
        public virtual DbSet<AnketSorulari> AnketSorulari { get; set; }
        public virtual DbSet<AnketTipleri> AnketTipleri { get; set; }
        public virtual DbSet<ArindirmaBasvuruBilgileri> ArindirmaBasvuruBilgileri { get; set; }
        public virtual DbSet<ArindirmaBasvuruKararlari> ArindirmaBasvuruKararlari { get; set; }
        public virtual DbSet<ArindirmaHizmetBilgileri> ArindirmaHizmetBilgileri { get; set; }
        public virtual DbSet<ArindirmaPersonelBilgileri> ArindirmaPersonelBilgileri { get; set; }
        public virtual DbSet<BasvuruBilgileriLabSonuc> BasvuruBilgileriLabSonuc { get; set; }
        public virtual DbSet<FizikselHastalikOykuleri> FizikselHastalikOykuleri { get; set; }
        public virtual DbSet<Hastalar> Hastalar { get; set; }
        public virtual DbSet<Il> Il { get; set; }
        public virtual DbSet<Ilce> Ilce { get; set; }
        public virtual DbSet<Kullanicilar> Kullanicilar { get; set; }
        public virtual DbSet<KullaniciRolleri> KullaniciRolleri { get; set; }
        public virtual DbSet<Kurumlar> Kurumlar { get; set; }
        public virtual DbSet<Log> Log { get; set; }
        public virtual DbSet<LogCikis> LogCikis { get; set; }
        public virtual DbSet<LogHataBildirimleri> LogHataBildirimleri { get; set; }
        public virtual DbSet<LogLoginIslemleri> LogLoginIslemleri { get; set; }
        public virtual DbSet<LogSchema> LogSchema { get; set; }
        public virtual DbSet<LogSifreDegistirme> LogSifreDegistirme { get; set; }
        public virtual DbSet<PersonelFotograflar> PersonelFotograflar { get; set; }
        public virtual DbSet<PersonelGorevHareketleri> PersonelGorevHareketleri { get; set; }
        public virtual DbSet<Personeller> Personeller { get; set; }
        public virtual DbSet<PoliklinikTurleri> PoliklinikTurleri { get; set; }
        public virtual DbSet<Roller> Roller { get; set; }
        public virtual DbSet<RolYetkileri> RolYetkileri { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Unvanlar> Unvanlar { get; set; }
        public virtual DbSet<Yatislar> Yatislar { get; set; }
        public virtual DbSet<YatisSonlanmaNedenleri> YatisSonlanmaNedenleri { get; set; }
        public virtual DbSet<Yetkiler> Yetkiler { get; set; }
        public virtual DbSet<HastaMaddeKullanimBilgileri> HastaMaddeKullanimBilgileri { get; set; }
        public virtual DbSet<Kullanici> Kullanici { get; set; }
        public virtual DbSet<ResmiTatiller> ResmiTatiller { get; set; }
        public virtual DbSet<Events> Events { get; set; }
        public virtual DbSet<RandevuBilgileri> RandevuBilgileri { get; set; }
        public virtual DbSet<KurumPoliklinikTurleri> KurumPoliklinikTurleri { get; set; }
        public virtual DbSet<Mesajlar> Mesajlar { get; set; }
        public virtual DbSet<MesajGidenKullanici> MesajGidenKullanici { get; set; }
        public virtual DbSet<KullaniciIletisimBilgileri> KullaniciIletisimBilgileri { get; set; }
        public virtual DbSet<KullaniciIzinTuru> KullaniciIzinTuru { get; set; }
        public virtual DbSet<KullaniciBirimBilgileri> KullaniciBirimBilgileri { get; set; }
        public virtual DbSet<KullaniciIzinBilgileri> KullaniciIzinBilgileri { get; set; }
        public virtual DbSet<LogSilinenRandevular> LogSilinenRandevular { get; set; }
        public virtual DbSet<KullaniciErisimBilgileri> KullaniciErisimBilgileri { get; set; }
        public virtual DbSet<ErisimKodlari> ErisimKodlari { get; set; }
        public virtual DbSet<HastaYakinBilgileri> HastaYakinBilgileri { get; set; }
        public virtual DbSet<HastaNotlari> HastaNotlari { get; set; }
        public virtual DbSet<Faaliyetler> Faaliyetler { get; set; }
        public virtual DbSet<HastaIlkKayitBilgileri> HastaIlkKayitBilgileri { get; set; }
        public virtual DbSet<HastaIzlemBilgileri> HastaIzlemBilgileri { get; set; }
        public virtual DbSet<HastaTimeline> HastaTimeline { get; set; }
        public virtual DbSet<IstihdamIsyerleri> IstihdamIsyerleri { get; set; }
        public virtual DbSet<IstihdamIsyeriIletisimKisileri> IstihdamIsyeriIletisimKisileri { get; set; }
        public virtual DbSet<IstihdamIsyeriGorusmeleri> IstihdamIsyeriGorusmeleri { get; set; }
        public virtual DbSet<KullaniciGorevTakip> KullaniciGorevTakip { get; set; }
        public virtual DbSet<KurumHizmetMerkezleri> KurumHizmetMerkezleri { get; set; }
        public virtual DbSet<RandevuTalepleri> RandevuTalepleri { get; set; }
        public virtual DbSet<RandevuAyarlari> RandevuAyarlari { get; set; }
        public virtual DbSet<IstihdamIsyeriAdresBilgisi> IstihdamIsyeriAdresBilgisi { get; set; }
        public virtual DbSet<MobilEkipVakaFormlari> MobilEkipVakaFormlari { get; set; }
        public virtual DbSet<AcilServisVakaBilgileri> AcilServisVakaBilgileri { get; set; }
        public virtual DbSet<TaniKodlari> TaniKodlari { get; set; }
        public virtual DbSet<YEDAMHastaBilgileri> YEDAMHastaBilgileri { get; set; }
        public virtual DbSet<VakaBilgileri112> VakaBilgileri112 { get; set; }
        public virtual DbSet<HastaBeyanBilgileri> HastaBeyanBilgileri { get; set; }
        public virtual DbSet<HastaEgitimBilgileri> HastaEgitimBilgileri { get; set; }
        public virtual DbSet<HastaIskurGorusmeleri> HastaIskurGorusmeleri { get; set; }
        public virtual DbSet<HastaBaharFormlari> HastaBaharFormlari { get; set; }
        public virtual DbSet<HastaBaharFormuSonuc> HastaBaharFormuSonuc { get; set; }
        public virtual DbSet<HastaBaharFormuSonucKodlari> HastaBaharFormuSonucKodlari { get; set; }
        public virtual DbSet<HastaOlceklerFormu> HastaOlceklerFormu { get; set; }
        public virtual DbSet<AcilServisVakaTanilari> AcilServisVakaTanilari { get; set; }
        public virtual DbSet<VakaTanilari112> VakaTanilari112 { get; set; }
        public virtual DbSet<Mahalle> Mahalle { get; set; }
        public virtual DbSet<IstihdamIsyeriSektorler> IstihdamIsyeriSektorler { get; set; }
        public virtual DbSet<ExcelYuklemeleri> ExcelYuklemeleri { get; set; }
        public virtual DbSet<KurumRandevulari> KurumRandevulari { get; set; }
        public virtual DbSet<HastaBagimlilikSiddetiFormlari> HastaBagimlilikSiddetiFormlari { get; set; }
    }
}