using IdentityServerExample.Models;
using IdentityServerExample.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerExample.Controllers
{
    public class HomeController : BaseController
    {

        public HomeController(UserManager<User> userManager, SignInManager<User> signInManager) : base(userManager, signInManager)
        {
            
        }
        public IActionResult Index()
        {

            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Member");
            else
                return View();
        }
        public ActionResult Login(string ReturnUrl)
        {
            TempData["ReturnUrl"] = ReturnUrl;

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel userLoginModel)
        {
            if (ModelState.IsValid)
            {
                //kullanıcı var mı? yok mu? kontrolü
                User user = await _userManager.FindByEmailAsync(userLoginModel.Email);

                if (user != null)
                {


                    if (await _userManager.IsLockedOutAsync(user))
                    {
                        ModelState.AddModelError("", "Hesabınız bir süreliğine kilitlenmiştir. Lütfen daha sonra tekrar deneyiniz.");
                        return View(userLoginModel);
                    }

                    //bir cookie var ise silsin yeniden giriş yaptığım için 
                    await _signInManager.SignOutAsync();

                    //passowrd dan sonraki parametreler cookie de kullanabilsin beni hatırla durumu
                    //diğeri ise logoutfail özelliği kullanıcı yanluş şifrede kilitleme durumu
                    Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, userLoginModel.Password, userLoginModel.RememberMe, false);

                    if (signInResult.Succeeded)
                    {
                        //başarısız sayısını restlenmek gerekiyor. 
                        await _userManager.ResetAccessFailedCountAsync(user);

                        if (TempData["ReturnUrl"] != null)
                            return Redirect(TempData["ReturnUrl"].ToString());
                        else
                            return RedirectToAction("Index", "Member");
                    }
                    else
                    {
                        ////hesap kilitli ise 
                        //if (signInResult.IsLockedOut)
                        //    ModelState.AddModelError("", "Geçersiz kullanıcı veya şifresi");
                        ////hesap kilitli ve süresi dolmadıysa engellenen süre
                        //else if (signInResult.IsNotAllowed)
                        //    ModelState.AddModelError("", "Geçersiz kullanıcı veya şifresi");
                        //else
                        //    ModelState.AddModelError("", "Geçersiz kullanıcı veya şifresi");

                        await _userManager.AccessFailedAsync(user);

                        int fail = await _userManager.GetAccessFailedCountAsync(user);
                        ModelState.AddModelError("", $"{fail} bir kez giriş.");
                        if (fail == 3)
                        {
                            await _userManager.SetLockoutEndDateAsync(user, new DateTimeOffset(DateTime.Now.AddMinutes(1)));
                            ModelState.AddModelError("", "Kullanıcı hesabınız kilitlenmiştir.");
                            return View(userLoginModel);
                        }
                    }
                }
                else
                {
                    //emailin altında çıkmasını sağlar
                    //ModelState.AddModelError(nameof(LoginViewModel.Email), "Geçersiz kullanıcı veya şifresi");
                    //summary de çıkar
                    ModelState.AddModelError("", "Geçersiz kullanıcı veya şifresi");
                }
            }

            return View(userLoginModel);
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
                    AddModelError(identityResult);
            }
            return View(userViewModel);
        }

        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(PasswordRestViewModel passwordRestViewModel)
        {

            User user = await _userManager.FindByEmailAsync(passwordRestViewModel.Email);

            if (user != null)
            {
                //password sıfırlaması için bir token oluşturuyoruz. Bunun içerisinde                 //
                string passwordResetToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                string passwordResetLink = Url.Action("ResetPasswordConfirm", "Home", new
                {
                    userId = user.Id,
                    token = passwordResetToken

                }, HttpContext.Request.Scheme);
                Helper.PasswordReset.PasswordResetSendEmail(passwordResetLink);

                ViewBag.Status = "SuccessFull";
            }
            else
                ModelState.AddModelError("", "Böyle bir kullanıcı bulunamadı");

            return View(passwordRestViewModel);
        }

        public IActionResult ResetPasswordConfirm(string userId, string token)
        {

            TempData["userId"] = userId;
            TempData["token"] = token;

            return View();


        }
        //Bind özelliği o kontrolün içerisinde sadece o metot ismi dolar.
        [HttpPost]
        public async Task<IActionResult> ResetPasswordConfirm([Bind("PasswordNew")] PasswordRestViewModel passwordRestViewModel)
        {
            string token = TempData["token"].ToString();

            string userId = TempData["userId"].ToString();


            User user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                IdentityResult identityResult = await _userManager.ResetPasswordAsync(user, token, passwordRestViewModel.PasswordNew);

                if (identityResult.Succeeded)
                {
                    //Security Stamp değerini
                    //şifresi sıfırlamaz isek eski şifreyle dolaşmaya devam eder. Cookie deki değeri güncelleme yapmadığından dolayı

                    await _userManager.UpdateSecurityStampAsync(user);
                    ViewBag.Status = "success";
                }
                else                
                    AddModelError(identityResult);
                
            }
            else
            {
                ModelState.AddModelError("", "Bir hata meydana gelmiştir. Lütfen şifre yenileme işlemlerini tekrarlayınız");
                return View(passwordRestViewModel);
            }
            return View(passwordRestViewModel);
        }
    }
}
