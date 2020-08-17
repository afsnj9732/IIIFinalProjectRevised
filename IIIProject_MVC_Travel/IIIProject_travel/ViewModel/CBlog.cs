using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IIIProject_travel.ViewModel
{
    public class CBlog
    {
        public string txtContent { get; set; }

        public string txtLocation { get; set; }

        public string txtTitle { get; set; }

        public string fImagPath { get; set; }

        public HttpPostedFileBase blogPhoto { get; set; }


    }
}