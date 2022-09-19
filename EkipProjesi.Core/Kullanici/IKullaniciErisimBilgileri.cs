using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkipProjesi.Core.Kullanici
{
    public class IKullaniciErisimBilgileri
    {
        public int ID { get; set; }
        public int KullaniciID { get; set; }
        public int ErisimKoduID { get; set; }
        public List<IKullaniciErisimBilgileri> ErisimBilgileriList { get; set; }
    }
}