using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EkipProjesi.Web.Models
{
    public class ArindirmaHizmetBilgileriFiltreModel
    {
        public string KurumAdi { get; set; }
        public string PoliklinikTuru { get; set; }
        public DateTime BaslangicTarihi { get; set; }
        public DateTime BitisTarihi { get; set; }
    }
}