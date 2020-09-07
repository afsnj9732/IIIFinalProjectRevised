using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IIIProject_travel.ViewModel
{
    public class CReset
    {
        [Required(ErrorMessage = "必填欄位", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string newPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("newPassword", ErrorMessage = "密碼不一致")]
        public string newPassword_confirm { get; set; }

        [Required]
        public string resetCode { get; set; }
    }
}