using System;

namespace EkipProjesi.API.Utils
{
    public class Result
    {
        public bool Basarili { get; set; }
        public string Mesaj { get; set; }
        public Exception Hata { get; set; }
    }
}