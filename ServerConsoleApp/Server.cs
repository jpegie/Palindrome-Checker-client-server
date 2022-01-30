using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ServerConsoleApp
{
    internal class Server
    {
        const string ip = "127.0.0.1";
        const int port = 8080;

        Socket tcpSocket;
        IPEndPoint tcpEndPoint;

        int maxRequestsAmnt = 0;
        int clientsAmnt = 16;

        SemaphoreSlim freeThreadsHandlingRequests;

        public Server(int maxRequestsAmnt)
        {
            this.maxRequestsAmnt = maxRequestsAmnt;
            Configure();
        }
        
        public void Start()
        {
            StartRequestsMonitoringThread(); 
        }

        private void Configure()
        {
            freeThreadsHandlingRequests = new SemaphoreSlim(maxRequestsAmnt, maxRequestsAmnt);
            tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            tcpSocket.Bind(tcpEndPoint);
            tcpSocket.Listen(clientsAmnt);
            PrintMessage("сервер запустился");
        }
        private void StartRequestsMonitoringThread()
        {
            new Thread(MonitorForRequests).Start();
        }
        private void MonitorForRequests()
        {
            while (true)
            {
                Socket requestListener = tcpSocket.Accept();

                RequestHandler requestHandler = new RequestHandler(requestListener);
                ResponseSender responseSender = new ResponseSender(requestListener);

                if (freeThreadsHandlingRequests.CurrentCount != 0)
                { 
                    new Thread(() =>
                    {
                        freeThreadsHandlingRequests.Wait();

                        Request handledRequest = (Request)requestHandler.Handle();
                        if (handledRequest != null)
                        {
                            handledRequest.State = PalindromeChecker.GetPalindromeState(handledRequest);
                            responseSender.SendStateAsResponse(handledRequest.State);
                        }
                        else
                        {
                            responseSender.SendStateAsResponse(States.TryAgain);
                        }

                        freeThreadsHandlingRequests.Release();
                    }).Start();  
                }
                else
                {
                    responseSender.SendStateAsResponse(States.ServerOverloaded);
                }
            }
        }
        private void PrintMessage(string message)
        {
            Console.WriteLine($"[server] {message}...");
        }
    }
}
