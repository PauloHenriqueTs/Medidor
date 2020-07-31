using SimpleTcp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using CryptoLib;
using TcpClient = SimpleTcp.TcpClient;

namespace TCP.Utils
{
    public class Client
    {
        public TcpClient listener;

        public Client()
        {
            listener = new TcpClient(GetLocalIPAddress().ToString(), 23, false, null, null);
            listener.Connect();
        }

        public void send(MeterCommand command)
        {
            var sender = JsonSerializer.Serialize(command);
            var encrypt = Protector.Encrypt(sender, "secret");
            listener.Send(Encoding.UTF8.GetBytes(encrypt));
        }

        public MeterCommand Read(DataReceivedFromServerEventArgs message)
        {
            var received = Encoding.UTF8.GetString(message.Data, 0, message.Data.Length);
            var decrypt = Protector.Decrypt(received, "secret");
            return JsonSerializer.Deserialize<MeterCommand>(decrypt);
        }

        public static IPAddress GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip;
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
    }
}