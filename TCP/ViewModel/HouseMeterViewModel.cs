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

        private string _error { get; set; }

        public string Error
        {
            get { return _error; }
            set
            {
                _error = value;
                OnPropertyChanged(nameof(Error));
            }
        }

        private string _port { get; set; }

        public string Port
        {
            get { return _port; }
            set
            {
                _port = value;
                OnPropertyChanged(nameof(Port));
            }
        }

        private string _serverRunning { get; set; }

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
            houseMeter.serialId = "2";
        }

        private void StartServer(object obj)
        {
            try
            {
                server = new TcpServer("127.0.0.1", Int32.Parse(Port), false, null, null);
                server.ClientConnected += ClientConnected;
                server.DataReceived += HandleDataReceived;
                server.Start();
                handleSyn();
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
            var type = json["type"].ToObject<AmrCommandType>();
            switch (type)
            {
                case (AmrCommandType.SYN):
                    handleSyn();
                    break;
            }
        }

        private void handleSyn()
        {
            var AckCommand = new AckArmCommand(houseMeter.serialId, Port, AmrCommandType.ACK);
            SendCommand(AckCommand);
        }

        private void ClientConnected(object sender, ClientConnectedEventArgs e)
        {
            Debug.WriteLine("[" + e.IpPort + "] client connected");
        }

        private async void SendCommand(IAmrCommand command)
        {
            try
            {
                TcpClient client = new TcpClient("127.0.0.1", 23, false, null, null);
                client.Connect();
                var json = JsonConvert.SerializeObject(command);
                client.Send(Encoding.UTF8.GetBytes(json));
                client.Dispose();
                Error = "";
            }
            catch (Exception) { Error = "Server Disconnect"; }
        }
    }
}