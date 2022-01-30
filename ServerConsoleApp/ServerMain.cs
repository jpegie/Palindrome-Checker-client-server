using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Text;

namespace ServerConsoleApp
{
    internal class ServerConsoleApp
    {
        /*const string ip = "127.0.0.1";
        const int port = 8080;*/

        static int maxRequestsAmnt = 0;
       /* static int clientsAmnt = 16;

        static SemaphoreSlim freeStreamsHandlingRequests;
        static IPEndPoint tcpEndPoint;
        static Socket tcpSocket;*/

        static void Main(string[] args)
        {
            Console.Write("[server] макс. кол-во запросов серверу : "); maxRequestsAmnt = int.Parse(Console.ReadLine());
            Server server = new Server(maxRequestsAmnt);
            server.Start();

            /*#region SERVER_CONFIGURATION
            
            
            
            freeStreamsHandlingRequests = new SemaphoreSlim(maxRequestsAmnt, maxRequestsAmnt);
            tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

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
                            if (request.State != States.TryAgain) //если не было ошибок, то определим полиндромность строки запроса
                            {
                                request.State = new PalindromeChecker(request).GetPalindromeState();
                            }
                            responseSender.SendStateAsResponse(request.State); 
                            freeStreamsHandlingRequests.Release();
                        }
                    }
                }).Start();
            }*/
        }
/*
        static void StartRequestsMonitoringThread()
        {
            new Thread(() => 
            {
                MonitorForRequests();
            }).Start();
        }

        static void MonitorForRequests()
        {
            while (true)
            {
                Socket requestListener = tcpSocket.Accept();
                new Thread(() =>
                {
                    RequestHandler requestHandler = new RequestHandler(requestListener);
                    Request handledRequest = (Request)requestHandler.Handle();
                }).Start();
            }
        }*/
    }
}
