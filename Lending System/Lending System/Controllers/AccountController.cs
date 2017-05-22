using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using System.Globalization;
using Lending_System.Models;
using System.Security.Cryptography;
using System.Web.Security;

using System.Data.Entity.Validation;

namespace Lending_System.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }
        public ActionResult UserList()
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
        public ActionResult Registration()
        {

            return View();

        }

        [HttpPost]
        public JsonResult Registration(tbl_user_validation_registration model)
        {
            try
            {
                db_lendingEntities db = new db_lendingEntities();

                tbl_user tbl = new tbl_user();

                tbl.firstname = model.firstname;

                tbl.lastname = model.lastname;

                tbl.email = model.email;

                tbl.username = model.username;

                if (model.password != null)
                {
                    tbl.password = FormsAuthentication.HashPasswordForStoringInConfigFile(model.password.Trim(), "md5");
                }

                //string hash = FormsAuthentication.HashPasswordForStoringInConfigFile(model.password.Trim(), "md5");

                if (model.gender == "Female")
                {
                    tbl.gender = "F";
                }
                else if (model.gender == "Male")
                {
                    tbl.gender = "M";
                }
                else
                {
                    tbl.gender = "U";
                }

                tbl.user_rank_id = 3;
                tbl.department_id = 1;
                tbl.system_type = "LS";

                db.tbl_user.Add(tbl);

                db.SaveChanges();

                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Failed", JsonRequestBehavior.DenyGet);
                throw ex;
            }
        }
        public ActionResult Login()
        {

            if (Session["UserId"] != null)
            {
                return RedirectToAction("Index", "Home");               
            }
            else
            {
                return View();
            }

        }
        [HttpPost]
        public JsonResult Login(tbl_user_validation_login model)
        {
            string result = "No user";
            try
            {
                db_lendingEntities db = new db_lendingEntities();

                tbl_user tbl = new tbl_user();
                if (model.password != null)
                {
                    string password = model.password;
                    password = FormsAuthentication.HashPasswordForStoringInConfigFile(password.Trim(), "md5");
                    tbl = db.tbl_user.SingleOrDefault(x => x.username == model.username &&  x.password == password);
                }

                if (tbl != null)
                {

                    Session["UserId"] = tbl.autonum;
                    Session["UserName"] = tbl.username;
                    Session["UserFullName"] = tbl.firstname;

                    if (tbl.user_rank_id == 3)
                    {
                        result = "User";
                    }
                    else if (tbl.user_rank_id == 1)
                    {
                        result = "Admin";
                    }
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(result, JsonRequestBehavior.DenyGet);
                throw ex;
            }
            
        }
        public ActionResult Logout()
        {

            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

            return RedirectToAction("Login", "Account");

        }
    }
}