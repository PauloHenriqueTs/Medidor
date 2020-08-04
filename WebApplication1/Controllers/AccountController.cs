using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using WebApplication1.Services;
using WebApplication1.ViewModel;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        public AccountController()
        {
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            AccountService AccountService = new AccountService();
            if (ModelState.IsValid)
            {
                var response = await AccountService.Register(model);
                if (response.IsSuccessStatusCode)
                {
                    return Redirect("Login");
                }
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            AccountService AccountService = new AccountService();

            if (ModelState.IsValid)
            {
                var response = await AccountService.Login(model);
                if (response.IsSuccessStatusCode)
                {
                    var Token = await response.Content.ReadAsStringAsync();
                    JwtService.SetJwt(HttpContext, Token);

                    return RedirectToRoute(new { controller = "Home", action = "Index" });
                }
            }
            return View();
        }
    }
}