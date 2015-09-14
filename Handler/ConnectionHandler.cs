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

        public ConnectionHandler()
        {
            serverEndPoint = new IPEndPoint(IPAddress.Parse("10.200.128.141"), 6789);
            client = new TcpClient();
        }

        public bool establishConnection()
        {
            try
            {
                client.Connect(serverEndPoint);
                stream = client.GetStream();
                sr = new StreamReader(stream);
                sw = new StreamWriter(stream);
                sw.WriteLine("192.168.0.1" + "~" + "Bob");
                sw.Flush();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Connection failed!\n " + e.Message);
                return false;
            }
        }

        public string listenForMessages()
        {
            return sr.ReadToEnd();
        }
    }
}
