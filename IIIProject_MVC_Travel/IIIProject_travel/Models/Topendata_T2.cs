using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IIIProject_travel.Models
{
    public class Topendata_T2
    {
        public string Listname { get; set; }
        public string Language { get; set; }
        public string Orgname { get; set; }
        public string Updatetime { get; set; }
        public Topendata_T3 Infos { get; set; }
    }
}