﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Persistence.DAO;
using WebApi.Dto;
using WebApi.Hubs;

using System.Text.Json;
using Persistence;
using WebApi.Command;
using Command;
using MeterCommand = WebApi.Command.MeterCommand;
using MeterCommandType = WebApi.Command.MeterCommandType;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EnergyMetersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly EnergyMeterRepository repository;
        private readonly CommandRepository commandRepository;

        private readonly IHubContext<ChatHub> _chatHubContext;

        public EnergyMetersController(CommandRepository commandRepository, UserManager<ApplicationUser> userManager, EnergyMeterRepository energyMeterRepository, IHubContext<ChatHub> chatHubContext)
        {
            this.commandRepository = commandRepository;
            _userManager = userManager;
            repository = energyMeterRepository;
            _chatHubContext = chatHubContext;
        }

        [HttpPost]
        public async Task<IActionResult> Create(EnergyMeterCreateDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var isExist = await repository.SerialIdExist(model.serialId);
            if (isExist)
            {
                return BadRequest($"A user named {model.serialId}  already exists");
            }

            var userId = _userManager.GetUserId(User);
            var newMeter = model.toEnergyMeter(userId);
            await repository.Create(newMeter);

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EnergyMeter>>> Get()
        {
            var userId = _userManager.GetUserId(User);
            var meters = await repository.Get(userId);

            return meters;
        }

        [HttpPut]
        public async Task<IActionResult> Update(EnergyMetersUpdateDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var userId = _userManager.GetUserId(User);
            var newMeter = model.toEnergyMeter(userId);
            await repository.Update(newMeter);

            return Ok();
        }

        [HttpPost("switch")]
        public async Task<IActionResult> Switch([FromBody] string serialId)
        {
            var userId = _userManager.GetUserId(User);
            var meter = new HouseMeter { count = "0", serialId = serialId, Switch = true };
            var value = new MeterCommand { value = meter, type = MeterCommandType.Switch };
            var message = Newtonsoft.Json.JsonConvert.SerializeObject(value);
            await _chatHubContext.Clients.Group(userId).SendAsync("ReceiveMessage", message);
            await commandRepository.CreateSwitchCommand(serialId, userId);
            return Ok();
        }

        [HttpPost("getCount")]
        public async Task<IActionResult> GetCount([FromBody] string serialId)
        {
            var userId = _userManager.GetUserId(User);
            var EnergyMeter = await repository.GetById(serialId, userId);
            if (EnergyMeter == null)
            {
                return BadRequest();
            }
            var meter = new HouseMeter { count = EnergyMeter.Count, serialId = EnergyMeter.SerialId, Switch = EnergyMeter.SwitchState };
            var value = new MeterCommand { value = meter, type = MeterCommandType.Count };
            var message = Newtonsoft.Json.JsonConvert.SerializeObject(value);
            await _chatHubContext.Clients.Group(userId).SendAsync("ReceiveMessage", message);
            await commandRepository.GetCountCommand(serialId, userId);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await repository.DeleteById(id);

            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EnergyMeter>> GetById(string id)
        {
            var userId = _userManager.GetUserId(User);
            var isExist = await repository.GetById(id, userId);
            if (isExist != null)
            {
                return isExist;
            }

            return BadRequest();
        }

        [HttpGet("verifySerialId")]
        public async Task<IActionResult> VerifySerialId(string serialId)
        {
            var isExist = await repository.SerialIdExist(serialId);
            if (isExist)
            {
                return Ok($"A user named {serialId}  already exists.");
            }

            return Ok(true);
        }
    }
}