using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace IIIProject_travel.ViewModel
{
    public class CLogin
    {
        [Required(AllowEmptyStrings =false,ErrorMessage ="必填欄位")]
        public string txtAccount { get; set; }
        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "必填欄位")]
        public string txtPassword { get; set; }

        public bool rememberMe { get; set; }
    }
}