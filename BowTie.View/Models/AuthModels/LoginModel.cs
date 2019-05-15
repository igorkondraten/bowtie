using System.ComponentModel.DataAnnotations;

namespace BowTie.View.Models.AuthModels
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "Ім'я")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}