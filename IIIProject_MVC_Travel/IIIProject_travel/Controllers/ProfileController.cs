using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IIIProject_travel.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        public ActionResult ProfileIndex()
        {
            //var article = from t in (new dbJoutaEntities()).tActivity
            //              where t.f活動類型 == "文章" 
            //              select t;
            //return View(article);

            var travel = from t in (new dbJoutaEntities()).tActivity
                         where t.f活動類型 == "旅遊"
                         select t;
            return View(travel);
        }
     


    }
}