using System;
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
        private ObservableCollection<string> _receivedMessages;
        private string _inputMessage;
        private RelayCommand _connectCommand;
        private RelayCommand _disconnectCommand;
        private ConnectionHandler _connection;
        private RelayCommand _sendMessageCommand;
        private string _userName;
        private Task getMessagesTask;

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
            get
            {
                if (_connection.isConnected)
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
            _receivedMessages = new ObservableCollection<string>();
            _connectCommand = new RelayCommand(connectToServer);
            _disconnectCommand = new RelayCommand(_connection.Disconnect);
            _sendMessageCommand = new RelayCommand(sendMessage);
            getMessagesTask = new Task(() =>
            {
                while (true)
                {
                    string receivedMessage = _connection.ListenForMessage();

                    //Invokes the main thread and performs the action
                    //Done because ObservableCollection doesn't allow updating outside of the invoking thread.
                    Application.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        ReceivedMessages.Add(receivedMessage);
                    }));
                }
            });
            
        }

        public void connectToServer()
        {
            ReceivedMessages.Add("Please input your desired username...");
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
                        ReceivedMessages.Add("Connection timeout. Please try re-connecting.");
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
                    ReceivedMessages.Add("Connection Failed.");
                }
            });
        }

        public void sendMessage()
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
