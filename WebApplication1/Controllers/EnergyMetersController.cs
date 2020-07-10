using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Entities;
using WebApplication1.ViewModel;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class EnergyMetersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EnergyMetersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: EnergyMeters
        public IActionResult Index()
        {
            // var applicationDbContext = _context.EnergyMeters.Include(e => e.user);
            return View();
        }

        // GET: EnergyMeters/Create
        public IActionResult Create()
        {
            var energyMeterCreateViewModel = new EnergyMeterCreateViewModel
            {
                serialId = "333",
                Select = "",
                meterOfPoles = new List<MeterOfPoleDto>
                {
                    new MeterOfPoleDto{meterSerialId="14343"},
                     new MeterOfPoleDto{meterSerialId="33332"}
                }
            };

            return View(energyMeterCreateViewModel);
        }

        // POST: EnergyMeters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EnergyMeterCreateViewModel energyMeterCreateViewModel)
        {
            var userId = _userManager.GetUserId(User);

            energyMeterCreateViewModel = new EnergyMeterCreateViewModel
            {
                serialId = "0",
                Select = "",
                meterOfPoles = new List<MeterOfPoleDto>()
            };
            return View(energyMeterCreateViewModel);
        }
    }
}