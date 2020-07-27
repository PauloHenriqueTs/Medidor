using Amr.Model;
using Amr.Utils;
using Entities;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace Amr.ViewModel
{
    public class MainViewModel
    {
        private Server server;
        private HubConnection connection;

        public ObservableCollection<HouseMeter> meters { get; set; }

        public HouseMeter Selected { get; set; }
        public ICommand AddCommand { get; set; }

        private string Jwt { get; set; }

        public MainViewModel(string token, IEnumerable<EnergyMeter> test)
        {
            Jwt = token;
            AddCommand = new RelayCommand(Add, e => true);
            server = new Server();
            meters = new ObservableCollection<HouseMeter>();
            foreach (var item in test)
            {
                meters.Add(new HouseMeter { serialId = item.SerialId, Switch = item.SwitchState, count = item.Count });
            }

            BindingOperations.EnableCollectionSynchronization(meters, new object());
            Task.Run(GetMessages);
            Task.Run(signalr);
        }

        private async Task signalr()
        {
            connection = new HubConnectionBuilder()
             .WithUrl("https://localhost:5001/chathub",
              options =>
              {
                  options.Headers.Add("Authorization", "Bearer " + Jwt);
              })
             .Build();
            connection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                try
                {
                    var meter = JsonSerializer.Deserialize<MeterCommand>(message);
                    server.WriteTCP(meter);
                }
                catch (Exception) { }
            });
            await connection.StartAsync();
        }

        private void Add(object o)
        {
            connection.InvokeAsync("SendMessage", "Hello", "Hello");
            server.WriteTCP(new MeterCommand { value = Selected, type = MeterCommandType.Switch });
        }

        private async Task GetMessages()
        {
            server.server.DataReceived += async (sender, message) =>
            {
                try
                {
                    var data = server.ReadTCP(message);
                    if (connection != null)
                        await connection.InvokeAsync("SendMessage", "Hello", JsonSerializer.Serialize<MeterCommand>(data));
                    if (!meters.Any(m => m.serialId == data.value.serialId))
                    {
                        meters.Add(data.value);
                    }
                    else
                    {
                        var meter = meters.FirstOrDefault(m => m.serialId == data.value.serialId);
                        meters[meters.IndexOf(meter)] = data.value;
                    }
                }
                catch (Exception)
                {
                }
            };
        }
    }
}