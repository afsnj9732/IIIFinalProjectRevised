using IIIProject_travel.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace IIIProject_travel.Models
{
    public abstract class ActivityModel
    {
        public dbJoutaEntities db = new dbJoutaEntities();

        public class CSelect //接收排序條件之類別
        {
            public string Order { get; set; }
            public string BackgroundColor { get; set; }
            public string Contain { get; set; }
            public string Category { get; set; }
            public string Label { get; set; }
            public int Page { get; set; }
        }

        public CSelect CSelectDeserialize(string condition) //Json反序列化抓取排序條件
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            CSelect obj = serializer.Deserialize<CSelect>(condition);
            return obj;
        }

        public abstract CActivity SortList(string condition); //活動列表+排序


        public CActivity GetLikeList(CActivity theList, int memberID)
        {
            var memberLikeList = GetMemberData(memberID, "f會員收藏的活動編號");
            if (!string.IsNullOrEmpty(memberLikeList))
            {
                theList.MemberLike = memberLikeList.Split(',');
            }
            return theList;
        }

        //使用ID取得想要的會員其資料庫中最新的特定欄位之資料
        dynamic GetMemberData(int memberID, string wantGet)
        {
            var targetMember = db.tMember.Where(t => t.f會員編號 == memberID).FirstOrDefault();

            //使用映射動態抓取想要的會員欄位之資料
            var targetData = targetMember.GetType().GetProperty(wantGet).GetValue(targetMember);
            return targetData;
        }

        public dynamic CalendarList(int memberID)
        {
            var memberAct = GetMemberData(memberID, "f會員參加的活動編號");
            if (!string.IsNullOrEmpty(memberAct))
            {
                string[] memberEvents = memberAct.Split(',');
                CalendarEvents[] nowMemberTotalEvents = new CalendarEvents[memberEvents.Length - 1];
                int i = 0;
                foreach (var item in memberEvents)
                {
                    if (string.IsNullOrEmpty(item))
                    {
                        continue;
                    }
                    var nowMemberAct = db.tActivity.Where(t => t.f活動編號.ToString() == item).FirstOrDefault();
                    CalendarEvents calendarEvent = new CalendarEvents();
                    calendarEvent.title = nowMemberAct.f活動標題;

                    calendarEvent.start = nowMemberAct.f活動開始時間;
                    calendarEvent.end =
                        nowMemberAct.f活動開始時間 == nowMemberAct.f活動結束時間 ? nowMemberAct.f活動結束時間 : nowMemberAct.f活動結束時間 + " 23:59:59";
                    calendarEvent.classNames = "CalendarEvent" + " " + "EventActID" + nowMemberAct.f活動編號;
                    nowMemberTotalEvents[i] = calendarEvent;
                    i++;
                }
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                var obj = serializer.Serialize(nowMemberTotalEvents);
                return obj;  //序列化後已是Json字串，傳到前端用JSON.parse即可轉成js物件
            }
            else
            {
                return "";
            }
        }

        public abstract string AutoCompleteList();

        public string DateLimit(int memberID, int actID)
        {
            var memberTimeLimit = GetMemberData(memberID, "f會員已占用時間");
            if (!string.IsNullOrEmpty(memberTimeLimit))
            {
                string[] timeList = memberTimeLimit.Split(',');
                //actID!=0，表示是編輯模式，要先移除該筆活動的占用時間才符合時間限制條件
                if (actID != 0)
                {
                    var targetAct = db.tActivity.Where(t => t.f活動編號 == actID).FirstOrDefault();
                    timeList = timeList.Where(t => t != targetAct.f活動開始時間 + "~" + targetAct.f活動結束時間).ToArray();
                }
                //若無，則為一般開團，直接回傳已佔用的時間陣列                    
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
                if (totalTime.Length > 2)
                    totalTime = totalTime.Substring(0, totalTime.Length - 1);
                return totalTime;
            }
            return null;
        }

        public string FeelGood(int memberID, int actID)
        {
            tActivity theTarget = db.tActivity.FirstOrDefault(x => x.f活動編號 == actID);
            int index = -1;
            if (!string.IsNullOrEmpty(theTarget.f活動按過讚的會員編號))
            {
                var past = theTarget.f活動按過讚的會員編號.Split(',');//將按過讚得會員編號 字串 切割 成陣列

                index = Array.IndexOf(past, memberID.ToString());//透過查詢值在陣列內的索引值(不存在則回傳-1)
                                                                 //查看是否會員編號包含在陣列內
            }

            if (index == -1)//陣列起始為0，因此只要pos>=0則表示該編號已存在，反之index=-1表示該編號不存在，可執行
            {
                theTarget.f活動讚數 = (theTarget.f活動讚數 + 1);
                theTarget.f活動按過讚的會員編號 += "," + memberID;
                db.SaveChanges();
                return null;
            }
            else
            {
                return "0";
            }
        }

        public void AddViewCounts(int actID)
        {
            var target = db.tActivity.Where(t => t.f活動編號 == actID).FirstOrDefault();
            target.f活動瀏覽次數 += 1;
            db.SaveChanges();
        }

        public CGetMore ReadMore(int memberID, int actID)
        {
            AddViewCounts(actID);

            var loginMember = db.tMember.Where(t => t.f會員編號 == memberID).FirstOrDefault();
            tActivity targetAct = db.tActivity.Where(t => t.f活動編號 == actID).FirstOrDefault();
            if (targetAct.tMember.f會員評分 == null)
                targetAct.tMember.f會員評分 = 0.0;
            CGetMore result = new CGetMore();

            int index = -1;
            if (!string.IsNullOrEmpty(loginMember.f會員收藏的活動編號))
            {
                var analyze = loginMember.f會員收藏的活動編號.Split(',');
                index = Array.IndexOf(analyze, actID.ToString());
            }
            result.MemberLike = index > -1;

            string[] blackList = { };
            if (!string.IsNullOrEmpty(loginMember.f會員黑名單))
            {
                blackList = loginMember.f會員黑名單.Split(',');
            }
            result.BlackList = blackList;
            result.TargetAct = targetAct;
            result.LoginMember = loginMember;
            return result;
        }

        public CGetMore ReadMore(int actID)
        {
            AddViewCounts(actID);
            tActivity targetAct = db.tActivity.Where(t => t.f活動編號 == actID).FirstOrDefault();
            if (targetAct.tMember.f會員評分 == null)
                targetAct.tMember.f會員評分 = 0.0;
            CGetMore result = new CGetMore();
            result.TargetAct = targetAct;
            return result;
        }

        public void UserLikeIt(int memberID, int actID)
        {
            var targetAct = db.tActivity.Where(t => t.f活動編號 == actID).FirstOrDefault();
            //要儲存修改，所以不能用GetMemberData
            //var MemberLike = GetMemberData(memberID, "f會員收藏的活動編號");
            var memberLike = db.tMember.Where(t => t.f會員編號 == memberID).FirstOrDefault();
            if (!string.IsNullOrEmpty(memberLike.f會員收藏的活動編號))
            {
                string[] isExist = memberLike.f會員收藏的活動編號.Split(',');
                int pos = Array.IndexOf(isExist, actID.ToString());
                if (pos < 0)   //若沒找到，存入資料庫，正確
                {
                    targetAct.f有收藏活動的會員編號 += "," + memberID;
                    memberLike.f會員收藏的活動編號 += "," + actID;
                    db.SaveChanges();
                }
                else  //若找到
                {
                    var FinalList = isExist.ToList();
                    FinalList.RemoveAt(pos);  //移除
                    memberLike.f會員收藏的活動編號 = string.Join(",", FinalList);
                    targetAct.f有收藏活動的會員編號 = string.Join(",",
                        targetAct.f有收藏活動的會員編號.Split(',').Where(t => t != memberID.ToString()));
                    db.SaveChanges();
                }
            }
            else
            {
                targetAct.f有收藏活動的會員編號 += "," + memberID;
                memberLike.f會員收藏的活動編號 += "," + actID; //若資料庫完全是空的，則不可能有重複值，直接存入
                db.SaveChanges();
            }
        }

        public void KickEvent(int targetMemberID, int actID)
        {
            var actList = db.tActivity.Where(t => t.f活動編號 == actID).FirstOrDefault();
            var kickMember = db.tMember.Where(t => t.f會員編號 == targetMemberID).FirstOrDefault();

            //if (!string.IsNullOrEmpty(ActList.f活動參加的會員編號)) 因為有團主，活動必定有人參加            
            string[] GuysList = actList.f活動參加的會員編號.Split(',');

            //移除占用時間
            string[] usedTime = kickMember.f會員已占用時間.Split(',');
            kickMember.f會員已占用時間 =
                string.Join(",", usedTime.Where(t => t != actList.f活動開始時間 + "~" + actList.f活動結束時間));

            //移除會員資料參加的會員參加的活動編號
            string[] NewList = kickMember.f會員參加的活動編號.Split(',');
            kickMember.f會員參加的活動編號 =
                string.Join(",", NewList.Where(t => t != actList.f活動編號.ToString()));
            //移除活動紀錄的會員編號
            actList.f活動參加的會員編號 =
                string.Join(",", GuysList.Where(t => t != kickMember.f會員編號.ToString()));
            db.SaveChanges();

        }

        public string AgreeAddEvent(int targetMemberID, int actID, string isAgree)
        {
            var actList = db.tActivity.Where(t => t.f活動編號 == actID).FirstOrDefault();
            var targetMember = db.tMember.Where(t => t.f會員編號 == targetMemberID).FirstOrDefault();
            if (isAgree == "agree") //點允許入團
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
                            if (string.Compare(actList.f活動開始時間, used[1]) > 0 || string.Compare(used[0], actList.f活動結束時間) > 0)
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
                targetMember.f會員已占用時間 += "," + actList.f活動開始時間 + "~" + actList.f活動結束時間;

                targetMember.f會員參加的活動編號 += "," + actList.f活動編號;
                actList.f活動參加的會員編號 += "," + targetMember.f會員編號;
            }
            actList.f活動審核名單 = string.Join(",",
                actList.f活動審核名單.Split(',').Where(t => t != targetMember.f會員編號.ToString()));
            db.SaveChanges();
            return null;
        }

        public string ScoreAddEvent(int memberID, int actID, int Score)
        {
            var actList = db.tActivity.Where(x => x.f活動編號 == actID)
                  .FirstOrDefault();
            string[] isExist = actList.f活動參加的會員編號.Split(',');
            int index;
            var memberStartAct = GetMemberData(memberID, "f會員發起的活動編號");
            if (!string.IsNullOrEmpty(memberStartAct))
            {
                index = Array.IndexOf(memberStartAct.Split(','), actID.ToString());
                if (index != -1)//團主不可自行評分
                {
                    return "5";
                }
            }
            var result = string.Compare(DateTime.Now.ToString("yyyy-MM-dd"), actList.f活動結束時間);
            if (result < 0)  //result=1 活動已結束 ， result=-1 活動尚未結束
            {
                return "3"; //活動尚未結束
            }

            index = Array.IndexOf(isExist, memberID.ToString());
            if (index == -1) //先找有沒有參加本次活動
            {
                return "0";//沒參加
            }

            if (!string.IsNullOrEmpty(actList.f活動評分過的會員編號))
            {
                //若有曾經評分過的會員編號
                isExist = actList.f活動評分過的會員編號.Split(',');
                index = Array.IndexOf(isExist, memberID.ToString());//再找現在登入會員有沒有評分過
                if (index != -1)
                {
                    return "1"; //有評分過
                }
            }
            //若無曾經評分過的會員編號則直接往下

            //若通過上面判定則進行評分
            actList.f活動評分過的會員編號 += "," + memberID;

            //防止會員註冊遺漏設定
            if (actList.tMember.f會員評分人數 == null)
            {
                actList.tMember.f會員評分人數 = 0;
            }
            actList.tMember.f會員評分人數 += 1;
            if (actList.tMember.f會員總分 == null)
            {
                actList.tMember.f會員總分 = 0;
            }
            actList.tMember.f會員總分 += Score;
            if (actList.tMember.f會員評分 == null)
            {
                actList.tMember.f會員評分 = 0;
            }
            actList.tMember.f會員評分 =
                Math.Round(Convert.ToDouble(actList.tMember.f會員總分 / actList.tMember.f會員評分人數), 1);
            db.SaveChanges();
            return "2";
        }


        public dynamic AddBlackListEvent(int memberID, int targetMemberID)
        {
            var loginMember = db.tMember.Where(t => t.f會員編號 == memberID).FirstOrDefault();
            if (!string.IsNullOrEmpty(loginMember.f會員黑名單))
            {
                var blackList = loginMember.f會員黑名單.Split(',');
                if (Array.IndexOf(blackList, targetMemberID.ToString()) > -1) //已經加入黑單
                {
                    return "1";
                }
            }
            loginMember.f會員黑名單 += "," + targetMemberID;
            db.SaveChanges();
            return null;
        }

        public void MsgAddEvent(int memberID, int actID, string Msg)
        {
            var actList = db.tActivity.Where(t => t.f活動編號 == actID).FirstOrDefault();
            var loginMember = db.tMember.Where(t => t.f會員編號 == memberID).FirstOrDefault();
            actList.f活動留言 += "_^$" + loginMember.f會員名稱 + ":" + Msg;
            actList.f活動留言時間 += "," + DateTime.Now.ToString("MM/dd HH:mm:ss") + "_^$" + loginMember.f會員編號;
            db.SaveChanges();
        }

        public dynamic MsgAddEvent(int memberID, int actID)
        {
            var loginMember = db.tMember.Where(t => t.f會員編號 == memberID).FirstOrDefault();
            string[] blackList = { };
            if (!string.IsNullOrEmpty(loginMember.f會員黑名單))
            {
                blackList = loginMember.f會員黑名單.Split(',');
            }

            string[] msgs = { };
            string[] msgsTime = { };
            string[] msgsID = { };
            string[] msgsPic = { };
            string[] msgsTimeAndID = { };
            var targetAct = db.tActivity.Where(t => t.f活動編號 == actID).FirstOrDefault();
            if (!string.IsNullOrEmpty(targetAct.f活動留言))
            {
                msgs = targetAct.f活動留言.Split(new string[] { "_^$" }, StringSplitOptions.None);//因包含對話內容，
                                                                                              //因此利用複數特殊字元做分割
                msgsTimeAndID = targetAct.f活動留言時間.Split(',');
                msgsTime = new string[msgsTimeAndID.Length];
                msgsID = new string[msgsTimeAndID.Length];
                msgsPic = new string[msgsTimeAndID.Length];
                for (int i = 0; i <= msgsTimeAndID.Length - 1; i++)
                {
                    if (string.IsNullOrEmpty(msgsTimeAndID[i]))
                        continue;
                    var temp = msgsTimeAndID[i].Split(new string[] { "_^$" }, StringSplitOptions.None);
                    msgsTime[i] = temp[0];
                    msgsID[i] = temp[1];
                    int realID = Convert.ToInt32(msgsID[i]);
                    msgsPic[i] = db.tMember.Where(t => t.f會員編號 == realID).FirstOrDefault().f會員大頭貼;
                    if (string.IsNullOrEmpty(msgsPic[i]))//防止註冊沒有預設大頭貼
                        msgsPic[i] = "default.jpg";
                }
            }
            CMsg result = new CMsg();
            result.Msgs = msgs;
            result.MsgsTime = msgsTime;
            result.MsgPic = msgsPic;
            result.MsgID = msgsID;
            result.BlackList = blackList;
            result.ActID = actID;
            return result;

        }

        public dynamic ActAddEvent(int actID, bool isAdd, int memberID)
        {
            var actList = db.tActivity.Where(t => t.f活動編號 == actID).FirstOrDefault();
            string[] blackList = { };
            if (!string.IsNullOrEmpty(actList.tMember.f會員黑名單))
            {
                blackList = actList.tMember.f會員黑名單.Split(',');
            }

            var loginMember = db.tMember.Where(t => t.f會員編號 == memberID)
                         .FirstOrDefault();//因Session存取的資料沒有和資料庫內部做綁定
                                           //所以不能存取，要用Session登入會員的會員編號
                                           //撈出目前會員的資料
                                           //檢查登入會員是否為本活動團主，團主不可入團退團
            int index = -1; //先假設不是團主
            if (!string.IsNullOrEmpty(loginMember.f會員發起的活動編號)) //若登入會員有開團紀錄
            {
                string[] leaderList = loginMember.f會員發起的活動編號.Split(',');
                index = Array.IndexOf(leaderList, actID.ToString());
                if (index != -1)//若找到，表示是團主
                {
                    return "1";
                }
            }

            //if (!string.IsNullOrEmpty(ActList.f活動參加的會員編號)) 因為有團主，活動必定有人參加            
            string[] guysList = actList.f活動參加的會員編號.Split(',');
            index = Array.IndexOf(guysList, loginMember.f會員編號.ToString());  //尋找登入中的會員是否有參加
                                                                            //注意會員標號是int，陣列內容是str，
                                                                            //不轉型index永遠會是-1 

            if (isAdd == true)//點選入團
            {
                if (blackList.Length > 1) //黑名單有編號
                {
                    if (Array.IndexOf(blackList, loginMember.f會員編號.ToString()) > -1)//對象在黑名單
                    {
                        return "7";
                    }
                }

                if (index == -1)//登入中的會員不存在名單
                {
                    //判別活動時段是否已占用
                    if (!string.IsNullOrEmpty(loginMember.f會員已占用時間))
                    {
                        string[] usedTime = loginMember.f會員已占用時間.Split(',');
                        string[] used;
                        foreach (var item in usedTime)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                used = item.Split('~');  //used[0] 已佔用的開始時間，used[1] 已佔用的結束時間
                                if (string.Compare(actList.f活動開始時間, used[1]) > 0 || string.Compare(used[0], actList.f活動結束時間) > 0)
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
                    if (!string.IsNullOrEmpty(actList.f活動審核名單))
                    {
                        isAgreeList = actList.f活動審核名單.Split(',');
                        if (Array.IndexOf(isAgreeList, loginMember.f會員編號.ToString()) > -1)
                        {
                            return "8";
                        }
                    }
                    //進入審核
                    actList.f活動審核名單 += "," + loginMember.f會員編號;
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
                if (!string.IsNullOrEmpty(actList.f活動審核名單))
                {
                    isAgreeList = actList.f活動審核名單.Split(',');
                    if (Array.IndexOf(isAgreeList, loginMember.f會員編號.ToString()) > -1)
                    {
                        actList.f活動審核名單 = string.Join(",",
                             actList.f活動審核名單.Split(',').Where(t => t != loginMember.f會員編號.ToString()));
                        db.SaveChanges();
                        return null;
                    }
                }

                if (index != -1)//登入中的會員存在參加名單則讓他退出並更動占用時間
                {
                    //移除占用時間
                    string[] usedTime = loginMember.f會員已占用時間.Split(',');
                    loginMember.f會員已占用時間 =
                        string.Join(",", usedTime.Where(t => t != actList.f活動開始時間 + "~" + actList.f活動結束時間));

                    //移除會員資料參加的會員參加的活動編號
                    string[] NewList = loginMember.f會員參加的活動編號.Split(',');
                    loginMember.f會員參加的活動編號 =
                        string.Join(",", NewList.Where(t => t != actList.f活動編號.ToString()));

                    //移除活動紀錄的會員編號
                    actList.f活動參加的會員編號 =
                        string.Join(",", guysList.Where(t => t != loginMember.f會員編號.ToString()));

                    db.SaveChanges();
                }
                else //若會員不存在
                {
                    return "";
                }
            }
            return null;
        }

        public CActAdd ActAddEvent(int actID, int memberID)
        {
            var loginMember = db.tMember.Where(t => t.f會員編號 == memberID).FirstOrDefault();
            string[] blackList = { };
            if (!string.IsNullOrEmpty(loginMember.f會員黑名單))
            {
                blackList = loginMember.f會員黑名單.Split(',');
            }
            var targetAct = db.tActivity.Where(t => t.f活動編號 == actID).FirstOrDefault();
            string[] attendGuysID = { };
            tMember[] attendGuys = { };
            if (!string.IsNullOrEmpty(targetAct.f活動參加的會員編號))
            {
                attendGuysID = targetAct.f活動參加的會員編號.Split(',');
                attendGuys = new tMember[attendGuysID.Length];
                for (int i = 0; i < attendGuys.Length; i++)
                {
                    if (string.IsNullOrEmpty(attendGuysID[i]))
                        continue;
                    int temp = Convert.ToInt32(attendGuysID[i]);
                    attendGuys[i] = db.tMember.Where(t => t.f會員編號 == temp).FirstOrDefault();
                }
            }
            string[] wantGuysID = { };
            tMember[] wantGuys = { };
            if (!string.IsNullOrEmpty(targetAct.f活動審核名單))
            {
                wantGuysID = targetAct.f活動審核名單.Split(',');
                wantGuys = new tMember[wantGuysID.Length];
                for (int i = 0; i < wantGuys.Length; i++)
                {
                    if (string.IsNullOrEmpty(wantGuysID[i]))
                        continue;
                    int temp = Convert.ToInt32(wantGuysID[i]);
                    wantGuys[i] = db.tMember.Where(t => t.f會員編號 == temp).FirstOrDefault();
                }
            }
            CActAdd result = new CActAdd();
            result.TargetAct = targetAct;
            result.AttendGuys = attendGuys;
            result.BlackList = blackList;
            result.WantGuys = wantGuys;
            return result;
        }

        public void DeleteActivity(int memberID, int actID)
        {
            var targetAct = db.tActivity.Where(t => t.f活動編號 == actID).FirstOrDefault();
            var loginMember = db.tMember.Where(t => t.f會員編號 == memberID).FirstOrDefault();
            //移除團主的活動發起紀錄
            loginMember.f會員發起的活動編號 =
                string.Join(",", loginMember.f會員發起的活動編號.Split(',').Where(t => t != actID.ToString()));
            //撈出所有參加會員的編號，並讓他們退團並退收藏
            if (!string.IsNullOrEmpty(targetAct.f活動參加的會員編號))
            {
                string[] deleteList = targetAct.f活動參加的會員編號.Split(',');
                foreach (var item in deleteList)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        //移除活動編號
                        tMember delete = db.tMember.Where(t => t.f會員編號.ToString() == item).FirstOrDefault();
                        delete.f會員參加的活動編號 =
                            string.Join(",", delete.f會員參加的活動編號.Split(',').Where(t => t != actID.ToString()));

                        //移除占用時間
                        string[] usedTime = delete.f會員已占用時間.Split(',');
                        delete.f會員已占用時間 =
                            string.Join(",", usedTime.Where(t => t != targetAct.f活動開始時間 + "~" + targetAct.f活動結束時間));

                        //移除收藏
                        if (!string.IsNullOrEmpty(delete.f會員收藏的活動編號))
                        {
                            delete.f會員收藏的活動編號 = string.Join(",",
                                  delete.f會員收藏的活動編號.Split(',').Where(t => t != actID.ToString())
                                );
                        }
                    }
                }
            }
            db.tActivity.Remove(targetAct);
            db.SaveChanges();
        }

        public abstract dynamic AddAct(int memberID, tActivity act, HttpPostedFileBase picFile, string filePath);

        public dynamic EditAct(int memberID, tActivity act, HttpPostedFileBase picFile, string filePath)
        {
            tActivity targetAct = db.tActivity.Where(t => t.f活動編號 == act.f活動編號).FirstOrDefault();

            var loginMember = db.tMember.Where(t => t.f會員編號 == memberID).FirstOrDefault();
            string[] usedTime = { };
            if (!string.IsNullOrEmpty(loginMember.f會員已占用時間))
            {
                usedTime = loginMember.f會員已占用時間.Split(',');
                //先移除登入會員原本這筆活動的活動時段
                usedTime = usedTime.Where(t => t != targetAct.f活動開始時間 + "~" + targetAct.f活動結束時間).ToArray();
                //再判別修改的活動時段是否已占用 
                string[] used;
                foreach (var item in usedTime)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        used = item.Split('~');  //used[0] 已佔用的開始時間，used[1] 已佔用的結束時間
                        if (string.Compare(act.f活動開始時間, used[1]) > 0 || string.Compare(used[0], act.f活動結束時間) > 0)
                        {

                        }
                        else
                        {
                            return "1";
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
            loginMember.f會員已占用時間 = string.Join(",", usedTime) + "," + act.f活動開始時間 + "~" + act.f活動結束時間;
            loginMember.f會員參加的活動編號 += "," + targetAct.f活動編號; //因為被剔除了，所以重新添加
            targetAct.f活動內容 = act.f活動內容;
            targetAct.f活動參加的會員編號 = loginMember.f會員編號.ToString();
            targetAct.f活動地區 = act.f活動地區;
            targetAct.f活動招募截止時間 = act.f活動招募截止時間;
            targetAct.f活動標題 = act.f活動標題;
            targetAct.f活動結束時間 = act.f活動結束時間;
            targetAct.f活動開始時間 = act.f活動開始時間;
            targetAct.f活動預算 = act.f活動預算;
            targetAct.f活動經度 = act.f活動經度;
            targetAct.f活動緯度 = act.f活動緯度;
            targetAct.f活動審核名單 = null;

            if (picFile != null)
            {
                var NewFileName = Guid.NewGuid() + Path.GetExtension(picFile.FileName);
                var NewFilePath = Path.Combine(filePath, NewFileName);
                picFile.SaveAs(NewFilePath);
                targetAct.f活動團圖 = NewFileName;
            }
            db.SaveChanges();
            return null;
        }
    }

}