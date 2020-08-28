//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace IIIProject_travel.Controllers
//{
//    public class 後台飯局Controller : Controller
//    {
//        // GET: 後台飯局
//        public ActionResult List()
//        {
//            var 飯局 = from t in (new dbJoutaEntities()).tEat
//                     select t;
//            return View(飯局);
//        }

//        public ActionResult d刪除(int? id)
//        {
//            if (id == null)
//                RedirectToAction("List");

//            tEat x = new tEat();
//            dbJoutaEntities db = new dbJoutaEntities();
//            x = db.tEat.FirstOrDefault(m => m.f飯局編號 == id);
//            db.tEat.Remove(x);
//            db.SaveChanges();

//            return RedirectToAction("List");

//        }
//        public ActionResult v查看()
//        {

//            return View();

//        }
//    }
//}