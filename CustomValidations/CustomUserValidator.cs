using IdentityServerExample.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServerExample.CustomValidations
{
    public class CustomUserValidator : IUserValidator<User>
    {
        public async Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user)
        {
            List<IdentityError> error = new List<IdentityError>();


            if (!Convert.ToBoolean(Convert.ToInt32(user.UserName[0])))
                error.Add(new IdentityError() { Code = "UserNameFirstDigit", Description = "Kullanıcı adı ilk kararteri sayısal veri olamaz" };


            if (error.Count == 0)
                return await Task.FromResult(IdentityResult.Success);
            else
                return await Task.FromResult(IdentityResult.Failed(error.ToArray());
        }
    }
}
