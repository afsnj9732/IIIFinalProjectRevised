using IIIProject_travel.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace IIIProject_travel.Controllers
{
    public class 聯絡我們Controller : Controller
    {
        //GET: 聯絡我們
        public ActionResult List(string date起日, string dateStartFilter, string date迄日, string dateEndFiler, string sortOrder, string txt關鍵字, string currentFilter, int page = 1)
        {
            var 意見 = from m in (new dbJoutaEntities()).tComment
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
                意見 = from m in (new dbJoutaEntities()).tComment
                     select m;
                if (!string.IsNullOrEmpty(date起日) && !string.IsNullOrEmpty(date迄日))
                    意見 = from p in 意見
                         where (string.Compare(p.f意見時間.Substring(0, 10), date起日) >= 0) &&
                               (string.Compare(p.f意見時間.Substring(0, 10), date迄日) <= 0)
                         select p;
            }

            else
            {
                意見 = from m in 意見
                     where (string.Compare(m.f意見時間.Substring(0, 10), date起日) >= 0) &&
                            (string.Compare(m.f意見時間.Substring(0, 10), date迄日) <= 0) &&
                           m.fID.ToString().Contains(txt關鍵字) || m.f名稱.Contains(txt關鍵字) ||
                           m.f性別.Contains(txt關鍵字) || m.f意見.Contains(txt關鍵字) ||
                           m.f意見類型.Contains(txt關鍵字) || m.f聯絡人.Contains(txt關鍵字) ||
                           m.f電子郵件.Contains(txt關鍵字) || m.f電話.Contains(txt關鍵字)
                     select m;
            }

            //排序 降冪和升冪
            ViewBag.當前搜尋 = txt關鍵字;
            ViewBag.當前起日 = date起日;
            ViewBag.當前迄日 = date迄日;
            ViewBag.名稱排序 = string.IsNullOrEmpty(sortOrder) ? "名稱降冪" : "";
            ViewBag.編號排序 = sortOrder == "編號升冪" ? "編號降冪" : "編號升冪";
            ViewBag.聯絡人排序 = sortOrder == "聯絡人升冪" ? "聯絡人降冪" : "聯絡人升冪";
            ViewBag.內容排序 = sortOrder == "內容升冪" ? "內容降冪" : "內容升冪";
            ViewBag.建立時間排序 = sortOrder == "建立時間升冪" ? "建立時間降冪" : "建立時間升冪";
            ViewBag.分類排序 = sortOrder == "分類升冪" ? "分類降冪" : "分類升冪";
            ViewBag.性別排序 = sortOrder == "性別升冪" ? "性別降冪" : "性別升冪";
            ViewBag.電子郵件排序 = sortOrder == "電子郵件升冪" ? "電子郵件降冪" : "電子郵件升冪";
            ViewBag.電話排序 = sortOrder == "電話升冪" ? "電話降冪" : "電話升冪";


            switch (sortOrder)
            {
                case "名稱降冪":
                    意見 = 意見.OrderByDescending(s => s.f名稱);
                    break;
                case "編號降冪":
                    意見 = 意見.OrderByDescending(s => s.fID);
                    break;
                case "編號升冪":
                    意見 = 意見.OrderBy(s => s.fID);
                    break;
                case "聯絡人降冪":
                    意見 = 意見.OrderByDescending(s => s.f聯絡人);
                    break;
                case "聯絡人升冪":
                    意見 = 意見.OrderBy(s => s.f聯絡人);
                    break;
                case "內容降冪":
                    意見 = 意見.OrderByDescending(s => s.f意見);
                    break;
                case "內容升冪":
                    意見 = 意見.OrderBy(s => s.f意見);
                    break;
                case "建立時間降冪":
                    意見 = 意見.OrderByDescending(s => s.f意見時間);
                    break;
                case "建立時間升冪":
                    意見 = 意見.OrderBy(s => s.f意見時間);
                    break;
                case "分類降冪":
                    意見 = 意見.OrderByDescending(s => s.f意見類型);
                    break;
                case "分類升冪":
                    意見 = 意見.OrderBy(s => s.f意見類型);
                    break;
                case "性別降冪":
                    意見 = 意見.OrderByDescending(s => s.f性別);
                    break;
                case "性別升冪":
                    意見 = 意見.OrderBy(s => s.f性別);
                    break;
                case "電子郵件降冪":
                    意見 = 意見.OrderByDescending(s => s.f電子郵件);
                    break;
                case "電子郵件升冪":
                    意見 = 意見.OrderBy(s => s.f電子郵件);
                    break;
                case "電話降冪":
                    意見 = 意見.OrderByDescending(s => s.f電話);
                    break;
                case "電話升冪":
                    意見 = 意見.OrderBy(s => s.f電話);
                    break;

                default:
                    意見 = 意見.OrderBy(s => s.f名稱);
                    break;
            }


            int 筆數 = 20;
            int 當前頁面 = page < 1 ? 1 : page;
            var 結果 = 意見.ToPagedList(當前頁面, 筆數);

            return View(結果);

        }
        public ActionResult d刪除(int? id)
        {
            if (id == null)
                RedirectToAction("List");

            tComment x = new tComment();
            dbJoutaEntities db = new dbJoutaEntities();
            x = db.tComment.FirstOrDefault(m => m.fID == id);
            db.tComment.Remove(x);
            db.SaveChanges();

            return RedirectToAction("List");

        }
        public ActionResult r回覆(int? id)
        {
            if (id == null)
                RedirectToAction("List");

            dbJoutaEntities db = new dbJoutaEntities();
            tComment x = new tComment();
            x = db.tComment.FirstOrDefault(m => m.fID == id);

            return View(x);
        }



        public ActionResult New()
        {
            return View();
        }
        public ActionResult Save()
        {
            dbJoutaEntities db = new dbJoutaEntities();
            tComment x = new tComment();
            x.f名稱 = Request.Form["txt名稱"];
            x.f意見 = Request.Form["txt意見"];
            x.f性別 = Request.Form["gender"];
            x.f意見類型 = Request.Form["txt意見類型"];
            x.f聯絡人 = Request.Form["txt聯絡人"];
            x.f電子郵件 = Request.Form["txtMail"];
            x.f電話 = Request.Form["txt電話"];

            db.tComment.Add(x);
            db.SaveChanges();
            return RedirectToAction("New");
        }

        //[HttpPost]
        //public ActionResult New( p)
        //{
        //    tMember xx = new tMember();
        //    xx.f會員名稱 = Request.Form[txt名稱];
        //    xx.f會員性別 = p.gender;
        //    xx.f會員電子郵件 = p.txt電子郵件;
        //    xx.f會員電話 = p.txt電話;
        //    tActivity yy = new tActivity();
        //    yy.f活動內容 = p.txt意見;
        //    yy.f活動類型 = "意見";

        //    dbJoutaEntities db = new dbJoutaEntities();
        //    db.tActivity.Add(yy);
        //    db.tMember.Add(xx);
        //    ViewBag.msg = "成功";
        //    db.SaveChanges();
        //return RedirectToAction("New");
        //}

    }
}