using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class EnergyMetersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAll()
        {
            var jwt = JwtService.GetJwt(HttpContext);
            if (jwt != null)
            {
                EnergyMetersService service = new EnergyMetersService(jwt);
                var meters = await service.GetAll(jwt);
                return View(meters);
            }
            else
            {
                return RedirectToRoute(new { controller = "Account", action = "Login" });
            }
        }
    }
}