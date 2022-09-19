using System;

namespace EkipProjesi.Core.Tatil
{
    public class IResmiTatiller
    {
        public int ID { get; set; }
        public Nullable<int> Yil { get; set; }
        public Nullable<System.DateTime> Tarih { get; set; }
        public Nullable<bool> TatilMi { get; set; }
        public string TatilAdi { get; set; }

        public int DateGun { get; set; }
        public int DateAy { get; set; }
        public int DateYil { get; set; }
    }
}