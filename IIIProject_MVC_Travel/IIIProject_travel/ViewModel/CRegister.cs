using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IIIProject_travel.ViewModel
{
    [MetadataType(typeof(CRegisterMetadata))]
    public partial class CRegister
    {
        public string txtAccount { get; set; }
        public string txtNickname { get; set; }
        public string txtPassword { get; set; }
        public string txtPassword_confirm { get; set; }
        public string txtEmail { get; set; }
        public bool fIs信箱已驗證 { get; set; }   //權限狀態管理
        public System.Guid fActivationCode { get; set; }     //認證碼
    }

    public class CRegisterMetadata
    {
        [Display(Name ="會員帳號")]
        [Required(AllowEmptyStrings =false,ErrorMessage ="帳號不可空白")]
        public string txtAccount { get; set; }

        [Display(Name ="會員名稱")]
        [Required(AllowEmptyStrings =false,ErrorMessage = "會員名稱不可空白")]
        public string txtNickname { get; set; }

        [Display(Name = "會員密碼")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "會員密碼不可空白")]
        [DataType(DataType.Password)]
        [MinLength(6,ErrorMessage ="密碼長度至少6位")]
        public string txtPassword { get; set; }

        [Display(Name = "再次輸入密碼")]
        [DataType(DataType.Password)]
        [Compare("password",ErrorMessage ="密碼不一致")]
        public string txtPassword_confirm { get; set; }

        [Display(Name = "會員信箱")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "會員信箱不可空白")]
        [DataType(DataType.EmailAddress)]
        public string txtEmail { get; set; }

        public bool fIs信箱已驗證 { get; set; }   //狀態管理
        public System.Guid fActivationCode { get; set; }     //認證碼
    }
}