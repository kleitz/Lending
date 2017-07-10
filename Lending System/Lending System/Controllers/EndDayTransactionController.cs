using Lending_System.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Routing.Constraints;

namespace Lending_System.Controllers
{
    public class EndDayTransactionController : Controller
    {
        // GET: EndDayTransaction
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
        public JsonResult LoadList()
        {
            try
            {
                using (db_lendingEntities db = new db_lendingEntities())
                {
                    var endOfDayTransactionList = db.tbl_end_of_day_transactions.OrderByDescending(d => d.date_trans).ToList();

                    var result = from e in endOfDayTransactionList
                        select new
                        {
                            e.autonum,
                            e.date_trans,
                            e.cash_begin,
                            e.cash_release,
                            e.cash_collected,
                            e.cash_pulled_out,
                            e.cash_end
                        };

                    return Json(new { data = result.OrderByDescending(d => d.date_trans) }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                throw;
                throw;
            }
        }
        [HttpPost, ActionName("Save")]
        public ActionResult Save(tbl_end_of_day_transactions model)
        {
            try
            {
                db_lendingEntities db = new db_lendingEntities();
                var datetimenow = DateTime.Now;
                var datenow = datetimenow.Date;

                var result = from d in db.tbl_end_of_day_transactions where (d.date_trans >= datenow && d.date_trans <= datenow) orderby d.autonum ascending select d;
                if (result.Count() == 0)
                {
                    tbl_end_of_day_transactions tbl = new tbl_end_of_day_transactions();

                    tbl.date_trans = model.date_trans;
                    tbl.cash_begin = model.cash_begin;
                    tbl.cash_release = model.cash_release;
                    tbl.cash_collected = model.cash_collected;
                    tbl.cash_pulled_out = model.cash_pulled_out;
                    tbl.cash_end = model.cash_end;
                    tbl.created_by = Session["UserName"].ToString();
                    tbl.date_created = DateTime.Now;

                    db.tbl_end_of_day_transactions.Add(tbl);

                    db.SaveChanges();
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //"date_trans", "cash_begin", "cash_release", "cash_collected", "cash_pulled_out", "cash_end"
                    var update = db.tbl_end_of_day_transactions.SingleOrDefault(d => d.date_trans >= datenow && d.date_trans <= datenow);
                    if (TryUpdateModel(update, "",
                       new string[] { "date_trans", "cash_begin", "cash_release", "cash_collected", "cash_pulled_out", "cash_end" }))
                    {
                        db.SaveChanges();
                        return Json("Success", JsonRequestBehavior.AllowGet);
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                return Json("Failed", JsonRequestBehavior.DenyGet);
                throw ex;
            }
        }
        public JsonResult GetCashBegin()
        {
            try
            {
                var datetimenow = DateTime.Now;
                var datenow = datetimenow.Date;
                decimal balance = 0;
                db_lendingEntities db = new db_lendingEntities();
                {

                    var result = from d in db.tbl_end_of_day_transactions where (d.date_trans < datenow ) orderby d.autonum ascending select d;
                    if (result != null)
                    {
                        foreach (var data in result)
                        {
                            balance = (decimal)data.cash_end;
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
                            balance = balance + (decimal)data.net_proceeds;
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

        public ActionResult Print(int? id)
        {
            try
            {
                using (db_lendingEntities db = new db_lendingEntities())
                {
                    tbl_end_of_day_transactions tbl_end_of_day_transactions = db.tbl_end_of_day_transactions.Find(id);

                    if (Session["UserId"] != null)
                    {
                        return View(tbl_end_of_day_transactions);
                    }
                    else
                    {
                        return RedirectToAction("Login", "Account");
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult PrintDetails(int? id)
        {
            using (db_lendingEntities db = new db_lendingEntities())
            {
                tbl_end_of_day_transactions tbl = db.tbl_end_of_day_transactions.Find(id);
                DateTime? date = null;
                if (tbl != null)
                {
                    ViewBag.CashBeginning = decimal.Round((decimal)tbl.cash_begin, 2, MidpointRounding.AwayFromZero);
                    ViewBag.CashCollection = decimal.Round((decimal)tbl.cash_collected, 2, MidpointRounding.AwayFromZero);
                    ViewBag.CashEnd = decimal.Round((decimal)tbl.cash_end, 2, MidpointRounding.AwayFromZero);
                    ViewBag.CashRelease = decimal.Round((decimal)tbl.cash_release, 2, MidpointRounding.AwayFromZero);
                    ViewBag.CashPullOut = decimal.Round((decimal)tbl.cash_pulled_out, 2, MidpointRounding.AwayFromZero);
                    date = tbl.date_trans;
                }
                ViewBag.dateString = String.Format("{0:MMMM dd, yyyy}", date);
                var result1 = db.tbl_loan_processing.Where(d => d.status == "Released" && (d.loan_date >= date && d.loan_date <= date)).ToList();
                ViewBag.ReleaseList = result1;

                var result2 = db.tbl_payment.Where(d => (d.date_trans >= date && d.date_trans <= date)).ToList();
                ViewBag.CollectionList = result2;
            }

            return PartialView("PrintDetails");
        }
    }
}