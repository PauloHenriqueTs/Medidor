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

        private string userId { get; set; }

        private string Jwt { get; set; }

        public MainViewModel(string token, IEnumerable<EnergyMeter> test)
        {
            Jwt = token;
            //AddCommand = new RelayCommand(Add, e => true);
            server = new Server();
            meters = new ObservableCollection<HouseMeter>();

            foreach (var item in test)
            {
                userId = item.UserId;
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
                //      AddCommand = new DelegateCommand(async (param) => await GetCount(param));
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

        private async Task GetCount(string id)
        {
            await Task.Run(() =>
            {
                var findMeter = meters.FirstOrDefault(m => m.serialId == id);
                var command = new SwitchAmrCommand(id);

                var json = JsonConvert.SerializeObject(command);

                SendCommand(json, findMeter.port);
                var BeforeMeter = meters.FirstOrDefault(m => m.serialId == id);

                Task t = Task.Run(() =>
                {
                    try
                    {
                        var Meter = meters.FirstOrDefault(m => m.serialId == id);
                        while (BeforeMeter.Switch == Meter.Switch)
                        {
                            Meter = meters.FirstOrDefault(m => m.serialId == id);
                        }
                    }
                    catch (Exception) { }

                    Debug.WriteLine("Deu Bom");
                });
                bool test = t.Wait(5000);
                if (!test)
                {
                    Debug.WriteLine("Error   ");
                }
            });
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

            connection.On<string>("ReceiveMessage", async (message) =>
           {
               try
               {
                   var command = System.Text.Json.JsonSerializer.Deserialize<MeterCommand>(message);
                   if (command.type == MeterCommandType.Switch)
                   {
                       await GetCount(command.value.serialId);
                   }
               }
               catch (Exception) { }
           });
            await connection.StartAsync();
            await connection.InvokeAsync("JoinGroup");
        }

        private void GetMessages()
        {
            server.server.DataReceived += async (sender, message) =>
           {
               var data = Encoding.UTF8.GetString(message.Data, 0, message.Data.Length);
               var json = JObject.Parse(data);

               var type = json["Type"].ToObject<AmrCommandType>();
               switch (type)
               {
                   case (AmrCommandType.ACK):
                       await HandleAck(data);
                       break;
               }
           };
        }

        private async Task HandleAck(string data)
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
                await connection.InvokeAsync("SendMessage", data);
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