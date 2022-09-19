using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkipProjesi.Core.Kullanici
{
    public class IRolYetkileri
    {
        public int ID { get; set; }
        public int RolId { get; set; }
        public int YetkiId { get; set; }
        public bool Goruntuleme { get; set; }
        public bool Ekleme { get; set; }
        public bool Duzenleme { get; set; }
        public bool Silme { get; set; }

        public string RolAdi { get; set; }
        public string YetkiAdi { get; set; }

        public List<IYetki> Moduller { get; set; }
        public List<IRolYetkileri> RolModulIliskisi { get; set; }

        public IEnumerable<IRolYetkileri> YetkiIslem { get; set; }
    }
}