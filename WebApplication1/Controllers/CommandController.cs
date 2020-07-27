using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Persistence;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class CommandController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly CommandRepository repository;

        public CommandController(UserManager<ApplicationUser> userManager, CommandRepository commandRepository)
        {
            _userManager = userManager;
            repository = commandRepository;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var commands = await repository.Get(userId);

            return View(commands);
        }
    }
}