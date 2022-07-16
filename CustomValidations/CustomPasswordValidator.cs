using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication3.Models;

namespace IdentityServerExample.CustomValidations
{
    public class CustomPasswordValidator : IPasswordValidator<User>
    {
        public async Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user, string password)
        {
            List<IdentityError> errors = new List<IdentityError>();

            if (password.ToLower().Contains(user.UserName.ToLower()))
                errors.Add(new IdentityError() { Code = "PasswordContaiansUserName", Description = "Şifre alanı kullanıcı adı içeremez." });

            if(password.ToLower().Contains("1234"))
                errors.Add(new IdentityError() { Code = "PasswordContaiansUserName1234", Description = "Şifre alanı ardışık sayı içeremez." });

            if (password.ToLower().Contains(user.Email))
                errors.Add(new IdentityError() { Code = "PasswordContaiansEmail", Description = "Şifre alanı email adresi içeremez." });

            if (errors.Count == 0)
                return await Task.FromResult(IdentityResult.Success);
            else
                return await Task.FromResult(IdentityResult.Failed(errors.ToArray()));
        }
    }
}
