//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Lending_System.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_loan_type
    {
        public int autonum { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public Nullable<decimal> interest { get; set; }
        public string interest_type { get; set; }
        public Nullable<int> days { get; set; }
    }
}
