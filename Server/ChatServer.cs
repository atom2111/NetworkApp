using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Network;

namespace Server
{
    public sealed class ChatServer
    {
        private static readonly Lazy<ChatServer> lazyInstance = new Lazy<ChatServer>(() => new ChatServer());
        public static ChatServer Instance => lazyInstance.Value;

        private UdpClient udpClient;
        private CancellationTokenSource cts;

        private ChatServer() { }

        public void Start(int port)
        {
            cts = new CancellationTokenSource();
            udpClient = new UdpClient(port);

            Task.Run(() => ListenForMessages(cts.Token), cts.Token);
            Console.WriteLine("Сервер запущен. Нажмите любую клавишу для завершения работы...");
            Console.ReadKey();
            Stop();
        }

        public void Stop()
        {
            cts.Cancel();
            udpClient.Close();
            Console.WriteLine("Сервер завершил работу.");
        }

        private async Task ListenForMessages(CancellationToken cancellationToken)
        {
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 0);

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    if (udpClient.Available > 0)
                    {
                        byte[] buffer = udpClient.Receive(ref iPEndPoint);
                        var messageText = Encoding.UTF8.GetString(buffer);

                        Message message = Message.DeserializeFromJson(messageText);
                        message.Print();

                        // Отправка подтверждения клиенту
                        byte[] confirmationBytes = Encoding.UTF8.GetBytes("ACK");
                        await udpClient.SendAsync(confirmationBytes, confirmationBytes.Length, iPEndPoint);
                    }
                }
            }
            catch (ObjectDisposedException)
            {
                // Исключение выбрасывается при закрытии udpClient
            }
        }
    }
}