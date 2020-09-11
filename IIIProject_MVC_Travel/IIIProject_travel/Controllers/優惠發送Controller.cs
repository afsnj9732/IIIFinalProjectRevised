using IIIProject_travel.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace IIIProject_travel.Controllers
{
    public class 優惠發送Controller : Controller
    {
        // GET: 優惠發送
        public ActionResult List()
        {
            return View();
        }

        [HttpPost]
        public ActionResult List(string randLocation)
        {
            dbJoutaEntities db = new dbJoutaEntities();

            var x = (from t in db.tMember
                     where t.f會員評分 >= 4
                     select new
                     {
                         mMemberNum = t.f會員編號,
                         mAccount = t.f會員帳號,
                         mName = t.f會員名稱,
                         mRating = t.f會員評分
                     })
                .OrderBy(t => Guid.NewGuid()).Take(1);

            return Json(x, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        public void Save(string txtCouponInfo, int memberId1)
        {

            dbJoutaEntities db = new dbJoutaEntities();
            var x = db.tMember.Where(a => a.f會員編號 == memberId1).FirstOrDefault();
            
            string randCode = Guid.NewGuid().ToString();

            //Jouta官方帳號
            string gmail_account = "Joutagroup445@gmail.com";
            string gmail_password = "admin123admin";
            string gmail_mail = "Joutagroup445@gmail.com";     //gmail信箱

            var verifyUrl = "/Home/ResetPassword/" + randCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress(gmail_mail, "Jouta服務團隊");
            var toEmail = new MailAddress(x.f會員電子郵件);
            string body = "您好，<br/><br/>恭喜您抽到" +txtCouponInfo+
            "<br/><br/>"+ randCode + "，使用期限至9/30，請盡速使用!</a>";

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
            mail.Subject = "重置密碼確認信";
            //內容
            mail.Body = body;
            //設定信箱內容為HTML格式
            mail.IsBodyHtml = true;
            smtpServer.Send(mail);
        }
    }
}