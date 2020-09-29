using IIIProject_travel.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Script.Serialization;

namespace IIIProject_travel.Models
{
    public class TravelModel:ActivityModel
    {

        public override CActivity SortList(string condition)
        {
            int totalPage, nowPage;
            CActivity returnValue = new CActivity();
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

        public override string AutoCompleteList()
        {
            var autoComplete = db.tActivity.Where(t => t.f活動類型 == "旅遊").Select(t => t.f活動標題).ToArray();
            return string.Join(",", autoComplete);
        }

        public override dynamic AddAct(int memberID, tActivity act, HttpPostedFileBase picFile, string filePath)
        {
            //判別登入會員其活動時段是否已占用
            var loginMember = db.tMember.Where(t => t.f會員編號 == memberID).FirstOrDefault();
            if (!string.IsNullOrEmpty(loginMember.f會員已占用時間))
            {
                string[] usedTime = loginMember.f會員已占用時間.Split(',');
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
            //添加占用時間
            loginMember.f會員已占用時間 += "," + act.f活動開始時間 + "~" + act.f活動結束時間;
            act.f會員編號 = memberID;
            act.f活動類型 = "旅遊";
            act.f活動參加的會員編號 = "," + memberID;
            var theCategory = Convert.ToDateTime(act.f活動結束時間) - Convert.ToDateTime(act.f活動開始時間);
            int timeCheck = Convert.ToInt32(theCategory.ToString("dd"));
            switch (timeCheck)//時間判斷
            {
                case 1:
                    act.f活動分類 = "兩天一夜";
                    break;
                case 2:
                    act.f活動分類 = "三天兩夜";
                    break;
                case 4:
                    act.f活動分類 = "五天四夜";
                    break;
                case 6:
                    act.f活動分類 = "七天六夜";
                    break;
                default:
                    act.f活動分類 = "其他";
                    break;
            }
            db.tActivity.Add(act);
            db.SaveChanges();
            int ID = db.tActivity.Where(t => t.f會員編號 == memberID)
                .OrderByDescending(t => t.f活動發起日期).Select(t => t.f活動編號).FirstOrDefault();
            loginMember.f會員發起的活動編號 += "," + ID;
            loginMember.f會員參加的活動編號 += "," + ID;

            if (picFile != null)
            {
                var newFileName = Guid.NewGuid() + Path.GetExtension(picFile.FileName);
                var newFilePath = Path.Combine(filePath, newFileName);
                picFile.SaveAs(newFilePath);
                act.f活動團圖 = newFileName;
            }
            db.SaveChanges();
            return null;
        }


    }
}