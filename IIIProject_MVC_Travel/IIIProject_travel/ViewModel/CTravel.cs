using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IIIProject_travel.ViewModel
{
    public class CTravel
    {
        public tTravel tTravel { get; set; }
        public tMember tMember { get; set; }
        public IQueryable<tTravel> tTravel_list{ get; set; }
        public string txtCondition;
    }
}