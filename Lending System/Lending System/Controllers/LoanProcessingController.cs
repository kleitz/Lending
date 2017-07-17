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
    public class LoanProcessingController : Controller
    {
        db_lendingEntities db = new db_lendingEntities();
       
        // GET: LoanProcessing
        public ActionResult Index()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.Form = "Loan Processing";
                ViewBag.Controller = "Loan Processing";
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
            try
            {
                using (db_lendingEntities db = new db_lendingEntities())
                {
                    //var data = db.tbl_loan_processing.Where(a => a.loan_date >= DateTime.Now && a.loan_date <= DateTime.Now).ToList();
                    var data = db.tbl_loan_processing.OrderBy(a => a.status).ToList();

                    return Json(new { data = data }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult Details(int? id)
        {
            db_lendingEntities db = new db_lendingEntities();

            if (Session["UserId"] != null)
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                tbl_loan_processing tbl_loan_processing = db.tbl_loan_processing.Find(id);
                if (tbl_loan_processing == null)
                {
                    return HttpNotFound();
                }

                ViewBag.Form = "Loan Processing";
                ViewBag.Controller = "Loan Processing";
                ViewBag.Action = "Details";

                LoadCustomer();
                LoadLoanType();
                return View(tbl_loan_processing);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost, ActionName("Review")]
        public ActionResult Review(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var LoanUpdate = db.tbl_loan_processing.Find(id);
            if (TryUpdateModel(LoanUpdate, "",
               new string[] { "status", "reviewed_by_name" }))
            {
                try
                {
                    db.SaveChanges();

                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                catch (DataException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(LoanUpdate);
        }

        [HttpPost, ActionName("Approve")]
        public ActionResult Approve(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var LoanUpdate = db.tbl_loan_processing.Find(id);
            if (TryUpdateModel(LoanUpdate, "",
               new string[] { "status", "approved_by_name" }))
            {
                try
                {
                    db.SaveChanges();

                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                catch (DataException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(LoanUpdate);
        }
        [HttpPost, ActionName("Release")]
        public ActionResult Release(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var LoanUpdate = db.tbl_loan_processing.Find(id);
            if (TryUpdateModel(LoanUpdate, "",
               new string[] { "status" }))
            {
                try
                {
                    db.SaveChanges();

                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                catch (DataException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(LoanUpdate);
        }
        public ActionResult Create()
        {
            db_lendingEntities db = new db_lendingEntities();
            ViewBag.InterestTypeID = "";
            if (Session["UserId"] != null)
            {
                ViewBag.Form = "Loan Processing";
                ViewBag.Controller = "Loan Processing";
                ViewBag.Action = "Create";

                LoadCustomer();
                LoadLoanType();

                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        [HttpPost]
        public JsonResult CreateAmortization(tbl_loan_processing_amortization_schedule model)
        {
            try
            {
                db_lendingEntities db = new db_lendingEntities();

                tbl_loan_processing_amortization_schedule tbl = new tbl_loan_processing_amortization_schedule();

                tbl.due_date = model.due_date;
                tbl.principal = model.principal;
                tbl.interest = model.interest;
                tbl.amount = model.amount;
                tbl.balance = model.balance;
                tbl.loan_no = model.loan_no;
                tbl.createdby = Session["UserName"].ToString();

                db.tbl_loan_processing_amortization_schedule.Add(tbl);

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
        public JsonResult CreateLedger(tbl_loan_ledger model)
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
        [HttpPost]
        public JsonResult Create(tbl_loan_processing model)
        {
            try
            {
                db_lendingEntities db = new db_lendingEntities();

                tbl_loan_processing tbl = new tbl_loan_processing();

                tbl.customer_id = model.customer_id;
                tbl.customer_name = model.customer_name;
                tbl.loan_no = model.loan_no;
                tbl.loantype_id = model.loantype_id;
                tbl.loan_name = model.loan_name;
                tbl.loan_granted = model.loan_granted;
                tbl.loan_interest_rate = model.loan_interest_rate;
                tbl.payment_scheme = model.payment_scheme;
                tbl.due_date = model.due_date;
                tbl.loan_date = model.loan_date;
                tbl.installment_no = model.installment_no;
                tbl.total_receivable = model.total_receivable;
                tbl.net_proceeds = model.net_proceeds;
                tbl.amortization_id = model.amortization_id;
                tbl.finance_charge_id = model.finance_charge_id;
                tbl.status = model.status;
                tbl.prepared_by_id = model.prepared_by_id;
                tbl.prepared_by_name = model.prepared_by_name;
                tbl.reviewed_by_id = model.reviewed_by_id;
                tbl.reviewed_by_name = model.reviewed_by_name;
                tbl.approved_by_id = model.approved_by_id;
                tbl.approved_by_name = model.approved_by_name;
                tbl.date_created = DateTime.Now;
                tbl.created_by = Session["UserName"].ToString();

                db.tbl_loan_processing.Add(tbl);

                db.SaveChanges();

                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Failed", JsonRequestBehavior.DenyGet);
                throw ex;
            }
        }
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
                ViewBag.Customer = new SelectList(CustomerList.OrderBy(a => a.Text), "Value", "Text");
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void LoadLoanType()
        {
            var LoanTypeList = new List<SelectListItem>();
            var dbQuery = from d in db.tbl_loan_type select d;
            foreach (var d in dbQuery)
            {
                if (d.description.Trim() != "")
                {
                    LoanTypeList.Add(new SelectListItem { Value = d.autonum.ToString(), Text = d.description });
                }
            }
            ViewBag.LoanType = new SelectList(LoanTypeList, "Value", "Text");
        }
        public ActionResult LoadLoanTypeInterestRate(long? id)
        {
            using (db_lendingEntities db = new db_lendingEntities())
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            tbl_loan_type tbl_loan_type = db.tbl_loan_type.Find(id);
            if (tbl_loan_type == null)
            {
                return HttpNotFound();
            }

            return Json(tbl_loan_type.interest, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadLoanTypeInterestType(long? id)
        {
            using (db_lendingEntities db = new db_lendingEntities())
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            tbl_loan_type tbl_loan_type = db.tbl_loan_type.Find(id);
            if (tbl_loan_type == null)
            {
                return HttpNotFound();
            }
            ViewBag.LoanTypeInterestType = tbl_loan_type.interest_type;
            return Json(tbl_loan_type.interest_type, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadLoanTypeDays(long? id)
        {
            using (db_lendingEntities db = new db_lendingEntities())
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            tbl_loan_type tbl_loan_type = db.tbl_loan_type.Find(id);
            if (tbl_loan_type == null)
            {
                return HttpNotFound();
            }

            return Json(tbl_loan_type.days, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadLoanTypeChargeType(long? id)
        {
            int loan_id = unchecked((int)id);
            var charge_type = "";

            var dbQuery = from d in db.tbl_loan_type_charges select d;

            if (dbQuery != null)
            {
                foreach (var d in dbQuery)
                {
                    if (d.loan_type_id == loan_id)
                    {
                        charge_type = d.charge_type;
                    }
                }
            }
            return Json(charge_type, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadLoanTypeCharges(long? id)
        {
            try
            {
                using (db_lendingEntities db = new db_lendingEntities())
                {
                    var data = db.tbl_loan_type_charges.Where(a => a.loan_type_id == id);
                    return Json(new { data = data }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult CheckLoanStatus(long? id)
        {
            int loan_id = unchecked((int)id);
            var status = "";

            var dbQuery = from d in db.tbl_loan_processing select d;

            if (dbQuery != null)
            {
                foreach (var d in dbQuery)
                {
                    if (d.autonum == loan_id)
                    {
                        status = d.status;
                    }
                }
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> getLoanNo()
        {
            try
            {
                using (db_lendingEntities db = new db_lendingEntities())
                {
                    if (db.tbl_loan_processing.Any())
                    {
                        // The table is empty
                        var data = await db.tbl_loan_processing.MaxAsync(a => a.autonum);
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
    }
}