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
        public ActionResult EatIndex(tEat p)
        {
            if (Session["member"] != null && p.f內容 != null)
            {
                if (p.fImgPath != null)
                {
                    var photoName = Guid.NewGuid() + Path.GetExtension(p.fImgPath.FileName);
                    var photoPath = Path.Combine(Server.MapPath("~/Content/images/"), photoName);
                    p.fImgPath.SaveAs(photoPath);
                    p.f團圖 = photoName;
                }
                tMember Member = (tMember)Session["member"];
                p.f帳號 = Member.f會員帳號;
                dbJoutaEntities db = new dbJoutaEntities();
                db.tEat.Add(p);
                db.SaveChanges();
            }
            return View();
        }

        public ActionResult eatArticleAjax(string p)
        {
            IEnumerable<tEat> order_eat_list;
            order_eat_list = from x in (new dbJoutaEntities()).tEat
                             select x;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            CSelect obj = serializer.Deserialize<CSelect>(p);
            if (!string.IsNullOrEmpty(obj.order))
            {
                var tEat_order = typeof(tEat).GetProperty(obj.order);
                order_eat_list = (new dbJoutaEntities()).tEat
                    .AsEnumerable().OrderByDescending(a => tEat_order.GetValue(a, null))
                    .Select(a => a);

                if (obj.background_color == "rgb(250,224,178)")
                {
                    order_eat_list = (new dbJoutaEntities()).tEat
                       .AsEnumerable().OrderBy(a => tEat_order.GetValue(a, null))
                       .Select(a => a);
                }

                if (!string.IsNullOrEmpty(obj.contain))
                {
                    order_eat_list = order_eat_list.Where(b => b.f開團標題.Contains(obj.contain))
                        .Select(a => a);
                }
            }
            return View(order_eat_list);
        }
    }
}