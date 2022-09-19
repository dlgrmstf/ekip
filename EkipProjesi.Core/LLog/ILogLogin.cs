using System;

namespace EkipProjesi.Core.LLog
{
    public class ILogLogin
    {
        public int ID { get; set; }
        public int KullaniciId { get; set; }
        public DateTime Tarih { get; set; }
        public string IP { get; set; }
        public string IsletimSistemi { get; set; }
        public string TarayiciAdi { get; set; }
        public string MobilMi { get; set; }
        public string SessionID { get; set; }
        public string KullaniciAdi { get; set; }
        public DateTime CikisTarihi { get; set; }
        public int? SessionCount { get; set; }
        public string MachineName { get; set; }
        public string MachineIP { get; set; }
        public string LogOutMachineName { get; set; }
        public string LogOutMachineIP { get; set; }
        public LogTipi LogTipi { get; set; }
        public int _LogTipi
        {
            get { return (short)LogTipi; }
            set { LogTipi = (LogTipi)value; }
        }
    }
}