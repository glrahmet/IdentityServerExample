﻿using IdentityServerExample.Models;
using IdentityServerExample.ViewModels;
using Mapster;
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
        public AdminController(UserManager<User> userManager, RoleManager<UserRole> roleManager) : base(userManager, null, roleManager)
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

        public IActionResult RoleCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RoleCreate(RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                UserRole userRole = new UserRole();

                userRole.Name = roleViewModel.Name;


                IdentityResult identityResult = await _roleManager.CreateAsync(userRole);

                if (identityResult.Succeeded)
                    return RedirectToAction("Roles");
                else
                {
                    AddModelError(identityResult);
                    return View(roleViewModel);
                }
            }
            else
                return View(roleViewModel);
        }


        public IActionResult Roles()
        {
            return View(_roleManager.Roles.ToList());
        }

        public async Task<IActionResult> RolDelete(string id)
        {
            UserRole userRole = await _roleManager.FindByIdAsync(id);
            if (userRole != null)
            {
                IdentityResult identityResult = await _roleManager.DeleteAsync(userRole);

                if (identityResult.Succeeded)
                    return RedirectToAction("Index");
                else
                    ViewBag.Error = "Hata Medana Geldi";

            }
            return RedirectToAction("Roles");
        }

        public async Task<IActionResult> RolEdit(string id)
        {
            UserRole userRole = await _roleManager.FindByIdAsync(id);                             
            return View(userRole.Adapt<RoleViewModel>());
        }

        [HttpPost]
        public async Task<IActionResult> RolEdit(RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                UserRole userRole = await _roleManager.FindByIdAsync(roleViewModel.Id.ToString());

                if (userRole != null)
                {
                    userRole.Name = roleViewModel.Name;
                    IdentityResult identityResult = await _roleManager.UpdateAsync(userRole);

                    if (identityResult.Succeeded)
                        return RedirectToAction("Roles");
                    else

                        ViewBag.Error = "Hata oluştu";
                }
                return View(roleViewModel);
            }
            else
                return View(roleViewModel);
        }
    }
}
