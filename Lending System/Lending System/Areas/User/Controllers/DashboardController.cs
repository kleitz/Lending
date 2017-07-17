using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lending_System.Areas.User.Controllers
{
    public class DashboardController : Controller
    {
        // GET: User/Dashboard
        public ActionResult Index()
        {
            ViewBag.Form = "Dashboard";
            ViewBag.Controller = "Dashboard";
            ViewBag.Action = "Dashboard";
            return View();
        }
    }
}