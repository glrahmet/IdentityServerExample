using System.ComponentModel.DataAnnotations;

namespace IdentityServerExample.ViewModels
{
    public class PasswordChangeViewModel
    {
        [Display(Name = "Eski Şifre")]
        [Required(ErrorMessage = "Eski şifreniz gereklidir")]
        public string PasswordOld { get; set; }

        [Display(Name = "Yeni Şifre")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Yeni şifreniz gereklidir")]

        public string PasswordNew { get; set; }

        [Display(Name = "Yeni Şifre Tekrar")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Yeni şifreniz tekrarı gereklidir")]
        [Compare("PasswordNew", ErrorMessage = "Yeni şifre tekrarı doğru değildir")]
        public string PasswordConfirm { get; set; }

    }
}
