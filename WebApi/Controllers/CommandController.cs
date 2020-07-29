using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Command;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Persistence;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommandController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly CommandRepository repository;

        public CommandController(UserManager<ApplicationUser> userManager, CommandRepository commandRepository)
        {
            _userManager = userManager;
            repository = commandRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<MeterCommand>>> Index()
        {
            var userId = _userManager.GetUserId(User);
            var commands = await repository.Get(userId);

            return Ok(commands);
        }
    }
}