using IIIProject_travel.Security;
using Jose;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace IIIProject_travel
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        //權限管理-設定角色
        protected void Application_OnPostAuthenticateRequest(object sender, EventArgs e)
        {
            //接收請求資料
            HttpRequest httpRequest = HttpContext.Current.Request;
            //設定JWT密鑰
            string secretKey = WebConfigurationManager.AppSettings["SecretKey"].ToString();
            //設定cookie名稱
            string cookieName = WebConfigurationManager.AppSettings["CookieName"].ToString();

            //檢查cookie內是否存放TOKEN
            if( httpRequest.Cookies[cookieName] != null)
            {
                JwtObj jo = JWT.Decode<JwtObj>(Convert.ToString(httpRequest.Cookies[cookieName].Value),
                    Encoding.UTF8.GetBytes(secretKey), JwsAlgorithm.HS512);
                //將使用者角色資料取出，併分割成陣列
                string[] roles = jo.Role.Split(new char[] { ',' });
                //自行建立Identity取代HttpRequest.Current.User的Identity
                //將資料塞進Claim
                Claim[] claims = new Claim[]
                    {
                        new Claim(ClaimTypes.Name,jo.Email),
                        new Claim(ClaimTypes.NameIdentifier,jo.Email)
                    };
                var claimIdentity = new ClaimsIdentity(claims,cookieName);
                //加入identityprovider這個Claim使得反仿冒語彙@html.AntiForgeryToken()能通過
                claimIdentity.AddClaim(
                    new Claim(@"http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "My Identity", @"http://www.w3.org/2001/XMLSchema#string")
                    ) ;
                //指派角色到目前這個HttpContext的User物件
                HttpContext.Current.User = new GenericPrincipal(claimIdentity,roles);
                Thread.CurrentPrincipal = HttpContext.Current.User;

            }
        }
    }
}
