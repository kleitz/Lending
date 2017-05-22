using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Lending_System.Models
{
    public class tbl_loan_type_validation
    {
        public int autonum { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public Nullable<decimal> interest { get; set; }
        public string interest_type { get; set; }
    }
}