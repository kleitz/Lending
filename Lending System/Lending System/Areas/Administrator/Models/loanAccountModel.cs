using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lending_System.Areas.Administrator.Models
{
    public partial class loanAccountModel
    {
        public string LoanNo { get; set; }
        public string CustomerName { get; set; }
        //public string Balance { get; set; }
        public Nullable<decimal> Balance { get; set; }
    }
}