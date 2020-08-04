using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Persistence.DAO;
using WebApplication1.Command;
using WebApplication1.Hubs;
using WebApplication1.ViewModel;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class EnergyMetersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly EnergyMeterRepository repository;

        private readonly IHubContext<ChatHub> _chatHubContext;

        public EnergyMetersController(UserManager<ApplicationUser> userManager, EnergyMeterRepository energyMeterRepository, IHubContext<ChatHub> chatHubContext)
        {
            _userManager = userManager;
            repository = energyMeterRepository;
            _chatHubContext = chatHubContext;
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
                serialId = "0",
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
            var isExist = await repository.SerialIdExist(energyMeterCreateViewModel.serialId);
            if (isExist)
            {
                return BadRequest($"A user named {energyMeterCreateViewModel.serialId}  already exists");
            }

            var userId = _userManager.GetUserId(User);
            var newMeter = energyMeterCreateViewModel.toEnergyMeter(userId);
            await repository.Create(newMeter);

            return Json(new { redirectToUrl = Url.Action("GetAll", "EnergyMeters") });
        }

        public async Task<IActionResult> GetAll()
        {
            var userId = _userManager.GetUserId(User);
            var meters = await repository.Get(userId);

            return View(meters);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EnergyMeter>>> Get()
        {
            var userId = _userManager.GetUserId(User);
            var meters = await repository.Get(userId);

            return meters;
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
        public async Task<IActionResult> Delete(string SerialId)
        {
            await repository.DeleteById(SerialId);

            return Redirect("GetAll");
        }

        [HttpPost]
        public async Task<IActionResult> Switch(string SerialId)
        {
            var userId = _userManager.GetUserId(User);
            var meter = new HouseMeter { count = "0", serialId = SerialId, Switch = true };
            var value = new MeterCommand { value = meter, type = MeterCommandType.Switch };
            var message = JsonSerializer.Serialize(value);
            await _chatHubContext.Clients.Group(userId).SendAsync("ReceiveMessage", message);
            return Redirect("GetAll");
        }

        public async Task<IActionResult> Update(string SerialId)
        {
            var userId = _userManager.GetUserId(User);
            var meter = await repository.GetById(SerialId, userId);

            var model = new EnergyMeterUpdateViewModel(meter);

            return View(model);
        }

        // POST: EnergyMeters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromForm] EnergyMeterUpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var userId = _userManager.GetUserId(User);
            var newMeter = model.toEnergyMeter(userId);
            await repository.Update(newMeter);

            return Json(new { redirectToUrl = Url.Action("GetAll", "EnergyMeters") });
        }
    }
}