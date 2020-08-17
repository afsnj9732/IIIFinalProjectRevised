using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
                //tMember Member=
            }
            return View();
        }
    }
}