using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IIIProject_travel.ViewModel
{
    public class CProfile
    {
        [Required]
        public string txtNickName { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime txtBirth { get; set; }

        public string txtHobby { get; set; }

        public string txtIntro { get; set; }
    }
}