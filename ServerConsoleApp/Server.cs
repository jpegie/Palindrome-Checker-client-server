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

        static int maxReqAmnt;
        static int clientsAmnt = 16;

        static void Main(string[] args)
        {
            #region SERVER_CONFIGURATION
            
            Console.Write("[server] макс. кол-во запросов серверу : "); maxReqAmnt = int.Parse(Console.ReadLine());

            SemaphoreSlim reqController = new SemaphoreSlim(maxReqAmnt, maxReqAmnt);
            IPEndPoint tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            Socket tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            
            tcpSocket.Bind(tcpEndPoint);
            tcpSocket.Listen(clientsAmnt);

            Console.WriteLine("[server] запустился...\n" + new string('-', 50));
            
            #endregion

            for (int i = 0; i < maxReqAmnt*2; ++i)
            {
                new Thread(() =>
                {
                    while (true)
                    {
                        //изначально запустим на 1 поток больше, чтобы он мониторил запросы, которые способствуют перегрузке сервера
                        Socket listener = tcpSocket.Accept();
                        Request request = new Request();
                        Response response = new Response();

                        if (reqController.CurrentCount == 0)
                        {
                            //если "слоты для обработку" закончились, то скажем об этом клиенту, отправив обратно ServerOverloaded статус
                            response.SendResponse(listener, ResultState.ServerOverloaded);
                        }
                        else
                        {
                            reqController.Wait();

                            request.Listen(listener);
                            Thread.Sleep(2000);

                            //если не было ошибок, то определим полиндромность строки запроса
                            if (request.State != ResultState.TryAgain && request.State != ResultState.ServerOverloaded)
                            {
                                request.State = checkPalindromeState(request);
                            }

                            response.SendResponse(listener, request); 
                            reqController.Release();
                        }
                    }
                }).Start();
            }
        }

        static ResultState checkPalindromeState(IRequest req)
        {
            ResultState result = ResultState.Palindrome;

            for(int i=0; i < req.Data.Length/2; ++i)
            {
                if(req.Data[i] != req.Data[req.Data.Length - 1 - i] && Char.ToLower(req.Data[i]) != Char.ToLower(req.Data[req.Data.Length - 1 - i]))
                {
                    result = ResultState.NotPalindrome;
                    return result;
                }
            }
            return result;
        }
    }
}
