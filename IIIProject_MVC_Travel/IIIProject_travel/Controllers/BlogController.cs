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
        //public actionresult articleindex(string keyword)        //首頁搜尋用
        //{
        //    string homesearch = keyword;
        //    return view((object)homesearch);
        //}
        //[httppost]
        //public actionresult articleindex(int? id)
        //{
        //    string homesearch = "";
        //    homesearch += request.form["txtkeyword"];
        //    homesearch += ",";
        //    return view((object)homesearch);
        //}

        public ActionResult Index(string sortOder,string txtKey, string currentfilter, int page = 1)
        {
            var article = from t in (new dbJoutaEntities()).tActivity
                          where t.f活動類型 == "文章"
                          orderby t.f活動發起日期 
                          select t;

            //IQueryable<tActivity> tarticle = null;

            if (txtKey != null)
            {
                page = 1;
            }
            else
            {
                txtKey = currentfilter;
            }

            if (string.IsNullOrEmpty(txtKey))
            {
                    article = from torder in (new dbJoutaEntities()).tActivity
                                               where torder.f活動類型 =="文章"
                                               orderby torder.f活動發起日期 
                                               select torder;
            }
                
            else
            {

                    article = from m in (new dbJoutaEntities()).tActivity
                           where m.f活動類型 == "文章" && m.f活動標題.Contains(txtKey)
                           orderby m.f活動發起日期 
                           select m;


            }
                
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
            var result = article.OrderBy(s => s.f活動發起日期).ToPagedList(currentPage, pagesize);


            return View(result);


        }

        public ActionResult personalIndex (int? id, string txtKey, string currentfilter , int page = 1)
        {


            var article = from t in (new dbJoutaEntities()).tActivity
                          where t.f活動類型 == "文章" && t.f會員編號 == id
                          orderby t.f會員編號
                          select t;
            if (txtKey != null)
            {

                page = 1;


            }

            else
            {
                txtKey = currentfilter;

            }

            if (string.IsNullOrEmpty(txtKey))
            {

                 article = from t in (new dbJoutaEntities()).tActivity
                              where t.f活動類型 == "文章" && t.f會員編號 == id
                              orderby t.f活動編號
                              select t;
            }

            else
            {

                article = from t in (new dbJoutaEntities()).tActivity
                          where t.f活動類型 == "文章" && t.f會員編號 == id && t.f活動標題.Contains(txtKey)
                          orderby t.f活動編號
                          select t;

            }


            int currentPage = page < 1 ? 1 : page;


            var articleList = article.ToList();
            var result = articleList.ToPagedList(currentPage, pagesize);


            return View(result);
        }
        public ActionResult BlogContent(int? id)
        {

            var article = from t in (new dbJoutaEntities()).tActivity
                          where t.f活動類型 == "文章"&&t.f活動編號==id
                          select t;


            dbJoutaEntities db = new dbJoutaEntities();
            var target = db.tActivity.Where(t => t.f活動編號 == id).FirstOrDefault();
            target.f活動瀏覽次數 += 1;
            db.SaveChanges();


            return View(article.FirstOrDefault());


        }



        public ActionResult LikeIt(int? id)
        {
            var article = from t in (new dbJoutaEntities()).tActivity
                          where t.f活動類型 == "文章" && t.f活動編號 == id
                          select t;

            dbJoutaEntities db = new dbJoutaEntities();
            var target = db.tActivity.Where(t => t.f活動編號 == id).FirstOrDefault();
            target.f活動讚數 += 1;
            db.SaveChanges();

            return RedirectToAction("BlogContent",new { id = id }) ;
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
            article.f活動團圖 = p.f活動團圖;
            article.fQRcode網址 = p.QRcode;
            article.fQRcodeImage = p.QRcodeImage;
            article.f活動發起日期 = (DateTime.Now).ToString();
            article.f活動地區 = p.f活動地區;
            article.f活動瀏覽次數 = 0;
            article.f活動讚數 = 0;

            var LoginMember = (tMember)Session["member"];
            article.f會員編號 = LoginMember.f會員編號; 


            dbJoutaEntities db = new dbJoutaEntities();
            db.tActivity.Add(article);
            db.SaveChanges();

            return RedirectToAction("index");

        }


        [HttpPost]
        public ActionResult AddComment (CBlogComment p )
        {



            var ct = DateTime.Now;

            var CurrentTime = ct.ToString();



            var coomment = new tBlogComment()
            {

                f活動編號 = p.f活動編號,
                fBlogComment = p.Content,
                fCreateDate = CurrentTime,

            };


            db.tBlogComment.Add(coomment);
            db.SaveChanges();


            return RedirectToAction("BlogContent", new { id = p.f活動編號});


        }




        public ActionResult QRcode(int? id)
        {
            var article = from t in (new dbJoutaEntities()).tActivity
                          where t.f活動類型 == "文章" && t.f活動編號 == id
                          select t;
            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Height = 50,
                    Width = 50,
                }
            };


            if (article.Select(t => t.fQRcode網址).FirstOrDefault() != null)
            {

            var img = writer.Write(article.Select(t => t.fQRcode網址).FirstOrDefault());
            string FileName = article.Select(t => t.fQRcodeImage).FirstOrDefault();
            Bitmap myBitmap = new Bitmap(img);
            string filepath = string.Format(Server.MapPath("~/Content/images/") + "{0}.bmp", FileName);

            ViewBag.filePath = filepath;

            myBitmap.Save(filepath, ImageFormat.Bmp);
            ViewBag.IMG = myBitmap;


            }

            else
            {
                var img = writer.Write("https://www.iii.org.tw/");
                string FileName = "iii";
                Bitmap myBitmap = new Bitmap(img);
                string filepath = string.Format(Server.MapPath("~/Content/images/") + "{0}.bmp", FileName);

                ViewBag.filePath = filepath;

                myBitmap.Save(filepath, ImageFormat.Bmp);
                ViewBag.IMG = myBitmap;



            }



            return View();
        }

        public ActionResult PhotoGet(int? id)
        {
            var article = from t in (new dbJoutaEntities()).tActivity
                          where t.f活動類型 == "文章" && t.f活動編號 == id
                          select t;


            if (article.Select(t => t.fQRcodeImage).FirstOrDefault() != null)
            {

            string FileName = article.Select(t => t.fQRcodeImage).FirstOrDefault(); 
            string filepath = string.Format(Server.MapPath("~/Content/images/")+"{0}.bmp", FileName);
            QRcode(id);
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

            else
            {

                string FileName = "iii";
                string filepath = string.Format(Server.MapPath("~/Content/images/") + "{0}.bmp", FileName);
                QRcode(id);
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
}