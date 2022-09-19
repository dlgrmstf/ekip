using System.Collections.Generic;

namespace EkipProjesi.Core.Kullanici
{
    public class IKullaniciRol
    {
        public IKullaniciRol()
        {
            Yetkiler = new List<IRolYetkileri>();
        }
        public int ID { get; set; }
        public string TipAdi { get; set; }
        public int KullaniciId { get; set; }
        public int RolId { get; set; }
        public string RolAdi { get; set; }

        public List<int> Roller { get; set; }
        public List<IRolYetkileri> Yetkiler { get; set; }

        public Roller RolEnum { get; set; }
        public int? _RolEnum
        {
            get { return (short)RolEnum; }
            set { RolEnum = (Roller)value; }
        }

    }
}