using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Lending_System.Models
{
    public class tbl_user_validation_registration
    {
        [Required(ErrorMessage = "First Name is Required.")]
        public string firstname { get; set; }
        [Required(ErrorMessage = "Last Name is Required.")]
        public string lastname { get; set; }
        [Required(ErrorMessage = "Email is Required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string email { get; set; }
        [Required(ErrorMessage = "Username is Required.")]
        public string username { get; set; }
        [Required(ErrorMessage = "Password is Required.")]
        [DataType(DataType.Password)]
        public string password { get; set; }
        [Compare("password", ErrorMessage = "Please confirm password.")]
        [DataType(DataType.Password)]
        public string cpassword { get; set; }

        public string gender { get; set; }
    }
}