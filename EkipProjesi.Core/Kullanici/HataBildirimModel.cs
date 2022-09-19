using System;
using System.ComponentModel.DataAnnotations;

namespace EkipProjesi.Core.Kullanici
{
    public class HataBildirimModel
    {
        [Required]
        public string Konu { get; set; }
        [Required]
        public string Mesaj { get; set; }
        public int UserID { get; set; }
        public DateTime Tarih { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string HTML { get; set; }
        public string FileFormat { get; set; }
        public string FileExt { get; set; }
        public byte[] File { get; set; }
        public bool? CozumDurumu { get; set; }
        public string CozumMesaji { get; set; }
        public string KullaniciAdi { get; set; }
    }
}