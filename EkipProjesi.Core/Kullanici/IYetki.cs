using System.Collections.Generic;
using EkipProjesi.Core;

namespace EkipProjesi.Core.Kullanici
{
    public class IYetki
    {
        public int ID { get; set; }
        public string YetkiAdi { get; set; }
        public int RolID { get; set; }
        public List<IYetki> ModulList { get; set; }

        public Moduller Modul { get; set; }
        public int _Modul
        {
            get { return (short)Modul; }
            set { Modul = (Moduller)value; }
        }
    }
}