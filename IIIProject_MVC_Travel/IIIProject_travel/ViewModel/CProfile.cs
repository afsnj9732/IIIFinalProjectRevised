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
        [DataType(DataType.EmailAddress)]
        public string txtEmail { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MinLength(6,ErrorMessage ="密碼字數不得少於6")]
        public string txtPassword { get; set; }
        [Required]
        public string txtNickName { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime txtBirth { get; set; }

        public string txtHobby { get; set; }

        public string txtIntro { get; set; }
    }
}