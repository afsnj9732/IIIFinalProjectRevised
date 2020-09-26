using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IIIProject_travel.ViewModel
{
    public class CTravel
    {
        public IEnumerable<tActivity> FinalList { get; set; }
        public int NowPage { get; set; }
        public int TotalPage { get; set; }
        public string[] MemberLike { get; set; }
    }
}