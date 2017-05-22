using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lending_System.Models
{
    public class tbl_loan_processing_validation
    {
        public int autonum { get; set; }
        public Nullable<int> customer_id { get; set; }
        public string customer_name { get; set; }
        public string loan_no { get; set; }
        public Nullable<int> loantype_id { get; set; }
        public string loan_name { get; set; }
        public Nullable<decimal> loan_granted { get; set; }
        public Nullable<decimal> loan_interest_rate { get; set; }
        public string payment_scheme { get; set; }
        public Nullable<System.DateTime> due_date { get; set; }
        public Nullable<System.DateTime> loan_date { get; set; }
        public Nullable<int> installment_no { get; set; }
        public Nullable<decimal> total_receivable { get; set; }
        public Nullable<decimal> net_proceeds { get; set; }
        public Nullable<int> amortization_id { get; set; }
        public Nullable<int> finance_charge_id { get; set; }
        public string status { get; set; }
        public Nullable<int> prepaired_by_id { get; set; }
        public string prepaired_by_name { get; set; }
        public Nullable<int> reviewed_by_id { get; set; }
        public string reviewed_by_name { get; set; }
        public Nullable<int> approved_by_id { get; set; }
        public string approved_by_name { get; set; }
    }
}