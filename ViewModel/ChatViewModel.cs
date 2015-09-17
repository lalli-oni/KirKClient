using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using KirKClient.Annotations;
using KirKClient.Handler;

namespace KirKClient.ViewModel
{
    public class ChatViewModel : INotifyPropertyChanged
    {
        private ConcurrentDictionary<string, string> _receivedMessages;
        private string _inputMessage;
        private RelayCommand _connectCommand;
        private RelayCommand _disconnectCommand;
        private ConnectionHandler _connection;
        private RelayCommand _sendMessageCommand;
        private string _userName;
        private Task getMessagesTask;

        public ConcurrentDictionary<string, string> ReceivedMessages
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
            get
            {
                if (!_connection.isConnected)
                {
                    return _connectCommand;
                }
                return _disconnectCommand;
            }
            set { _connectCommand = value; }
        }

        public RelayCommand SendMessageCommand
        {
            get { return _sendMessageCommand; }
            set { _sendMessageCommand = value; }
        }

        public ChatViewModel()
        {
            _connection = new ConnectionHandler();
            _receivedMessages = new ConcurrentDictionary<string, string>();
            _connectCommand = new RelayCommand(ConnectToServer);
            _disconnectCommand = new RelayCommand(_connection.Disconnect);
            _sendMessageCommand = new RelayCommand(SendMessage);
            getMessagesTask = new Task(() =>
            {
                while (true)
                {
                    string receivedMessage = _connection.ListenForMessage();
                    string[] splitMsg = receivedMessage.Split('~');
                    ReceivedMessages.TryAdd(splitMsg[0], splitMsg[1]);
                    OnPropertyChanged(nameof(ReceivedMessages));
                }
            });
            
        }

        public void ConnectToServer()
        {
            ReceivedMessages.TryAdd("","Please input your desired username...");
            Task connectingTask = Task.Run(() =>
            {
                int i = 0;
                if (_connection.EstablishConnection().Result || i < 100)
                {
                    i = 0;
                    while (_inputMessage == null || i < 1000)
                    {
                        i ++;
                    }
                    if (i < 90)
                    {
                        _connection.Disconnect();
                        ReceivedMessages.TryAdd("","Connection timeout. Please try re-connecting.");
                        OnPropertyChanged(nameof(ReceivedMessages));
                        return;
                    }
                    _userName = InputMessage;
                    InputMessage = "";
                    if (_connection.RegisterUsername(_userName).Result)
                    {
                        getMessagesTask.Start();
                    }
                }
                else
                {
                    _connection.Disconnect();
                    ReceivedMessages.TryAdd("","Connection Failed.");
                    OnPropertyChanged(nameof(ReceivedMessages));
                }
            });
        }

        public void SendMessage()
        {
            if (_inputMessage != null)
            {
                _connection.SendMessage(_inputMessage);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
