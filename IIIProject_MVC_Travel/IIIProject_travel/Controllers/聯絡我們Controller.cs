using IIIProject_travel.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IIIProject_travel.Controllers
{
    public class 聯絡我們Controller : Controller
    {
        //GET: 聯絡我們
        public ActionResult List()
        {
            var 意見表 = from t in (new dbJoutaEntities()).tActivity
                      where t.f活動類型 == "聯絡我們"
                      select t;
            return View(意見表);
        }
        public ActionResult New()
        {
            return View();
        }
        [HttpPost]
        public ActionResult New(PData p)
        {
            tMember x = new tMember();
            x.f會員性別 = p.gender;
            x.f會員電子郵件 = p.txt電子郵件;
            x.f會員電話 = p.txt電話;
            tActivity y = new tActivity();
            y.f活動內容 = p.txt意見;
            y.f活動類型 = "意見";

            dbJoutaEntities db = new dbJoutaEntities();
            db.tActivity.Add(y);
            db.tMember.Add(x);
            db.SaveChanges();

            return RedirectToAction("New");
        }

    }
}