using MultiColumnFilter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace MultiColumnFilter.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoadData()
        {
            //JQuery datatables parameters
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            //find paging info
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            //find order column info
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            //Find search column info
            var firsttName = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
            var country = Request.Form.GetValues("columns[5][search][value]").FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToUInt16(start) : 0;
            int recordsTotal = 0;

            using (MyDatabaseEntities dc =new MyDatabaseEntities())
            {
                var v = (from a in dc.CustomerTables select a);

                //SEAARCHING
                if (!string.IsNullOrEmpty(firsttName))
                    v = v.Where(a => a.FirstName.Contains(firsttName));

                if (!string.IsNullOrEmpty(country))
                    v = v.Where(a => a.Country == country);

                //SORTING... (for sorting we need to add a rederence System.Linq.Dynamic, Therefore Nuget package add)
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                    v = v.OrderBy(sortColumn + " " + sortColumnDir);

                recordsTotal = v.Count();
                var data = v.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}