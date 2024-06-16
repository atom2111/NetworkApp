using ChatLibrary;
using System;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var messageSource = new NetMQMessageSource("127.0.0.1", 12345);
            messageSource.MessageReceived += (sender, eventArgs) =>
            {
                Console.WriteLine($"Received message: {eventArgs.Message}");
            };

            messageSource.Start();

            Console.WriteLine("Сервер запущен. Нажмите Enter для выхода...");
            Console.ReadLine();

            messageSource.Stop();
        }
    }
}
