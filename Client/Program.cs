using Network;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SendMessage(args[0], args[1]);
        }

        public static void SendMessage(string From, string ip)
        {
            UdpClient udpClient = new UdpClient();
            IPEndPoint IpEndPoint = new IPEndPoint(IPAddress.Parse(ip), 12345);

            while (true)
            {
                string messageText;
                do
                {
                    Console.Clear();
                    Console.WriteLine("Введите сообщение (или 'Exit' для выхода):");
                    messageText = Console.ReadLine();
                }
                while (string.IsNullOrEmpty(messageText));

                if (messageText.ToLower() == "exit")
                {
                    break; // Завершаем цикл и программу
                }

                Message message = new Message()
                {
                    Text = messageText,
                    NicknameFrom = From,
                    NicknameTo = "Server",
                    DateTime = DateTime.Now
                };
                string json = message.SerializeMessageToJson();

                byte[] data = Encoding.UTF8.GetBytes(json);
                udpClient.Send(data, data.Length, IpEndPoint);

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
