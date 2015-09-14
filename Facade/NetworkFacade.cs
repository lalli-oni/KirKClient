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
        private ConnectionHandler connectHandler;
        public NetworkFacade()
        {
            connectHandler = new ConnectionHandler();
        }
        public void Connect()
        {
            connectHandler.establishConnection();
        }
    }
}
