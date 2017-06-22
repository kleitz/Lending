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
        db_lendingEntities db = new db_lendingEntities();
        DateTime _serverDateTime = DateTime.Now;

        public ActionResult Index()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.Form = "Dashboard";
                ViewBag.Controller = "Home";
                ViewBag.Action = "Index";

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
                            balance = balance + (decimal)data.net_proceeds;
                        }
                    }
                    balance = decimal.Round((decimal)balance, 2, MidpointRounding.AwayFromZero);
                }
                var strBalance = balance.ToString("0.00");
                return Json(strBalance, JsonRequestBehavior.AllowGet);
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
                    balance = decimal.Round((decimal)balance, 2, MidpointRounding.AwayFromZero);
                }

                var strBalance = balance.ToString("0.00");
                return Json(strBalance, JsonRequestBehavior.AllowGet);
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

                var strBalance = balance;
                return Json(strBalance, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Failed", JsonRequestBehavior.DenyGet);
                throw ex;
            }
        }
        public JsonResult GetReceivablesForTheDay()
        {
            try
            {
                decimal? totalBalance = 0;
                using (db = new db_lendingEntities())
                {
                    DateTime dateVar = _serverDateTime;

                    var result = from d in db.tbl_loan_processing where (d.due_date <= dateVar) && d.status == "Released" orderby d.customer_name ascending select d;
                    foreach (var dt in result)
                    {

                        decimal? principal = decimal.Round((decimal)dt.loan_granted, 2, MidpointRounding.AwayFromZero);
                        decimal? principalInterest = dt.loan_granted * (dt.loan_interest_rate / 100);
                        decimal? interest = GetInterest(dt.loan_no);
                        decimal? additionalInterest = GetAdditionalInterest(dt.loan_no);
                        interest = decimal.Round((decimal)(principalInterest + interest + additionalInterest), 2, MidpointRounding.AwayFromZero);
                        decimal? payment = decimal.Round((decimal)GetPayments(dt.loan_no), 2, MidpointRounding.AwayFromZero);
                        decimal? balance = decimal.Round((decimal)(principal + interest - payment), 2, MidpointRounding.AwayFromZero);

                        if (balance > 0)
                        {
                            totalBalance = totalBalance + balance;
                        }
                    }

                    ViewBag.totalBalance = decimal.Round((decimal)totalBalance, 2, MidpointRounding.AwayFromZero);
                    ViewBag.totalBalance = String.Format("{0:0.00}", ViewBag.totalBalance);
                }

                return Json(ViewBag.totalBalance, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public decimal? GetLedgerBalance(string id)
        {
            using (db = new db_lendingEntities())
            {
                decimal? balance = 0;
                var result =
                    from d in db.tbl_loan_ledger
                    where d.loan_no.Equals(id)
                    select d;

                foreach (var data in result)
                {
                    switch (data.trans_type)
                    {
                        case "Beginning Balance":
                            balance = data.balance;
                            break;
                        case "Late Payment Interest":
                            balance = balance + decimal.Round((decimal)data.interest, 2, MidpointRounding.AwayFromZero);
                            break;
                        case "OR Payment":
                            balance = balance - decimal.Round((decimal)data.amount_paid, 2, MidpointRounding.AwayFromZero);
                            break;
                        case "OR Payment Interest":
                            balance = balance - decimal.Round((decimal)data.interest, 2, MidpointRounding.AwayFromZero);
                            break;
                        default:
                            break;
                    }
                }
                return balance;
            }
        }
        public decimal GetPayments(string id)
        {

            db = new db_lendingEntities();
            {
                decimal balance = 0;
                var result = from d in db.tbl_payment_details where d.loan_no.Equals(id) select d.amount;
                foreach (var data in result)
                {
                    balance = balance + decimal.Round((decimal)data.Value, 2, MidpointRounding.AwayFromZero);
                }

                return balance;
            }
        }
        public decimal? GetInterest(string id)
        {

            db = new db_lendingEntities();
            {
                decimal? interest = 0;
                var result =
                    from d in db.tbl_loan_ledger
                    where d.loan_no.Equals(id)
                    select d;

                foreach (var data in result)
                {
                    switch (data.trans_type)
                    {
                        case "Late Payment Interest":
                            interest = interest + decimal.Round((decimal)data.interest, 2, MidpointRounding.AwayFromZero);
                            break;
                        default:
                            break;
                    }
                }
                return interest;
            }
        }
        public decimal GetAdditionalInterest(string id)
        {
            using (db = new db_lendingEntities())
            {
                decimal totalInterest = 0;
                var result =
                    from d in db.tbl_loan_processing
                    where d.loan_no == id
                          && d.status == "Released"
                          && d.due_date < _serverDateTime
                    select d;
                foreach (var dt in result)
                {
                    var balance = (decimal)GetLedgerBalance(dt.loan_no);

                    if (balance > 0)
                    {
                        var interestRate = (decimal)dt.loan_interest_rate;
                        var noOfDays = 0;
                        var dateStart = GetInterestStartDate(dt.loan_no);

                        if (GetInterestType(dt.loan_name) == "1")
                        {
                            noOfDays = decimal.ToInt32((_serverDateTime - dateStart).Value.Days);
                        }
                        else
                        {
                            if ((decimal.ToInt32((_serverDateTime - dateStart).Value.Days)) >= 30)
                            {
                                noOfDays = (((_serverDateTime - dateStart).Value.Days)) / 30;
                            }
                        }

                        for (var c = 0; c < noOfDays; c++)
                        {
                            var interest = (balance * (interestRate / 100));
                            balance = balance + interest;
                            totalInterest = totalInterest + interest;
                        }
                    }
                }
                return totalInterest;
            }
        }
        public DateTime? GetInterestStartDate(string id)
        {
            using (db = new db_lendingEntities())
            {
                DateTime? dateStart = null;
                var result =
                    from d in db.tbl_loan_ledger
                    where d.loan_no.Equals(id)
                    select d;

                foreach (var data in result)
                {
                    switch (data.trans_type)
                    {
                        case "Beginning Balance":
                            if (data.interest_type == "1")
                            {
                                dateStart = data.date_trans.Value.AddDays(1);
                            }
                            else if (data.interest_type == "2")
                            {
                                dateStart = data.date_trans.Value.AddDays(30);
                            }
                            break;
                        case "Late Payment Interest":
                            dateStart = data.date_trans;
                            break;
                        default:
                            break;
                    }
                }
                return dateStart;
            }
        }
        public string GetInterestType(string id)
        {

            db = new db_lendingEntities();
            {
                var interest_type = "";
                var result = from d in db.tbl_loan_type where d.description == id select d;
                foreach (var data in result)
                {
                    interest_type = data.interest_type;
                }

                return interest_type;
            }
        }
    }
}