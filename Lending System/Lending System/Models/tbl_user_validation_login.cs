using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Lending_System.Models
{
    public class tbl_user_validation_login
    {
        [Required(ErrorMessage = "Username is Required.")]
        public string username { get; set; }
        [Required(ErrorMessage = "Password is Required.")]
        [DataType(DataType.Password)]
        public string password { get; set; }
    }
}