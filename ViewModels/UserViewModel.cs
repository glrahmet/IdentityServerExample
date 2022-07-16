using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerExample.ViewModels
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "Kullanıcı ismi gereklidir")]
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }


        [Display(Name = "Telefon Numarası")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email adresi gereklidir")]
        [Display(Name = "Email Adresiniz")]
        [EmailAddress(ErrorMessage = "Email Adresiniz Doğru Formatta Değildir.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre gereklidir")]
        [Display(Name = "Şifre")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
