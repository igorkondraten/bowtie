using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BowTie.View.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Група")]
        public string Role { get; set; }
        [Display(Name = "Ім'я")]
        public string Name { get; set; }
        [Display(Name = "Email адреса")]
        public string Email { get; set; }
    }
}