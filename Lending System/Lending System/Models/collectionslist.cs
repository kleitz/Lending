using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lending_System.Models
{
    public partial class collectionslist
    {
        public string loan_no { get; set; }
        public string loan_type { get; set; }
        public string customerName { get; set; }
        public Nullable<System.DateTime> due_date { get; set; }
        public Nullable<decimal> amount_due { get; set; }
        public Nullable<decimal> payment { get; set; }
        public Nullable<decimal> interest { get; set; }
        public string interest_type { get; set; }
        public string is_additional_interest { get; set; }
    }
}