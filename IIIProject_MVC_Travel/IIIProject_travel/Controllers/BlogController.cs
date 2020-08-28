using IIIProject_travel.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Compilation;
using System.Web.Mvc;

namespace IIIProject_travel.Controllers
{
    public class BlogController : Controller
    {
        // GET: Blog


        public ActionResult Index()
        {

            var article = from t in (new dbJoutaEntities()).tActivity
                          where t.f活動類型=="文章"
                          select t;


            return View(article);


        }
        public ActionResult List()
        {

            var article = from t in (new dbJoutaEntities()).tActivity
                          where t.f活動類型 == "文章"
                          select t;


            return View(article);


        }


        public ActionResult Create()
        {

            return View();

        }


        [HttpPost]
        public ActionResult Create(CBlog p)
        {
            if (p.blogPhoto != null)
            {
                string photName = Guid.NewGuid().ToString() + Path.GetExtension(p.blogPhoto.FileName);

                var path = Path.Combine(Server.MapPath("~/Content/images/"), photName);
                p.blogPhoto.SaveAs(path);
                p.fImagPath = "~/Content/images/" + photName;
            }



            tActivity article = new tActivity();

            article.f活動類型 = "文章";
            article.f活動標題 = p.txtTitle;
            article.f活動地點 = p.txtLocation;
            article.f活動內容 = p.txtContent;
            //article.f大頭貼路徑 = p.fImagPath;



            dbJoutaEntities db = new dbJoutaEntities();
            db.tActivity.Add(article);
            db.SaveChanges();

            return RedirectToAction("List");

        }
    }
}