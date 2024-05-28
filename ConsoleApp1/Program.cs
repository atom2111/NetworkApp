using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Network
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Server("Hello");
        }

        public void task1()
        {
            Message msg = new Message()
            {
                Text = "Hello",
                DateTime = DateTime.Now,
                NicknameFrom = "Anvar",
                NicknameTo = "Atom"
            };
            string json = msg.SerializeMessageToJson();
            Console.WriteLine(json);
            Message? msgDeserialize = Message.DeserializeFromJson(json);
        }

        public static void Server(string name)
        {
            UdpClient udpClient = new UdpClient(12345);
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 0);
            Console.WriteLine("Сервер ждёт сообщение от клиента");

            while (true)
            {
                byte[] buffer = udpClient.Receive(ref iPEndPoint);

                if (buffer == null) break;
                var messageText = Encoding.UTF8.GetString(buffer);

                Message message = Message.DeserializeFromJson(messageText);
                message.Print();

                // Отправка подтверждения клиенту
                byte[] confirmationBytes = Encoding.UTF8.GetBytes("ACK");
                udpClient.Send(confirmationBytes, confirmationBytes.Length, iPEndPoint);
            }
        }

    }
}