using IIIProject_travel.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Script.Serialization;

namespace IIIProject_travel.Models
{
    public class TravelModel
    {
        dbJoutaEntities db = new dbJoutaEntities();

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

        public CTravel SortList(string condition) //活動列表+排序
        {
            int totalPage, nowPage;
            CTravel returnValue = new CTravel();
            CSelect obj = CSelectDeserialize(condition);
            var order = typeof(tActivity).GetProperty(obj.Order);//使用映射動態抓取排序條件
            var activityList = db.tActivity.AsEnumerable()
                .Where(t => t.f活動類型 == "旅遊").Select(t => t);

            activityList = (obj.BackgroundColor == "rgb(250, 224, 178)")
                ? activityList.OrderByDescending(t => order.GetValue(t, null))//降冪
                : activityList.OrderBy(t => order.GetValue(t, null));//升冪        

            if (!string.IsNullOrEmpty(obj.Contain)) //搜尋欄位若非空
            {
                activityList = activityList.Where(t => t.f活動標題.Contains(obj.Contain));
            }

            if (obj.Category != "所有")
            {
                activityList = activityList.Where(t => t.f活動分類 == obj.Category);
            }

            if (obj.Label != "全部")
            {
                activityList = activityList.Where(t => t.f活動讚數 > Convert.ToInt32(obj.Label));
            }

            //頁數判斷
            totalPage = (activityList.Count() % 4 == 0 && activityList.Count() != 0)
                ? activityList.Count() / 4
                : (activityList.Count() - activityList.Count() % 4) / 4 + 1;

            nowPage = (obj.Page == 0 || obj.Page > totalPage) ? 1 : obj.Page;

            activityList = activityList.Skip(4 * (nowPage - 1)).Take(4);

            returnValue.FinalList = activityList;
            returnValue.TotalPage = totalPage;
            returnValue.NowPage = nowPage;
            return returnValue;
        }

        //使用ID取得想要的會員其資料庫中最新的特定欄位之資料
        public dynamic GetMemberData(int memberID, string wantGet)
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

        public string AutoCompleteList()
        {
            var autoComplete = db.tActivity.Where(t => t.f活動類型 == "旅遊").Select(t => t.f活動標題).ToArray();
            return string.Join(",", autoComplete);
        }

        public string DateLimit(int memberID , int actID)
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

        public string FeelGood(int memberID , int actID)
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

        public tActivity ReadMore(int actID)
        {
            AddViewCounts(actID);
            tActivity targetAct = db.tActivity.Where(t => t.f活動編號 == actID).FirstOrDefault();
            return targetAct;
        }
    }
}