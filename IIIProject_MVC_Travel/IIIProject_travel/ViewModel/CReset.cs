using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IIIProject_travel.ViewModel
{
    public class CReset
    {
        [DisplayName("新密碼")]
        [Required(ErrorMessage = "必填欄位", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string newPassword { get; set; }

        [DisplayName("再次輸入密碼")]
        [DataType(DataType.Password)]
        [Compare("newPassword", ErrorMessage = "密碼不一致")]
        public string newPassword_confirm { get; set; }
        
        public string resetCode { get; set; }
    }
}