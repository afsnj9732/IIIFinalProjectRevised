using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IIIProject_travel.ViewModel
{
    public class CData
    {
        public IEnumerable<tMember> tMembers { get; set; }
        public IEnumerable<tActivity> tActivities { get; set; }
    }
}