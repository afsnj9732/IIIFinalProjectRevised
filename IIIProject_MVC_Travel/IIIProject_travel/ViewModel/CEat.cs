using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IIIProject_travel.ViewModel
{
    public class CEat
    {
        public tEat tEat { get; set; }
        public tMember tMember { get; set; }
        public IQueryable<tEat> tEat_List { get; set; }
        public string txtCondition;
    }
}