using IIIProject_travel.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IIIProject_travel.Controllers
{
    public class TravelController : Controller
    {
        public JsonResult CountView(string target, string p)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            CSelect obj = serializer.Deserialize<CSelect>(p);
            dbJoutaEntities db = new dbJoutaEntities();
            if (target != null)
            {
                int select = Convert.ToInt32(target);
                tActivity theTarget = db.tActivity.FirstOrDefault(x => x.f活動編號 == select);
                theTarget.f活動瀏覽次數 = (theTarget.f活動瀏覽次數 + 1);
                db.SaveChanges();
            }
            var tTravel_order = typeof(tActivity).GetProperty(obj.order);

            //不使用if，動態抓取排序條件
            var CountViewList = db.tActivity
                        .AsEnumerable().OrderByDescending(a => tTravel_order.GetValue(a, null))
                        .Select(a => a); //升冪

            if (obj.background_color == "rgb(250, 224, 178)")
            {
                CountViewList = db.tActivity
                .AsEnumerable().OrderBy(a => tTravel_order.GetValue(a, null))
                .Select(a => a); //降冪
            }

            if (!string.IsNullOrEmpty(obj.contain)) //搜尋欄位若非空
            {
                CountViewList = CountViewList.Where(b => b.f活動標題.Contains(obj.contain))
                            .Select(a => a);
            }
            if (obj.category != "所有")
            {
                CountViewList = CountViewList
                                    .Where(b => b.f活動分類 == obj.category)
                                    .Select(a => a);
            }

            if (obj.label != "全部")
            {
                CountViewList = CountViewList
                                    .Where(b => b.f活動讚數 > Convert.ToInt32(obj.label))
                                    .Select(a => a);
            }


            var FinalList = CountViewList                            
                            .Select(a => a.f活動瀏覽次數);

            return Json(FinalList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult FeelGood(string target, string p)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            CSelect obj = serializer.Deserialize<CSelect>(p);
            dbJoutaEntities db = new dbJoutaEntities();
            if (target != null)
            {
                int select = Convert.ToInt32(target);
                tActivity theTarget = db.tActivity.FirstOrDefault(x => x.f活動編號 == select);
                theTarget.f活動讚數 = (theTarget.f活動讚數 + 1);
                db.SaveChanges();
            }
            var tTravel_order = typeof(tActivity).GetProperty(obj.order);

            //不使用if，動態抓取排序條件
            var CountViewList = db.tActivity
                        .AsEnumerable().OrderByDescending(a => tTravel_order.GetValue(a, null))
                        .Select(a => a); //升冪

            if (obj.background_color == "rgb(250, 224, 178)")
            {
                CountViewList = db.tActivity
                .AsEnumerable().OrderBy(a => tTravel_order.GetValue(a, null))
                .Select(a => a); //降冪
            }

            if (!string.IsNullOrEmpty(obj.contain)) //搜尋欄位若非空
            {
                CountViewList = CountViewList.Where(b => b.f活動標題.Contains(obj.contain))
                            .Select(a => a);
            }
            if (obj.category != "所有")
            {
                CountViewList = CountViewList
                                    .Where(b => b.f活動分類 == obj.category)
                                    .Select(a => a);
            }

            if (obj.label != "全部")
            {
                CountViewList = CountViewList
                                    .Where(b => b.f活動讚數 > Convert.ToInt32(obj.label))
                                    .Select(a => a);
            }


            var FinalList = CountViewList
                            .Select(a => a.f活動讚數);

            return Json(FinalList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult article_AJAX(string p)
        {
            IEnumerable<tActivity> order_travel_list;
            order_travel_list = from x in (new dbJoutaEntities()).tActivity
                                select x;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            CSelect obj = serializer.Deserialize<CSelect>(p);
            if (!string.IsNullOrEmpty(obj.order))
            {
                //不使用if，動態抓取排序條件
                var tTravel_order = typeof(tActivity).GetProperty(obj.order);
                order_travel_list = (new dbJoutaEntities()).tActivity
                        .AsEnumerable().OrderByDescending(a => tTravel_order.GetValue(a, null))
                        .Select(a => a); //升冪

                if (obj.background_color == "rgb(250, 224, 178)")
                {
                    order_travel_list = (new dbJoutaEntities()).tActivity
                        .AsEnumerable().OrderBy(a => tTravel_order.GetValue(a, null))
                        .Select(a => a); //降冪
                }

                if (!string.IsNullOrEmpty(obj.contain)) //搜尋欄位若非空
                {
                    order_travel_list = order_travel_list.Where(b => b.f活動標題.Contains(obj.contain))
                                .Select(a => a);                                                 
                }
                if (obj.category != "所有")
                {
                    order_travel_list = order_travel_list
                                        .Where(b => b.f活動分類 == obj.category)
                                        .Select(a => a);
                }

                if (obj.label != "全部") {
                order_travel_list = order_travel_list
                                    .Where(b => b.f活動讚數 > Convert.ToInt32(obj.label))
                                    .Select(a => a);
                }

            }
            return View(order_travel_list);
        }

        // GET: Travel
        public ActionResult TravelIndex()
        {
            //var data = from x in (new dbJoutaEntities()).tTravel
            //           select x.f文章讚數;
            //var myChart = new Chart(width: 600, height: 400)
            //    .AddTitle("Product Sales")
            //    .DataBindTable(dataSource: data, xField: "Name");

            //Session["Chart"] = myChart;

            return View();
        }

        [HttpPost]
        public ActionResult TravelIndex(tActivity p)
        {
            
            if (Session["member"] != null&&p.f活動標題!=null)
            {
                HttpPostedFileBase PicFile = Request.Files["PicFile"];
                if (PicFile != null)
                {
                    var NewFileName = Guid.NewGuid() + Path.GetExtension(PicFile.FileName);
                    var NewFilePath = Path.Combine(Server.MapPath("~/Content/images/"), NewFileName);
                    PicFile.SaveAs(NewFilePath);
                    p.f活動團圖 = NewFileName;
                }
                tMember Member = (tMember)Session["member"];
                p.f會員編號 = Member.f會員編號;
                dbJoutaEntities db = new dbJoutaEntities();
                db.tActivity.Add(p);
                db.SaveChanges();
            }
            return View();
        }
    }
}