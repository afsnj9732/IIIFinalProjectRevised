using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IIIProject_travel.Controllers
{
    public class 優惠發送Controller : Controller
    {
        // GET: 優惠發送
        public ActionResult List()
        {
            var x = from t in (new dbJoutaEntities()).tActivity
                    where t.f活動類型 == "優惠"
                    select t;
            return View(x);
        }

        public ActionResult Delete(int? id)
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
    }
}