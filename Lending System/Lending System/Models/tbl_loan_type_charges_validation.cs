using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Lending_System.Models
{
    public class tbl_loan_type_charges_validation
    {
        public int autonum { get; set; }
        public string description { get; set; }
        public Nullable<decimal> amount { get; set; }
        public string charge_type { get; set; }
    }
}