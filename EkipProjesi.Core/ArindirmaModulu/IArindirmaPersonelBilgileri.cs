using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkipProjesi.Core.ArindirmaModulu
{
    public class IArindirmaPersonelBilgileri
    {
        public int ID { get; set; }
        public int KurumKodu { get; set; }
        public DateTime KayitTarihi { get; set; }
        public int TumUzmanHekimSayisi { get; set; }
        public int TumAsistanHekimSayisi { get; set; }        
        public int TumPratisyenHekimSayisi { get; set; }
        public int HastaneAktifPsikiyatriUzmaniSayisi { get; set; }
        public int HastaneAktifCocukPsikiyatriUzmaniSayisi { get; set; }
        public int HastaneAktifPsikiyatriAsistaniSayisi { get; set; }
        public int HastaneAktifCocukPsikiyatriAsistaniSayisi { get; set; }
        public int HastaneAktifPsikologSayisi { get; set; }
        public int HastaneAktifHemsireSayisi { get; set; }
        public int AmatemAktifPsikiyatriUzmaniSayisi { get; set; }
        public int CematemAktifCocukPsikiyatriUzmaniSayisi { get; set; }
        public int AmatemAktifPsikiyatriAsistaniSayisi { get; set; }
        public int CematemAktifCocukPsikiyatriAsistaniSayisi { get; set; }
        public int AmatemAktifPsikologSayisi { get; set; }
        public int CematemAktifPsikologSayisi { get; set; }
        public int AmatemAktifHemsireSayisi { get; set; }
        public int CematemAktifHemsireSayisi { get; set; }
        public int MaddeBagimliligiEgitimiAlmisTumHekimSayisi { get; set; }
        public int MaddeBagimliligiEgitimiAlmisPsikiyatriUzmaniSayisi { get; set; }
        public int MaddeBagimliligiEgitimiAlmisCocukPsikiyatriUzmaniSayisi { get; set; }
        public int MaddeBagimliligiEgitimiAlmisPsikologSayisi { get; set; }
        public int MaddeBagimliligiEgitimiAlmisHemsireSayisi { get; set; }
    }
}