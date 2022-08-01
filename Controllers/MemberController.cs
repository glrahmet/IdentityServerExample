using IdentityServerExample.Enums;
using IdentityServerExample.Models;
using IdentityServerExample.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.IO;
using System.Threading.Tasks;

namespace IdentityServerExample.Controllers
{
    [Authorize]
    public class MemberController : BaseController
    {

        public MemberController(UserManager<User> userManager, SignInManager<User> signInManager) : base(userManager, signInManager)
        {

        }

        public async Task<IActionResult> Index()
        {
            User user = await this.CurrentUser;

            //mapster kütüphanesi automappper dan daha küçük map kütüphanesidir.

            UserViewModel userViewModel = user.Adapt<UserViewModel>();

            return View(userViewModel);
        }

        public async Task<IActionResult> UserEdit()
        {
            User user = await this.CurrentUser;
            UserViewModel userViewModel = user.Adapt<UserViewModel>();

            ViewBag.Gender = new SelectList(Enum.GetNames(typeof(Gender)));
            return View(userViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UserEdit(UserViewModel userViewModel, IFormFile userPicture)
        {
            try
            {
                ViewBag.Gender = new SelectList(Enum.GetNames(typeof(Gender)));
                //password alanının validation özelliğine takılmaması için 
                ModelState.Remove("Password");
                if (ModelState.IsValid)
                {
                    User user = await this.CurrentUser;
                    if (userPicture != null && userPicture.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(userPicture.FileName);

                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserPicture", fileName);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await userPicture.CopyToAsync(stream);
                            user.PictureUrl = fileName;
                        }
                    }

                    user.UserName = userViewModel.UserName;
                    user.Email = userViewModel.Email;
                    user.PhoneNumber = userViewModel.PhoneNumber;
                    user.City = userViewModel.City;
                    user.BirthDay = userViewModel.BirthDay;
                    user.Gender = (int)userViewModel.Gender;


                    IdentityResult identityResult = await _userManager.UpdateAsync(user);
                    if (identityResult.Succeeded)
                    {
                        //username değiştiği için stamp ayarını değiştiriyoruz 

                        ViewBag.success = "true";
                        await _userManager.UpdateSecurityStampAsync(user);

                        await _signInManager.SignOutAsync();
                        await _signInManager.SignInAsync(user, true);
                    }
                    else
                        AddModelError(identityResult);

                }
                return View(userViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(userViewModel);
            }
        }

        public IActionResult PasswordChange()
        {
            return View(); ;
        }

        [HttpPost]
        public async Task<IActionResult> PasswordChange(PasswordChangeViewModel passwordChangeViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User user = await this.CurrentUser;

                    if (await _userManager.CheckPasswordAsync(user, passwordChangeViewModel.PasswordOld))
                    {
                        IdentityResult identityResult = await _userManager.ChangePasswordAsync(user, passwordChangeViewModel.PasswordOld, passwordChangeViewModel.PasswordNew);
                        if (identityResult.Succeeded)
                        {
                            ViewBag.Success = "true";
                            //eski şifreyle dolaşmasını engellemek için session süresini beklemeden 
                            await _userManager.UpdateSecurityStampAsync(user);


                            await _signInManager.SignOutAsync();
                            //ispersisten 
                            await _signInManager.PasswordSignInAsync(user, passwordChangeViewModel.PasswordNew, true, false);

                        }
                        else
                            AddModelError(identityResult);
                    }
                    else
                        ModelState.AddModelError("", "Eksi şifreniz doğru değildir");
                }
                return View(passwordChangeViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(passwordChangeViewModel);
            }
        }


        ////klasik yöntemle
        //public async Task<IActionResult> LogOut()
        //{
        //    await _signInManager.SignOutAsync();
        //    //klasik yöntem 
        //    return RedirectToAction("Index", "Home");

        //} 

        public async void LogOut()
        {
            await _signInManager.SignOutAsync();

        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
