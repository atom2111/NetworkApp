using System;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string serverIp = args.Length > 0 ? args[0] : "127.0.0.1";
            int serverPort = args.Length > 1 ? int.Parse(args[1]) : 12345;
            string username = args.Length > 2 ? args[2] : "Atom";

            ChatClient client = new ChatClient(serverIp, serverPort);
            client.Start(username);
        }
    }
}
