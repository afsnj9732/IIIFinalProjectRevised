using IIIProject_travel.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IIIProject_travel.ViewModel
{
    public class CRegisterModel
    {
        public CRegister newMember { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "會員密碼不可空白")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "密碼長度至少6位")]
        public string txtPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("txtPassword", ErrorMessage = "密碼不一致")]
        public string txtPassword_confirm { get; set; }
        
        public HttpPostedFileBase MembersImg { get; set; }      //使用者圖示
    }

    
}