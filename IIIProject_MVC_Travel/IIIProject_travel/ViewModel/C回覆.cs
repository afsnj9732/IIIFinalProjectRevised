using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IIIProject_travel.ViewModel
{
    public class C回覆
    {
        [DataType(DataType.EmailAddress)]
        public string f電子郵件 { get; set; }
        public string f電話 { get; set; }
        public string f標題 { get; set; }
        public string f意見 { get; set; }
        public string f性別 { get; set; }
        public string f聯絡人 { get; set; }
        public string f意見類型 { get; set; }
        public string f意見時間 { get; set; }
        public string f回覆 { get; set; }


    }
}