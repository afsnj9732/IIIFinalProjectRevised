using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace IIIProject_travel.Controllers
{
    public class 後台文章Controller : Controller
    {
        // GET: 後台文章
        public ActionResult List(string date起日, string dateStartFilter, string date迄日, string dateEndFiler, string sortOrder, string txt關鍵字, string currentFilter, int page = 1)
        {

            //搜尋
            var 文章 = from m in (new dbJoutaEntities()).tActivity
                     where m.f活動類型 == "文章"
                     select m;

            if (txt關鍵字 != null && date起日 != null && date迄日 != null)
            {
                page = 1;
            }
            else
            {
                txt關鍵字 = currentFilter;
                date起日 = dateStartFilter;
                date迄日 = dateEndFiler;
            }

            if (string.IsNullOrEmpty(txt關鍵字))
            {
                文章 = from m in (new dbJoutaEntities()).tActivity
                     where m.f活動類型 == "文章"
                     select m;
                if (!string.IsNullOrEmpty(date起日) && !string.IsNullOrEmpty(date迄日))
                    文章 = from p in 文章
                         where (string.Compare(p.f活動發起日期.Substring(0, 10), date起日) >= 0) &&
                               (string.Compare(p.f活動發起日期.Substring(0, 10), date迄日) <= 0)
                         select p;
            }

            else
            {
                文章 = from m in 文章
                     where m.f活動類型.Contains("文章") &&
                            (string.Compare(m.f活動發起日期.Substring(0, 10), date起日) >= 0) &&
                            (string.Compare(m.f活動發起日期.Substring(0, 10), date迄日) <= 0) &&
                           m.f活動編號.ToString().Contains(txt關鍵字) || m.f活動標題.Contains(txt關鍵字) ||
                           m.f活動標籤.Contains(txt關鍵字) || m.f活動內容.Contains(txt關鍵字) ||
                           m.f活動團圖.Contains(txt關鍵字) || m.f活動地區.Contains(txt關鍵字) ||
                           m.f活動地點.Contains(txt關鍵字) || m.f活動所屬.Contains(txt關鍵字) ||
                           m.f活動招募截止時間.Contains(txt關鍵字) || m.f活動分類.Contains(txt關鍵字) ||
                           m.f活動留言.Contains(txt關鍵字) || m.f活動留言時間.Contains(txt關鍵字) ||
                           m.f活動發起日期.Contains(txt關鍵字) || m.f活動結束時間.Contains(txt關鍵字) ||
                           m.f活動開始時間.Contains(txt關鍵字)
                     select m;
            }

            //排序 降冪和升冪
            ViewBag.當前搜尋 = txt關鍵字;
            ViewBag.當前起日 = date起日;
            ViewBag.當前迄日 = date迄日;
            ViewBag.標題排序 = string.IsNullOrEmpty(sortOrder) ? "標題降冪" : "";
            ViewBag.編號排序 = sortOrder == "編號升冪" ? "編號降冪" : "編號升冪";
            ViewBag.帳號排序 = sortOrder == "帳號升冪" ? "帳號降冪" : "帳號升冪";
            ViewBag.內容排序 = sortOrder == "內容升冪" ? "內容降冪" : "內容升冪";
            ViewBag.建立時間排序 = sortOrder == "建立時間升冪" ? "建立時間降冪" : "建立時間升冪";
            ViewBag.地區排序 = sortOrder == "地區升冪" ? "地區降冪" : "地區升冪";

            switch (sortOrder)
            {
                case "標題降冪":
                    文章 = 文章.OrderByDescending(s => s.f活動標題);
                    break;
                case "編號降冪":
                    文章 = 文章.OrderByDescending(s => s.f活動編號);
                    break;
                case "編號升冪":
                    文章 = 文章.OrderBy(s => s.f活動編號);
                    break;
                case "帳號降冪":
                    文章 = 文章.OrderByDescending(s => s.f會員編號);
                    break;
                case "帳號升冪":
                    文章 = 文章.OrderBy(s => s.f會員編號);
                    break;
                case "內容降冪":
                    文章 = 文章.OrderByDescending(s => s.f活動內容);
                    break;
                case "內容升冪":
                    文章 = 文章.OrderBy(s => s.f活動內容);
                    break;
                case "建立時間降冪":
                    文章 = 文章.OrderByDescending(s => s.f活動發起日期);
                    break;
                case "建立時間升冪":
                    文章 = 文章.OrderBy(s => s.f活動發起日期);
                    break;
                case "地區降冪":
                    文章 = 文章.OrderByDescending(s => s.f活動地區);
                    break;
                case "地區升冪":
                    文章 = 文章.OrderBy(s => s.f活動地區);
                    break;

                default:
                    文章 = 文章.OrderBy(s => s.f活動標題);
                    break;
            }



            //分頁
            int 筆數 = 20;
            int 當前頁面 = page < 1 ? 1 : page;
            var 結果 = 文章.ToPagedList(當前頁面, 筆數);

            return View(結果);
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
        public ActionResult v查看(int? id)
        {
            if (id == null)
                RedirectToAction("List");

            dbJoutaEntities db = new dbJoutaEntities();
            tActivity x = new tActivity();
            x = db.tActivity.FirstOrDefault(m => m.f活動編號 == id);

            return View(x);
        }


    }
}