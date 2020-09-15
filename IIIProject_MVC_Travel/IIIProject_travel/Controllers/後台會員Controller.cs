using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;



namespace IIIProject_travel.Controllers
{
    public class 後台會員Controller : Controller
    {

        // GET: 後台會員
        public ActionResult List(string sortOrder,string txt關鍵字, string currentFilter, int page = 1 )
         {
            //搜尋
            IQueryable<tMember> 會員 = null;

            if (txt關鍵字 != null)
            {
                page = 1;
            }
            else
            {
                txt關鍵字 = currentFilter;
            }

            if (string.IsNullOrEmpty(txt關鍵字))
                會員 = from m in (new dbJoutaEntities()).tMember
                     select m;
            else
                會員 = from m in (new dbJoutaEntities()).tMember
                     where m.f會員名稱.Contains(txt關鍵字) || m.f會員評分.ToString().Contains(txt關鍵字)||
                           m.f會員稱號.Contains(txt關鍵字) || m.f會員帳號.Contains(txt關鍵字) ||
                           m.f會員密碼.Contains(txt關鍵字) || m.f會員電子郵件.Contains(txt關鍵字) ||
                           m.f會員手機.Contains(txt關鍵字) || m.f會員電話.Contains(txt關鍵字) ||
                           m.f會員生日.Contains(txt關鍵字) || m.f會員自我介紹.Contains(txt關鍵字) ||
                           m.f會員暱稱.Contains(txt關鍵字) || m.f會員編號.ToString().Contains(txt關鍵字)||
                           m.f會員性別.Contains(txt關鍵字) || m.f會員興趣.Contains(txt關鍵字) 
                           select m;

            //排序 降冪和升冪
            // string str =  True ? "A" : "B"
            // string str =  False ? "A" : "B"

            ViewBag.當前搜尋 = txt關鍵字;
            ViewBag.名稱排序 = string.IsNullOrEmpty(sortOrder) ? "名稱降冪" : "";
            ViewBag.編號排序 = sortOrder == "編號升冪" ? "編號降冪" : "編號升冪";
            ViewBag.評分排序 = sortOrder == "評分升冪" ? "評分降冪" : "評分升冪";
            ViewBag.稱號排序 = sortOrder == "稱號升冪" ? "稱號降冪" : "稱號升冪";
            ViewBag.帳號排序 = sortOrder == "帳號升冪" ? "帳號降冪" : "帳號升冪";
            ViewBag.排序 = sortOrder == "評分升冪" ? "評分降冪" : "評分升冪";
            ViewBag.電子郵件排序 = sortOrder == "郵件升冪" ? "郵件降冪" : "郵件升冪";
            ViewBag.手機排序 = sortOrder == "手機升冪" ? "手機降冪" : "手機升冪";
            ViewBag.電話排序 = sortOrder == "電話升冪" ? "電話降冪" : "電話升冪";
            ViewBag.生日排序 = sortOrder == "生日升冪" ? "生日降冪" : "生日升冪";
            ViewBag.暱稱排序 = sortOrder == "暱稱升冪" ? "暱稱降冪" : "暱稱升冪";
            ViewBag.性別排序 = sortOrder == "性別升冪" ? "性別降冪" : "性別升冪";
            ViewBag.興趣排序 = sortOrder == "興趣升冪" ? "興趣降冪" : "興趣升冪";

            switch (sortOrder)
            {
                case "名稱降冪":
                    會員 = 會員.OrderByDescending(s => s.f會員名稱);
                    break;
                case "編號降冪":
                    會員 = 會員.OrderByDescending(s => s.f會員編號);
                    break;
                case "編號升冪":
                    會員 = 會員.OrderBy(s => s.f會員編號);
                    break;
                case "評分降冪":
                    會員 = 會員.OrderByDescending(s => s.f會員評分);
                    break;
                case "評分升冪":
                    會員 = 會員.OrderBy(s => s.f會員評分);
                    break;
                case "稱號降冪":
                    會員 = 會員.OrderByDescending(s => s.f會員稱號);
                    break;
                case "稱號升冪":
                    會員 = 會員.OrderBy(s => s.f會員稱號);
                    break;
                case "帳號降冪":
                    會員 = 會員.OrderByDescending(s => s.f會員帳號);
                    break;
                case "帳號升冪":
                    會員 = 會員.OrderBy(s => s.f會員帳號);
                    break;
                case "郵件降冪":
                    會員 = 會員.OrderByDescending(s => s.f會員電子郵件);
                    break;
                case "郵件升冪":
                    會員 = 會員.OrderBy(s => s.f會員電子郵件);
                    break;
                case "手機降冪":
                    會員 = 會員.OrderByDescending(s => s.f會員手機);
                    break;
                case "手機升冪":
                    會員 = 會員.OrderBy(s => s.f會員手機);
                    break;
                case "電話降冪":
                    會員 = 會員.OrderByDescending(s => s.f會員電話);
                    break;
                case "電話升冪":
                    會員 = 會員.OrderBy(s => s.f會員電話);
                    break;
                case "生日降冪":
                    會員 = 會員.OrderByDescending(s => s.f會員生日);
                    break;
                case "生日升冪":
                    會員 = 會員.OrderBy(s => s.f會員生日);
                    break;
                case "暱稱降冪":
                    會員 = 會員.OrderByDescending(s => s.f會員暱稱);
                    break;
                case "暱稱升冪":
                    會員 = 會員.OrderBy(s => s.f會員暱稱);
                    break;
                case "性別降冪":
                    會員 = 會員.OrderByDescending(s => s.f會員性別);
                    break;
                case "性別升冪":
                    會員 = 會員.OrderBy(s => s.f會員性別);
                    break;
                case "興趣降冪":
                    會員 = 會員.OrderByDescending(s => s.f會員興趣);
                    break;
                case "興趣升冪":
                    會員 = 會員.OrderBy(s => s.f會員興趣);
                    break;

                default:
                    會員 = 會員.OrderBy(s => s.f會員名稱);
                    break;
            }

            //分頁
            int 筆數 = 7;
            int 當前頁面 = page < 1 ? 1 : page;
            var 結果 = 會員.ToPagedList(當前頁面, 筆數);
            

            return View(結果);
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
                A.f會員性別 = p.f會員性別;
                A.f會員興趣 = p.f會員興趣;
                db.SaveChanges();
            }

            return RedirectToAction("List");

        }
    }
}