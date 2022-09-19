using System.ComponentModel.DataAnnotations;

namespace EkipProjesi.Core.Personel
{
    public class IUnvan
    {
        public int ID { get; set; }
        [Required]
        public string UnvanAdi { get; set; }
        [Required]
        public int IsletmeId { get; set; }
        public bool Status { get; set; }
        public int PersonelSayisi { get; set; }
    }
}