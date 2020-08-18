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

            var article = from t in (new dbJoutaEntities()).tArticle
                          select t;


            return View(article);


        }
        public ActionResult List()
        {

            var article = from t in (new dbJoutaEntities()).tArticle
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



            tArticle article = new tArticle();

            article.f標題 = p.txtTitle;
            article.f地點 = p.txtLocation;
            article.f發文內容 = p.txtContent;
            article.f大頭貼路徑 = p.fImagPath;

            

            dbJoutaEntities db = new dbJoutaEntities();
            db.tArticle.Add(article);
            db.SaveChanges();

            return RedirectToAction ("List");

        }
    }
}