using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IIIProject_travel.Security
{
    public class JwtObj
    {
        public string Email { get; set; }
        public string Role { get; set; }
        //到期時間
        public string Expire { get; set; }
    }
}