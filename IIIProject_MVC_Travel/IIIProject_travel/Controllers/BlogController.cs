using IIIProject_travel.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Compilation;
using System.Web.Mvc;
using System.Drawing;
using System.Drawing.Imaging;
using ZXing;
using ZXing.QrCode;
using PagedList;

namespace IIIProject_travel.Controllers
{
    public class BlogController : Controller
    {

        dbJoutaEntities db = new dbJoutaEntities();
        int pagesize = 3;
        // GET: Blog


        public ActionResult Index(string sortOder,string txtKey, string currentfilter, int page = 1)
        {
            var article = from t in (new dbJoutaEntities()).tActivity
                          where t.f活動類型 == "文章"
                          orderby t.f活動編號
                          select t;
            IQueryable<tActivity> tarticle = null;

            if (txtKey != null)
            {
                page = 1;
            }
            else
            {
                txtKey = currentfilter;
            }

            if (string.IsNullOrEmpty(txtKey))
                tarticle = from torder in (new dbJoutaEntities()).tActivity
                           where torder.f活動類型 =="文章"
                           select torder;
            else
                tarticle = from torder in (new dbJoutaEntities()).tActivity
                           where torder.f活動類型 == "文章"
                           where torder.f活動標題.Contains(txtKey)
                           select torder;

            ViewBag.Sernow = txtKey;
            ViewBag.keyName = string.IsNullOrEmpty(sortOder) ? "NameDown" : "";
            ViewBag.dateOrder = sortOder == "dateUp" ? "dateDown" : "dateUp";
            ViewBag.preViewOrder = sortOder == "preViewUp" ? "preViewDown" : "preViewUp";
            ViewBag.LikeOrder = sortOder == "LikeUp" ? "LikeDown" : "LikeUp";
            //ViewBag.Pointorder = sortOder == "帳號升冪" ? "帳號降冪" : "帳號升冪";



            switch (sortOder)
            {
                case "NameDown":
                    article = article.OrderByDescending(s => s.f活動標題);
                    break;

                case "dateUp":
                    article = article.OrderBy(s => s.f活動發起日期);
                    break;
                case "dateDown":
                    article = article.OrderByDescending(s => s.f活動發起日期);
                    break;
                case "preViewUp":
                    article = article.OrderBy(s => s.f活動瀏覽次數);
                    break;
                case "preViewDown":
                    article = article.OrderByDescending(s => s.f活動瀏覽次數);
                    break;
                case "LikeUp":
                    article = article.OrderBy(s => s.f活動讚數);
                    break;
                case "LikeDown":
                    article = article.OrderByDescending(s => s.f活動讚數);
                    break;
                default:
                    article = article.OrderBy(s => s.f活動標題);
                    break;
            }



            int currentPage = page < 1 ? 1 : page;

           
            //var articleList = article.ToList();
            var result = article.ToPagedList(currentPage, pagesize);


            return View(result);


        }

        public ActionResult personalIndex (int page = 1)
        {

            int currentPage = page < 1 ? 1 : page;

            var article = from t in (new dbJoutaEntities()).tActivity
                          where t.f活動類型 == "文章"
                          orderby t.f活動編號
                          select t;
            var articleList = article.ToList();
            var result = articleList.ToPagedList(currentPage, pagesize);


            return View(result);
        }
        public ActionResult BlogContent(int? id)
        {

            var article = from t in (new dbJoutaEntities()).tActivity
                          where t.f活動類型 == "文章"&&t.f活動編號==id
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
            HttpPostedFileBase blogPhoto = Request.Files["blogPhoto"];
            if (p.blogPhoto != null)
            {
                string photName = Guid.NewGuid().ToString() + Path.GetExtension(p.blogPhoto.FileName);

                var path = Path.Combine(Server.MapPath("~/Content/images/"), photName);
                p.blogPhoto.SaveAs(path);
                p.f活動團圖 =  photName;
            }



            tActivity article = new tActivity();

            article.f活動類型 = "文章";
            article.f活動標題 = p.txtTitle;
            article.f活動地點 = p.txtLocation;
            article.f活動內容 = p.txtContent;
            article.f活動團圖 = p.fImagPath;
            article.fQRcode網址 = p.QRcode;
            article.f會員編號 = 3;//暫時用，要改成儲存登入會員的編號


            dbJoutaEntities db = new dbJoutaEntities();
            db.tActivity.Add(article);
            db.SaveChanges();

            return RedirectToAction("index");

        }



        public ActionResult QRcode()
        {

            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Height = 50,
                    Width = 50,
                }
            };



            var img = writer.Write("https://localhost:44380/");
            string FileName = "michelin-guide";
            Bitmap myBitmap = new Bitmap(img);
            string filepath = string.Format(Server.MapPath("~/Content/images/") + "{0}.bmp", FileName);

            ViewBag.filePath = filepath;

            myBitmap.Save(filepath, ImageFormat.Bmp);
            ViewBag.IMG = myBitmap;

            return View();
        }

        public ActionResult PhotoGet()
        {
            string FileName = "michelin-guide";
            string filepath = string.Format(Server.MapPath("~/Content/images/")+"{0}.bmp", FileName);
            QRcode();
            byte[] filedata = System.IO.File.ReadAllBytes(filepath);
            string contentType = MimeMapping.GetMimeMapping(filepath);
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = filepath,
                Inline = false
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(filedata, contentType);

        }
    }
}