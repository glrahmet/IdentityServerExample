using System.ComponentModel.DataAnnotations;

namespace IdentityServerExample.ViewModels
{
    public class RoleViewModel
    {
        [Required(ErrorMessage = "Rol ismi gereklidir")]
        [Display(Name = "Rol Adı")]
        public string Name { get; set; }
        public string Id { get; set; }

    }
}
