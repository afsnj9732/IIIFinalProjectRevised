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
        public ActionResult BlogContent()
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
            article.f會員編號 = 3;//暫時用，要改成儲存登入會員的編號


            dbJoutaEntities db = new dbJoutaEntities();
            db.tActivity.Add(article);
            db.SaveChanges();

            return RedirectToAction("BlogContent");

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

            string filePath = string.Format("C:\\Users\\User\\Desktop\\slnprjQRcode\\prjQRcode\\Content\\{0}.bmp", FileName);
            ViewBag.filePath = filePath;

            myBitmap.Save(filePath, ImageFormat.Bmp);
            ViewBag.IMG = myBitmap;

            return View();
        }

        public ActionResult PhotoGet()
        {
            string FileName = "michelin-guide";
            string filepath = string.Format("C:\\Users\\User\\Desktop\\slnprjQRcode\\prjQRcode\\Content\\{0}.bmp", FileName);
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