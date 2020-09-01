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
        public ActionResult List(int 頁 = 1)
        {
            int pagesize = 3;
            int pagecurrent = 頁 < 1 ? 1 : 頁;
            IQueryable<tMember> 會員 = null;
            string k關鍵字 = Request.Form["txt關鍵字"];
            if (string.IsNullOrEmpty(k關鍵字))
            {
                會員 = from p in (new dbJoutaEntities()).tMember
                     select p;
            }
            else
            {
                會員 = from p in (new dbJoutaEntities()).tMember
                     where p.f會員名稱.Contains(k關鍵字)
                     select p;
            }
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
                A.f會員電子郵件 = p.f會員電子郵件;
                A.f會員手機 = p.f會員手機;
                A.f會員電話 = p.f會員電話;
                A.f會員生日 = p.f會員生日;
                A.f會員自我介紹 = p.f會員自我介紹;
                A.f會員暱稱 = p.f會員暱稱;
                A.f會員英文姓名 = p.f會員英文姓名;
                A.f會員性別 = p.f會員性別;
                A.f會員興趣 = p.f會員興趣;

                db.SaveChanges();
            }

            return RedirectToAction("List");

        }


    }
}