using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IIIProject_travel.Controllers
{
    public class 後台HomeController : Controller
    {
        // GET: 後台Home
        public ActionResult Home()
        {
            return View();
        }

        public ActionResult GetLineChartData()
        {
            dbJoutaEntities db = new dbJoutaEntities();
            var query = from t in db.tActivity
                        select new { name = t.f活動地區, count = t.f活動預算 };
            //var query = context.tActivity.Include("f活動預算")
            //.GroupBy(p => p.Product.ProductName)
            //.Select(g => new { name = g.Key, name =  g.Sum(w => w.Quantity) }).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPieChartData()
        {
            dbJoutaEntities db = new dbJoutaEntities();

            var north = db.tActivity.Where(x => x.f活動地區 == "北部").Count();
            var south = db.tActivity.Where(x => x.f活動地區 == "南部").Count();
            var east = db.tActivity.Where(x => x.f活動地區 == "東部").Count();

            Ratio obj = new Ratio();
            obj.North = north;
            obj.South = south;
            obj.East = east;

            return Json(obj, JsonRequestBehavior.AllowGet);

        }

        public class Ratio
        {
            public int North { get; set; }
            public int South { get; set; }
            public int East { get; set; }
        }
    }
}