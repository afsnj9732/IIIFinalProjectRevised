using IIIProject_travel.Models;
using IIIProject_travel.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace IIIProject_travel.Services
{
    public class MembersService
    {
        //建立與資料庫的連接字串
        dbJoutaEntities db = new dbJoutaEntities();

        //註冊新會員
        public void Register(CRegisterModel newMember)
        {
            //sql新增     isAdmin預設為0

            tMember t = new tMember();
            t.f會員電子郵件 = newMember.txtEmail;
            t.f會員帳號 = newMember.txtEmail;
            t.f會員名稱 = newMember.txtNickname;
            t.f會員密碼 = newMember.txtPassword;
            t.f驗證碼 = newMember.fActivationCode;
            t.isAdmin = false;
            db.tMember.Add(t);
            db.SaveChanges();
        }

        //藉由信箱取得單筆資料(全部資料)
        private CRegisterModel getAccount(string email)
        {
            CRegisterModel c = new CRegisterModel();
            tMember t = db.tMember.FirstOrDefault(k=>k.f會員電子郵件 == email);
            try
            {
                c.txtEmail =t.f會員電子郵件;
                c.txtNickname = t.f會員名稱;
                c.txtPassword = t.f會員密碼;
                c.fActivationCode =t.f驗證碼 ;
                c.isAdmin = Convert.ToBoolean(t.isAdmin);
            }
            catch (Exception)
            {
                //查無資料
                c = null;
            }
            return c;
        }

        //確認信箱是否重複註冊
        public bool accountCheck(string email)
        {
            CRegisterModel c = getAccount(email);
            //判斷是否查到資料
            bool result = (c == null);
            return result;
        }


        //取得公開資料
        public CRegisterModel getAccount_openSource(string email)
        {
            CRegisterModel c = new CRegisterModel();
            tMember t = db.tMember.FirstOrDefault(k => k.f會員電子郵件 == email);
            try
            {
                c.txtEmail = t.f會員電子郵件;
                c.txtNickname = t.f會員名稱;
            }
            catch (Exception)
            {
                //查無資料
                c = null;
            }
            return c;
        }

       
        //信箱驗證碼驗證
        public string emailValidation(string email, string authCode)
        {
            CRegisterModel c = getAccount(email);
            //宣告驗證後訊息字串
            string validationStr = string.Empty;
            if (c != null)
            {
                dbJoutaEntities db = new dbJoutaEntities();
                tMember t = db.tMember.FirstOrDefault(k => k.f會員電子郵件 == email && k.f驗證碼 == authCode);
                t.f驗證碼 = "";
                validationStr = "信箱驗證成功，現在可以登入囉~";
            }
            return validationStr;
        }


        //密碼確認
        private bool passwordCheck(CRegisterModel p, string password)
        {
            //判斷DB裡的密碼資料與傳入密碼資料Hash後是否一致
            bool result = p.txtPassword.Equals(password);
            return result;
        }

        public string LoginCheck(string email,string password)
        {
            //取得傳入email的會員資料
            CRegisterModel p = getAccount(email);
            //判斷是否有此人
            if (p != null)
            {
                p.fActivationCode = null;
                //進行信箱密碼驗證
                if (passwordCheck(p, password))
                {
                    return "";
                }
                else
                {
                    return "密碼輸入錯誤";
                }
            }
            else
            {
                return "此信箱尚未註冊，請去註冊";
            }
        }

        //更改密碼
        public string ChangePassword(string email,string password,string newPassword)
        {
            CRegisterModel p = getAccount(email);
            //確認舊密碼的正確性
            if (passwordCheck(p, password))
            {
                p.txtPassword = newPassword;
                tMember t = db.tMember.FirstOrDefault(k => k.f會員密碼 == p.txtPassword);
                db.tMember.Add(t);
                db.SaveChanges();
                return "密碼修改成功";
            }
            else
            {
                return "原密碼輸入錯誤";
            }
        }

        //會員權限角色管理
        public string getRole(string email)
        {
            //初始角色
            string Role = "User";
            CRegisterModel p = getAccount(email);
            //判斷DB欄位，以確認是否為Admin
            if (p.isAdmin)
            {
                Role += "Admin";       //添加Admin
            }
            return Role;
        }


        //檢查圖片類型
        public bool CheckImg(string ContentType)
        {
            switch (ContentType)
            {
                case "image/jpg":
                case "image/jpeg":
                case "image/png":
                        return true;
            }
            return false;
        }

    }

}