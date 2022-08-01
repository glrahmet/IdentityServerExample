﻿using IdentityServerExample.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        } 
        protected void AddModelError(IdentityResult result)
        {
            foreach (IdentityError item in result.Errors)
                ModelState.AddModelError("", item.Description);
        }

    }
}
