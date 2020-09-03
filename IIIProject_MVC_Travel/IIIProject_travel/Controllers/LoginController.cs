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
            //if (User.Identity.IsAuthenticated)
            //    return RedirectToAction("Home","Home");     //已登入則重新導向
            CLogin c = new CLogin();
            c.txtRememberMe = "記住我";
            return View(c);
        }

        [HttpPost]
        public ActionResult LoginIndex(CLogin user)
        {
            tMember target = (new dbJoutaEntities()).tMember
                .FirstOrDefault(a=>a.f會員電子郵件 == user.txtEmail&&a.f會員密碼==user.txtPassword);

            Session["member"] = target;
            if (user.txtEmail == "Admin@gmail.com" && user.txtPassword == "admin123456")
                return RedirectToAction("List", "後台會員");
            return RedirectToAction("Home", "Home");
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