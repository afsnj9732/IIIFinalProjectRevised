using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IIIProject_travel.Controllers
{
    public class 後台飯局Controller : Controller
    {
        // GET: 後台飯局
        public ActionResult List(string sortOrder, string txt關鍵字, string currentFilter, int page = 1)
        {
            //搜尋
            //IQueryable<tActivity> 飯局 = null;

            //if (txt關鍵字 != null)
            //{
            //    page = 1;
            //}
            //else
            //{
            //    txt關鍵字 = currentFilter;
            //}

            //if (string.IsNullOrEmpty(txt關鍵字))
            //    飯局 = from m in (new dbJoutaEntities()).tActivity
            //         select m;
            //else
            //    飯局 = from m in (new dbJoutaEntities()).tMember
            //         where m.f.Contains(txt關鍵字) || m.f會員評分.ToString().Contains(txt關鍵字) ||
            //               m.f會員稱號.Contains(txt關鍵字) || m.f會員帳號.Contains(txt關鍵字) ||
            //               m.f會員密碼.Contains(txt關鍵字) || m.f會員電子郵件.Contains(txt關鍵字) ||
            //               m.f會員手機.Contains(txt關鍵字) || m.f會員電話.Contains(txt關鍵字) ||
            //               m.f會員生日.Contains(txt關鍵字) || m.f會員自我介紹.Contains(txt關鍵字) ||
            //               m.f會員暱稱.Contains(txt關鍵字) || m.f會員編號.ToString().Contains(txt關鍵字) ||
            //               m.f會員性別.Contains(txt關鍵字) || m.f會員興趣.Contains(txt關鍵字)
            //         select m;


            var 飯局 = from t in (new dbJoutaEntities()).tActivity
                     where t.f活動類型 == "飯局"
                     select t;
            return View(飯局);
        }

        public ActionResult d刪除(int? id)
        {
            if (id == null)
                RedirectToAction("List");

            tActivity x = new tActivity();
            dbJoutaEntities db = new dbJoutaEntities();
            x = db.tActivity.FirstOrDefault(m => m.f活動編號 == id);
            db.tActivity.Remove(x);
            db.SaveChanges();

            return RedirectToAction("List");

        }
        public ActionResult v查看()
        {

            return View();

        }
    }
}