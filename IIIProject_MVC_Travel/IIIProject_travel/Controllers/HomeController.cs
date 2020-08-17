using IIIProject_travel.Models;
using IIIProject_travel.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IIIProject_travel.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Home()
        {
                return View();   
        }

        public ActionResult Login()
        {
            string code = Session[CDictionary.SK_USERLOGIN_CODE] as string;
            if (string.IsNullOrEmpty(code))
            {
                Random r = new Random();
                code = r.Next(0, 10).ToString() + r.Next(0, 10).ToString() + r.Next(0, 10).ToString() + r.Next(0, 10).ToString();
                Session[CDictionary.SK_USERLOGIN_CODE] = code;
            }
            ViewBag.CODE = code;
            return View();
        }
        [HttpPost]
        public ActionResult Login(CLogin p)
        {

            string code = Session[CDictionary.SK_USERLOGIN_CODE] as string;
            if (!p.txtPassword.Equals(code))
            {
                ViewBag.CODE = code;
                return View();
            }
            String fAccount = p.txtAccount;
            
            tMember cust = (new dbJoutaEntities()).tMember.FirstOrDefault(t => t.f會員帳號== fAccount && t.f會員密碼.Equals(p.txtPassword));
            if (cust == null)
                return View();
            Session[CDictionary.SK_LOGIN_MEMBER] = cust;
            return RedirectToAction("Home");
        }

        public ActionResult Register()
        {
            var members = from t in (new dbJoutaEntities()).tMember
                            select t;
            return View();
        }

        public ActionResult Save()
        {
            tMember x = new tMember();
            x.f會員帳號 = Request.Form["txtAccount"];
            x.f會員密碼 = Request.Form["txtPassword"];
            x.f性別 = Request.Form["txtGender"];
            x.f暱稱 = Request.Form["txtNickname"];
            x.f電子郵件 = Request.Form["txtEmail"];

            dbJoutaEntities db = new dbJoutaEntities();
            db.tMember.Add(x);
            db.SaveChanges();
            return RedirectToAction("Login");
        }
       
    }
}