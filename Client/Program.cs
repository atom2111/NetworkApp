using ChatLibrary;
using System;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var messageSourceClient = new NetMQMessageSource("127.0.0.1", 12345);
            messageSourceClient.Connect();

            Console.WriteLine("Введите сообщение для отправки:");
            while (true)
            {
                var message = Console.ReadLine();
                messageSourceClient.SendMessage(message);
            }
        }
    }
}
