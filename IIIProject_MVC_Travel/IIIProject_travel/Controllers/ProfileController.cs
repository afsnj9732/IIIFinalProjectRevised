﻿using IIIProject_travel.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IIIProject_travel.Controllers
{
    public class ProfileController : Controller
    {
        dbJoutaEntities db = new dbJoutaEntities();
        // GET: Profile
        
        public ActionResult ProfileIndex(string coupon)
        {
            CMember c = new CMember();
            DateTime date = DateTime.Now;
            ViewBag.Date = date;
            var travel = from t in (new dbJoutaEntities()).tActivity
                         select t;  //從資料表抓資料
            c.tActivities = travel;
            var x = (tMember)Session["member"];
            c.tMembers = db.tMember.Where(a=>a.f會員編號 == x.f會員編號).FirstOrDefault();
            return View(c);
        }

        public ActionResult Save()
        {
            tMember y = (tMember)Session["member"];
            var x = db.tMember.Where(a=>a.f會員編號 == y.f會員編號).FirstOrDefault();
            x.f會員名稱 = Request.Form["txtNickName"];
            x.f會員生日 = Request.Form["txtBirth"];
            x.f會員興趣 = Request.Form["txtHobby"];
            x.f會員自我介紹 = Request.Form["txtIntro"];
            
            db.SaveChanges();
            ViewBag.msg = "資料修改成功";
            return RedirectToAction("ProfileIndex");
        }

        public ActionResult Chat()
        {
            return View();
        }
        public ActionResult otherprofile(int? id)
        {
            CMember c = new CMember();
            var y = (tMember)Session["member"];
            if (y.f會員編號 == id)
            {
                return RedirectToAction("ProfileIndex");
            }
            else
            {
                var travel = from t in (new dbJoutaEntities()).tActivity
                         where t.f會員編號 == id
                         select t;  //從資料表抓資料
                var member = (new dbJoutaEntities()).tMember.Where(x => x.f會員編號 == id).FirstOrDefault();

                c.tActivities = travel;
                c.tMembers = member;
                return View(c);
            }
            
        }

    }
}