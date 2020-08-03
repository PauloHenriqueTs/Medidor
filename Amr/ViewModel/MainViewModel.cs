using Amr.Model;
using Amr.Utils;
using Command.AmrCommand;
using CryptoLib;
using Entities;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleTcp;
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
            //AddCommand = new RelayCommand(Add, e => true);
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

        public MainViewModel()
        {
            try
            {
                server = new Server();
                meters = new ObservableCollection<HouseMeter>();
                meters.Add(new HouseMeter { serialId = "2", count = "200", connect = true, Switch = false });
                Task.Run(GetMessages);
                BindingOperations.EnableCollectionSynchronization(meters, new object());
            }
            catch (Exception)
            {
            }
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

        private async Task GetMessages()
        {
            server.server.DataReceived += (sender, message) =>
            {
                var data = Encoding.UTF8.GetString(message.Data, 0, message.Data.Length);
                var json = JObject.Parse(data);

                var type = json["Type"].ToObject<AmrCommandType>();
                switch (type)
                {
                    case (AmrCommandType.ACK):
                        HandleAck(data);
                        break;
                }
            };
        }

        private void HandleAck(string data)
        {
            var command = JsonConvert.DeserializeObject<AckArmCommand>(data);
            var meter = meters.FirstOrDefault(m => m.serialId == command.Meter.serialId);
            meter.port = Int32.Parse(command.port);
            if (command.Meter.count == null)
            {
                var SynCommand = new SynArmCommand(new AmrMeter(meter.serialId, meter.count, meter.Switch));
                var json = JsonConvert.SerializeObject(SynCommand);
                SendCommand(json, meter.port);
            }
            else
            {
                meter.count = command.Meter.count;
                meter.Switch = command.Meter.Switch;
                meters[meters.IndexOf(meter)] = meter;
            }
        }

        private void SendCommand(string c, int port)
        {
            try
            {
                TcpClient client = new TcpClient("127.0.0.1", port, false, null, null);
                client.Connect();

                client.Send(Encoding.UTF8.GetBytes(c));
                client.Dispose();
            }
            catch (Exception) { }
        }
    }
}