using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Meter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                Connect("192.168.1.7", "Hello I'm Device 1...");
            }).Start();
        }

        private static void Connect(String server, String message)
        {
            try
            {
                Int32 port = 13000;
                TcpClient client = new TcpClient(server, port);
                NetworkStream stream = client.GetStream();
                int count = 0;
                while (count++ < 3)
                {
                    // Translate the Message into ASCII.
                    Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                    // Send the message to the connected TcpServer.
                    stream.Write(data, 0, data.Length);
                    Console.WriteLine("Sent: {0}", message);
                    // Bytes Array to receive Server Response.
                    data = new Byte[256];
                    String response = String.Empty;
                    // Read the Tcp Server Response Bytes.
                    Int32 bytes = stream.Read(data, 0, data.Length);
                    response = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                    Console.WriteLine("Received: {0}", response);
                    Thread.Sleep(2000);
                }
                stream.Close();
                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e);
            }
            Console.Read();
        }
    }
}