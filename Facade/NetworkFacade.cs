using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KirKClient.Handler;

namespace KirKClient.Facade
{
    class NetworkFacade
    {
        public bool isConnected;
        private ConnectionHandler connectHandler;
        public NetworkFacade()
        {
            isConnected = false;
            connectHandler = new ConnectionHandler();
        }
        public void Connect()
        {
            isConnected = connectHandler.establishConnection();
        }

        public string receiveMessages()
        {
            return connectHandler.listenForMessages();
        }

        public void broadcastMessage(string inpString)
        {
            connectHandler.sendMessage(inpString);
        }
    }
}
