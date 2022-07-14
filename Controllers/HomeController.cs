using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication3.Models;
using WebApplication3.ViewModels;

namespace WebApplication3.Controllers
{
    public class HomeController : Controller
    {
        private UserManager<User> _userManager { get; }

        public HomeController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                User _users = new User();
                _users.UserName = userViewModel.UserName;
                _users.PhoneNumber = userViewModel.PhoneNumber;
                _users.Email = userViewModel.Email;

                //password hashliyor kendisi 
                IdentityResult identityResult = await _userManager.CreateAsync(_users, userViewModel.Password);

                if (identityResult.Succeeded)
                    return RedirectToAction("Login");
                else
                {
                    foreach (IdentityError item in identityResult.Errors)
                        ModelState.AddModelError("", item.Description);
                }
            }
            return View(userViewModel);
        }
    }
}
