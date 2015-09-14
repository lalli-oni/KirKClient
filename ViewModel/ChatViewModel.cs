using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KirKClient.Facade;
using KirKClient.Handler;

namespace KirKClient.ViewModel
{
    class ChatViewModel
    {
        private ObservableCollection<string> _receivedMessages;
        private string _inputMessage;
        private RelayCommand _connectCommand;
        private NetworkFacade _netFacade;

        public ObservableCollection<string> ReceivedMessages
        {
            get { return _receivedMessages; }
            set { _receivedMessages = value; }
        }

        public string InputMessage
        {
            get { return _inputMessage; }
            set { _inputMessage = value; }
        }

        public RelayCommand ConnectCommand
        {
            get { return _connectCommand; }
            set { _connectCommand = value; }
        }

        public ChatViewModel()
        {
            _netFacade = new NetworkFacade();
            _receivedMessages = new ObservableCollection<string>();
            _connectCommand = new RelayCommand(connectToServer);
        }

        public void connectToServer()
        {
            _netFacade.Connect();
        }
    }
}
