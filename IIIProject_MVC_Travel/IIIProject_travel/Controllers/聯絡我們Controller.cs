using IIIProject_travel.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Net.Mail;

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
                           m.fID.ToString().Contains(txt關鍵字) || m.f標題.Contains(txt關鍵字) ||
                           m.f性別.Contains(txt關鍵字) || m.f意見.Contains(txt關鍵字) ||
                           m.f意見類型.Contains(txt關鍵字) || m.f聯絡人.Contains(txt關鍵字) ||
                           m.f電子郵件.Contains(txt關鍵字) || m.f電話.Contains(txt關鍵字)||
                           m.f意見狀態.Contains(txt關鍵字)
                     select m;
            }

            //排序 降冪和升冪
            ViewBag.當前搜尋 = txt關鍵字;
            ViewBag.當前起日 = date起日;
            ViewBag.當前迄日 = date迄日;
            ViewBag.名稱排序 = string.IsNullOrEmpty(sortOrder) ? "標題降冪" : "";
            ViewBag.編號排序 = sortOrder == "編號升冪" ? "編號降冪" : "編號升冪";
            ViewBag.聯絡人排序 = sortOrder == "聯絡人升冪" ? "聯絡人降冪" : "聯絡人升冪";
            ViewBag.內容排序 = sortOrder == "內容升冪" ? "內容降冪" : "內容升冪";
            ViewBag.建立時間排序 = sortOrder == "建立時間升冪" ? "建立時間降冪" : "建立時間升冪";
            ViewBag.分類排序 = sortOrder == "分類升冪" ? "分類降冪" : "分類升冪";
            ViewBag.性別排序 = sortOrder == "性別升冪" ? "性別降冪" : "性別升冪";
            ViewBag.電子郵件排序 = sortOrder == "電子郵件升冪" ? "電子郵件降冪" : "電子郵件升冪";
            ViewBag.電話排序 = sortOrder == "電話升冪" ? "電話降冪" : "電話升冪";
            ViewBag.狀態排序 = sortOrder == "狀態升冪" ? "狀態降冪" : "狀態升冪";


            switch (sortOrder)
            {
                case "標題降冪":
                    意見 = 意見.OrderByDescending(s => s.f標題);
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
                case "狀態降冪":
                    意見 = 意見.OrderByDescending(s => s.f意見狀態);
                    break;
                case "狀態升冪":
                    意見 = 意見.OrderBy(s => s.f意見狀態);
                    break;


                default:
                    意見 = 意見.OrderBy(s => s.f標題);
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
        [HttpPost]
        public ActionResult r回覆(C回覆 y)
        {
            send(y);
            ViewBag.kk = "傳送成功";
            return RedirectToAction("List");
        }


        [NonAction]
        public void send(C回覆 y)
        {
            //Jouta官方帳號
            string gmail_account = "Joutagroup445@gmail.com";
            string gmail_password = "admin123admin";
            string gmail_mail = "Joutagroup445@gmail.com";     
            var fromEmail = new MailAddress(gmail_mail, "Jouta服務團隊");
            var toEmail = new MailAddress(y.f電子郵件);
            string body = "你好"+y.f聯絡人+y.f性別+"已收到您的意見" + "<br/>"+
                          "您對"+y.f標題+ y.f意見+"的問題"+ "<br/>"+
                          "在此向您說明"+y.f回覆+"<br/>"+
                          "如有任何問題，歡迎隨時與我們聯繫，謝謝。";

            SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
            smtpServer.Port = 587;
            smtpServer.Credentials = new System.Net.NetworkCredential(gmail_account, gmail_password);
            //開啟SSL
            smtpServer.EnableSsl = true;

            MailMessage mail = new MailMessage();
            //設定來源信箱
            mail.From = new MailAddress(gmail_mail);
            //設定收信者信箱
            mail.To.Add(toEmail);
            //主旨
            mail.Subject = "Jouta意見回覆";
            //內容
            mail.Body = body;
            //設定信箱內容為HTML格式
            mail.IsBodyHtml = true;
            smtpServer.Send(mail);
        }




        public ActionResult New()
        {
            return View();
        }
        public ActionResult Save()
        {
            dbJoutaEntities db = new dbJoutaEntities();
            tComment x = new tComment();
            x.f標題 = Request.Form["txt名稱"];
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


    }
}