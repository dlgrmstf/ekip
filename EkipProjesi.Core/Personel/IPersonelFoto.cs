using System.ComponentModel.DataAnnotations;

namespace EkipProjesi.Core.Personel
{
    public class IPersonelFoto
    {
        public int ID { get; set; }
        [Required]
        public int PersonelID { get; set; }
        public byte[] FileByte { get; set; }
        public string ContentType { get; set; }
        public bool Status { get; set; }
    }
}