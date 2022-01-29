using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace ServerConsoleApp
{
    internal class Server
    {
        const string ip = "127.0.0.1";
        const int port = 8080;

        static int maxRequestsAmnt = 0;
        static int clientsAmnt = 16;

        static void Main(string[] args)
        {
            #region SERVER_CONFIGURATION
            
            Console.Write("[server] макс. кол-во запросов серверу : "); maxRequestsAmnt = int.Parse(Console.ReadLine());

            SemaphoreSlim freeStreamsHandlingRequests = new SemaphoreSlim(maxRequestsAmnt, maxRequestsAmnt);
            IPEndPoint tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            Socket tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            
            tcpSocket.Bind(tcpEndPoint);
            tcpSocket.Listen(clientsAmnt);

            Console.WriteLine("[server] запустился...");
            
            #endregion

            for (int i = 0; i < maxRequestsAmnt + 1; ++i)
            {
                new Thread(() =>
                {
                    while (true)
                    {
                        //изначально запустим на 1 поток больше, чтобы он мониторил запросы, которые способствуют перегрузке сервера
                        Socket clientSocket = tcpSocket.Accept();
                        RequestGetter requestGetter = new RequestGetter(clientSocket);

                        IRequest request = new Request();
                        IResponseSender responseSender = new ResponseSender(clientSocket);
                        
                        if (freeStreamsHandlingRequests.CurrentCount == 0)
                        {   
                            responseSender.SendStateAsResponse(States.ServerOverloaded); //если "слоты для обработку" закончились, то скажем об этом клиенту, отправив обратно ServerOverloaded статус
                        }
                        else
                        {
                            freeStreamsHandlingRequests.Wait();
                            request = requestGetter.GetRequest();
                            if (request.State != States.TryAgain && request.State != States.ServerOverloaded) //если не было ошибок, то определим полиндромность строки запроса
                            {
                                request.State = new PalindromeChecker(request).GetPalindromeState();
                            }
                            responseSender.SendStateAsResponse(request.State); 
                            freeStreamsHandlingRequests.Release();
                        }
                    }
                }).Start();
            }
        }        
    }
}
