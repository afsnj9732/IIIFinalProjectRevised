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
                           m.f會員暱稱.Contains(txt關鍵字) || m.f會員英文姓名.Contains(txt關鍵字) ||
                           m.f會員性別.Contains(txt關鍵字) || m.f會員興趣.Contains(txt關鍵字) ||
                           m.f會員編號.ToString().Contains(txt關鍵字)
                           select m;

            //排序 降冪和升冪
            // string str =  True ? "A" : "B"
            // string str =  False ? "A" : "B"

            ViewBag.當前文件 = txt關鍵字;
            ViewBag.名稱排序 = string.IsNullOrEmpty(sortOrder) ? "名稱降冪" : "";
            ViewBag.電子郵件排序 = sortOrder == "郵件升冪" ? "郵件降冪" : "郵件升冪";
            ViewBag.手機排序 = sortOrder == "手機升冪" ? "手機降冪" : "手機升冪";
            switch (sortOrder)
            {
                case "名稱降冪":
                    會員 = 會員.OrderByDescending(s => s.f會員名稱);
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
                default:
                    會員 = 會員.OrderBy(s => s.f會員名稱);
                    break;
            }

            //分頁
            int 筆數 = 20;
            int 當前頁面 = page < 1 ? 1 : page;
            var 結果 = 會員.ToPagedList(當前頁面, 筆數);
            
            return View(結果);
        }

       

        public ActionResult Index(string sortOrder, string searchString)
        {
            dbJoutaEntities db = new dbJoutaEntities();
            ViewBag.名稱排序 = string.IsNullOrEmpty(sortOrder) ? "名稱描述" : "";
            ViewBag.電子郵件排序 = sortOrder == "郵件" ? "郵件描述" : "郵件";
            ViewBag.手機排序 = sortOrder == "手機" ? "手機描述" : "手機";

            var 學生 = from s in db.tMember
                     select s;

            if (!string.IsNullOrEmpty(searchString))
            {
                學生 = 學生.Where(s => s.f會員名稱.Contains(searchString)
                                        || s.f會員帳號.Contains(searchString)
                            );
            }
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