using IIIProject_travel.Models;
using IIIProject_travel.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;

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
        public ActionResult TravelIndex(string msg) //view需改良
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
            return View("Actadd", actID);
        }

        public dynamic AgreeAdd(int targetMemberID, int actID, string isAgree)
        {
            var result = travelModel.AgreeAddEvent(targetMemberID, actID, isAgree);
            if (result == "6")
                return "6";
            return View("Actadd", actID);
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
            return ((actTarget == "msg") ? View("MsgAdd", actID) : View("Actadd", actID));
        }

        public ActionResult MsgAdd(int actID, string sentMsg) //view需改良
        {
            if (!string.IsNullOrEmpty(sentMsg))
            {
                var loginMember = (tMember)Session["member"];
                travelModel.MsgAddEvent(loginMember.f會員編號, actID, sentMsg);
            }
            return View(actID);
        }

        public dynamic ActAdd(int actID, bool isAdd) //退團或入團 //view需改良
        {
            var loginMember = (tMember)Session["member"];
            var result = travelModel.ActAddEvent(actID, isAdd, loginMember.f會員編號);
            if (result != null)
                return result;
            return View(actID);
        }

        // up is finish

        dbJoutaEntities db = new dbJoutaEntities();  // will remove








        [ValidateInput(false)]
        public ActionResult Edit(tActivity p)
        {
            tMember Member = (tMember)Session["member"];
            tActivity targetAct = db.tActivity.Where(t => t.f活動編號 == p.f活動編號).FirstOrDefault();


            var NowMember = db.tMember.Where(t => t.f會員編號 == Member.f會員編號).FirstOrDefault();
            string[] usedTime = { };
            if (!string.IsNullOrEmpty(NowMember.f會員已占用時間))
            {
                usedTime = NowMember.f會員已占用時間.Split(',');
                //先移除登入會員原本這筆活動的活動時段
                usedTime = usedTime.Where(t => t != targetAct.f活動開始時間 + "~" + targetAct.f活動結束時間).ToArray();
                //再判別修改的活動時段是否已占用 
                string[] used;
                foreach (var item in usedTime)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        used = item.Split('~');  //used[0] 已佔用的開始時間，used[1] 已佔用的結束時間
                        if (string.Compare(p.f活動開始時間, used[1]) > 0 || string.Compare(used[0], p.f活動結束時間) > 0)
                        {

                        }
                        else
                        {
                            return RedirectToAction("TravelIndex", "Travel", new { msg = "錯誤! 修改的活動時間與既有活動時間相衝" });
                        }
                    }

                }
            }
            //時間過關
            //因為活動時段變更所以要剔除所有參加者(不是每個人都想參加新時段)
            //撈出所有參加會員的編號，並讓他們退團
            if (!string.IsNullOrEmpty(targetAct.f活動參加的會員編號))
            {
                string[] DeleteList = targetAct.f活動參加的會員編號.Split(',');
                foreach (var item in DeleteList)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        //移除活動編號
                        tMember Delete = db.tMember.Where(t => t.f會員編號.ToString() == item).FirstOrDefault();
                        Delete.f會員參加的活動編號 =
                            string.Join(",", Delete.f會員參加的活動編號.Split(',').Where(t => t != targetAct.f活動編號.ToString()));

                        //移除占用時間
                        string[] usedTime2 = Delete.f會員已占用時間.Split(',');
                        Delete.f會員已占用時間 =
                            string.Join(",", usedTime2.Where(t => t != targetAct.f活動開始時間 + "~" + targetAct.f活動結束時間));

                    }
                }
            }
            //修改變更項目
            //使用刪除舊活動過後的占用時間加上新的活動時間
            NowMember.f會員已占用時間 = string.Join(",", usedTime) + "," + p.f活動開始時間 + "~" + p.f活動結束時間;
            NowMember.f會員參加的活動編號 += "," + targetAct.f活動編號; //因為被剔除了，所以重新添加
            tActivity Temp = new tActivity();
            targetAct.f活動內容 = Request.Form["f活動內容2"]; //配合文字編輯器，待改良;
            targetAct.f活動參加的會員編號 = NowMember.f會員編號.ToString();
            targetAct.f活動地區 = p.f活動地區;
            targetAct.f活動招募截止時間 = p.f活動招募截止時間;
            targetAct.f活動標題 = p.f活動標題;
            targetAct.f活動結束時間 = p.f活動結束時間;
            targetAct.f活動開始時間 = p.f活動開始時間;
            targetAct.f活動預算 = p.f活動預算;
            targetAct.f活動經度 = p.f活動經度;
            targetAct.f活動緯度 = p.f活動緯度;
            targetAct.f活動審核名單 = null;

            HttpPostedFileBase PicFile = Request.Files["PicFile2"];
            if (PicFile != null)
            {
                var NewFileName = Guid.NewGuid() + Path.GetExtension(PicFile.FileName);
                var NewFilePath = Path.Combine(Server.MapPath("~/Content/images/"), NewFileName);
                PicFile.SaveAs(NewFilePath);
                targetAct.f活動團圖 = NewFileName;
            }
            db.SaveChanges();
            return RedirectToAction("TravelIndex");
        }

        [ValidateInput(false)]
        public ActionResult Add(tActivity p)
        {
            tMember Member = (tMember)Session["member"];

            //判別登入會員其活動時段是否已占用
            var NowMember = db.tMember.Where(t => t.f會員編號 == Member.f會員編號).FirstOrDefault();
            if (!string.IsNullOrEmpty(NowMember.f會員已占用時間))
            {
                string[] usedTime = NowMember.f會員已占用時間.Split(',');
                string[] used;
                foreach (var item in usedTime)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        used = item.Split('~');  //used[0] 已佔用的開始時間，used[1] 已佔用的結束時間
                        if (string.Compare(p.f活動開始時間, used[1]) > 0 || string.Compare(used[0], p.f活動結束時間) > 0)
                        {

                        }
                        else
                        {
                            return RedirectToAction("TravelIndex", "Travel", new { msg = "錯誤! 新增的活動與既有活動時間相衝" });
                        }
                    }

                }
            }
            //添加占用時間
            NowMember.f會員已占用時間 += "," + p.f活動開始時間 + "~" + p.f活動結束時間;
            p.f會員編號 = Member.f會員編號;
            p.f活動類型 = "旅遊";
            p.f活動參加的會員編號 = "," + Member.f會員編號;
            var theCategory = Convert.ToDateTime(p.f活動結束時間) - Convert.ToDateTime(p.f活動開始時間);
            int timeCheck = Convert.ToInt32(theCategory.ToString("dd"));
            switch (timeCheck)//時間判斷
            {
                case 1:
                    p.f活動分類 = "兩天一夜";
                    break;
                case 2:
                    p.f活動分類 = "三天兩夜";
                    break;
                case 4:
                    p.f活動分類 = "五天四夜";
                    break;
                case 6:
                    p.f活動分類 = "七天六夜";
                    break;
                default:
                    p.f活動分類 = "其他";
                    break;
            }
            db.tActivity.Add(p);
            db.SaveChanges();
            int ID = db.tActivity.Where(t => t.f會員編號 == Member.f會員編號)
                .OrderByDescending(t => t.f活動發起日期).Select(t => t.f活動編號).FirstOrDefault();
            NowMember.f會員發起的活動編號 += "," + ID;
            NowMember.f會員參加的活動編號 += "," + ID;
            HttpPostedFileBase PicFile = Request.Files["PicFile"];
            if (PicFile != null)
            {
                var NewFileName = Guid.NewGuid() + Path.GetExtension(PicFile.FileName);
                var NewFilePath = Path.Combine(Server.MapPath("~/Content/images/"), NewFileName);
                PicFile.SaveAs(NewFilePath);
                p.f活動團圖 = NewFileName;
            }
            db.SaveChanges();
            return RedirectToAction("TravelIndex");
        }

        public ActionResult Delete(int? id)
        {
            tMember LoginMember = (tMember)Session["member"];
            var target = db.tActivity.Where(t => t.f活動編號 == id).FirstOrDefault();
            var NowMember = db.tMember.Where(t => t.f會員編號 == LoginMember.f會員編號).FirstOrDefault();
            NowMember.f會員發起的活動編號 =
                string.Join(",", NowMember.f會員發起的活動編號.Split(',').Where(t => t != id.ToString()));
            //撈出所有參加會員的編號，並讓他們退團並退收藏
            if (!string.IsNullOrEmpty(target.f活動參加的會員編號))
            {
                string[] DeleteList = target.f活動參加的會員編號.Split(',');
                foreach (var item in DeleteList)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        //移除活動編號
                        tMember Delete = db.tMember.Where(t => t.f會員編號.ToString() == item).FirstOrDefault();
                        Delete.f會員參加的活動編號 =
                            string.Join(",", Delete.f會員參加的活動編號.Split(',').Where(t => t != id.ToString()));

                        //移除占用時間
                        string[] usedTime = Delete.f會員已占用時間.Split(',');
                        Delete.f會員已占用時間 =
                            string.Join(",", usedTime.Where(t => t != target.f活動開始時間 + "~" + target.f活動結束時間));

                        //移除收藏
                        if (!string.IsNullOrEmpty(Delete.f會員收藏的活動編號))
                        {
                            Delete.f會員收藏的活動編號 = string.Join(",",
                                  Delete.f會員收藏的活動編號.Split(',').Where(t => t != id.ToString())
                                );
                        }
                    }
                }
            }
            db.tActivity.Remove(target);
            db.SaveChanges();
            return RedirectToAction("TravelIndex");
        }
        

     
        








    }
}