using Newtonsoft.Json.Linq;
using SimpleTcp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using TCP.Model;
using TCP.Utils;

namespace TCP.ViewModel
{
    internal class HouseMeterViewModel
    {
        public HouseMeter houseMeter { get; set; }
        public ICommand StopCommand { get; set; }

        private Client client;

        public HouseMeterViewModel()
        {
            try
            {
                houseMeter = new HouseMeter();

                StopCommand = new RelayCommand(Stop, param => true);
                client = new Client();
                Task.Run(GetCommand);
                Task.Run(SendCountCommand);
            }
            catch (Exception)
            {
            }
        }

        private void Stop(object obj)
        {
            //  client.send(new MeterCommand { type = MeterCommandType.Count, serialId = houseMeter.serialId, value = houseMeter.count });
            houseMeter.Switch = !houseMeter.Switch;
        }

        private void GetCommand()
        {
            client.listener.DataReceived += (sender, message) =>
                {
                    try
                    {
                        handleMessage(message);
                    }
                    catch (NullReferenceException e) { Debug.Write(e.ToString()); }
                };
        }

        private void handleMessage(DataReceivedFromServerEventArgs message)
        {
            var receivedMessage = Encoding.UTF8.GetString(message.Data, 0, message.Data.Length);
            var json = JObject.Parse(receivedMessage);
            var type = json["type"].ToObject<MeterCommandType>();

            switch (type)
            {
                case (MeterCommandType.Ack):
                    handleAck(json["value"]["count"].ToObject<string>());
                    break;
            }
        }

        private void handleAck(string count)
        {
            houseMeter.count = count;
            houseMeter.connect = true;
        }

        private async void SendCountCommand()
        {
            while (true)
            {
                if (!String.IsNullOrEmpty(houseMeter.serialId) && houseMeter.connect == true)
                {
                    await Task.Delay(500);
                    client.send(new MeterCommand { type = MeterCommandType.Count, value = houseMeter });
                }
                else if (!String.IsNullOrEmpty(houseMeter.serialId) && houseMeter.connect == false)
                {
                    await Task.Delay(500);
                    client.send(new MeterCommand { type = MeterCommandType.Syn, value = houseMeter });
                }
            }
        }
    }
}