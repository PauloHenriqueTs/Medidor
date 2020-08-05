using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Entities;

using Microsoft.AspNetCore.Authorization;

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
                ViewBag.Jwt = jwt.Replace("\"", string.Empty).Trim();
                EnergyMetersService service = new EnergyMetersService(jwt);
                var meters = await service.GetAll(jwt);
                return View(meters);
            }
            else
            {
                return RedirectToRoute(new { controller = "Account", action = "Login" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Switch(string SerialId)
        {
            var jwt = JwtService.GetJwt(HttpContext);
            if (jwt != null)
            {
                EnergyMetersService service = new EnergyMetersService(jwt);
                var responseMessage = await service.Switch(jwt, SerialId);
                if (responseMessage.IsSuccessStatusCode)
                {
                    return Redirect("GetAll");
                }
                return BadRequest();
            }
            else
            {
                return RedirectToRoute(new { controller = "Account", action = "Login" });
            }
        }
    }
}