using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IIIProject_travel.ViewModel
{
    public class CActAdd
    {
        public tMember[] AttendGuys { get; set; }
        public tMember[] WantGuys { get; set; }
        public string[] BlackList { get; set; }
        public tActivity TargetAct { get; set; }
    }
}