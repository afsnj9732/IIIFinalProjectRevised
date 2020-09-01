using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace IIIProject_travel.ViewModel
{
    public class CLogin
    {
        [DisplayName("信箱")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "會員信箱不可空白")]
        [EmailAddress(ErrorMessage = "信箱格式有誤")]
        public string txtEmail { get; set; }

        [DisplayName("密碼")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "會員密碼不可空白")]
        [DataType(DataType.Password)]
        //[RegularExpression(@"[a-zA-Z]|\d{6,}", ErrorMessage = "密碼須包含英文，數字且字數6位以上")]
        [MinLength(6, ErrorMessage = "密碼長度至少6位")]
        public string txtPassword { get; set; }

        public bool remember { get; set; }

        public string txtRememberMe { get; set; }
    }
}