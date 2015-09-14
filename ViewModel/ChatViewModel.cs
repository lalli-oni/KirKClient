﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirKClient.ViewModel
{
    class ChatViewModel
    {
        private ObservableCollection<string> _receivedMessages;
        private string _inputMessage;

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

        public ChatViewModel()
        {
            _receivedMessages = new ObservableCollection<string>();
        }
    }
}
