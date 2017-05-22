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

namespace Lending_System.Controllers
{
    public class LoanLedgerController : Controller
    {
        db_lendingEntities db = new db_lendingEntities();
        // GET: LoanLedger
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
            try
            {
                using (db_lendingEntities db = new db_lendingEntities())
                {
                    //var data = db.tbl_loan_processing.Where(a => a.loan_date >= DateTime.Now && a.loan_date <= DateTime.Now).ToList();
                    var data = db.tbl_loan_processing.OrderByDescending(a => a.autonum).ToList();

                    //var data = from d in db.tbl_loan_processing select new { d.autonum, d.loan_date, d.loan_no, d.loan_name, d.loan_granted };

                    return Json(new { data = data }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult LoadLedger(string id)
        {
            try
            {
                using (db_lendingEntities db = new db_lendingEntities())
                {
                    decimal counter = 0;
                    decimal balance = 0;

                    List<ledger> ledger = new List<ledger>();

                    var result = db.tbl_loan_ledger.Where(a => a.loan_no == id).ToList().OrderBy(a => a.autonum);
                    foreach (var dt in result)
                    {
                        if (counter == 0)
                        {
                            ledger.Add(new ledger { autonum = dt.autonum, date_trans = dt.date_trans, trans_type = dt.trans_type, reference_no = dt.reference_no, loan_no = dt.loan_no, loan_type_name = dt.loan_type_name, customer_id = dt.customer_id, customer_name = dt.customer_name, interest_type = dt.interest_type, interest_rate = dt.interest_rate, interest = dt.interest, amount_paid = dt.amount_paid, principal = dt.principal, balance = dt.balance });
                            balance = (decimal)dt.balance;
                            counter = counter + 1;
                        }
                        else
                        {
                            balance = balance - (decimal)dt.amount_paid;
                            ledger.Add(new ledger { autonum = dt.autonum, date_trans = dt.date_trans, trans_type = dt.trans_type, reference_no = dt.reference_no, loan_no = dt.loan_no, loan_type_name = dt.loan_type_name, customer_id = dt.customer_id, customer_name = dt.customer_name, interest_type = dt.interest_type, interest_rate = dt.interest_rate, interest = dt.interest, amount_paid = dt.amount_paid, principal = dt.principal, balance = balance });                     
                            counter = counter + 1;
                        }

                    }

                    var data = ledger.ToList();
                    return Json(new { data = data }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}