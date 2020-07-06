using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication1.Entities;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<ApplicationUser> _SignInManager;

        public HomeController(ILogger<HomeController> logger, SignInManager<ApplicationUser> SignInManager)
        {
            _logger = logger;
            _SignInManager = SignInManager;
        }

        public IActionResult Index()
        {
            if (_SignInManager.IsSignedIn(User))
            {
                return Redirect("/EnergyMeters/Create");
            }
            else { return View(); }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}