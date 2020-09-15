using IIIProject_travel.Models;
using IIIProject_travel.Services;
using IIIProject_travel.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.Design;

namespace IIIProject_travel.Controllers
{
    public class HomeController : Controller
    {
        //宣告service物件
        private readonly MembersService membersService = new MembersService();
        //宣告寄信用service物件
        private readonly MailService mailService = new MailService();

        // GET: Home
        [AllowAnonymous]        //不須做登入驗證即可進入
        public ActionResult Home(int? id)
        {
            CData c = new CData();
            var x = from m in (new dbJoutaEntities()).tMember
                    where !string.IsNullOrEmpty(m.f會員自我介紹)
                    select m;
            var y = from k in (new dbJoutaEntities()).tActivity
                    where k.tMember.f會員評分>4&&k.f活動類型=="旅遊"
                    select k;
            y = y.Take(3);
            x = x.OrderBy(t=> Guid.NewGuid()).Take(3);
            c.tMembers = x;
            c.tActivities = y;
            if (id == 0)
            {
                Session.Remove("member");
            }
            return View(c);
        }

        
        public ActionResult QuickMatch()
        {
            return View();
        }

        [HttpPost]
        public ActionResult QuickMatch(int tabNum, double? curLat, double? curLng)
        {
            dbJoutaEntities db = new dbJoutaEntities();

            //tabNum 0飯局 1旅遊
            //curLat curLng 現在定位經緯度
            if (tabNum == 0)
            {
                if (curLat != null && curLng != null)
                {
                    var x = (from t in db.tActivity
                             where (t.f活動類型 == "飯局") && 
                             (t.f活動經度 > curLng - 0.02) && 
                             (t.f活動經度 < curLng + 0.02) && 
                             (t.f活動緯度 > curLat - 0.02) && 
                             (t.f活動緯度 < curLat + 0.02)
                             select new
                             {
                                 mImg=t.f活動團圖,
                                 mContent = t.f活動內容,
                                 mSort=t.f活動分類,
                                 mPlace=t.f活動地區,
                                 mEstimate=t.f活動預算,
                                 mView=t.f活動瀏覽次數,
                                 mLike=t.f活動讚數,
                                 mTitle=t.f活動標題,
                                 mName = t.tMember.f會員名稱,
                                 mDeadline=t.f活動招募截止時間
                             })
                      .OrderBy(t => Guid.NewGuid()).Take(1);

                    return Json(x, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var x = (from t in db.tActivity
                             where t.f活動類型 == "飯局"
                             select new
                             {
                                 mImg = t.f活動團圖,
                                 mContent = t.f活動內容,
                                 mSort = t.f活動分類,
                                 mPlace = t.f活動地區,
                                 mEstimate = t.f活動預算,
                                 mView = t.f活動瀏覽次數,
                                 mLike = t.f活動讚數,
                                 mTitle = t.f活動標題,
                                 mName = t.tMember.f會員名稱,
                                 mDeadline = t.f活動招募截止時間
                             })
                      .OrderBy(t => Guid.NewGuid()).Take(1);

                    return Json(x, JsonRequestBehavior.AllowGet);
                }             
            }
            else
            {
                if (curLat != null && curLng != null)
                {
                    var x = (from t in db.tActivity
                             where (t.f活動類型 == "旅遊") && 
                             (t.f活動經度 > curLng - 0.02) && 
                             (t.f活動經度 < curLng + 0.02) && 
                             (t.f活動緯度 > curLat - 0.02) && 
                             (t.f活動緯度 < curLat + 0.02)
                             select new
                             {
                                 mImg = t.f活動團圖,
                                 mContent = t.f活動內容,
                                 mSort = t.f活動分類,
                                 mPlace = t.f活動地區,
                                 mEstimate = t.f活動預算,
                                 mView = t.f活動瀏覽次數,
                                 mLike = t.f活動讚數,
                                 mTitle = t.f活動標題,
                                 mName = t.tMember.f會員名稱,
                                 mDeadline = t.f活動招募截止時間
                             })
                      .OrderBy(t => Guid.NewGuid()).Take(1);

                    return Json(x, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var x = (from t in db.tActivity
                             where t.f活動類型 == "旅遊"
                             select new
                             {
                                 mImg = t.f活動團圖,
                                 mContent = t.f活動內容,
                                 mSort = t.f活動分類,
                                 mPlace = t.f活動地區,
                                 mEstimate = t.f活動預算,
                                 mView = t.f活動瀏覽次數,
                                 mLike = t.f活動讚數,
                                 mTitle = t.f活動標題,
                                 mName = t.tMember.f會員名稱,
                                 mDeadline = t.f活動招募截止時間
                             })
                      .OrderBy(t => Guid.NewGuid()).Take(1);

                    return Json(x, JsonRequestBehavior.AllowGet);
                }
            }
        }



        public ActionResult Register()
        {
            //判斷使用者是否已經過登入驗證
            //if (User.Identity.IsAuthenticated)
            //若無登入驗證，則導向註冊頁面
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(CRegisterModel p)
        {
            if ((p == null) && (!ModelState.IsValid))
                return View();
            //判斷資料是否通過驗證
            if (ModelState.IsValid)
            {
                //將頁面資料中的密碼填入
                //p.newMember.txtPassword = p.txtPassword;
                //取得信箱驗證碼
                string AuthCode = mailService.getValidationCode();
                //填入驗證碼
                p.fActivationCode = AuthCode;
                //呼叫service註冊新會員
                membersService.Register(p);
                string tempMail = System.IO.File.ReadAllText(
                    Server.MapPath("~/Views/Shared/RegisterEmailTemplate.html"));

                //宣告Email驗證用Url
                UriBuilder validateUrl = new UriBuilder(Request.Url)
                {
                    Path = Url.Action("emailValidation", "Home", new
                    {
                        email = p.txtEmail,
                        authCode = AuthCode
                    })
                };
                //將資料寫入信中
                string MailBody = mailService.getRegisterMailBody(tempMail, p.txtNickname, validateUrl.ToString().Replace("%3F", "?"));
                //寄信
                mailService.sendRegisterMail(MailBody, p.txtEmail);
                //以tempData儲存註冊訊息
                TempData["RegisterState"] = "註冊成功，請去收取驗證信";
                return RedirectToAction("RegisterResult");
            }
            //未經驗證清空密碼相關欄位
            p.txtPassword = null;
            p.txtPassword_confirm = null;
            //資料回填至view中
            return View(p);
        }

        //註冊結果顯示頁面
        public ActionResult RegisterResult()
        {
            return View();
        }

        //判斷信箱是否被註冊過
        public JsonResult accountCheck(CRegisterModel p)
        {
            //呼叫service來判斷，並回傳結果
            return Json(membersService.accountCheck(p.txtEmail), JsonRequestBehavior.AllowGet);
        }

        //接收驗證信連結傳進來
        public ActionResult emailValidation(string email, string AuthCode)
        {
            //用ViewData儲存，使用Service進行信箱驗證後的結果訊息
            return View();
        }

        //忘記密碼
        public ActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgetPassword(string Email)
        {
            string message = "";
            //bool status = false;
            using (dbJoutaEntities db = new dbJoutaEntities())
            {
                var account = db.tMember.Where(a => a.f會員電子郵件 == Email).FirstOrDefault();
                if (account != null)
                {
                    string resetCode = Guid.NewGuid().ToString();
                    sendResetPasswordMail(account.f會員電子郵件, resetCode);
                    account.f重置驗證碼 = resetCode;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                    message = "重置密碼連結已經發送至您指定信箱，請前往設置!";
                }
                else
                {
                    if (Email == "")
                        message = "請填入信箱";
                    else
                        message = "該信箱不存在!!";
                }
            }
            ViewBag.Msg = message;
            return View();
        }

        public ActionResult ResetPassword(string id)
        {
            using (dbJoutaEntities db = new dbJoutaEntities())
            {
                var user = db.tMember.Where(a => a.f重置驗證碼 == id).FirstOrDefault();
                if (user != null)
                {
                    CReset c = new CReset();
                    c.resetCode = id;
                    return View(c);
                }
                else
                {
                    return HttpNotFound();
                }
            }

        }

        [HttpPost]
        public ActionResult ResetPassword(CReset c)
        {
            var message = "";
            if (ModelState.IsValid)
            {
                using (dbJoutaEntities db = new dbJoutaEntities())
                {
                    var user = db.tMember.Where(a => a.f重置驗證碼 == c.resetCode).FirstOrDefault();
                    if (user != null)
                    {
                        user.f會員密碼 = c.newPassword;
                        user.f重置驗證碼 = "";
                        db.Configuration.ValidateOnSaveEnabled = false;
                        db.SaveChanges();
                        message = "新密碼重置成功!";
                    }
                }
            }
            else
            {
                if (c.newPassword == null)
                {
                    message = "內容必填";
                }
                else
                    message = "格式錯誤";
            }
            ViewBag.Message = message;
            return View();
        }

        [AllowAnonymous]
        public ActionResult About()
        {
            return View();
        }

        [NonAction]
        public void sendResetPasswordMail(string Email, string resetCode)
        {
            //Jouta官方帳號
            string gmail_account = "Joutagroup445@gmail.com";
            string gmail_password = "admin123admin";
            string gmail_mail = "Joutagroup445@gmail.com";     //gmail信箱

            var verifyUrl = "/Home/ResetPassword/" + resetCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress(gmail_mail, "Jouta服務團隊");
            var toEmail = new MailAddress(Email);
            string body = "您好，<br/><br/>已收到您重置密碼的需求，請點擊以下連結重置密碼" +
            "<br/><br/><a href=" + link + ">重置密碼連結</a>";

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