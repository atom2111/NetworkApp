using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit; // Используйте xUnit
using Network;
using Client; // Пространство имен вашего клиента

namespace ClientTests
{
    public class ChatClientTests
    {
        private UdpClient serverUdpClient;
        private IPEndPoint serverEndPoint;
        private string receivedMessage;
        private CancellationTokenSource serverCts;

        public ChatClientTests()
        {
            Setup();
        }

        private void Setup()
        {
            serverUdpClient = new UdpClient(12345);
            serverEndPoint = new IPEndPoint(IPAddress.Any, 0);
            serverCts = new CancellationTokenSource();
            receivedMessage = string.Empty;

            Task.Run(() => ServerListener());
        }

        private void Cleanup()
        {
            serverCts.Cancel();
            serverUdpClient.Close();
        }

        private void ServerListener()
        {
            while (!serverCts.Token.IsCancellationRequested)
            {
                byte[] buffer = serverUdpClient.Receive(ref serverEndPoint);
                receivedMessage = Encoding.UTF8.GetString(buffer);
                byte[] confirmationBytes = Encoding.UTF8.GetBytes("ACK");
                serverUdpClient.Send(confirmationBytes, confirmationBytes.Length, serverEndPoint);
            }
        }

        [Fact]
        public void TestSendMessage()
        {
            ChatClient client = new ChatClient("127.0.0.1", 12345);
            client.Start("TestUser");

            Assert.Equal("Test message", receivedMessage);
            Cleanup();
        }
    }
}
