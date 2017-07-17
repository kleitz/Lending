using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lending_System.Areas.Administrator.Models
{
    public partial class collectionSaveModel
    {
        public string LoanNo { get; set; }
        public string LoanName { get; set; }
        public string LoanDueDate { get; set; }
        public string PaymentNo { get; set; }
        public string PaymentDate { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Payment { get; set; }

        public string AmountDuePrincipal { get; set; }
        public string AmountDueInterest { get; set; }
    }
}