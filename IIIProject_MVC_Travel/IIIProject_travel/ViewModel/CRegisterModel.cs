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

        [Required(AllowEmptyStrings = false, ErrorMessage = "會員信箱不可空白")]
        [EmailAddress(ErrorMessage = "信箱格式有誤")]
        [StringLength(200, ErrorMessage = "名稱長度最多200字元")]
        public string txtEmail { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "會員名稱不可空白")]
        [StringLength(20, ErrorMessage = "名稱長度最多20字元")]
        public string txtNickname { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "會員密碼不可空白")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^.*(?=.{6,})(?=.*\d)(?=.*[a-zA-Z]).*$", ErrorMessage = "密碼須包含英文，數字且字數6位以上")]
        [MinLength(6, ErrorMessage = "密碼長度至少6位")]
        public string txtPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("txtPassword", ErrorMessage = "密碼不一致")]
        public string txtPassword_confirm { get; set; }

        //public HttpPostedFileBase MembersImg { get; set; }      //使用者圖示

        //[FileExtensions(ErrorMessage = "所上傳檔案不是圖片")]
        //public string txtFiles { get; set; }    //使用者圖示

        public string fActivationCode { get; set; }     //認證碼
        public bool isAdmin { get; set; }       //管理者
    }
    
}