using NetMQ;
using NetMQ.Sockets;
using System;
using System.Threading.Tasks;

namespace ChatLibrary
{
    public class NetMQMessageSource : IMessageSource, IMessageSourceClient
    {
        private readonly string _address;
        private readonly int _port;
        private ResponseSocket _responseSocket;
        private RequestSocket _requestSocket;

        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        public NetMQMessageSource(string address, int port)
        {
            _address = address;
            _port = port;
        }

        public void Start()
        {
            Task.Run(() =>
            {
                using (_responseSocket = new ResponseSocket())
                {
                    _responseSocket.Bind($"tcp://{_address}:{_port}");
                    Console.WriteLine($"Response socket bound to tcp://{_address}:{_port}");

                    while (true)
                    {
                        var message = _responseSocket.ReceiveFrameString();
                        Console.WriteLine($"Message received: {message}");
                        MessageReceived?.Invoke(this, new MessageReceivedEventArgs(message));
                        _responseSocket.SendFrame("ACK");
                    }
                }
            });
        }

        public void Stop()
        {
            _responseSocket?.Close();
        }

        public void Connect()
        {
            _requestSocket = new RequestSocket();
            _requestSocket.Connect($"tcp://{_address}:{_port}");
            Console.WriteLine($"Request socket connected to tcp://{_address}:{_port}");
        }

        public void SendMessage(string message)
        {
            _requestSocket.SendFrame(message);
            Console.WriteLine($"Message sent: {message}");
            var response = _requestSocket.ReceiveFrameString();
            Console.WriteLine($"Response received: {response}");
        }
    }
}
