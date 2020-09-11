using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IIIProject_travel.ViewModel
{
    public class CBlog : tActivity
    {
        public string txtContent { get; set; }

        public string txtLocation { get; set; }

        public string txtTitle { get; set; }

        public string fImagPath { get; set; }

        public string QRcode { get; set; }

        public string QRcodeImage { get; set; }

        public string txtTime { get; set; }

        public string txtComment { get; set; }

        public HttpPostedFileBase blogPhoto { get; set; }

        public int id { get; set; }

        public string content { get; set; }

        public string CreateDate { get; set; }






    }
}