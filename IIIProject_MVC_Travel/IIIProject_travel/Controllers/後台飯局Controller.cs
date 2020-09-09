using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace IIIProject_travel.Controllers
{
    public class 後台飯局Controller : Controller
    {
        // GET: 後台飯局
        public ActionResult List(string date起日, string date迄日, string sortOrder, string txt關鍵字, string currentFilter, int page = 1)
        {

            //搜尋
            //IQueryable<tActivity> 飯局 = null;
            var 飯局 = from m in (new dbJoutaEntities()).tActivity
                     where m.f活動類型 == "飯局"
                     select m;


            if (txt關鍵字 != null)
            {
                page = 1;
            }
            else
            {
                txt關鍵字 = currentFilter;
            }


            if (string.IsNullOrEmpty(txt關鍵字))
            {
                飯局 = from m in (new dbJoutaEntities()).tActivity
                     where m.f活動類型 == "飯局"
                     select m;
                if (!string.IsNullOrEmpty(date起日) && !string.IsNullOrEmpty(date迄日))
                飯局 = from p in 飯局
                     where (string.Compare(p.f活動發起日期, date起日) >= 0) && (string.Compare(p.f活動發起日期, date迄日) <= 0)
                     select p;
            }

            else
            {

                飯局 = from m in 飯局
                     where m.f活動類型.Contains("飯局") &&
                           (string.Compare(m.f活動發起日期, date起日) >= 0) && (string.Compare(m.f活動發起日期, date迄日) <= 0) &&
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
            ViewBag.標題排序 = string.IsNullOrEmpty(sortOrder) ? "標題降冪" : "";
            ViewBag.編號排序 = sortOrder == "編號升冪" ? "編號降冪" : "編號升冪";
            ViewBag.帳號排序 = sortOrder == "帳號升冪" ? "帳號降冪" : "帳號升冪";
            ViewBag.內容排序 = sortOrder == "內容升冪" ? "內容降冪" : "內容升冪";
            ViewBag.點讚排序 = sortOrder == "點讚升冪" ? "點讚降冪" : "點讚升冪";
            ViewBag.建立時間排序 = sortOrder == "建立時間升冪" ? "建立時間降冪" : "建立時間升冪";
            ViewBag.招募截止時間排序 = sortOrder == "招募截止時間升冪" ? "招募截止時間降冪" : "招募截止時間升冪";
            ViewBag.開始時間排序 = sortOrder == "開始時間升冪" ? "開始時間降冪" : "開始時間升冪";
            ViewBag.結束時間排序 = sortOrder == "結束時間升冪" ? "結束時間降冪" : "結束時間升冪";
            ViewBag.地區排序 = sortOrder == "地區升冪" ? "地區降冪" : "地區升冪";
            ViewBag.地點排序 = sortOrder == "地點升冪" ? "地點降冪" : "地點升冪";
            ViewBag.預算排序 = sortOrder == "預算升冪" ? "預算降冪" : "預算升冪";
            ViewBag.分類排序 = sortOrder == "分類升冪" ? "分類降冪" : "分類升冪";

            switch (sortOrder)
            {
                case "標題降冪":
                    飯局 = 飯局.OrderByDescending(s => s.f活動標題);
                    break;
                case "編號降冪":
                    飯局 = 飯局.OrderByDescending(s => s.f活動編號);
                    break;
                case "編號升冪":
                    飯局 = 飯局.OrderBy(s => s.f活動編號);
                    break;
                case "帳號降冪":
                    飯局 = 飯局.OrderByDescending(s => s.f會員編號);
                    break;
                case "帳號升冪":
                    飯局 = 飯局.OrderBy(s => s.f會員編號);
                    break;
                case "內容降冪":
                    飯局 = 飯局.OrderByDescending(s => s.f活動內容);
                    break;
                case "內容升冪":
                    飯局 = 飯局.OrderBy(s => s.f活動內容);
                    break;
                case "點讚降冪":
                    飯局 = 飯局.OrderByDescending(s => s.f活動讚數);
                    break;
                case "點讚升冪":
                    飯局 = 飯局.OrderBy(s => s.f活動讚數);
                    break;
                case "建立時間降冪":
                    飯局 = 飯局.OrderByDescending(s => s.f活動發起日期);
                    break;
                case "建立時間升冪":
                    飯局 = 飯局.OrderBy(s => s.f活動發起日期);
                    break;
                case "招募截止時間降冪":
                    飯局 = 飯局.OrderByDescending(s => s.f活動招募截止時間);
                    break;
                case "招募截止時間升冪":
                    飯局 = 飯局.OrderBy(s => s.f活動招募截止時間);
                    break;
                case "開始時間降冪":
                    飯局 = 飯局.OrderByDescending(s => s.f活動開始時間);
                    break;
                case "開始時間升冪":
                    飯局 = 飯局.OrderBy(s => s.f活動開始時間);
                    break;
                case "結束時間降冪":
                    飯局 = 飯局.OrderByDescending(s => s.f活動結束時間);
                    break;
                case "結束時間升冪":
                    飯局 = 飯局.OrderBy(s => s.f活動結束時間);
                    break;
                case "地區降冪":
                    飯局 = 飯局.OrderByDescending(s => s.f活動地區);
                    break;
                case "地區升冪":
                    飯局 = 飯局.OrderBy(s => s.f活動地區);
                    break;
                case "地點降冪":
                    飯局 = 飯局.OrderByDescending(s => s.f活動地點);
                    break;
                case "地點升冪":
                    飯局 = 飯局.OrderBy(s => s.f活動地點);
                    break;
                case "預算降冪":
                    飯局 = 飯局.OrderByDescending(s => s.f活動預算);
                    break;
                case "預算升冪":
                    飯局 = 飯局.OrderBy(s => s.f活動預算);
                    break;
                case "分類降冪":
                    飯局 = 飯局.OrderByDescending(s => s.f活動分類);
                    break;
                case "分類升冪":
                    飯局 = 飯局.OrderBy(s => s.f活動分類);
                    break;

                default:
                    飯局 = 飯局.OrderBy(s => s.f活動標題);
                    break;
            }



            //分頁
            int 筆數 = 20;
            int 當前頁面 = page < 1 ? 1 : page;
            var 結果 = 飯局.ToPagedList(當前頁面, 筆數);

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