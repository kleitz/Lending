using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lending_System.Areas.Administrator.Models
{
    public partial class loanListModel
    {
        public string LoanNo { get; set; }
        public string LoanType { get; set; }
        public string DueDate { get; set; }
        public string AmountDue { get; set; }
    }
}