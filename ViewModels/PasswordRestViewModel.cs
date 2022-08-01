using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerExample.ViewModels
{
    public class PasswordRestViewModel
    {
        [Display(Name = "Email Adresiniz")]
        [Required(ErrorMessage = "Email alanı gereklidir")]
        [EmailAddress]
        public string Email { get; set; }


        [Required(ErrorMessage = "Yeni Şifre gereklidir")]
        [Display(Name = "Yeni Şifre")]
        [DataType(DataType.Password)]
        public string PasswordNew { get; set; }
    }
}
