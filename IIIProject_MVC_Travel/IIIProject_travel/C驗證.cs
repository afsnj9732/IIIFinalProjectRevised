using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace IIIProject_travel
{
    public static class C驗證
    {//加密處理
        public static string Hash(string v)
        {
            return Convert.ToBase64String(
                    System.Security.Cryptography.SHA256.Create()
                    .ComputeHash(Encoding.UTF8.GetBytes(v))
                ) ;
        
        }
    }
}