using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Network;

namespace Client
{
    public class ChatClient
    {
        private UdpClient udpClient;
        private IPEndPoint serverEndPoint;

        public ChatClient(string serverIp, int serverPort)
        {
            udpClient = new UdpClient();
            serverEndPoint = new IPEndPoint(IPAddress.Parse(serverIp), serverPort);
        }

        public void Start(string username)
        {
            Console.WriteLine("Введите сообщение (или 'Exit' для выхода):");

            while (true)
            {
                string messageText = Console.ReadLine();
                if (messageText.ToLower() == "exit")
                {
                    break;
                }

                Message message = new Message()
                {
                    Text = messageText,
                    NicknameFrom = username,
                    NicknameTo = "Server",
                    DateTime = DateTime.Now
                };
                string json = message.SerializeMessageToJson();

                byte[] data = Encoding.UTF8.GetBytes(json);
                udpClient.Send(data, data.Length, serverEndPoint);

                // Ожидание подтверждения
                IPEndPoint remoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] receiveBytes = udpClient.Receive(ref remoteIpEndPoint);
                string confirmation = Encoding.UTF8.GetString(receiveBytes);
                if (confirmation == "ACK")
                {
                    Console.WriteLine("Сообщение доставлено.");
                }
            }

            udpClient.Close();
            Console.WriteLine("Клиент завершил работу.");
        }
    }
}
