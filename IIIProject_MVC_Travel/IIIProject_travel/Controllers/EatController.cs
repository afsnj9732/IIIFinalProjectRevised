using IIIProject_travel.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IIIProject_travel.Controllers
{
    public class EatController : Controller
    {
        // GET: Eat
        public ActionResult EatIndex()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EatIndex(tActivity p)
        {
            if (Session["member"] != null && p.f活動標題 != null)
            {
                HttpPostedFileBase PicFile = Request.Files["PicFile"];
                if (PicFile != null)
                {
                    var photoName = Guid.NewGuid() + Path.GetExtension(PicFile.FileName);
                    var photoPath = Path.Combine(Server.MapPath("~/Content/images/"), photoName);
                    PicFile.SaveAs(photoPath);
                    p.f活動團圖 = photoName;
                }
                tMember Member = (tMember)Session["member"];
                p.f會員編號 = Member.f會員編號;
                dbJoutaEntities db = new dbJoutaEntities();
                db.tActivity.Add(p);
                db.SaveChanges();
            }
            return View();
        }

        public ActionResult eatArticleAjax(string p)
        {
            IEnumerable<tActivity> order_eat_list;
            order_eat_list = from x in (new dbJoutaEntities()).tActivity
                             select x;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            CSelect obj = serializer.Deserialize<CSelect>(p);
            if (!string.IsNullOrEmpty(obj.order))
            {
                var tEat_order = typeof(tActivity).GetProperty(obj.order);
                order_eat_list = (new dbJoutaEntities()).tActivity
                    .AsEnumerable().OrderByDescending(a => tEat_order.GetValue(a, null))
                    .Select(a => a);

                if (obj.background_color == "rgb(250,224,178)")
                {
                    order_eat_list = (new dbJoutaEntities()).tActivity
                       .AsEnumerable().OrderBy(a => tEat_order.GetValue(a, null))
                       .Select(a => a);
                }

                if (!string.IsNullOrEmpty(obj.contain))
                {
                    order_eat_list = order_eat_list.Where(b => b.f活動標題.Contains(obj.contain))
                        .Select(a => a);
                }
            }
            return View(order_eat_list);
        }
    }
}