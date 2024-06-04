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
            Console.WriteLine("Введите сообщение (или 'Exit' для выхода, 'List' для получения непрочитанных сообщений):");

            while (true)
            {
                string messageText = Console.ReadLine();
                if (messageText.ToLower() == "exit")
                {
                    break;
                }
                else if (messageText.ToLower() == "list")
                {
                    RequestUnreadMessages(username);
                    continue;
                }

                SendMessage(username, messageText);
            }

            udpClient.Close();
            Console.WriteLine("Клиент завершил работу.");
        }

        private void SendMessage(string username, string messageText)
        {
            Message message = new Message()
            {
                Text = messageText,
                NicknameFrom = username,
                NicknameTo = "Server",
                DateTime = DateTime.Now,
                Type = MessageType.Regular
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

        private void RequestUnreadMessages(string username)
        {
            Message requestMessage = new Message()
            {
                Text = "Requesting unread messages",
                NicknameFrom = username,
                NicknameTo = "Server",
                DateTime = DateTime.Now,
                Type = MessageType.List
            };
            string json = requestMessage.SerializeMessageToJson();

            byte[] data = Encoding.UTF8.GetBytes(json);
            udpClient.Send(data, data.Length, serverEndPoint);

            // Ожидание и получение непрочитанных сообщений
            IPEndPoint remoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {
                byte[] receiveBytes = udpClient.Receive(ref remoteIpEndPoint);
                string messageText = Encoding.UTF8.GetString(receiveBytes);
                if (string.IsNullOrEmpty(messageText))
                {
                    break;
                }

                Message message = Message.DeserializeFromJson(messageText);
                message.Print();
            }
        }
    }
}
