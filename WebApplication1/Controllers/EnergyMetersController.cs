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
        public async Task<IActionResult> Index()
        {
            // var applicationDbContext = _context.EnergyMeters.Include(e => e.user);
            return View();
        }

        // GET: EnergyMeters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var energyMeter = await _context.EnergyMeters
                .Include(e => e.user)
                .FirstOrDefaultAsync(m => m.serialId == id);
            if (energyMeter == null)
            {
                return NotFound();
            }

            return View(energyMeter);
        }

        // GET: EnergyMeters/Create
        public IActionResult Create()
        {
            var energyMeterCreateViewModel = new EnergyMeterCreateViewModel
            {
                serialId = 3333,
                Select = "",
                meterOfPoles = new List<MeterOfPoleDto>
                {
                    new MeterOfPoleDto{meterSerialId=14343},
                     new MeterOfPoleDto{meterSerialId=33332}
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
            var user = await _userManager.GetUserAsync(User);
            var energyMeter = EnergyMeter.Create(user, energyMeterCreateViewModel);
            var entityEntry = await _context.EnergyMeters.AddAsync(energyMeter);
            // await _context.SaveChangesAsync();
            energyMeterCreateViewModel = new EnergyMeterCreateViewModel
            {
                serialId = 0,
                Select = "",
                meterOfPoles = new List<MeterOfPoleDto>()
            };
            return View(energyMeterCreateViewModel);
        }

        // GET: EnergyMeters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var energyMeter = await _context.EnergyMeters.FindAsync(id);
            if (energyMeter == null)
            {
                return NotFound();
            }
            ViewData["userId"] = new SelectList(_context.Users, "Id", "Id", energyMeter.userId);
            return View(energyMeter);
        }

        // POST: EnergyMeters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("serialId,userId")] EnergyMeter energyMeter)
        {
            if (id != energyMeter.serialId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(energyMeter);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnergyMeterExists(energyMeter.serialId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["userId"] = new SelectList(_context.Users, "Id", "Id", energyMeter.userId);
            return View(energyMeter);
        }

        // GET: EnergyMeters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var energyMeter = await _context.EnergyMeters
                .Include(e => e.user)
                .FirstOrDefaultAsync(m => m.serialId == id);
            if (energyMeter == null)
            {
                return NotFound();
            }

            return View(energyMeter);
        }

        // POST: EnergyMeters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var energyMeter = await _context.EnergyMeters.FindAsync(id);
            _context.EnergyMeters.Remove(energyMeter);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EnergyMeterExists(int id)
        {
            return _context.EnergyMeters.Any(e => e.serialId == id);
        }
    }
}