using Lending_System.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Lending_System.Controllers
{
    public class ReceivablesController : Controller
    {
        db_lendingEntities db = new db_lendingEntities();
        DateTime _serverDateTime = DateTime.Now;

        // GET: Receivables
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

        public ActionResult Print(string date)
        {
            if (Session["UserId"] != null)
            {
                try
                {
                    using (db = new db_lendingEntities())
                    {
                        DateTime dateVar = DateTime.Parse(date);
                        decimal? totalBalance = 0;
                        decimal? Balance1 = 0;
                        decimal? Balance2 = 0;
                        decimal? Balance3 = 0;

                        List<receivables> receivablesList = new List<receivables>();
                        List<receivables> receivablesList1 = new List<receivables>();
                        List<receivables> receivablesList2 = new List<receivables>();
                        var count = 0;
                        var count1 = 0;
                        var count2 = 0;

                        var result = from d in db.tbl_loan_processing where (d.due_date <= dateVar) && d.status == "Released" orderby d.customer_name ascending select d;
                        foreach (var dt in result)
                        {

                            decimal? principal = decimal.Round((decimal)dt.loan_granted, 2, MidpointRounding.AwayFromZero);
                            decimal? principalInterest = dt.loan_granted * (dt.loan_interest_rate/100);
                            decimal? interest = GetInterest(dt.loan_no);
                            decimal? additionalInterest = GetAdditionalInterest(dt.loan_no);
                            interest = decimal.Round((decimal)(principalInterest+interest+additionalInterest), 2, MidpointRounding.AwayFromZero);
                            decimal? payment = decimal.Round((decimal)GetPayments(dt.loan_no), 2, MidpointRounding.AwayFromZero);
                            decimal? balance =  decimal.Round((decimal)(principal+interest-payment), 2, MidpointRounding.AwayFromZero);

                            if (balance > 0)
                            {
                                if (GetInterestType(dt.loan_name) == "1")
                                {
                                    receivablesList.Add(new receivables { loanNo = dt.loan_no, customerName = dt.customer_name.ToString().ToUpperInvariant(), dueDate = String.Format("{0:MM/dd/yyyy}", dt.due_date), principal = String.Format("{0:n}", principal), interest = String.Format("{0:n}", interest), payment = String.Format("{0:n}", payment), balance = String.Format("{0:n}", balance) });
                                    Balance1 = Balance1 + balance;
                                    count += 1;
                                }
                                else if (GetInterestType(dt.loan_name) == "2")
                                {
                                    receivablesList1.Add(new receivables { loanNo = dt.loan_no, customerName = dt.customer_name.ToString().ToUpperInvariant(), dueDate = String.Format("{0:MM/dd/yyyy}", dt.due_date), principal = String.Format("{0:n}", principal), interest = String.Format("{0:n}", interest), payment = String.Format("{0:n}", payment), balance = String.Format("{0:n}", balance) });
                                    Balance2 = Balance2 + balance;
                                    count1 += 1;
                                }
                                else if (GetInterestType(dt.loan_name) != "1" && GetInterestType(dt.loan_name) != "2")
                                {
                                    receivablesList2.Add(new receivables { loanNo = dt.loan_no, customerName = dt.customer_name.ToString().ToUpperInvariant(), dueDate = String.Format("{0:MM/dd/yyyy}", dt.due_date), principal = String.Format("{0:n}", principal), interest = String.Format("{0:n}", interest), payment = String.Format("{0:n}", payment), balance = String.Format("{0:n}", balance) });
                                    Balance3 = Balance3 + balance;
                                    count2 += 1;
                                }
                         
                                totalBalance = totalBalance + balance;
                            }
                        }

                        ViewBag.dateString = String.Format("{0:MMMM dd, yyyy}", date);

                        ViewBag.receivableList = receivablesList;
                        ViewBag.receivableList1 = receivablesList1;
                        ViewBag.receivableList2 = receivablesList2;

                        ViewBag.count = count;
                        ViewBag.count1 = count1;
                        ViewBag.count2 = count2;

                        ViewBag.Balance1 = decimal.Round((decimal)Balance1, 2, MidpointRounding.AwayFromZero);
                        ViewBag.Balance1 = String.Format("{0:n}", ViewBag.Balance1);

                        ViewBag.Balance2 = decimal.Round((decimal)Balance2, 2, MidpointRounding.AwayFromZero);
                        ViewBag.Balance2 = String.Format("{0:n}", ViewBag.Balance2);

                        ViewBag.Balance3 = decimal.Round((decimal)Balance3, 2, MidpointRounding.AwayFromZero);
                        ViewBag.Balance3 = String.Format("{0:n}", ViewBag.Balance3);

                        ViewBag.totalBalance = decimal.Round((decimal)totalBalance, 2, MidpointRounding.AwayFromZero);
                        ViewBag.totalBalance = String.Format("{0:n}", ViewBag.totalBalance);                      
                    }           
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
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
                            balance = balance + data.interest;
                            break;
                        case "OR Payment":
                            balance = balance - data.amount_paid;
                            break;
                        case "OR Payment Interest":
                            balance = balance - data.interest;
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
                    balance = balance + data.Value;
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
                            interest = interest + data.interest;
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