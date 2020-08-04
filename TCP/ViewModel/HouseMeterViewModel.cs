using Command.AmrCommand;
using CryptoLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleTcp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    internal class HouseMeterViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string nomePropriedade)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nomePropriedade));
        }

        public HouseMeter houseMeter { get; set; }
        public ICommand StartServerCommand { get; set; }
        private string _port { get; set; }
        private string _error { get; set; }
        private string _serverRunning { get; set; }

        public string Error
        {
            get { return _error; }
            set
            {
                _error = value;
                OnPropertyChanged(nameof(Error));
            }
        }

        public string Port
        {
            get { return _port; }
            set
            {
                _port = value;
                OnPropertyChanged(nameof(Port));
            }
        }

        public string ServerRunning
        {
            get { return _serverRunning; }
            set
            {
                _serverRunning = value;
                OnPropertyChanged(nameof(ServerRunning));
            }
        }

        private TcpServer server;

        public HouseMeterViewModel()
        {
            StartServerCommand = new RelayCommand(StartServer);
            houseMeter = new HouseMeter();
        }

        private void StartServer(object obj)
        {
            try
            {
                server = new TcpServer("127.0.0.1", Int32.Parse(Port), false, null, null);
                server.DataReceived += HandleDataReceived;
                server.Start();
                SendAck();
                Error = "";
                ServerRunning = "Running";
            }
            catch (Exception)
            {
                Error = "Invalid Port";
            }
        }

        private void HandleDataReceived(object sender, DataReceivedFromClientEventArgs message)
        {
            var data = Encoding.UTF8.GetString(message.Data, 0, message.Data.Length);
            var json = JObject.Parse(data);
            var type = json["Type"].ToObject<AmrCommandType>();
            switch (type)
            {
                case (AmrCommandType.GetCount):
                    HandleGetCount(data);
                    break;

                case (AmrCommandType.SYN):
                    HandleSYN(data);
                    break;

                case (AmrCommandType.Switch):
                    HandleSwitch(data);
                    break;
            }
        }

        private void HandleSwitch(string data)
        {
            var command = JsonConvert.DeserializeObject<SwitchAmrCommand>(data);
            if (houseMeter.serialId == command.serialId)
            {
                houseMeter.Switch = !houseMeter.Switch;
                SendAck();
            }
        }

        private void HandleSYN(string data)
        {
            var command = JsonConvert.DeserializeObject<SynArmCommand>(data);
            if (houseMeter.serialId == command.Meter.serialId)
            {
                houseMeter.count = command.Meter.count;
                houseMeter.Switch = command.Meter.Switch;
                houseMeter.connect = true;
            }
            SendAck();
        }

        private void HandleGetCount(string data)
        {
            SendAck();
        }

        private void SendAck()
        {
            var AckCommand = new AckArmCommand(new AmrMeter(houseMeter.serialId, houseMeter.count, houseMeter.Switch), Port);
            var json = JsonConvert.SerializeObject(AckCommand);
            SendCommand(json);
        }

        private void SendCommand(string c)
        {
            try
            {
                TcpClient client = new TcpClient("127.0.0.1", 23, false, null, null);
                client.Connect();

                client.Send(Encoding.UTF8.GetBytes(c));
                client.Dispose();
                Error = "";
            }
            catch (Exception) { Error = "Server Disconnect"; }
        }
    }
}