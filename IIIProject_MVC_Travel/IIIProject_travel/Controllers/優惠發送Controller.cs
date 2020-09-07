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
            //var x = from t in (new dbJoutaEntities()).tActivity
            //        select t;
            //return View(x);
            return View();
        }

        [HttpPost]
        public ActionResult List(string location)
        {
            return View();
        }
    }
}