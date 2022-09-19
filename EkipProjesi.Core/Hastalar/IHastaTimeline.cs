using System;

namespace EkipProjesi.Core.Hastalar
{
    public class IHastaTimeline
    {
        public int ID { get; set; }
        public int HastaID { get; set; }
        public string Baslik { get; set; }
        public string Aciklama { get; set; }
        public DateTime Tarih { get; set; }
    }
}