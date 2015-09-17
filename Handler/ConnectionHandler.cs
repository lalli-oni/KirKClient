using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Converters;
using KirkClient.Handler;
using KirKClient.Handler;

namespace KirKClient.Handler
{
    class ConnectionHandler
    {
        private static TcpClient client;
        private IPEndPoint serverEndPoint;
        private NetworkStream stream;
        private StreamReader sr;
        private StreamWriter sw;
        public bool isConnected;

        public ConnectionHandler()
        {
            serverEndPoint = new IPEndPoint(IPAddress.Parse("10.200.128.171"), 6789);
            client = new TcpClient();
        }

        public async Task<bool> EstablishConnection()
        {
            try
            {
                client.Connect(serverEndPoint);
                stream = client.GetStream();
                sr = new StreamReader(stream);
                sw = new StreamWriter(stream) {AutoFlush = true};
                sw.WriteLine("192.168.0.1");
                isConnected = true;
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Connection failed!\n " + e.Message);
                isConnected = false;
                return false;
            }
        }

        public async Task<bool> RegisterUsername(string userName)
        {
            try
            {
                await sw.WriteLineAsync(userName);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Connection failed!\n " + e.Message);
                isConnected = false;
                return false;
            }
        }

        public string ListenForMessage()
        {
            try
            {
                return sr.ReadLine();
            }
            catch (Exception e)
            {
                return "Error: " + e.Message;
            }
        }

        public void SendMessage(string inpString)
        {
            sw.WriteLine(inpString);
        }
    }
}
