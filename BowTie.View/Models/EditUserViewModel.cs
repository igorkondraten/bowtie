using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BowTie.View.Models
{
    public class EditUserViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Група")]
        public int RoleId { get; set; }
        public IEnumerable<SelectListItem> Roles { get; set; }
        [Required]
        [Display(Name = "Ім'я")]
        public string Name { get; set; }
        [Display(Name = "Email адреса")]
        public string Email { get; set; }
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }
}