using MVC_Data_Table.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Data_Table.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        //Write action for return database data
        public ActionResult loaddata()
        {
            using (MyDatabaseEntities dc = new MyDatabaseEntities())
            {
                var data = dc.Cusomers.OrderBy(a => a.CompanyName).ToList();
                return Json(new { data = data }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}