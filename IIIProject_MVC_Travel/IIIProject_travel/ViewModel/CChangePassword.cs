using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IIIProject_travel.ViewModel
{
    public class CChangePassword
    {
        [DisplayName("原密碼")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "請輸入密碼")]
        [DataType(DataType.Password)]
        public string txtPassword { get; set; }

        [DisplayName("新密碼")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "請輸入密碼")]
        [DataType(DataType.Password)]
        [RegularExpression(@"/[a-zA-Z]|\d{6,}/", ErrorMessage = "密碼須包含英文，數字且字數6位以上")]
        [MinLength(6, ErrorMessage = "密碼長度至少6位")]
        public string txtNewPassword { get; set; }

        [DisplayName("再次輸入新密碼")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "請輸入密碼")]
        [DataType(DataType.Password)]
        [Compare("txtNewPassword", ErrorMessage = "密碼不一致")]
        public string txtNewPassword_check { get; set; }
    }
}