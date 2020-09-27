using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IIIProject_travel.ViewModel
{
    public class CGetMore
    {
        public bool MemberLike { get; set; }
        public string[] BlackList { get; set; }
        public tActivity TargetAct { get; set; }
        public tMember LoginMember { get; set; }
    }
}