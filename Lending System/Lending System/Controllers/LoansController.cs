using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lending_System.Models;
using System.Threading.Tasks;
using System.Data;

namespace Lending_System.Controllers
{
    public class LoansController : Controller
    {
        private db_lendingEntities db = new db_lendingEntities();
        // GET: Loans
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
        public ActionResult LoadLoans()
        {
            try
            {
                using (db_lendingEntities db = new db_lendingEntities())
                {
                    var data = db.tbl_loan_type.ToList();
                    return Json(new { data = data }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
        public ActionResult Add()
        {
            if (Session["UserId"] != null)
            {
                LoadCharges();
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        [HttpPost]
        public JsonResult Add(tbl_loan_type model)
        {
            try
            {
                db_lendingEntities db = new db_lendingEntities();

                tbl_loan_type tbl = new tbl_loan_type();

                tbl.code = "0";
                tbl.description = model.description;
                tbl.interest = model.interest;
                tbl.interest_type = model.interest_type;
                tbl.days = model.days;

                db.tbl_loan_type.Add(tbl);

                db.SaveChanges();

                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            catch (DataException ex)
            {
                return Json("Failed", JsonRequestBehavior.DenyGet);
                throw ex;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add1([Bind(Include = "autonum, code, description, interest, interest_type")]tbl_loan_type model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.tbl_loan_type.Add(model);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult AddLoanCharges(tbl_loan_type_charges model)
        {
            try
            {
                db.tbl_loan_type_charges.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }
        public void LoadCharges()
        {
            var ChargesList = new List<SelectListItem>();
            var dbQuery = from d in db.tbl_fees select d;
            foreach (var d in dbQuery)
            {
                if (d.fees_description.Trim() != "")
                {
                    ChargesList.Add(new SelectListItem { Value = d.fees_description, Text = d.fees_description });
                }
            }
            ViewBag.Charges = new SelectList(ChargesList, "Value", "Text");
        }
        public ActionResult LoadList()
        {
            try
            {
                using (db_lendingEntities db = new db_lendingEntities())
                {
                    var data = db.tbl_loan_type_charges.Where(a => a.autonum == 0).ToList();
                    return Json(new { data = data }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult UpdateLoans()
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
        public JsonResult UpdateLoans(tbl_loan_type_validation model)
        {
            return Json("Success", JsonRequestBehavior.AllowGet);
        }
    }
}