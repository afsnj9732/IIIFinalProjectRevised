using IIIProject_travel.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace IIIProject_travel.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        [AllowAnonymous]
        public ActionResult LoginIndex()
        {
            CLogin c = new CLogin();
            c.txtRememberMe = "記住我";
            return View(c);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]      //驗證來源網站是否是偽造，用於防止駭客攻擊
        public ActionResult LoginIndex(CLogin user,string returnUrl="")
        {
            string msg = "";
            tMember target = (new dbJoutaEntities()).tMember
                             .FirstOrDefault(o => o.f會員電子郵件 == user.txtEmail
                             && o.f會員密碼 == user.txtPassword);
            
            if (target != null)
            {
                if (string.Compare(user.txtPassword, target.f會員密碼) == 0)
                {
                    int timeout = user.remember ? 525600 : 20;   //525600 min = 1 year(保質期一年)
                    var ticket = new FormsAuthenticationTicket(user.txtEmail, user.remember, timeout);
                    string encrypted = FormsAuthentication.Encrypt(ticket);
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                    cookie.Expires = DateTime.Now.AddMinutes(timeout);
                    cookie.HttpOnly = true;

                    Response.Cookies.Add(cookie);
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else {
                        return RedirectToAction("Home","Home");
                    }
                } else
                    msg = "帳號或密碼錯誤，請重新登入";
            }
            Session["member"] = target;
            if (user.txtEmail == "Admin@gmail.com" && user.txtPassword == "1234")
                return RedirectToAction("List", "後台會員");
            return RedirectToAction("Home", "Home");
        }

        //Logout
        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("LoginIndex", "Login");
        }


        [NonAction]
        public bool is信箱存在(string emailId)
        {
            dbJoutaEntities db = new dbJoutaEntities();
            var t = db.tMember.FirstOrDefault(a => a.f會員電子郵件 == emailId);
            return t != null;
        }
    }
}