using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lending_System.Models
{
    public class ledger
    {
        public int autonum { get; set; }
        public Nullable<System.DateTime> date_trans { get; set; }
        public string trans_type { get; set; }
        public string reference_no { get; set; }
        public string loan_no { get; set; }
        public string loan_type_name { get; set; }
        public Nullable<int> customer_id { get; set; }
        public string customer_name { get; set; }
        public string interest_type { get; set; }
        public Nullable<decimal> interest_rate { get; set; }
        public Nullable<decimal> interest { get; set; }
        public Nullable<decimal> amount_paid { get; set; }
        public Nullable<decimal> principal { get; set; }
        public Nullable<decimal> balance { get; set; }
    }
}