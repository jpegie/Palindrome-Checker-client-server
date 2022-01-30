using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Text;

namespace ServerConsoleApp
{
    internal class ServerConsoleApp
    {
        static int maxRequestsAmnt = 0;
        static void Main(string[] args)
        {
            Console.Write("[server] макс. кол-во запросов серверу : "); maxRequestsAmnt = int.Parse(Console.ReadLine());
            Server server = new Server(maxRequestsAmnt);
            server.Start();
        }
    }
}
