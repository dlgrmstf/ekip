using System;
using System.Collections.Generic;

namespace EkipProjesi.Web.Models
{
    public class FiltreModel
    {
        public DateTime IseGirisTarihiMax { get; set; }
        public DateTime IseGirisTarihiMin { get; set; }
        public DateTime IstenCikisTarihiMax { get; set; }
        public DateTime IstenCikisTarihiMin { get; set; }
        public List<int> Cinsiyet { get; set; }
        public List<int> PersonelTuruId { get; set; }
        public List<int> CalismaSekliId { get; set; }
        public List<int> GorevSekliId { get; set; }
        public List<int> UstBirimId { get; set; }
        public List<int> BirimId { get; set; }
        public List<int> AltBirimId { get; set; }
        public List<int> UnvanId { get; set; }
        public List<int> GorevId { get; set; }
        public List<int> KadroId { get; set; }
        public List<int> EgitimSeviyesiId { get; set; }
        public List<int> IzinTuruId { get; set; }
    }

    public class FluentVar
    {
        public string Name { get; set; }
        public DateTime NextRun { get; set; }
        public bool Disabled { get; set; }
    }
}