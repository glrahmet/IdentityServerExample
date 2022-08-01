using IdentityServerExample.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerExample.Controllers
{
    public class AdminController : BaseController
    {
         
        public AdminController(UserManager<User> userManager, RoleManager<UserRole> roleManager) : base(userManager)
        {
           
        }
        public IActionResult Index()
        {
         return View();
        }

        public IActionResult Users()
        {
            return View(_userManager.Users.ToList());
        }
    }
}
