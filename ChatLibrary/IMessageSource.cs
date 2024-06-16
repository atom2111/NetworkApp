using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatLibrary
{
    public interface IMessageSource
    {
        event EventHandler<MessageReceivedEventArgs> MessageReceived;
        void Start();
        void Stop();
        void SendMessage(string message);
    }

    public class MessageReceivedEventArgs : EventArgs
    {
        public string Message { get; }

        public MessageReceivedEventArgs(string message)
        {
            Message = message;
        }
    }
}

