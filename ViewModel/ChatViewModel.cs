using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using KirKClient.Annotations;
using KirKClient.Facade;
using KirKClient.Handler;

namespace KirKClient.ViewModel
{
    public class ChatViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<string> _receivedMessages;
        private string _inputMessage;
        private RelayCommand _connectCommand;
        private NetworkFacade _netFacade;
        private RelayCommand _sendMessageCommand;

        public ObservableCollection<string> ReceivedMessages
        {
            get { return _receivedMessages; }
            set
            {
                _receivedMessages = value;
                OnPropertyChanged("ReceivedMessages");
            }
        }

        public string InputMessage
        {
            get { return _inputMessage; }
            set
            {
                _inputMessage = value;
                OnPropertyChanged("InputMessage");
            }
        }

        public RelayCommand ConnectCommand
        {
            get { return _connectCommand; }
            set { _connectCommand = value; }
        }

        public RelayCommand SendMessageCommand
        {
            get { return _sendMessageCommand; }
            set { _sendMessageCommand = value; }
        }

        public ChatViewModel()
        {
            _netFacade = new NetworkFacade();
            _receivedMessages = new ObservableCollection<string>();
            _connectCommand = new RelayCommand(connectToServer);
            _sendMessageCommand = new RelayCommand(sendMessage);
            Task getMessagesTask = Task.Run(() =>
            {
                while (_netFacade.isConnected)
                {
                    ReceivedMessages.Add(_netFacade.receiveMessages());
                }
            });
            
        }

        public void connectToServer()
        {
            _netFacade.Connect();
        }

        public void sendMessage()
        {
            _netFacade.broadcastMessage(_inputMessage);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
