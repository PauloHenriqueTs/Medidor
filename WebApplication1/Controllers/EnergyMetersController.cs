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
using WebApplication1.Data.Repository;
using WebApplication1.Entities;
using WebApplication1.ViewModel;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class EnergyMetersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly EnergyMeterRepository repository;

        public EnergyMetersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, [FromServices] EnergyMeterRepository energyMeterRepository)
        {
            _context = context;
            _userManager = userManager;
            repository = energyMeterRepository;
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
            var list = new List<MeterOfPoleDto>();

            list.Add(new MeterOfPoleDto { meterSerialId = "4" });

            var energyMeterCreateViewModel = new EnergyMeterCreateViewModel
            {
                serialId = Guid.Empty,
                Select = "",
                meterOfPoles = list
            };

            return View(energyMeterCreateViewModel);
        }

        // POST: EnergyMeters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] EnergyMeterCreateViewModel energyMeterCreateViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(energyMeterCreateViewModel);
            }
            var userId = _userManager.GetUserId(User);
            var newMeter = energyMeterCreateViewModel.toEnergyMeter(userId);
            await repository.Create(newMeter);

            return Redirect("GetAll");
        }

        public async Task<IActionResult> GetAll()
        {
            var userId = _userManager.GetUserId(User);
            var meters = await repository.Get(userId);

            return View(meters);
        }

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> VerifySerialId(string serialId)
        {
            var isExist = await repository.SerialIdExist(serialId);
            if (isExist)
            {
                return Json($"A user named {serialId}  already exists.");
            }

            return Json(true);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid SerialId)
        {
            await repository.DeleteById(SerialId);

            return Redirect("GetAll");
        }

        public IActionResult Update()
        {
            var list = new List<MeterOfPoleDto>();

            list.Add(new MeterOfPoleDto { meterSerialId = "4" });

            var energyMeterCreateViewModel = new EnergyMeterCreateViewModel
            {
                serialId = Guid.Empty,
                Select = "",
                meterOfPoles = list
            };

            return View(energyMeterCreateViewModel);
        }

        // POST: EnergyMeters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromForm] EnergyMeterCreateViewModel energyMeterCreateViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(energyMeterCreateViewModel);
            }
            var userId = _userManager.GetUserId(User);
            var newMeter = energyMeterCreateViewModel.toEnergyMeter(userId);
            await repository.Update(newMeter);

            return Redirect("GetAll");
        }
    }
}