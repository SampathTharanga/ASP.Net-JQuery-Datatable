using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using Data_Sorting.Models;

namespace Data_Sorting.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        //Write action here for return json data
        [HttpPost]
        public ActionResult LoadData()
        {
            //get parameters

            

            //get start (paging start index) and length (page size for paging)
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();

            //get sort columns value
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;


            using (CustomerDatabaseEntities dc =new CustomerDatabaseEntities())
            {
                var v = (from a in dc.tblCustomers select a);
                //Sorting
                if(!string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir))
                {
                    v = v.OrderBy(sortColumn + " " + sortColumnDir);
                }

                totalRecords = v.Count();
                var data = v.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordFiltered = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);
            }
            
            /*
            using (CustomerDatabaseEntities dc = new CustomerDatabaseEntities())
            {
                var data = (from a in dc.tblCustomers select a);
                return Json(new { data = data }, JsonRequestBehavior.AllowGet);
            }
            */
        }
    }
}