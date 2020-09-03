using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace IIIProject_travel.Controllers
{
    public class 學生Controller : Controller
    {
        dbJoutaEntities db = new dbJoutaEntities();
        // GET: 學生
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.currentFilter = sortOrder;
            ViewBag.名稱排序 = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.帳號排序 = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            var 學生 = from s in db.tMember
                           select s;
            if (!string.IsNullOrEmpty(searchString))
            {
                學生 = 學生.Where(s => s.f會員名稱.Contains(searchString)|| 
                                      s.f會員帳號.Contains(searchString)
                            );
            }
            switch (sortOrder)
            {
                case "name_desc":
                    學生 = 學生.OrderByDescending(s => s.f會員名稱);
                    break;
                default:
                    學生 = 學生.OrderBy(s => s.f會員名稱);
                    break;
                case "Date":
                    學生 = 學生.OrderBy(s => s.f會員帳號);
                    break;
                case "date_desc":
                    學生 = 學生.OrderByDescending(s => s.f會員帳號);
                    break;
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(學生.ToPagedList(pageNumber, pageSize));
        }
    }
}