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
using WebApplication1.Command;

namespace WebApplication1.Hubs
{
    [Authorize(Policy = "Signalr")]
    public class ChatHub : Hub
    {
        private readonly EnergyMeterRepository repository;

        public ChatHub(EnergyMeterRepository repository)
        {
            this.repository = repository;
        }

        public async Task SendMessage(string user, string message)
        {
            try
            {
                var userId = NameUserIdProvider.GetUserId(Context);
                Debug.WriteLine(message);
                var command = JsonSerializer.Deserialize<MeterCommand>(message);
                var meter = new EnergyMeter(command.value.serialId, userId, TypeOfEnergyMeter.House, null, command.value.count, command.value.Switch);
                await repository.Update(meter);
            }
            catch (Exception) { }

            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task ErrorMessage(string message)
        {
            Debug.WriteLine(message);
            await Clients.All.SendAsync("ReceiveMessage", "hello", message);
        }
    }
}