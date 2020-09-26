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

        public ViewModel.CTravel SortList(string condition) //活動列表+排序
        {
            int totalPage, nowPage;
            ViewModel.CTravel returnValue = new ViewModel.CTravel();
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
        public dynamic GetMemberData(int memberID,string wantGet) 
        {
            var targetMember = db.tMember.Where(t => t.f會員編號 == memberID).FirstOrDefault();

            //使用映射動態抓取想要的會員欄位之資料
            var targetData = targetMember.GetType().GetProperty(wantGet).GetValue(targetMember);
            return targetData;
        }


    }
}