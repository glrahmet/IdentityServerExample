using System.ComponentModel.DataAnnotations;

namespace IdentityServerExample.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email alanı gereklidir")]
        [EmailAddress]
        [Display(Name = "Email Adres")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password alanı gereklidir")]
        [Display(Name = "Şifre")]
        [DataType(DataType.Password)]
        [MinLength(4)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
