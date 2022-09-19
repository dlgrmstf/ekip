namespace EkipProjesi.Core.LLog
{
    public class LogHataliGirislerModel
    {
        public long ID { get; set; }
        public int UserID { get; set; }
        public System.DateTime Tarih { get; set; }
        public string IP { get; set; }
        public string IsletimSistemi { get; set; }
        public string MobilMi { get; set; }
        public string TarayiciAdi { get; set; }
    }
}