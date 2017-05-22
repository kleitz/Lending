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
    public class CashPullOutController : Controller
    {
        // GET: CashPullOut
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
                    var data = db.tbl_cash_out.OrderByDescending(a => a.autonum).ToList();

                    return Json(new { data = data }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public JsonResult Save(tbl_cash_out model)
        {
            try
            {
                db_lendingEntities db = new db_lendingEntities();

                tbl_cash_out tbl = new tbl_cash_out();

                tbl.date_trans = model.date_trans;
                tbl.username = model.username;
                tbl.datefrom = model.datefrom;
                tbl.dateto = model.dateto;
                tbl.amount = model.amount;
                tbl.created_by = Session["UserName"].ToString();
                tbl.date_created = DateTime.Now;

                db.tbl_cash_out.Add(tbl);

                db.SaveChanges();

                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Failed", JsonRequestBehavior.DenyGet);
                throw ex;
            }
        }
    }
}