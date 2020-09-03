using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using IIIProject_travel.Models;



namespace IIIProject_travel.Controllers
{
    public class 後台會員Controller : Controller
    {

        // GET: 後台會員
        public ActionResult List(string sortOrder,int page = 1 )
        {
            dbJoutaEntities db = new dbJoutaEntities();

            //搜尋
            List<tMember> 會員 = null;
            
            string k關鍵字 = Request.Form["txt關鍵字"];

            if (string.IsNullOrEmpty(k關鍵字))
                會員 = db.tMember.OrderBy(m => m.f會員編號).ToList();
            else
                會員 = 會員.Where(
                    m => m.f會員名稱.Contains(k關鍵字) || m.f會員評分.ToString().Contains(k關鍵字) ||
                         m.f會員稱號.Contains(k關鍵字) || m.f會員帳號.Contains(k關鍵字) || 
                         m.f會員密碼.Contains(k關鍵字) || m.f會員電子郵件.Contains(k關鍵字) ||
                         m.f會員手機.Contains(k關鍵字) || m.f會員電話.Contains(k關鍵字) || 
                         m.f會員生日.Contains(k關鍵字) || m.f會員自我介紹.Contains(k關鍵字) ||
                         m.f會員暱稱.Contains(k關鍵字) || m.f會員英文姓名.Contains(k關鍵字)||
                         m.f會員性別.Contains(k關鍵字) || m.f會員興趣.Contains(k關鍵字)||
                         m.f會員編號.ToString().Contains(k關鍵字))
                    .ToList();
            //排序
            ViewBag.名稱排序 = string.IsNullOrEmpty(sortOrder) ? "名稱描述" : "";
            ViewBag.電子郵件排序 = sortOrder == "郵件" ? "郵件描述" : "郵件";
            ViewBag.手機排序 = sortOrder == "手機" ? "手機描述" : "手機";
            var 學生 = from s in 會員
                     select s;
            switch (sortOrder)
            {
                case "名稱描述":
                    學生 = 學生.OrderByDescending(s => s.f會員名稱);
                    break;
                default:
                    學生 = 學生.OrderBy(s => s.f會員名稱);
                    break;
                case "郵件描述":
                    學生 = 學生.OrderByDescending(s => s.f會員電子郵件);
                    break;
                case "郵件":
                    學生 = 學生.OrderBy(s => s.f會員電子郵件);
                    break;
                case "手機描述":
                    學生 = 學生.OrderByDescending(s => s.f會員手機);
                    break;
                case "手機":
                    學生 = 學生.OrderBy(s => s.f會員手機);
                    break;
            }

            //分頁
            int 筆數 = 20;
            int 當前頁面 = page < 1 ? 1 : page;

            var 結果 = 學生.ToPagedList(當前頁面, 筆數);
            return View(結果);
        }
       

        public ActionResult Index(string sortOrder)
        {
            dbJoutaEntities db = new dbJoutaEntities();
            ViewBag.名稱排序 = string.IsNullOrEmpty(sortOrder) ? "名稱描述" : "";
            ViewBag.電子郵件排序 = sortOrder == "郵件" ? "郵件描述" : "郵件";
            ViewBag.手機排序 = sortOrder == "手機" ? "手機描述" : "手機";

            var 學生 = from s in db.tMember
                     select s;
            switch (sortOrder)
            {
                case "名稱描述":
                    學生 = 學生.OrderByDescending(s => s.f會員名稱);
                    break;
                default:
                    學生 = 學生.OrderBy(s => s.f會員名稱);
                    break;
                case "郵件描述":
                    學生 = 學生.OrderByDescending(s => s.f會員電子郵件);
                    break;
                case "郵件":
                    學生 = 學生.OrderBy(s => s.f會員電子郵件);
                    break;
                case "手機描述":
                    學生 = 學生.OrderByDescending(s => s.f會員手機);
                    break;
                case "手機":
                    學生 = 學生.OrderBy(s => s.f會員手機);
                    break;

            }
            return View(學生.ToList());
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