using IIIProject_travel.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IIIProject_travel.Controllers
{
    public class ProfileController : Controller
    {
        dbJoutaEntities db = new dbJoutaEntities();
        // GET: Profile
        
        public ActionResult ProfileIndex()
        {
            CMember c = new CMember();
            var travel = from t in (new dbJoutaEntities()).tActivity
                         select t;  //從資料表抓資料
            c.tActivities = travel;
            c.tMembers = (tMember)Session["member"];
            return View(c);
        }

        public ActionResult Chat()
        {

            return View();
        }
    }
}