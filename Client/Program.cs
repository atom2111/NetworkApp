using System;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Использование: Client <username> <serverIp>");
                return;
            }

            string username = args[0];
            string serverIp = args[1];

            ChatClient client = new ChatClient(serverIp, 12345);
            client.Start(username);
        }
    }
}
