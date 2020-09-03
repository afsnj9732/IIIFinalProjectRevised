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
            return View(AJAXcondition(p).Where(a => a.f活動類型 == "旅遊").Select(a => a));
        }
        // GET: Travel
        public ActionResult TravelIndex()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TravelIndex(tActivity p)
        {
            if (Session["member"] != null && p.f活動標題 != null)  //揪團驗證，待改良
            {
                HttpPostedFileBase PicFile = Request.Files["PicFile"];
                if (PicFile != null)
                {
                    var NewFileName = Guid.NewGuid() + Path.GetExtension(PicFile.FileName);
                    var NewFilePath = Path.Combine(Server.MapPath("~/Content/images/"), NewFileName);
                    PicFile.SaveAs(NewFilePath);
                    p.f活動團圖 = NewFileName;
                }
                tMember Member = (tMember)Session["member"];
                p.f會員編號 = Member.f會員編號;
                dbJoutaEntities db = new dbJoutaEntities();
                db.tActivity.Add(p);
                db.SaveChanges();
            }
            return View();
        }

        public void ViewCounts(int ActivityID)
        {
            dbJoutaEntities db = new dbJoutaEntities();
            var target = db.tActivity.Where(t => t.f活動編號 == ActivityID).FirstOrDefault();
            target.f活動瀏覽次數 += 1;
            db.SaveChanges();
        }

        public void likeIt(string ActivityID)
        {
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
            //string[] MsgsList = ActList.f活動留言.Split(',');
            //index = Array.FindIndex(MsgsList,a=>a.StartsWith(NowMember.f會員名稱));  
            ActList.f活動留言 += ","+NowMember.f會員名稱 + ":" + sentMsg;
            db.SaveChanges();
            return View(target);
        }

        public ActionResult ActAdd(int target , bool isAdd)
        {
            var NowMember = (tMember)Session["member"];
            dbJoutaEntities db = new dbJoutaEntities();
            var ActList = db.tActivity.Where(n => n.f活動編號 == target).FirstOrDefault();
            string[] GuysList = { };
            int index = -1; //若此活動無人參加，則登入中的會員必定無參加

            if (!string.IsNullOrEmpty(ActList.f活動參加的會員編號)) //若此活動有人參加
            {
                GuysList = ActList.f活動參加的會員編號.Split(',');
                index = Array.IndexOf(GuysList, NowMember.f會員編號.ToString());  //尋找登入中的會員是否有參加
            }                                                                     //注意會員標號是int，陣列內容是str，
                                                                                  //不轉型index永遠會是-1
            if (isAdd == true)//點選入團
            {
                if (index == -1)//登入中的會員不存在名單則直接加入
                {
                    ActList.f活動參加的會員編號 += "," + NowMember.f會員編號;
                    db.SaveChanges();
                }
                else //若存在則要??
                {
                    return null;
                }
            }
            else //點選退出
            {
                if (index != -1)//登入中的會員存在則讓他退出
                {
                    var List = GuysList.ToList();
                    List.RemoveAt(index);
                    var FinalList = string.Join(",",List);
                    ActList.f活動參加的會員編號 = FinalList;
                    db.SaveChanges();
                }
                else //若不存在則要??
                {
                    return null;
                }
            }                                            
            return View(target);
        }

        //public JsonResult CountView(string target, string p)
        //{            
        //    if (target != null)
        //    {
        //        dbJoutaEntities db = new dbJoutaEntities();
        //        int select = Convert.ToInt32(target);
        //        tActivity theTarget = db.tActivity.FirstOrDefault(x => x.f活動編號 == select);
        //        theTarget.f活動瀏覽次數 = (theTarget.f活動瀏覽次數 + 1);
        //        db.SaveChanges();
        //    }

        //    var FinalList = AJAXcondition(p)
        //                    .Where(a=>a.f活動類型=="旅遊")
        //                    .Select(a => a.f活動瀏覽次數);

        //    return Json(FinalList, JsonRequestBehavior.AllowGet);
        //}
        public JsonResult FeelGood(string target, string p)
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
                    return Json("0", JsonRequestBehavior.AllowGet);
                }
            }
 
            var FinalList = AJAXcondition(p)
                            .Where(a => a.f活動類型 == "旅遊")
                            .Select(a => a.f活動讚數);

            return Json(FinalList, JsonRequestBehavior.AllowGet);
        }




    }
}