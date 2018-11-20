using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BowTie.View.Models.AuthModels
{
    public class RegisterModel
    {
        [Required]
        [Display(Name = "Ім'я")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Email адреса")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Пароль")]
        [MinLength(6, ErrorMessage = "Довжина паролю повинна бути від 6 символів.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Display(Name = "Повторіть пароль")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Паролі не співпадають")]
        public string ConfirmPassword { get; set; }
    }
}