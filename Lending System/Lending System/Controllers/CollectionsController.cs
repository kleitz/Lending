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
    public class CollectionsController : Controller
    {
        db_lendingEntities db = new db_lendingEntities();
        DateTime _serverDateTime = DateTime.Now;

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

                    var data = db.tbl_payment.OrderBy(a => a.autonum).ToList();

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
                using (db = new db_lendingEntities())
                {
                    var customerList = new List<SelectListItem>();
                    var dbQuery = from d in db.tbl_customer select d;
                    foreach (var d in dbQuery)
                    {
                        if ((d.firstname != null) && (d.lastname != null) && (d.middlename != null))
                        {
                            customerList.Add(new SelectListItem { Value = d.autonum.ToString(), Text = d.lastname.ToUpper() + ", " + d.firstname.ToUpper() + " " + d.middlename.ToUpper() });
                        }
                        if ((d.firstname != null) && (d.lastname != null) && (d.middlename == null))
                        {
                            customerList.Add(new SelectListItem { Value = d.autonum.ToString(), Text = d.lastname.ToUpper() + ", " + d.firstname.ToUpper() });
                        }
                        if ((d.firstname != null) && (d.lastname == null) && (d.middlename == null))
                        {
                            customerList.Add(new SelectListItem { Value = d.autonum.ToString(), Text =  d.firstname.ToUpper() });
                        }
                    }
                    ViewBag.Customer = new SelectList(customerList, "Value", "Text");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
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
                decimal paymentsInterest = 0;

                List<collectionslist> collectionslist = new List<collectionslist>();
                collectionslist collection = new collectionslist();

                var result = from d in db.tbl_loan_processing where d.customer_id == id && d.status == "Released" orderby d.loantype_id ascending select d;
                foreach (var dt in result)
                {
                    var interest_type = GetInterestType(dt.loan_name);

                    balance = GetBalance(dt.loan_no);
                    payments = GetPayments(dt.loan_no);
                    paymentsInterest = GetPaymentsInterest(dt.loan_no);
                    total_balance = balance - payments;

                    interest_rate = (decimal)dt.loan_interest_rate;
                    loan_granted = (decimal)dt.loan_granted;
                    interest = loan_granted * (interest_rate / 100);

                    total_balance = decimal.Round((decimal)total_balance, 2, MidpointRounding.AwayFromZero);

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

        public JsonResult GenerateInterest(long? id)
        {
            using (db = new db_lendingEntities())
            {
                var result =
                    from d in db.tbl_loan_processing
                    where d.customer_id == id
                          && d.status == "Released"
                          && d.due_date <= _serverDateTime
                    orderby d.loantype_id ascending
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
                            noOfDays = decimal.ToInt32((_serverDateTime - dateStart).Value.Days) ;
                        }
                        else
                        {
                            if ((decimal.ToInt32((_serverDateTime - dateStart).Value.Days)) >= 30)
                            {
                                noOfDays = (((_serverDateTime - dateStart).Value.Days)) / 30;
                            }
                        }
                        decimal totalInterest = 0;
                        for (var c = 0; c < noOfDays; c++)
                        {
                            var interest = (balance * (interestRate / 100));
                            balance = balance + interest;
                            totalInterest = totalInterest + interest;
                        }

                        db_lendingEntities dbSave = new db_lendingEntities();
                        tbl_loan_ledger tbl = new tbl_loan_ledger();

                        tbl.date_trans = _serverDateTime;
                        tbl.trans_type = "Late Payment Interest";
                        tbl.reference_no = "";
                        tbl.loan_no = dt.loan_no;
                        tbl.loan_type_name = dt.loan_name;
                        tbl.customer_id = dt.customer_id;
                        tbl.customer_name = dt.customer_name;
                        tbl.interest_type = GetInterestType(dt.loan_no);
                        tbl.interest_rate = dt.loan_interest_rate;
                        tbl.interest = totalInterest;
                        tbl.amount_paid = 0;
                        tbl.principal = 0;
                        tbl.balance = 0;
                        tbl.date_created = DateTime.Now;
                        tbl.created_by = Session["UserName"].ToString();

                        dbSave.tbl_loan_ledger.Add(tbl);

                        dbSave.SaveChanges();
                    }                    
                }
            }

            return Json("Success", JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadInterest()
        {
            return Json("Success", JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadInterestDues(long? id)
        {
            db_lendingEntities db = new db_lendingEntities();

            var datetimenow = DateTime.Now;
            var datenow = datetimenow.Date;

            if (Session["UserId"] != null)
            {
                GenerateInterest(id);
                List<collectionslist> collectionslist = new List<collectionslist>();
                collectionslist collection = new collectionslist();

                var result = from d in db.tbl_loan_processing where d.customer_id == id && d.status == "Released" orderby d.date_created select d;
                foreach (var dt in result)
                {
                    var initialInterest = dt.loan_granted * (dt.loan_interest_rate / 100);
                    decimal? interestBalance = 0;
                    if (GetInterestType(dt.loan_name) == "1")
                    {
                         interestBalance = GetLedgerInterestBalance(dt.loan_no, initialInterest);
                    }
                    else
                    {
                         interestBalance = GetLedgerInterestBalance(dt.loan_no, 0);
                    }

                    interestBalance = decimal.Round((decimal)interestBalance, 2, MidpointRounding.AwayFromZero);

                    if (interestBalance > 0)
                    {
                        collectionslist.Add(new collectionslist { loan_no = dt.loan_no, loan_type = dt.loan_name, due_date = dt.due_date, amount_due = interestBalance, payment = 0, interest = dt.loan_interest_rate, interest_type = GetInterestType(dt.loan_name) });
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
                var result = from d in db.tbl_loan_processing where d.loan_no == id select d.total_receivable;
                foreach (var data in result)
                {
                    balance = data.Value;
                }

                return balance;
            }           
        }
        public decimal? GetLedgerBalance(string id)
        {
            using(db = new db_lendingEntities())
            {
                var found = false;
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

                    if (data.date_trans.Value.Day == _serverDateTime.Day && data.date_trans.Value.DayOfYear == _serverDateTime.DayOfYear)
                    {
                        found = true;
                    }
                }
                if (found)
                {
                    balance = 0;
                    return balance;
                }
                else
                {
                    return balance;
                }
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
        public decimal? DisplayLedgerBalance(string id, string referenceNo)
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
                    int refNo = 0;
                    try
                    {
                         refNo = Int32.Parse(data.reference_no);
                    }
                    catch (Exception )
                    {
                        refNo = 0;
                    }               
                    int crefNo = Int32.Parse(referenceNo);
                    if (refNo <= crefNo)
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
                }

                return balance;
          
            }
        }
        public decimal? GetLedgerInterestBalance(string id, decimal? initialInterest)
        {
            using (db = new db_lendingEntities())
            {
                decimal? interest = initialInterest;
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
                        case "OR Payment":
                            interest = interest - data.interest;
                            break;
                        default:
                            break;
                    }
                }
                return interest;
            }
        }
    
        public decimal GetPayments(string id)
        {

            db_lendingEntities db = new db_lendingEntities();
            {
                decimal balance = 0;
                var result = from d in db.tbl_payment_details where d.loan_no == id && d.payment_type.Equals("OR Payment") select d.amount;
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
                var result = from d in db.tbl_loan_type where d.description.Equals(id) select d;
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

                return Json(tbl.reference_no, JsonRequestBehavior.AllowGet);
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

                return Json(tbl.reference_no, JsonRequestBehavior.AllowGet);
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
                    db = new db_lendingEntities();
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

                    db = new db_lendingEntities();
                    List<receiptbalancelist> list2 = new List<receiptbalancelist>();

                    int customerid = Convert.ToInt32(GetBorrowerid(id));
                    var result2 = from d in db.tbl_loan_processing where d.customer_id == customerid select d.loan_no;

                    foreach (var dt2 in result2)
                    {
                        if (DisplayLedgerBalance(dt2, id) > 0){
                            list2.Add(new receiptbalancelist { loan_no = dt2, balance = String.Format("{0:0.00}", DisplayLedgerBalance(dt2, id)) });
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
                var borrowerid = "0";
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