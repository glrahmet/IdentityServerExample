using IdentityServerExample.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServerExample.Controllers
{
    public class BaseController : Controller
    {
        protected UserManager<User> _userManager { get; }
        protected SignInManager<User> _signInManager { get; }
        protected RoleManager<UserRole> _roleManager { get; }
        protected Task<User> CurrentUser => _userManager.FindByNameAsync(User.Identity.Name);



        public BaseController(UserManager<User> userManager, SignInManager<User> signInManager = null, RoleManager<UserRole> roleManager = null)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        protected void AddModelError(IdentityResult result)
        {
            foreach (IdentityError item in result.Errors)
                ModelState.AddModelError("", item.Description);
        }


        protected async Task<IList<string>> getUserRole(User _user)
        {
            return await _userManager.GetRolesAsync(_user);
        }
         
    }
}
