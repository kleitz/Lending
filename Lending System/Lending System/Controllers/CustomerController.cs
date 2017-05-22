using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lending_System.Models;
using System.Web.Security;

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

                tbl.customer_no = model.customer_no;
                tbl.date_registered = model.date_registered;
                tbl.lastname = model.lastname;
                tbl.firstname = model.firstname;
                tbl.middlename = model.middlename;
                tbl.civil_status = model.civil_status;
                tbl.address = model.address;              
                tbl.contact_no = model.contact_no;
                tbl.email = model.email;
                tbl.date_of_birth = model.date_of_birth;
                tbl.birth_place = model.birth_place;
                tbl.occupation = model.occupation;
                tbl.credit_limit = model.credit_limit;
                tbl.annual_income = model.annual_income;

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


        public ActionResult UpdateCustomer()
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
        public JsonResult UpdateCustomer(tbl_customer_validation model)
        {
            return Json("Success", JsonRequestBehavior.AllowGet);
        }

    }
}