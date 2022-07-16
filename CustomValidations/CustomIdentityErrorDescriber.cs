using Microsoft.AspNetCore.Identity;

namespace IdentityServerExample.CustomValidations
{
    public class CustomIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError InvalidUserName(string userName)
        {
            return new IdentityError()
            {
                Code = "InvalidUserName",
                Description = $"Bu {userName} geçersizdir"
            };
        }

        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError()
            {
                Code = "DublicateEmail",
                Description = $"Bu {email} kullanılmaktadır"
            };
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError()
            {
                Code = "PasswordToShort",
                Description = $"Şifreniz  en az {length} karakterli olmalıdır"
            };
        }
    }
}
