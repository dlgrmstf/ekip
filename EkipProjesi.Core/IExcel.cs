using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkipProjesi.Core
{
    public class IExcel
    {
        public long ID { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<System.DateTime> Tarih { get; set; }
        public Nullable<int> SatirSayisi { get; set; }
        public Nullable<bool> Durum { get; set; }
        public string SessionID { get; set; }
        public int? HataliSatirSayisi { get; set; }
        public string ExcelID { get; set; }
        public ExcelTurleri ExcelTurleri { get; set; }
        public int? _ExcelTurleri
        {
            get { return (short)ExcelTurleri; }
            set { ExcelTurleri = (ExcelTurleri)value; }
        }

        public ExcelDurumlari ExcelDurumlari { get; set; }
        public int? _ExcelDurumlari
        {
            get { return (short)ExcelDurumlari; }
            set { ExcelDurumlari = (ExcelDurumlari)value; }
        }
    }

    public enum ExcelDurumlari
    {
        Basarili = 0,
        ExcelHatasi = 1,
        KayitHatasi = 2
    }

    public enum ExcelTurleri
    {
        Personel = 0      
    }
}