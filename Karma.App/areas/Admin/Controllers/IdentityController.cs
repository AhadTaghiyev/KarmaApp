using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Karma.Core.DTOS;
using Karma.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Karma.App.areas.Admin.Controllers
{
    [Area("admin")]

    public class IdentityController : Controller
    {
        readonly UserManager<AppUser> _userManager;
        readonly SignInManager<AppUser> _signInManager;

        public IdentityController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            AppUser appUser =await _userManager.FindByNameAsync(loginDto.UserNameOrEmail);


            
            if (appUser == null)
            {
                appUser = await _userManager.FindByEmailAsync(loginDto.UserNameOrEmail);
            }

            if (appUser == null)
            {
                ModelState.AddModelError("", "Username or email or password incorrect");
                return View();
            }

           var roles= await _userManager.GetRolesAsync(appUser);

            if (!roles.Contains("Admin") && !roles.Contains("SuperAdmin"))
            {
                ModelState.AddModelError("", "Username or email or password incorrect");
                return View();
            }


               Microsoft.AspNetCore.Identity.SignInResult res=  await _signInManager.PasswordSignInAsync(appUser,loginDto.Password,true,false);
            if (!res.Succeeded)
            {
                if (res.IsLockedOut)
                {
                    ModelState.AddModelError("", "Your Account was blocked for 1 minutes");
                    return View();
                }

                ModelState.AddModelError("", "Username or email or password incorrect");
                return View();
            }

            return RedirectToAction("Index","Home");
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(Login));
        }
    }
}

