using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IIIProject_travel.Controllers
{
    public class 後台文章Controller : Controller
    {
        // GET: 後台文章
        public ActionResult List()
        {
            var 文章 = from t in (new dbJoutaEntities()).tActivity
                     where t.f活動類型 == "文章"
                     select t;
            return View(文章);
        }



        public ActionResult d刪除(int? id)
        {
            if (id == null)
                RedirectToAction("List");

            tActivity x = new tActivity();
            dbJoutaEntities db = new dbJoutaEntities();
            x = db.tActivity.FirstOrDefault(m => m.f活動編號 == id);
            db.tActivity.Remove(x);
            db.SaveChanges();

            return RedirectToAction("List");

        }
        public ActionResult v查看()
        {

            return View();

        }


    }
}