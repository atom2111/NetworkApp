using System;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ChatServer.Instance.Start(12345);
        }
    }
}
