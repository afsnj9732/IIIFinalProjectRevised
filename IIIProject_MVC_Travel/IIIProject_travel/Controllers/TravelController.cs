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
        public string FeelGood(string target)
        {
            if (Session["member"] == null)
            {
                return "0";
            }

            //限制同一會員只能點一次，未完成
            //tMember now_member = (tMember)Session["member"];

            //  ((tMember)Session["member"]).f會員編號;

            dbJoutaEntities db = new dbJoutaEntities();
            //var tTravel_order = typeof(tTravel).GetProperty(order);
            //order_travel_list = (new dbJoutaEntities()).tTravel
            //        .AsEnumerable().OrderBy(a => tTravel_order.GetValue(a, null))
            //        .Select(a => a); //升冪

            int variable = Convert.ToInt32(target);
            tTravel the_target = db.tTravel.FirstOrDefault(o=>o.f旅遊文章編號== variable);

            the_target.f文章讚數 += 1;
            db.SaveChanges();
            return "1";
        }

        public ActionResult article_AJAX(string p)
        {
            IEnumerable<tTravel> order_travel_list;
            order_travel_list = from x in (new dbJoutaEntities()).tTravel
                                select x;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            CSelect obj = serializer.Deserialize<CSelect>(p);
            if (!string.IsNullOrEmpty(obj.order))
            {
                //不使用if，動態抓取排序條件
                var tTravel_order = typeof(tTravel).GetProperty(obj.order);
                order_travel_list = (new dbJoutaEntities()).tTravel
                        .AsEnumerable().OrderByDescending(a => tTravel_order.GetValue(a, null))
                        .Select(a => a); //升冪

                if (obj.border_color == "rgb(250, 224, 178)")
                {
                    order_travel_list = (new dbJoutaEntities()).tTravel
                        .AsEnumerable().OrderBy(a => tTravel_order.GetValue(a, null))
                        .Select(a => a); //降冪
                }

                if (!string.IsNullOrEmpty(obj.contain)) //搜尋欄位若非空
                {
                    order_travel_list = order_travel_list.Where(b => b.f文章標題.Contains(obj.contain))
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
        public ActionResult TravelIndex(tTravel p)
        {
            HttpPostedFileBase PicFile = Request.Files["PicFile"];
            if (Session["member"] != null&&p.f文章標題!=null)
            {
                if (PicFile != null)
                {
                    var NewFileName = Guid.NewGuid() + Path.GetExtension(PicFile.FileName);
                    var NewFilePath = Path.Combine(Server.MapPath("~/Content/images/"), NewFileName);
                    PicFile.SaveAs(NewFilePath);
                    p.f文章團圖 = NewFileName;
                }
                tMember Member = (tMember)Session["member"];
                p.f團主會員編號 = Member.f會員編號;
                dbJoutaEntities db = new dbJoutaEntities();
                db.tTravel.Add(p);
                db.SaveChanges();
            }
            return View();
        }
    }
}