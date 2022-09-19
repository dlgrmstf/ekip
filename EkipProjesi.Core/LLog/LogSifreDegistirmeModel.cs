namespace EkipProjesi.Core.LLog
{
    public class LogSifreDegistirmeModel
    {
        public long ID { get; set; }
        public int UserID { get; set; }
        public System.DateTime Tarih { get; set; }
        public string IP { get; set; }
        public string SessionID { get; set; }
    }
}