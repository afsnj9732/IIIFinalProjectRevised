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
            return View();
        }

        [HttpPost]
        public ActionResult List(string randLocation)
        {
            dbJoutaEntities db = new dbJoutaEntities();

            var x = (from t in db.tMember
                     where t.f會員評分 >= 4
                     select new
                     {
                         mMemberNum = t.f會員編號,
                         mAccount = t.f會員帳號,
                         mName = t.f會員名稱,
                         mRating = t.f會員評分
                     })
                .OrderBy(t => Guid.NewGuid()).Take(3);

            return Json(x, JsonRequestBehavior.AllowGet);
        }
    }
}