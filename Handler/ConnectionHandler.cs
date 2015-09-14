using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using KirkClient.Handler;
using KirKClient.Handler;

namespace KirKClient.Handler
{
    class ConnectionHandler
    {
        private static TcpClient client;
        private IPEndPoint serverEndPoint;

        public ConnectionHandler()
        {
            AddressService adrServ = new AddressService();
            IPAddress serverAddress = adrServ.getIP("localhost");
            serverEndPoint = new IPEndPoint(serverAddress, 6789);
            client = new TcpClient();
        }

        public void establishConnection()
        {
            try
            {
                client.Connect(serverEndPoint);
            }
            catch (Exception e)
            {
                MessageBox.Show("Connection failed!\n " + e.Message);
            }
        }
    }
}
