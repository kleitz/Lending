using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lending_System.Models;
using System.Web.Security;
using System.Data;
using System.Net;

namespace Lending_System.Controllers
{
    public class CustomerController : Controller
    {
        db_lendingEntities db = new db_lendingEntities();
        // GET: Customer
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
        public ActionResult LoadCustomer()
        {
            try
            {
                using (db_lendingEntities db = new db_lendingEntities())
                {
                    var data = db.tbl_customer.ToList();
                    return Json(new { data = data }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult AddCustomer()
        {
            db_lendingEntities db = new db_lendingEntities();

            var dt = DateTime.Now;

            ViewBag.datetime = dt;

            if (Session["UserId"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        [HttpPost]
        public JsonResult AddCustomer(tbl_customer_validation model)
        {
            try
            {
                db_lendingEntities db = new db_lendingEntities();

                tbl_customer tbl = new tbl_customer();

                if (model.date_registered != null)
                {
                    tbl.customer_no = model.customer_no;
                }
                else
                {
                    tbl.date_registered = model.date_registered;
                }
                //
                if (model.lastname != null)
                {
                    tbl.lastname = model.lastname.ToUpper();
                }
                else
                {

                }
                if (model.firstname != null)
                {
                    tbl.firstname = model.firstname.ToUpper();
                }
                else
                {

                }
                //
                if (model.middlename != null)
                {
                    tbl.middlename = model.middlename.ToUpper();
                }
                else
                {

                }
                if (model.civil_status != null)
                {
                    tbl.civil_status = model.civil_status;
                }
                else
                {

                }
                if (model.address != null)
                {
                    tbl.address = model.address.ToUpper();
                }
                else
                {

                }
                if (model.contact_no != null)
                {
                    tbl.contact_no = model.contact_no;
                }
                else
                {

                }
                if (model.email != null)
                {
                    tbl.email = model.email;
                }
                else
                {

                }
                if (model.date_of_birth != null)
                {
                    tbl.date_of_birth = model.date_of_birth;
                }
                else
                {

                }
                if (model.birth_place != null)
                {
                    tbl.birth_place = model.birth_place.ToUpper();
                }
                else
                {

                }
                if (model.occupation != null)
                {
                    tbl.occupation = model.occupation.ToUpper();
                }
                else
                {

                }
                if (model.credit_limit != null)
                {
                    tbl.credit_limit = model.credit_limit;
                }
                else
                {

                }
                if (model.annual_income != null)
                {
                    tbl.annual_income = model.annual_income;
                }
                else
                {

                }

                db.tbl_customer.Add(tbl);

                db.SaveChanges();

                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Failed", JsonRequestBehavior.DenyGet);
                throw ex;
            }
        }

        public ActionResult Details(int? id)
        {
            try
            {
                using (db_lendingEntities db = new db_lendingEntities())
                {
                    tbl_customer tbl_customer = db.tbl_customer.Find(id);

                    if (Session["UserId"] != null)
                    {
                        return View(tbl_customer);
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
        [HttpPost, ActionName("Update")]
        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var LoanUpdate = db.tbl_customer.Find(id);
            if (TryUpdateModel(LoanUpdate, "",
               new string[] { "customer_no", "date_registered", "firstname", "middlename", "lastname", "civil_status", "address", "contact_no", "email", "date_of_birth", "birth_place", "occupation", "credit_limit", "annual_income" }))
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

    }
}