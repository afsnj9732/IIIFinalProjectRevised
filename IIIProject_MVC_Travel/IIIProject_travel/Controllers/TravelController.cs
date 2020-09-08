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
        public ActionResult article_AJAX(string p)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            CSelect obj = serializer.Deserialize<CSelect>(p);
            var FinalList = AJAXcondition(p).Where(a => a.f活動類型 == "旅遊").Select(a => a);
            CTravel List = new CTravel();
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
        public ActionResult TravelIndex()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TravelIndex(int? id)
        {
            var HSCategory = Request.Form["txtTravelCategory"];
            var HSGood = Request.Form["txtTotalGood"];
            var HSKey = Request.Form["txtTravelKeyword"];

            return View();
        }

        public ActionResult Add(tActivity p)
        {
            //判別登入會員其活動時段是否已占用((未完成

            //添加占用時間((未完成


            tMember Member = (tMember)Session["member"];
            dbJoutaEntities db = new dbJoutaEntities();
            p.f會員編號 = Member.f會員編號;
            p.f活動類型 = "旅遊";
            p.f活動參加的會員編號 += "," + Member.f會員編號;
            db.tActivity.Add(p);
            int ID = db.tActivity.Where(t => t.f會員編號 == Member.f會員編號)
                     .Select(t => t.f活動編號).FirstOrDefault();
            tMember NowMember = db.tMember.Where(t => t.f會員編號 == Member.f會員編號).FirstOrDefault();
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
            dbJoutaEntities db = new dbJoutaEntities();
            var target = db.tActivity.Where(t => t.f活動編號 == id).FirstOrDefault();
            var NowMember = db.tMember.Where(t => t.f會員編號 == LoginMember.f會員編號).FirstOrDefault();
            NowMember.f會員發起的活動編號 =
                string.Join(",", NowMember.f會員發起的活動編號.Split(',').Where(t => t != id.ToString()));
            //撈出所有參加會員的編號，並讓他們退團
            if (!string.IsNullOrEmpty(target.f活動參加的會員編號))
            {
                string[] DeleteList = target.f活動參加的會員編號.Split(',');
                foreach (var item in DeleteList)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        //移除占用時間((未完成

                        //移除活動編號
                        tMember Delete = db.tMember.Where(t => t.f會員編號.ToString() == item).FirstOrDefault();
                        Delete.f會員參加的活動編號 =
                            string.Join(",", Delete.f會員參加的活動編號.Split(',').Where(t => t != id.ToString()));
                    }
                }
            }
            db.tActivity.Remove(target);
            db.SaveChanges();          
            return RedirectToAction("TravelIndex");
        }

        public dynamic ActAdd(int target, bool isAdd) //退團或入團
        {
            var LoginMember = (tMember)Session["member"];
            dbJoutaEntities db = new dbJoutaEntities();
            var ActList = db.tActivity.Where(t => t.f活動編號 == target).FirstOrDefault();

            //檢查登入會員是否為本活動團主，團主不可入團退團
            int index = -1; //先假設不是
            if (!string.IsNullOrEmpty(LoginMember.f會員發起的活動編號)) //若登入會員有開團紀錄
            {
                string[] LeaderList = LoginMember.f會員發起的活動編號.Split(',');
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

            var NowMember = db.tMember.Where(t => t.f會員編號 == LoginMember.f會員編號)
                         .Select(a => a).FirstOrDefault();//因Session存取的資料沒有和資料庫內部做綁定
                                                          //所以不能存取，要用Session登入會員的會員編號
                                                          //撈出目前會員的資料

            if (isAdd == true)//點選入團
            {
                //判別活動時段是否已占用((未完成


                if (index == -1)//活動時段未占用且登入中的會員不存在名單則加入
                {
                    //添加占用時間((未完成
                    


                    //增加會員資料參加的會員編號
                    NowMember.f會員參加的活動編號 += "," + ActList.f活動編號;
                    ActList.f活動參加的會員編號 += "," + LoginMember.f會員編號;
                    db.SaveChanges();
                }
                else //若會員已存在
                {
                    return "";
                }
            }
            else //點選退出
            {
                if (index != -1)//登入中的會員存在則讓他退出並更動占用時間
                {
                    //移除占用時間((未完成




                    //移除會員資料參加的會員編號
                    string[] NewList = NowMember.f會員參加的活動編號.Split(',');
                    NowMember.f會員參加的活動編號 =
                        string.Join(",", NewList.Where(t => t != ActList.f活動編號.ToString()));
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

        public object ScoreAdd(int target,int Score)
        {
            dbJoutaEntities db = new dbJoutaEntities();
            var NowMember = (tMember)Session["member"];
            var theActivity = db.tActivity.Where(x => x.f活動編號 == target)
                              .Select(a => a);
            string[] isExist = theActivity.Select(a => a.f活動參加的會員編號)
                               .FirstOrDefault().Split(',');
            var Member = db.tMember.Where(t => t.f會員編號 == NowMember.f會員編號).FirstOrDefault()
                .f會員發起的活動編號.Split(',');
            int pos = Array.IndexOf(Member, target.ToString());
            if (pos != -1)//團主不可自行評分
            {
                return "5";
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

        public void ViewCounts(int ActivityID)
        {
            dbJoutaEntities db = new dbJoutaEntities();
            var target = db.tActivity.Where(t => t.f活動編號 == ActivityID).FirstOrDefault();
            target.f活動瀏覽次數 += 1;
            db.SaveChanges();
        }

        public string likeIt(string ActivityID)
        {
            if (Session["member"] == null)
                return "0";
            dbJoutaEntities db = new dbJoutaEntities();
            var condition = (tMember)Session["member"];
            var member = db.tMember.Where(x => x.f會員編號 == condition.f會員編號).Select(a => a).FirstOrDefault();
            if (!string.IsNullOrEmpty(member.f會員收藏的活動編號))
            {
                string[] isExist = member.f會員收藏的活動編號.Split(',');
                int pos = Array.IndexOf(isExist, ActivityID);
                if (pos < 0)   //若沒找到，存入資料庫，正確
                {
                    member.f會員收藏的活動編號 += "," + ActivityID;
                    db.SaveChanges();
                }
                else  //若找到
                {                     
                    var FinalList = isExist.ToList();
                    FinalList.RemoveAt(pos);  //移除
                    member.f會員收藏的活動編號 = string.Join(",", FinalList); 
                    db.SaveChanges();
                }
            }
            else
            {
                member.f會員收藏的活動編號 += "," + ActivityID; //若資料庫完全是空的，則不可能有重複值，直接存入
                db.SaveChanges();
            }
            return "1";
        }

        public string autoComplete()
        {
            var x = from t in (new dbJoutaEntities()).tActivity
                    where t.f活動類型=="旅遊"
                    select t.f活動標題;

            return string.Join(",", x.ToArray());
        }

        public IEnumerable<tActivity> AJAXcondition(string p)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            CSelect obj = serializer.Deserialize<CSelect>(p);
            dbJoutaEntities db = new dbJoutaEntities();

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
            dbJoutaEntities db = new dbJoutaEntities();
            var ActList = db.tActivity.Where(n => n.f活動編號 == target).FirstOrDefault();
            ActList.f活動留言 += "_^$"+NowMember.f會員名稱 + ":" + sentMsg ;
            ActList.f活動留言時間 += "," + DateTime.Now.ToString("MM/dd HH:mm:ss") + "_^$" + NowMember.f會員編號;
            db.SaveChanges();
            return View(target);
        }



        public /*JsonResult*/ string FeelGood(string target)
        {           
            if (target != null&& Session["member"]!=null)
            {
                var temp = (tMember)Session["member"];                             
                dbJoutaEntities db = new dbJoutaEntities();
                int select = Convert.ToInt32(target);
                tActivity theTarget = db.tActivity.FirstOrDefault(x => x.f活動編號 == select);
                int pos = -1;
                if (!string.IsNullOrEmpty(theTarget.f活動按過讚的會員編號))
                {
                    var past = theTarget.f活動按過讚的會員編號.Split(',');//將按過讚得會員編號 字串 切割 成陣列

                    pos = Array.IndexOf(past, temp.f會員編號.ToString());//透過查詢值在陣列內的索引值(不存在則回傳-1)
                                                                         //查看是否會員編號包含在陣列內
                }

                if ( pos == -1 )//陣列起始為0，因此只要pos>=0則表示該編號已存在，反之pos=-1表示該編號不存在，可執行
                {
                theTarget.f活動讚數 = (theTarget.f活動讚數 + 1);
                theTarget.f活動按過讚的會員編號 += "," + temp.f會員編號;
                db.SaveChanges();
                }
                else
                {
                    return "0"; /*Json("0", JsonRequestBehavior.AllowGet);*/
                }
            }

            return "1"; /*Json(FinalList, JsonRequestBehavior.AllowGet)*/;
        }




    }
}