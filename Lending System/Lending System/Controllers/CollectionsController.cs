﻿using Lending_System.Models;
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
    public class CollectionsController : Controller
    {
        db_lendingEntities db = new db_lendingEntities();

        // GET: Collections
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

                    var data = db.tbl_payment.OrderByDescending(a => a.autonum).ToList();

                    return Json(new { data = data }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult Create()
        {
            db_lendingEntities db = new db_lendingEntities();

            if (Session["UserId"] != null)
            {
                LoadCustomer();
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        //Functions
        public void LoadCustomer()
        {
            try
            {
                var CustomerList = new List<SelectListItem>();
                var dbQuery = from d in db.tbl_customer select d;
                foreach (var d in dbQuery)
                {
                    if (d.firstname != "" || d.firstname != null)
                    {
                        CustomerList.Add(new SelectListItem { Value = d.autonum.ToString(), Text = d.lastname + ", " + d.firstname + " " + d.middlename });
                    }
                }
                ViewBag.Customer = new SelectList(CustomerList, "Value", "Text");
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<ActionResult> getReferenceNo()
        {
            try
            {
                using (db_lendingEntities db = new db_lendingEntities())
                {
                    if (db.tbl_payment.Any())
                    {
                        // The table is empty
                        var data = await db.tbl_payment.MaxAsync(a => a.autonum);
                        if (data == 0)
                        {
                            return HttpNotFound();
                        }
                        return Json(data + 1, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(1, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (DataException)
            {
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult LoadPrincipalDues(long? id)
        {
            db_lendingEntities db = new db_lendingEntities();

            if (Session["UserId"] != null)
            {
                decimal balance = 0;
                decimal payments = 0;
                decimal total_balance = 0;
                decimal loan_granted = 0;
                decimal interest_rate = 0;
                decimal interest = 0;

                List<collectionslist> collectionslist = new List<collectionslist>();
                collectionslist collection = new collectionslist();

                var result = from d in db.tbl_loan_processing where d.customer_id == id && d.status == "Released" orderby d.loantype_id ascending select d;
                foreach (var dt in result)
                {
                    var interest_type = GetInterestType(dt.loan_name);

                    balance = GetBalance(dt.loan_no);
                    payments = GetPayments(dt.loan_no);
                    total_balance = balance - payments;

                    interest_rate = (decimal)dt.loan_interest_rate;
                    loan_granted = (decimal)dt.loan_granted;
                    interest = loan_granted * (interest_rate / 100);

                    if (interest_type == "1")
                    {
                        total_balance = total_balance - interest;
                    }
                    
                    if (total_balance > 0)
                    {
                        collectionslist.Add(new collectionslist { loan_no = dt.loan_no, loan_type = dt.loan_name, due_date = dt.due_date, amount_due = total_balance, payment = 0, interest = dt.loan_interest_rate, interest_type = GetInterestType(dt.loan_name) });
                    }
                }
                        
                var data = collectionslist.ToList();
                return Json(new { data = data }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult LoadInterestDues(long? id)
        {
            db_lendingEntities db = new db_lendingEntities();

            var datetimenow = DateTime.Now;
            var datenow = datetimenow.Date;

            if (Session["UserId"] != null)
            {
                decimal balance = 0;
                decimal payments = 0;
                decimal total_balance = 0;
                decimal loan_granted = 0;
                decimal interest_rate = 0;        
                decimal interest = 0;
                decimal interest_balance = 0;

                List<collectionslist> collectionslist = new List<collectionslist>();
                collectionslist collection = new collectionslist();

                var result = from d in db.tbl_loan_processing where d.customer_id == id && d.status == "Released" orderby d.date_created select d;
                foreach (var dt in result)
                {
                    balance = GetBalance(dt.loan_no);
                    payments = GetPaymentsInterest(dt.loan_no);
                    total_balance = balance - payments;

                    interest_rate = (decimal)dt.loan_interest_rate;
                    loan_granted = (decimal)dt.loan_granted;
                    interest = loan_granted * (interest_rate / 100);
                    interest_balance = interest - payments;

                    var interest_type = GetInterestType(dt.loan_name);

                    DateTime now = DateTime.Now;

                    if (now > dt.due_date)
                    {
                        if (total_balance > 0)
                        {
                            if (interest_balance > 0)
                            {
                                collectionslist.Add(new collectionslist { loan_no = dt.loan_no, loan_type = dt.loan_name, due_date = dt.due_date, amount_due = interest_balance, payment = 0, interest = dt.loan_interest_rate, interest_type = GetInterestType(dt.loan_name) });
                                collectionslist.Add(new collectionslist { loan_no = dt.loan_no, loan_type = dt.loan_name, due_date = dt.due_date, amount_due = interest, payment = 0, interest = dt.loan_interest_rate, interest_type = GetInterestType(dt.loan_name) });
                            }
                            else
                            {
                                collectionslist.Add(new collectionslist { loan_no = dt.loan_no, loan_type = dt.loan_name, due_date = dt.due_date, amount_due = interest, payment = 0, interest = dt.loan_interest_rate, interest_type = GetInterestType(dt.loan_name) });
                            }

                        }
                    }
                    else
                    {
                        if (interest_type == "1")
                        {
                            if (interest_balance > 0)
                            {
                                collectionslist.Add(new collectionslist { loan_no = dt.loan_no, loan_type = dt.loan_name, due_date = dt.due_date, amount_due = interest_balance, payment = 0, interest = dt.loan_interest_rate, interest_type = GetInterestType(dt.loan_name) });
                            }
                        }
                        else
                        {

                        }
                    }
                }

                var data = collectionslist.ToList();
                return Json(new { data = data }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
        }
        public decimal GetBalance(string id)
        {
           
            db_lendingEntities db = new db_lendingEntities();
            {
                decimal balance = 0;
                var result = from d in db.tbl_loan_processing where d.loan_no.Contains(id) select d.total_receivable;
                foreach (var data in result)
                {
                    balance = data.Value;
                }

                return balance;
            }           
        }
        public decimal GetPayments(string id)
        {

            db_lendingEntities db = new db_lendingEntities();
            {
                decimal balance = 0;
                var result = from d in db.tbl_payment_details where d.loan_no.Contains(id) && d.payment_type.Equals("OR Payment") select d.amount;
                foreach (var data in result)
                {
                    balance = balance + data.Value;
                }

                return balance;
            }
        }
        public decimal GetPaymentsInterest(string id)
        {
            db_lendingEntities db = new db_lendingEntities();
            {
                decimal balance = 0;
                var result = from d in db.tbl_payment_details where d.loan_no.Contains(id) && d.payment_type.Contains("OR Payment Interest") select d.amount;
                foreach (var data in result)
                {
                    balance = balance + data.Value;
                }

                return balance;
            }
        }
        public string GetInterestType(string id)
        {

            db_lendingEntities db = new db_lendingEntities();
            {
                var interest_type = "";
                var result = from d in db.tbl_loan_type where d.description.Contains(id) select d;
                foreach (var data in result)
                {
                    interest_type = data.interest_type;
                }

                return interest_type;
            }
        }
        //SAVING
        [HttpPost]
        public JsonResult SavePayment(tbl_payment model)
        {
            try
            {
                db_lendingEntities db = new db_lendingEntities();

                tbl_payment tbl = new tbl_payment();

                tbl.reference_no = model.reference_no;
                tbl.date_trans = model.date_trans;
                tbl.payor_id = model.payor_id;
                tbl.payor_name = model.payor_name;
                tbl.total_amount = model.total_amount;
                tbl.created_by = Session["UserName"].ToString();
                tbl.date_created = DateTime.Now;

                db.tbl_payment.Add(tbl);

                db.SaveChanges();

                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Failed", JsonRequestBehavior.DenyGet);
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult SavePaymentDetails(tbl_payment_details model)
        {
            try
            {
                db_lendingEntities db = new db_lendingEntities();

                tbl_payment_details tbl = new tbl_payment_details();

                tbl.reference_no = model.reference_no;
                tbl.payment_type = model.payment_type;
                tbl.loan_no = model.loan_no;
                tbl.loan_name = model.loan_name;
                tbl.due_date = model.due_date;
                tbl.amount = model.amount;
                tbl.created_by = Session["UserName"].ToString();
                tbl.date_created = DateTime.Now;

                db.tbl_payment_details.Add(tbl);

                db.SaveChanges();

                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Failed", JsonRequestBehavior.DenyGet);
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult SaveLedger(tbl_loan_ledger model)
        {
            try
            {
                db_lendingEntities db = new db_lendingEntities();

                tbl_loan_ledger tbl = new tbl_loan_ledger();

                tbl.date_trans = model.date_trans;
                tbl.trans_type = model.trans_type;
                tbl.reference_no = model.reference_no;
                tbl.loan_no = model.loan_no;
                tbl.loan_type_name = model.loan_type_name;
                tbl.customer_id = model.customer_id;
                tbl.customer_name = model.customer_name;
                tbl.interest_type = model.interest_type;
                tbl.interest_rate = model.interest_rate;
                tbl.interest = model.interest;
                tbl.amount_paid = model.amount_paid;
                tbl.principal = model.principal;
                tbl.balance = model.balance;
                tbl.date_created = DateTime.Now;
                tbl.created_by = Session["UserName"].ToString();

                db.tbl_loan_ledger.Add(tbl);

                db.SaveChanges();

                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Failed", JsonRequestBehavior.DenyGet);
                throw ex;
            }
        }
        //VIEWING
        public ActionResult Details(int? id)
        {
            try
            {
                using (db_lendingEntities db = new db_lendingEntities())
                {
                    tbl_payment tbl_payment = db.tbl_payment.Find(id);

                    if (Session["UserId"] != null)
                    {
                        return View(tbl_payment);
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
        public JsonResult ViewPrincipalDues(string id)
        {
            db_lendingEntities db = new db_lendingEntities();
            try
            {
                if (Session["UserId"] != null)
                {
                    var result = from d in db.tbl_payment_details where d.reference_no == id && d.payment_type == "OR Payment" orderby d.loan_no, d.payment_type ascending select d;

                    return Json(new { data = result }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Failed", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public JsonResult ViewInterestDues(String id)
        {
            db_lendingEntities db = new db_lendingEntities();
            try
            {
                if (Session["UserId"] != null)
                {
                    var result = from d in db.tbl_payment_details where d.reference_no == id && d.payment_type == "OR Payment Interest" orderby d.loan_no, d.payment_type ascending select d;

                    return Json(new { data = result }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Failed", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public JsonResult LoadRePrint(String id)
        {
            db_lendingEntities db = new db_lendingEntities();
            try
            {
                if (Session["UserId"] != null)
                {
                    List<tbl_payment_details> list = new List<tbl_payment_details>();
                    var result = from d in db.tbl_payment_details where d.reference_no == id orderby d.loan_no, d.payment_type ascending select d;

                    foreach (var dt in result)
                    {
                        list.Add(new tbl_payment_details { reference_no = dt.reference_no, loan_name = dt.loan_name, amount = dt.amount});
                    }

                    var data = list.ToList();
                    return Json(new { data = data }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Failed", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult Print(String id)
        {

            try
            {
                if (Session["UserId"] != null)
                {
                    db_lendingEntities db = new db_lendingEntities();
                    List<receiptlist> list = new List<receiptlist>();

                    ViewBag.receiptno = id.ToString().PadLeft(5, '0'); ;
                    ViewBag.receiptdate = DateTime.Now.ToString("MM/dd/yyyy");

                    decimal total_amount_paid = 0;
                    var result = from d in db.tbl_payment_details where d.reference_no == id  orderby d.loan_no, d.payment_type ascending select d;

                    foreach (var dt in result)
                    {
                        if (ViewBag.borrower == "" || ViewBag.borrower == null)
                        {
                            ViewBag.borrower = GetBorrower(id);
                        }
                        if (ViewBag.borrower_id == "" || ViewBag.borrower_id == null)
                        {
                            ViewBag.borrower_id = GetBorrowerid(id);
                        }                           
                        if (dt.payment_type == "OR Payment")
                        {
                            list.Add(new receiptlist { reference_no = dt.loan_no, particulars = "Principal", amount = String.Format("{0:0.00}", dt.amount) });
                        }
                        else
                        {
                            list.Add(new receiptlist { reference_no = dt.loan_no, particulars = "Interest", amount = String.Format("{0:0.00}", dt.amount) });
                        }

                        total_amount_paid = total_amount_paid + (decimal)dt.amount;
                    }
                    ViewBag.total_amount_paid = String.Format("{0:0.00}", total_amount_paid);
                    ViewBag.receiptdetailslist = list;

                    List<receiptbalancelist> list2 = new List<receiptbalancelist>();

                    int customerid = Convert.ToInt32(GetBorrowerid(id));

                    var result2 = from d in db.tbl_loan_processing where d.customer_id == customerid select d.loan_no;

                    foreach (var dt2 in result2)
                    {
                        if (GetBalance(dt2) > 0){
                            list2.Add(new receiptbalancelist { loan_no = dt2, balance = String.Format("{0:0.00}", GetBalance(dt2)) });
                        }                
                    }

                    ViewBag.receiptdetailsbalancelist = list2;

                    return PartialView("Print");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }          
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string GetBorrower(string id)
        {

            db_lendingEntities db = new db_lendingEntities();
            {
                var borrower = "";
                var result = from d in db.tbl_payment where d.reference_no.Equals(id) select d.payor_name;
                foreach (var data in result)
                {
                    borrower =  data;
                }
                return borrower;
            }
        }
        public string GetBorrowerid(string id)
        {

            db_lendingEntities db = new db_lendingEntities();
            {
                var borrowerid = "";
                var result = from d in db.tbl_payment where d.reference_no.Equals(id) select d.payor_id;
                foreach (var data in result)
                {
                    borrowerid = data.ToString();
                }
                return borrowerid;
            }
        }
    }
}