using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IIIProject_travel.Controllers
{
    public class 後台會員Controller : Controller
    {
        // GET: 後台會員
        public ActionResult List()
        {
            var 會員 = from t in (new dbJoutaEntities()).tMember
                      select t;
            return View(會員);

        }

        public ActionResult d刪除(int? id)
        {
            if (id == null)
                RedirectToAction("List");

            tMember x = new tMember();
            dbJoutaEntities db = new dbJoutaEntities();
            x = db.tMember.FirstOrDefault(m => m.f會員編號 == id);
            db.tMember.Remove(x);
            db.SaveChanges();

            return RedirectToAction("List");

        }
        public ActionResult e修改(int? id)
        {
            if (id == null)
                RedirectToAction("List");

            dbJoutaEntities db = new dbJoutaEntities();
            tMember x = new tMember();
            x = db.tMember.FirstOrDefault(m => m.f會員編號 == id);

            return View(x);

        }
        [HttpPost]
        public ActionResult e修改(tMember p)
        {
            dbJoutaEntities db = new dbJoutaEntities();
            tMember A = db.tMember.FirstOrDefault(m => m.f會員編號 == p.f會員編號);
            if (A != null)
            {
                A.f會員名稱 = p.f會員名稱;
                A.f會員評分 = p.f會員評分;
                A.f會員稱號 = p.f會員稱號;
                A.f會員大頭貼 = p.f會員大頭貼;
                A.f會員帳號 = p.f會員帳號;
                A.f會員密碼 = p.f會員密碼;
                A.f電子郵件 = p.f電子郵件;
                A.f手機 = p.f手機;
                A.f電話 = p.f電話;
                A.f生日 = p.f生日;
                A.f自我介紹 = p.f自我介紹;
                A.f暱稱 = p.f暱稱;
                A.f英文名字 = p.f英文名字;
                A.f性別 = p.f性別;
                A.f興趣 = p.f興趣;

                db.SaveChanges();
            }

            return RedirectToAction("List");

        }


    }
}