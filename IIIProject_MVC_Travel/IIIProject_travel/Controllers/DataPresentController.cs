using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace IIIProject_travel.Controllers
{
    public class DataPresentController : Controller
    {
        // GET: DataPresent
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetData()
        {
            dbJoutaEntities context = new dbJoutaEntities();

            //var query = context.tActivity.Include("f活動預算")
            //.GroupBy(p => p.Product.ProductName)
            //.Select(g => new { name = g.Key, count = g.Sum(w => w.Quantity) }).ToList();
            //return Json(query, JsonRequestBehavior.AllowGet);
            return View();

        }


    }
}