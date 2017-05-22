using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Lending_System.Models;

namespace Lending_System.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["UserId"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        public ActionResult LoadList()
        {
            var datetimenow = DateTime.Now;
            var datenow = datetimenow.Date;
            try
            {
                using (db_lendingEntities db = new db_lendingEntities())
                {

                    var data = db.tbl_loan_processing.Where(a => ((a.loan_date <= datenow) || (a.loan_date >= datenow && a.loan_date <= datenow)) && (a.status != "Released" && a.status != "Approved")).ToList();

                    return Json(new { data = data }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public JsonResult GetCashRelease()
        {
            try
            {
                var datetimenow = DateTime.Now;
                var datenow = datetimenow.Date;
                decimal balance = 0;
                db_lendingEntities db = new db_lendingEntities();
                {

                    var result = from d in db.tbl_loan_processing where d.status.Contains("Released") && (d.loan_date >= datenow && d.loan_date <= datenow) orderby d.autonum ascending select d;
                    if (result != null)
                    {
                        foreach (var data in result)
                        {
                            balance = balance + (decimal)data.total_receivable;
                        }
                    }
                }

                return Json(balance, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Failed", JsonRequestBehavior.DenyGet);
                throw ex;
            }
        }
        public JsonResult GetCashCollect()
        {
            try
            {
                var datetimenow = DateTime.Now;
                var datenow = datetimenow.Date;
                decimal balance = 0;
                db_lendingEntities db = new db_lendingEntities();
                {

                    var result = from d in db.tbl_payment where (d.date_trans >= datenow && d.date_trans <= datenow) orderby d.autonum ascending select d;
                    if (result != null)
                    {
                        foreach (var data in result)
                        {
                            balance = balance + (decimal)data.total_amount;
                        }
                    }
                }

                return Json(balance, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Failed", JsonRequestBehavior.DenyGet);
                throw ex;
            }
        }
        public JsonResult GetCashPullOut()
        {
            try
            {
                var datetimenow = DateTime.Now;
                var datenow = datetimenow.Date;
                decimal balance = 0;
                db_lendingEntities db = new db_lendingEntities();
                {

                    var result = from d in db.tbl_cash_out where (d.date_trans >= datenow && d.date_trans <= datenow) orderby d.autonum ascending select d;
                    if (result != null)
                    {
                        foreach (var data in result)
                        {
                            balance = balance + (decimal)data.amount;
                        }
                    }
                }

                return Json(balance, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Failed", JsonRequestBehavior.DenyGet);
                throw ex;
            }
        }
    }
}