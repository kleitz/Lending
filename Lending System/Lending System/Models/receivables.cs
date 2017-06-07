using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lending_System.Models
{
    public class receivables
    {
        public string loanNo { get; set; }
        public string customerName { get; set; }
        public string dueDate { get; set; }
        public string principal { get; set; }
        public string interest { get; set; }
        public string payment { get; set; }
        public string balance { get; set; }
    }
}