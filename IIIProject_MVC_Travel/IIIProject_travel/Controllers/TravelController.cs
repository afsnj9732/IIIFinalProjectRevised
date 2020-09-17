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
        dbJoutaEntities db = new dbJoutaEntities();


        public dynamic getCalendar()
        {
            if (Session["member"] != null)
            {
                var LoginMember = (tMember)Session["member"];
                var nowRealMember = db.tMember.Where(t => t.f會員編號 == LoginMember.f會員編號).FirstOrDefault();
                if (!string.IsNullOrEmpty(nowRealMember.f會員參加的活動編號))
                {
                    string[] nowMemberEvents = nowRealMember.f會員參加的活動編號.Split(',');
                    CalendarEvents[] NowMemberTotalEvents = new CalendarEvents[nowMemberEvents.Length - 1];
                    int i = 0;
                    foreach (var item in nowMemberEvents)
                    {
                        if (string.IsNullOrEmpty(item))
                        {
                            continue;
                        }
                        var NowMemberAct = db.tActivity.Where(t => t.f活動編號.ToString() == item).FirstOrDefault();
                        CalendarEvents CalendarEvent = new CalendarEvents();
                        CalendarEvent.title = NowMemberAct.f活動標題;
                        
                        CalendarEvent.start = NowMemberAct.f活動開始時間;
                        CalendarEvent.end = 
                            NowMemberAct.f活動開始時間 == NowMemberAct.f活動結束時間 ?  NowMemberAct.f活動結束時間 : NowMemberAct.f活動結束時間 + " 23:59:59";
                        CalendarEvent.classNames = "CalendarEvent" + " " + "EventActID" + NowMemberAct.f活動編號;
                        NowMemberTotalEvents[i] = CalendarEvent;
                        i++;
                    }
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    var obj = serializer.Serialize(NowMemberTotalEvents);
                    return obj;  //序列化後已是Json字串，傳到前端用JSON.parse即可轉成js物件
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "1";
            }           
        }

        public ActionResult get_ajax_readmore(int act_id)
        {
            ViewCounts(act_id);
            tActivity target_act = db.tActivity.Where(t => t.f活動編號 == act_id).FirstOrDefault();            
            return View(target_act);
        }

        public ActionResult article_AJAX(string condition)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            CSelect obj = serializer.Deserialize<CSelect>(condition);
            var FinalList = AJAXcondition(condition).Where(a => a.f活動類型 == "旅遊").Select(a => a);
            CTravel List = new CTravel();

            //頁數判斷
            if (FinalList.Count() % 4 == 0 && FinalList.Count()!=0)
            {
                List.TotalPage = FinalList.Count() / 4;
            }
            else
            {
                List.TotalPage = (FinalList.Count() - FinalList.Count() % 4) / 4 + 1;
            }   
            List.NowPage = obj.page;
            if (obj.page == 0 || obj.page>List.TotalPage)
            {
                List.NowPage = 1;  
            }
            
            List.FinalList = FinalList.Skip(4 * (List.NowPage - 1)).Take(4);
            return View(List);
        }

        // GET: Travel
        public ActionResult TravelIndex(string msg)
        {
            string HomeSearch = ",所有,全部,"+ msg;
            return View((object)HomeSearch);
        }

        [HttpPost]
        public ActionResult TravelIndex(int? id) //HomeSearch才用
        {
            string HomeSearch = "";
            HomeSearch += Request.Form["txtTravelKeyword"]; 
            HomeSearch += ","+ Request.Form["txtTravelCategory"]; 
            HomeSearch += ","+ Request.Form["txtTotalGood"];
            HomeSearch += ",";
            return View((object)HomeSearch);
        }

        public string GetDateLimit(int act_id)
        {
            if (Session["member"] != null)
            {
                tMember loginMember = (tMember)Session["member"];
                var realMember = db.tMember.Where(t => t.f會員編號 == loginMember.f會員編號).FirstOrDefault();
                if (!string.IsNullOrEmpty(realMember.f會員已占用時間))
                {
                    string[] timeList = realMember.f會員已占用時間.Split(',');
                    //act_id!=0，表示是編輯模式，要先移除該筆活動的占用時間才符合時間限制條件
                    if (act_id != 0)
                    {
                        var targetAct = db.tActivity.Where(t => t.f活動編號 == act_id).FirstOrDefault();
                        timeList = timeList.Where(t => t != targetAct.f活動開始時間 + "~" + targetAct.f活動結束時間).ToArray();
                    }
                    //若無，則為一般開團，直接回傳已佔用的時間陣列                    
                    //string totalTime = "[";
                    string totalTime = "";
                    foreach (string item in timeList)
                    {
                        if (string.IsNullOrEmpty(item))
                            continue;
                        string[] timeRange = item.Split('~');
                        double limit = Convert.ToInt32((Convert.ToDateTime(timeRange[1]) - Convert.ToDateTime(timeRange[0]))
                                .ToString("dd"));
                        for (double i = 0.0; i <= limit; i++)
                        {
                            totalTime += Convert.ToDateTime(timeRange[0]).AddDays(i).ToString("yyyy-MM-dd") + ",";                           
                        }
                    }
                    if(totalTime.Length>2)
                        totalTime = totalTime.Substring(0, totalTime.Length - 1);
                    //totalTime += "]";
                    return totalTime;
                }
            }
            return null;
        }

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
                        return  RedirectToAction("TravelIndex","Travel",new { msg = "錯誤! 新增的活動與既有活動時間相衝" } );
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
            switch(timeCheck)//時間判斷
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
                .OrderByDescending(t=>t.f活動發起日期).Select(t => t.f活動編號).FirstOrDefault();
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

        public dynamic addBlackList(int target_member, int act_id, string act_target)
        {

            var loginMember = (tMember)Session["member"];
            if (target_member == loginMember.f會員編號)
            {
                return "0";
            }

            
            var nowRealMember = db.tMember.Where(t => t.f會員編號 == loginMember.f會員編號).FirstOrDefault();
            if (!string.IsNullOrEmpty(nowRealMember.f會員黑名單))
            {
                var black_list = nowRealMember.f會員黑名單.Split(',');
                if (Array.IndexOf(black_list,target_member.ToString())>-1) //已經加入黑單
                {
                    return "1";
                }
            }

            nowRealMember.f會員黑名單 += "," + target_member;
            db.SaveChanges();
            if (act_target == "msg")
            {
                return View("MsgAdd", act_id);
            }
            else
            {
                return View("Actadd", act_id);
            }
        }

        public dynamic ActKick(int target_member,int act_id)
        {
            var loginMember = (tMember)Session["member"];
            if (target_member == loginMember.f會員編號) //不可以黑自己
            {
                return "";
            }
            var ActList = db.tActivity.Where(t => t.f活動編號 == act_id).FirstOrDefault();
            var kick_member = db.tMember.Where(t => t.f會員編號 == target_member).FirstOrDefault();


            //if (!string.IsNullOrEmpty(ActList.f活動參加的會員編號)) 因為有團主，活動必定有人參加            
            string[] GuysList = ActList.f活動參加的會員編號.Split(',');

            //移除占用時間
            string[] usedTime = kick_member.f會員已占用時間.Split(',');
            kick_member.f會員已占用時間 =
                string.Join(",", usedTime.Where(t => t != ActList.f活動開始時間 + "~" + ActList.f活動結束時間));

            //移除會員資料參加的會員參加的活動編號
            string[] NewList = kick_member.f會員參加的活動編號.Split(',');
            kick_member.f會員參加的活動編號 =
                string.Join(",", NewList.Where(t => t != ActList.f活動編號.ToString()));
            //移除活動紀錄的會員編號
            ActList.f活動參加的會員編號 =
                string.Join(",", GuysList.Where(t => t != kick_member.f會員編號.ToString()));
            db.SaveChanges();

            return View("Actadd", act_id);
        }

        public dynamic Actadd(int target, bool isAdd) //退團或入團
        {
            var LoginMember = (tMember)Session["member"];
            var ActList = db.tActivity.Where(t => t.f活動編號 == target).FirstOrDefault();
            string[] black_list = { };
            if (ActList.tMember.f會員黑名單!=null)
            {
                black_list = ActList.tMember.f會員黑名單.Split(',');
            }
            
            var NowMember = db.tMember.Where(t => t.f會員編號 == LoginMember.f會員編號)
                         .FirstOrDefault();//因Session存取的資料沒有和資料庫內部做綁定
                                                          //所以不能存取，要用Session登入會員的會員編號
                                                          //撈出目前會員的資料
                                                          //檢查登入會員是否為本活動團主，團主不可入團退團
            int index = -1; //先假設不是團主
            if (!string.IsNullOrEmpty(NowMember.f會員發起的活動編號)) //若登入會員有開團紀錄
            {
                string[] LeaderList = NowMember.f會員發起的活動編號.Split(',');
                index = Array.IndexOf(LeaderList, target.ToString());
                if (index != -1)//若找到，表示是團主
                {
                    return "1";
                }
            }

            //if (!string.IsNullOrEmpty(ActList.f活動參加的會員編號)) 因為有團主，活動必定有人參加            
            string[] GuysList = ActList.f活動參加的會員編號.Split(',');
            index = Array.IndexOf(GuysList, LoginMember.f會員編號.ToString());  //尋找登入中的會員是否有參加
                                                                            //注意會員標號是int，陣列內容是str，
                                                                            //不轉型index永遠會是-1 


            if (isAdd == true)//點選入團
            {
                if (black_list.Length > 1)
                {
                    if (Array.IndexOf(black_list,LoginMember.f會員編號.ToString())>-1)
                    {
                        return "7";
                    }
                }

                if (index == -1)//登入中的會員不存在名單
                {
                    //判別活動時段是否已占用
                    if (!string.IsNullOrEmpty(NowMember.f會員已占用時間))
                    {
                        string[] usedTime = NowMember.f會員已占用時間.Split(',');
                        string[] used;
                        foreach (var item in usedTime)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                used = item.Split('~');  //used[0] 已佔用的開始時間，used[1] 已佔用的結束時間
                                if (string.Compare(ActList.f活動開始時間, used[1]) > 0 || string.Compare(used[0], ActList.f活動結束時間) > 0)
                                {

                                }
                                else
                                {
                                    return "6";
                                }
                            }

                        }
                    }
                    //判別目前登入對象是否已在審核
                    string[] isAgreeList;
                    if (!string.IsNullOrEmpty(ActList.f活動審核名單))
                    {
                        isAgreeList = ActList.f活動審核名單.Split(',');
                        if (Array.IndexOf(isAgreeList, LoginMember.f會員編號.ToString()) > -1)
                        {
                            return "8";
                        }
                    }                    
                    //進入審核
                    ActList.f活動審核名單 += "," + NowMember.f會員編號;
                    db.SaveChanges();
                }
                else //若會員已存在
                {
                    return "0";
                }
            }
            else //點選退出
            {
                //若只是審核中的團員
                //判別目前登入對象是否已在審核
                string[] isAgreeList;
                if (!string.IsNullOrEmpty(ActList.f活動審核名單))
                {
                    isAgreeList = ActList.f活動審核名單.Split(',');
                    if (Array.IndexOf(isAgreeList, LoginMember.f會員編號.ToString()) > -1)
                    {
                        ActList.f活動審核名單 = string.Join(",",
                             ActList.f活動審核名單.Split(',').Where(t => t != NowMember.f會員編號.ToString()));
                        db.SaveChanges();
                        return View(target);
                    }
                }

                if (index != -1)//登入中的會員存在參加名單則讓他退出並更動占用時間
                {
                    //移除占用時間
                    string[] usedTime = NowMember.f會員已占用時間.Split(',');
                    NowMember.f會員已占用時間 =
                        string.Join(",", usedTime.Where(t => t != ActList.f活動開始時間 + "~" + ActList.f活動結束時間));



                    //移除會員資料參加的會員參加的活動編號
                    string[] NewList = NowMember.f會員參加的活動編號.Split(',');
                    NowMember.f會員參加的活動編號 =
                        string.Join(",", NewList.Where(t => t != ActList.f活動編號.ToString()));
                    //移除活動紀錄的會員編號
                    ActList.f活動參加的會員編號 =
                        string.Join(",", GuysList.Where(t => t != NowMember.f會員編號.ToString()));
                    db.SaveChanges();
                }
                else //若會員不存在
                {
                    return "";
                }
            }
            return View(target);
        }

        public dynamic agree_add(int target_member , int act_id , string act)
        {
            var ActList = db.tActivity.Where(t => t.f活動編號 == act_id).FirstOrDefault();
            var targetMember = db.tMember.Where(t => t.f會員編號 == target_member).FirstOrDefault();
            if (act == "agree") //點允許入團
            {
                //判斷欲允許的審核對象在審核期間是否成功加入了其他活動
                if (!string.IsNullOrEmpty(targetMember.f會員已占用時間))
                {
                    string[] usedTime = targetMember.f會員已占用時間.Split(',');
                    string[] used;
                    foreach (var item in usedTime)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            used = item.Split('~');  //used[0] 已佔用的開始時間，used[1] 已佔用的結束時間
                            if (string.Compare(ActList.f活動開始時間, used[1]) > 0 || string.Compare(used[0], ActList.f活動結束時間) > 0)
                            {

                            }
                            else
                            {
                                return "6";
                            }
                        }

                    }
                }
                //加入會員占用時間
                targetMember.f會員已占用時間 += "," + ActList.f活動開始時間 + "~" + ActList.f活動結束時間;

                targetMember.f會員參加的活動編號 += "," + ActList.f活動編號;
                ActList.f活動參加的會員編號 += "," + targetMember.f會員編號;
                
            }
            ActList.f活動審核名單 = string.Join(",",
                ActList.f活動審核名單.Split(',').Where(t => t != targetMember.f會員編號.ToString()));
            db.SaveChanges();
            return View("Actadd",act_id);
        }



        public object ScoreAdd(int target,int Score)
        {
            var NowMember = (tMember)Session["member"];
            var theActivity = db.tActivity.Where(x => x.f活動編號 == target)
                              .Select(a => a);
            string[] isExist = theActivity.Select(a => a.f活動參加的會員編號)
                               .FirstOrDefault().Split(',');
            int pos;
            var nowRealMember = db.tMember.Where(t => t.f會員編號 == NowMember.f會員編號).FirstOrDefault();
            if (!string.IsNullOrEmpty(nowRealMember.f會員發起的活動編號))
            { 
                pos = Array.IndexOf(nowRealMember.f會員發起的活動編號.Split(','), target.ToString());
                if (pos != -1)//團主不可自行評分
                {
                    return "5";
                }
            }
            var result = string.Compare(DateTime.Now.ToString("yyyy,MM,dd"), theActivity.FirstOrDefault().f活動結束時間);           
            if (result < 0)  //result=1 活動已結束 ， result=-1 活動尚未結束
            {
                return "3"; //活動尚未結束
            }

            pos = Array.IndexOf(isExist, NowMember.f會員編號.ToString());
            if (pos == -1 ) //先找有沒有參加本次活動
            { 
                return "0";//沒參加
            }

            if (!string.IsNullOrEmpty(theActivity.Select(a => a.f活動評分過的會員編號).FirstOrDefault()))
            {  
                //若有曾經評分過的會員編號
                isExist = theActivity.Select(a => a.f活動評分過的會員編號).FirstOrDefault().Split(',');
                pos = Array.IndexOf(isExist, NowMember.f會員編號.ToString());//再找現在登入會員有沒有評分過
                if (pos != -1) 
                {
                    return "1"; //有評分過
                }
            }
            //若無曾經評分過的會員編號則直接往下

            //若通過上面判定則進行評分
            var SaveData = theActivity.FirstOrDefault();            
            SaveData.f活動評分過的會員編號 += "," + NowMember.f會員編號;
            if(SaveData.tMember.f會員評分人數 == null)
            {
                SaveData.tMember.f會員評分人數 = 0;
            }
            SaveData.tMember.f會員評分人數 += 1;
            if (SaveData.tMember.f會員總分 == null)
            {
                SaveData.tMember.f會員總分 = 0;
            }
            SaveData.tMember.f會員總分 += Score;
            if (SaveData.tMember.f會員評分 == null)
            {
                SaveData.tMember.f會員評分 = 0;
            }
            SaveData.tMember.f會員評分 = 
                Math.Round(Convert.ToDouble(SaveData.tMember.f會員總分 / SaveData.tMember.f會員評分人數), 1);
            db.SaveChanges();
            return "2";
        }

        public void ViewCounts(int activityID)
        {
            var target = db.tActivity.Where(t => t.f活動編號 == activityID).FirstOrDefault();
            target.f活動瀏覽次數 += 1;
            db.SaveChanges();
        }

        public string likeIt(string ActivityID)  
        {
            var condition = (tMember)Session["member"];
            var targetAct = db.tActivity.Where(t => t.f活動編號.ToString() == ActivityID).FirstOrDefault();
            var member = db.tMember.Where(x => x.f會員編號 == condition.f會員編號).Select(a => a).FirstOrDefault();
            if (!string.IsNullOrEmpty(member.f會員收藏的活動編號))
            {
                string[] isExist = member.f會員收藏的活動編號.Split(',');
                int pos = Array.IndexOf(isExist, ActivityID);
                if (pos < 0)   //若沒找到，存入資料庫，正確
                {
                    targetAct.f有收藏活動的會員編號 += "," + member.f會員編號;
                    member.f會員收藏的活動編號 += "," + ActivityID;
                    db.SaveChanges();
                }
                else  //若找到
                {                     
                    var FinalList = isExist.ToList();
                    FinalList.RemoveAt(pos);  //移除
                    member.f會員收藏的活動編號 = string.Join(",", FinalList);
                    targetAct.f有收藏活動的會員編號 = string.Join(",",
                        targetAct.f有收藏活動的會員編號.Split(',').Where(t => t != member.f會員編號.ToString()));
                    db.SaveChanges();
                }
            }
            else
            {
                targetAct.f有收藏活動的會員編號 += "," + member.f會員編號;
                member.f會員收藏的活動編號 += "," + ActivityID; //若資料庫完全是空的，則不可能有重複值，直接存入
                db.SaveChanges();
            }
            return "1";
        }

        public string autoComplete()
        {
            var autoComplete = db.tActivity.Where(t => t.f活動類型 == "旅遊").Select(t => t.f活動標題).ToArray();
            return string.Join(",", autoComplete);
        }

        public IEnumerable<tActivity> AJAXcondition(string condition)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            CSelect obj = serializer.Deserialize<CSelect>(condition);

            var tTravel_order = typeof(tActivity).GetProperty(obj.order);

            //不使用if，動態抓取排序條件
            var CountViewList = db.tActivity
                        .AsEnumerable().OrderBy(a => tTravel_order.GetValue(a, null))
                        .Select(a => a); //升冪

            if (obj.background_color == "rgb(250, 224, 178)")
            {
                CountViewList = db.tActivity
                .AsEnumerable().OrderByDescending(a => tTravel_order.GetValue(a, null))
                .Select(a => a); //降冪
            }

            if (!string.IsNullOrEmpty(obj.contain)) //搜尋欄位若非空
            {
                CountViewList = CountViewList.Where(b => b.f活動標題.Contains(obj.contain))
                            .Select(a => a);
            }
            if (obj.category != "所有")
            {
                CountViewList = CountViewList
                                    .Where(b => b.f活動分類 == obj.category)
                                    .Select(a => a);
            }

            if (obj.label != "全部")
            {
                CountViewList = CountViewList
                                    .Where(b => b.f活動讚數 > Convert.ToInt32(obj.label))
                                    .Select(a => a);
            }
            return CountViewList;
        }


        public ActionResult MsgAdd(int target, string sentMsg)
        {
            var NowMember = (tMember)Session["member"];
            var ActList = db.tActivity.Where(n => n.f活動編號 == target).FirstOrDefault();
            ActList.f活動留言 += "_^$"+NowMember.f會員名稱 + ":" + sentMsg ;
            ActList.f活動留言時間 += "," + DateTime.Now.ToString("MM/dd HH:mm:ss") + "_^$" + NowMember.f會員編號;
            db.SaveChanges();
            return View(target);
        }



        public string FeelGood(string target)
        {           
            if (Session["member"]!=null)
            {
                var loginMember = (tMember)Session["member"];                             
                int select = Convert.ToInt32(target);
                tActivity theTarget = db.tActivity.FirstOrDefault(x => x.f活動編號 == select);
                int index = -1;
                if (!string.IsNullOrEmpty(theTarget.f活動按過讚的會員編號))
                {
                    var past = theTarget.f活動按過讚的會員編號.Split(',');//將按過讚得會員編號 字串 切割 成陣列

                    index = Array.IndexOf(past, loginMember.f會員編號.ToString());//透過查詢值在陣列內的索引值(不存在則回傳-1)
                                                                         //查看是否會員編號包含在陣列內
                }

                if (index == -1 )//陣列起始為0，因此只要pos>=0則表示該編號已存在，反之index=-1表示該編號不存在，可執行
                {
                theTarget.f活動讚數 = (theTarget.f活動讚數 + 1);
                theTarget.f活動按過讚的會員編號 += "," + loginMember.f會員編號;
                db.SaveChanges();
                }
                else
                {
                    return "0";
                }
            }

            return "1";
        }




    }
}