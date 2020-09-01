using IIIProject_travel.Security;
using IIIProject_travel.Services;
using IIIProject_travel.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;

namespace IIIProject_travel.Controllers
{
    public class LoginController : Controller
    {
        //宣告service物件
        private readonly MembersService membersService = new MembersService();
        //宣告寄信用service物件
        private readonly MailService mailService = new MailService();

        // GET: Login
        [AllowAnonymous]    //是人皆可進
        public ActionResult LoginIndex()
        {
            //判斷使用者是否通過登入驗證
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Home","Home");     //已登入則重新導向
            CLogin c = new CLogin();
            c.txtRememberMe = "記住我";
            return View(c);
        }

        [HttpPost]
        public ActionResult LoginIndex(CLogin user)
        {
            string validateStr = membersService.LoginCheck(user.txtEmail, user.txtPassword);
            //判斷驗證後結果是否有錯誤訊息
            if (string.IsNullOrEmpty(validateStr))
            {
                //無錯誤就登入
                //先藉由Service取得登入者角色資料
                string RoleData = membersService.getRole(user.txtEmail);
                //設定JWT
                //JwtService js = new JwtService();
                //從Web.Config撈出資料
                string cookieName = WebConfigurationManager.AppSettings["CookieName"].ToString();
                //string token = js.GenerateToken(user.txtEmail, RoleData);
                //產生一個cookie
                HttpCookie cookie = new HttpCookie(cookieName);
                cookie["txtEmail"] = user.txtEmail;
                cookie["txtPassword"] = user.txtPassword;
                //設定單值
                //cookie.Value = Server.UrlEncode(token);
                //寫到用戶端
                Response.Cookies.Add(cookie);
                //設定cookie期限
                Response.Cookies[cookieName].Expires = DateTime.Now.AddMinutes(Convert.ToInt32(WebConfigurationManager.AppSettings["ExpireMinutes"]));
                return RedirectToAction("LoginIndex", "Login");
            }
            else
            {
                ModelState.AddModelError("", validateStr);
                //資料回填至view中
                return View(user);
            }


            //string msg = "";
            //tMember target = (new dbJoutaEntities()).tMember
            //                 .FirstOrDefault(o => o.f會員電子郵件 == user.txtEmail
            //                 && o.f會員密碼 == user.txtPassword);

            //if (target != null)
            //{
            //    if (string.Compare(user.txtPassword, target.f會員密碼) == 0)
            //    {
            //        int timeout = user.remember ? 525600 : 20;   //525600 min = 1 year(保質期一年)
            //        var ticket = new FormsAuthenticationTicket(user.txtEmail, user.remember, timeout);
            //        string encrypted = FormsAuthentication.Encrypt(ticket);
            //        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
            //        cookie.Expires = DateTime.Now.AddMinutes(timeout);
            //        cookie.HttpOnly = true;

            //        Response.Cookies.Add(cookie);
            //        if (Url.IsLocalUrl(returnUrl))
            //        {
            //            return Redirect(returnUrl);
            //        }
            //        else {
            //            return RedirectToAction("Home","Home");
            //        }
            //    } else
            //        msg = "帳號或密碼錯誤，請重新登入";
            //}
            //Session["member"] = target;
            //if (user.txtEmail == "Admin@gmail.com" && user.txtPassword == "1234")
            //    return RedirectToAction("List", "後台會員");
            //return RedirectToAction("Home", "Home");
        }

        //Logout
        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            string cookieName = WebConfigurationManager.AppSettings["CookieName"].ToString();
            //清除cookie
            HttpCookie cookie = new HttpCookie(cookieName);
            cookie.Expires = DateTime.Now.AddDays(-1);
            cookie.Values.Clear();
            Response.Cookies.Set(cookie);
            return RedirectToAction("LoginIndex");
        }


        //[NonAction]
        //public bool is信箱存在(string emailId)
        //{
        //    dbJoutaEntities db = new dbJoutaEntities();
        //    var t = db.tMember.FirstOrDefault(a => a.f會員電子郵件 == emailId);
        //    return t != null;
        //}
    }
}