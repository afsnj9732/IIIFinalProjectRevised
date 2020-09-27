using IIIProject_travel.Models;
using System;
using System.Web;
using System.Web.Mvc;

namespace IIIProject_travel.Controllers
{
    public class TravelController : Controller
    {       
        TravelModel travelModel = new TravelModel();

        public ActionResult GetTravelList(string condition) //活動總列表回傳
        {
            var Final = travelModel.SortList(condition);
            if (Session["member"] != null)
            {
                tMember loginMember = (tMember)Session["member"];
                Final = travelModel.GetLikeList(Final,loginMember.f會員編號);
            }
            return View(Final);
        }

        // GET: Travel
        public ActionResult TravelIndex(string msg) 
        {
            string HomeSearch = ",所有,全部," + msg;
            return View((object)HomeSearch);
        }

        [HttpPost]
        public ActionResult TravelIndex(int? id) //HomeSearch才用
        {
            string HomeSearch = "";
            HomeSearch += Request.Form["txtTravelKeyword"];
            HomeSearch += "," + Request.Form["txtTravelCategory"];
            HomeSearch += "," + Request.Form["txtTotalGood"];
            HomeSearch += ",";
            return View((object)HomeSearch);
        }

        public dynamic GetCalendar()
        {
            if (Session["member"] != null)
            {                
                var loginMember = (tMember)Session["member"];
                return travelModel.CalendarList(loginMember.f會員編號);
            }
            else
            {
                return "1";
            }
        }

        public string AutoComplete()
        {
            return travelModel.AutoCompleteList();
        }

        public string GetDateLimit(int actID)
        {
            if (Session["member"] != null)
            {
                tMember loginMember = (tMember)Session["member"];
                return travelModel.DateLimit(loginMember.f會員編號, actID);
            }
            return null;
        }

        public string GetFeelGood(int actID)
        {
            if (Session["member"] != null)
            {
                var loginMember = (tMember)Session["member"];
                return travelModel.FeelGood(loginMember.f會員編號, actID);
            }

            return "1";
        }

        public ActionResult GetReadMore(int actID)  //view需改良
        {             
            return View(travelModel.ReadMore(actID));
        }

        public void LikeIt(int actID)
        {
            var loginMember = (tMember)Session["member"];
            travelModel.UserLikeIt(loginMember.f會員編號, actID);
        }

        public dynamic ActKick(int targetMemberID, int actID)
        {
            var loginMember = (tMember)Session["member"];
            if (loginMember.f會員編號 == targetMemberID)
                return "";//不能把自己加入黑名單

            travelModel.KickEvent(targetMemberID, actID);
            return View("ActAdd", (object)travelModel.ActAddEvent(actID, loginMember.f會員編號));
        }

        public dynamic AgreeAdd(int targetMemberID, int actID, string isAgree)
        {
            var loginMember = (tMember)Session["member"];
            var result = travelModel.AgreeAddEvent(targetMemberID, actID, isAgree);
            if (result == "6")
                return "6";
            return View("ActAdd", (object)travelModel.ActAddEvent(actID, loginMember.f會員編號));
        }

        public object ScoreAdd(int actID, int Score)
        {
            var loginMember = (tMember)Session["member"];
            return travelModel.ScoreAddEvent(loginMember.f會員編號, actID, Score);
        }

        public dynamic AddBlackList(int targetMemberID, int actID, string actTarget)
        {

            var loginMember = (tMember)Session["member"];
            if (targetMemberID == loginMember.f會員編號)
            {
                return "0";
            }

            var blackAct = travelModel.AddBlackListEvent(loginMember.f會員編號, targetMemberID);
            if (blackAct == "1")
                return "1";
            //根據actTarget決定刷新哪種頁面
            if (actTarget == "msg")//此處用?:無法做出兩種不同型別之判斷
            {
                return RedirectToAction("MsgAdd", new { actID = actID, sentMsg = "" });
            }
            else
            {
                return View("ActAdd", (object)travelModel.ActAddEvent(actID, loginMember.f會員編號));
            }
        }

        public ActionResult MsgAdd(int actID, string sentMsg) 
        {
            var loginMember = (tMember)Session["member"];
            if (!string.IsNullOrEmpty(sentMsg))
            {            
                travelModel.MsgAddEvent(loginMember.f會員編號, actID, sentMsg);
            }
            return View(travelModel.MsgAddEvent(loginMember.f會員編號, actID));
        }

        public dynamic ActAdd(int actID, bool isAdd) //退團或入團 //view需改良
        {
            var loginMember = (tMember)Session["member"];
            var result = travelModel.ActAddEvent(actID, isAdd, loginMember.f會員編號);
            if (result != null)
                return result;
            return View(travelModel.ActAddEvent(actID, loginMember.f會員編號));
        }

        public ActionResult Delete(int id)
        {
            tMember loginMember = (tMember)Session["member"];
            travelModel.DeleteActivity(loginMember.f會員編號, id);
            return RedirectToAction("TravelIndex");
        }

        [ValidateInput(false)]
        public ActionResult Add(tActivity act)
        {
            tMember loginMember = (tMember)Session["member"];
            HttpPostedFileBase picFile = Request.Files["picFile"];
            string filePath = Server.MapPath("~/Content/images/");
            var result = travelModel.AddAct(loginMember.f會員編號, act, picFile, filePath);
            if (result == "1")            
                return RedirectToAction("TravelIndex", "Travel", new { msg = "錯誤! 新增的活動與既有活動時間相衝" });            
            return RedirectToAction("TravelIndex");
        }

        [ValidateInput(false)]
        public ActionResult Edit(tActivity act)
        {
            tMember loginMember = (tMember)Session["member"];
            HttpPostedFileBase picFile = Request.Files["picFile"];
            string filePath = Server.MapPath("~/Content/images/");
            var result = travelModel.EditAct(loginMember.f會員編號, act, picFile, filePath);
            if(result=="1")
                RedirectToAction("TravelIndex", "Travel", new { msg = "錯誤! 修改的活動時間與既有活動時間相衝" });
            return RedirectToAction("TravelIndex");
        }




        

     
        








    }
}