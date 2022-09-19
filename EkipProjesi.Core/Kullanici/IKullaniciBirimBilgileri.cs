using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkipProjesi.Core.Kullanici
{
    public class IKullaniciBirimBilgileri
    {
        public int ID { get; set; }
        public int KullaniciID { get; set; }
        public string Kurum { get; set; }
        public string Bina { get; set; }
        public string Bolge { get; set; }
    }
}