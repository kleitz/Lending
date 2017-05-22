using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Lending_System.Models
{
    public class tbl_customer_validation
    {
        public string customer_no { get; set; }
        public Nullable<System.DateTime> date_registered { get; set; }
        [Required(ErrorMessage = "First Name is Required.")]
        public string firstname { get; set; }
        [Required(ErrorMessage = "Middle Name is Required.")]
        public string middlename { get; set; }
        [Required(ErrorMessage = "Last Name is Required.")]
        public string lastname { get; set; }
        public string civil_status { get; set; }
        public string address { get; set; }
        public string contact_no { get; set; }
        public string email { get; set; }
        public Nullable<System.DateTime> date_of_birth { get; set; }
        public string birth_place { get; set; }
        public string occupation { get; set; }
        public Nullable<decimal> credit_limit { get; set; }
        public Nullable<decimal> annual_income { get; set; }
    }
}