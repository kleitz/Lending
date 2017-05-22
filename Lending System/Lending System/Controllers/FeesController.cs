using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lending_System.Models;

namespace Lending_System.Controllers
{
    public class FeesController : Controller
    {
        // GET: Fees
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
        public ActionResult LoadFees()
        {
            try
            {
                using (db_lendingEntities db = new db_lendingEntities())
                {
                    var data = db.tbl_fees.ToList();
                    return Json(new { data = data }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
        public ActionResult AddFee()
        {
            db_lendingEntities db = new db_lendingEntities();

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
        public JsonResult AddFee(tbl_fees_validation model)
        {
            try
            {
                db_lendingEntities db = new db_lendingEntities();

                tbl_fees tbl = new tbl_fees();

                tbl.fees_description = model.fees_description;

                db.tbl_fees.Add(tbl);

                db.SaveChanges();

                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Failed", JsonRequestBehavior.DenyGet);
                throw ex;
            }
        }


        public ActionResult UpdateFee()
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
        public JsonResult UpdateFee(tbl_fees_validation model)
        {
            return Json("Success", JsonRequestBehavior.AllowGet);
        }
    }
}