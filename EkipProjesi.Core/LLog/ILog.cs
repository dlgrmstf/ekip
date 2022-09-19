using System;
using System.Collections.Generic;

namespace EkipProjesi.Core.LLog
{
    public class ILog
    {
        public int Id { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Thread { get; set; }
        public string Levels { get; set; }
        public string Logger { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public Nullable<int> UserId { get; set; }
        public string Application { get; set; }
        public string Url { get; set; }
        public string UrlRefferer { get; set; }
        public string SessionID { get; set; }
        public string JsError { get; set; }
        public List<ILog> LogList { get; set; }
        public bool Status { get; set; }
        public string UserName { get; set; }
        public int? IsletmeID { get; set; }
        public string IsletmeAdi { get; set; }
    }
}