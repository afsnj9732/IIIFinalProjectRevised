using IIIProject_travel.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IIIProject_travel.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult LoginIndex()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginIndex(CLogin p)
        {
            tMember target = (new dbJoutaEntities()).tMember
                             .FirstOrDefault(o => o.f會員帳號 == p.txtAccount 
                             && o.f會員密碼 == p.txtPassword);
            Session["member"] = target;
            if (p.txtAccount == "Admin" && p.txtPassword == "1234")
                return RedirectToAction("List", "後台會員");
            return RedirectToAction("Home","Home");
        }
    }
}