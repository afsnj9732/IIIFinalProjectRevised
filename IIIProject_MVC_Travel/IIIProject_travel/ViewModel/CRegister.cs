using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IIIProject_travel.ViewModel
{
    public class CRegister
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "會員信箱不可空白")]
        [EmailAddress(ErrorMessage ="信箱格式有誤")]
        public string txtEmail { get; set; }

        [Required(AllowEmptyStrings =false,ErrorMessage = "會員名稱不可空白")]
        public string txtNickname { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "會員密碼不可空白")]
        [DataType(DataType.Password)]
        [RegularExpression(@"/[a-zA-Z]|\d{6,}/",ErrorMessage ="密碼須包含英文，數字且字數6位以上")]
        [MinLength(6,ErrorMessage ="密碼長度至少6位")]
        public string txtPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("txtPassword",ErrorMessage ="密碼不一致")]
        public string txtPassword_confirm { get; set; }

        [FileExtensions(ErrorMessage ="所上傳檔案不是圖片")]
        public string txtFiles { get; set; }    //使用者圖示

        public bool fIs信箱已驗證 { get; set; }   //狀態管理
        public System.Guid fActivationCode { get; set; }     //認證碼
    }
}