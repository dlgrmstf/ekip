using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EkipProjesi.Core.Kullanici
{
    public class IRol
    {
        public int ID { get; set; }
        [Required]
        public string Rol { get; set; }

        public List<IRol> Roller { get; set; }
    }
}