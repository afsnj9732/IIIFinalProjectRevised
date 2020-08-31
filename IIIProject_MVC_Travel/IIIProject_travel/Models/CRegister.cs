using IIIProject_travel.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IIIProject_travel.Models
{
    public class CRegister
    {

        [Required(AllowEmptyStrings = false, ErrorMessage = "會員信箱不可空白")]
        [EmailAddress(ErrorMessage = "信箱格式有誤")]
        [StringLength(200, ErrorMessage = "名稱長度最多200字元")]
        public string txtEmail { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "會員名稱不可空白")]
        [StringLength(20,ErrorMessage ="名稱長度最多20字元")]
        public string txtNickname { get; set; }

        public string txtPassword { get; set; }

        [FileExtensions(ErrorMessage = "所上傳檔案不是圖片")]
        public string txtFiles { get; set; }    //使用者圖示

        public string fActivationCode { get; set; }     //認證碼
        public bool isAdmin { get; set; }       //管理者
    }
}