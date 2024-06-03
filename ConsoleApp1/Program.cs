using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Network
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Server();
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

        public static void Server()
        {
            UdpClient udpClient = new UdpClient(12345);
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 0);
            Console.WriteLine("Сервер ждёт сообщение от клиента");

            // Асинхронный вызов для ожидания нажатия клавиши
            Task.Run(() =>
            {
                Console.WriteLine("Нажмите любую клавишу для завершения работы сервера...");
                Console.ReadKey();
                udpClient.Close();
            });

            try
            {
                while (true)
                {
                    byte[] buffer = udpClient.Receive(ref iPEndPoint);

                    if (buffer == null || buffer.Length == 0) break;
                    var messageText = Encoding.UTF8.GetString(buffer);

                    Message message = Message.DeserializeFromJson(messageText);
                    message.Print();

                    // Отправка подтверждения клиенту
                    byte[] confirmationBytes = Encoding.UTF8.GetBytes("ACK");
                    udpClient.Send(confirmationBytes, confirmationBytes.Length, iPEndPoint);
                }
            }
            catch (ObjectDisposedException)
            {
                // Это исключение возникает, когда udpClient закрывается из-за нажатия клавиши
                Console.WriteLine("Сервер завершил работу.");
            }
        }
    }
}
