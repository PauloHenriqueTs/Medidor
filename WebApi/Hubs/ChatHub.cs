using Command.AmrCommand;
using Entities;
using Entities.ValueObjects;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Persistence.DAO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using WebApi;
using WebApi.Command;

namespace WebApi.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChatHub : Hub
    {
        private readonly EnergyMeterRepository repository;

        public ChatHub(EnergyMeterRepository repository)
        {
            this.repository = repository;
        }

        public async Task JoinGroup()
        {
            //Get userId
            var userId = NameUserIdProvider.GetUserId(Context);
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        }

        public async Task SendMessage(string message)
        {
            var userId = NameUserIdProvider.GetUserId(Context);
            if (userId != null)
            {
                var command = Newtonsoft.Json.JsonConvert.DeserializeObject<AckArmCommand>(message);
                var meter = new EnergyMeter(command.Meter.serialId, userId, TypeOfEnergyMeter.House, null, command.Meter.count, command.Meter.Switch);
                await repository.Update(meter);
            }
        }

        public async Task ErrorMessage(string message)
        {
            var userId = NameUserIdProvider.GetUserId(Context);
            await Clients.Group(userId).SendAsync("ReceiveMessage", message);
        }
    }
}