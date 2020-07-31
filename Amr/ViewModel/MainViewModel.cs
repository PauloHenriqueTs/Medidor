using Amr.Model;
using Amr.Utils;
using CryptoLib;
using Entities;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
                meters.Add(new HouseMeter { serialId = item.SerialId, Switch = false, count = item.Count });
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
            connection.On<string, string>("ReceiveMessage", async (user, message) =>
            {
                try
                {
                    var command = System.Text.Json.JsonSerializer.Deserialize<MeterCommand>(message);
                    var meter = meters.FirstOrDefault(m => m.serialId == command.value.serialId);

                    server.WriteTCP(command);
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
                    var encrypt = Encoding.UTF8.GetString(message.Data, 0, message.Data.Length);
                    var receivedMessage = Protector.Decrypt(encrypt, "secret");
                    var json = JObject.Parse(receivedMessage);
                    var type = json["type"].ToObject<MeterCommandType>();

                    switch (type)
                    {
                        case (MeterCommandType.Syn):
                            handleSyn(receivedMessage, message.IpPort);
                            break;

                        case (MeterCommandType.Count):
                            await handleCountAsync(receivedMessage);
                            break;
                    }
                }
                catch (Exception)
                {
                }
            };
            server.server.ClientDisconnected += async (sender, message) =>
            {
                var meter = meters.FirstOrDefault(m => String.Equals(m.ip, message.IpPort));
                if (meter != null)
                {
                    meter.ip = null;
                    meter.connect = false;
                    meter.Switch = false;
                    meters[meters.IndexOf(meter)] = meter;
                }
            };
        }

        private void handleSyn(string v, string ipPort)
        {
            var data = JsonConvert.DeserializeObject<MeterCommand>(v);
            var meter = meters.FirstOrDefault(m => m.serialId == data.value.serialId);
            meter.connect = true;
            data.type = MeterCommandType.Ack;
            data.value.count = meter.count;

            meter.ip = ipPort;
            meters[meters.IndexOf(meter)] = meter;

            server.WriteTCP(data);
        }

        private async Task handleCountAsync(string v)
        {
            var data = JsonConvert.DeserializeObject<MeterCommand>(v);
            if (connection != null)
                await connection.InvokeAsync("SendMessage", "Hello", v);
            if (!meters.Any(m => m.serialId == data.value.serialId))
            {
                data.value.Switch = true;
                meters.Add(data.value);
            }
            else
            {
                var meter = meters.FirstOrDefault(m => m.serialId == data.value.serialId);
                meter.Switch = true;
                meter.count = data.value.count;
                meters[meters.IndexOf(meter)] = meter;
            }
        }
    }
}