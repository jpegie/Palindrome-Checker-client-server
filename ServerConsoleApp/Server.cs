using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Collections.Generic;
using System.Text.Json;

namespace ServerConsoleApp
{
    internal class Server
    {
        const string ip = "127.0.0.1";
        const int port = 8080;

        static int maxReqAmnt;

        static void Main(string[] args)
        {
            #region server configuration
            
            Console.Write("[server] макс. кол-во запросов серверу : ");
            maxReqAmnt = int.Parse(Console.ReadLine());

            SemaphoreSlim JIexa = new SemaphoreSlim(maxReqAmnt, maxReqAmnt);
            IPEndPoint tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            Socket tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            tcpSocket.Bind(tcpEndPoint);
            tcpSocket.Listen(16);

            Console.WriteLine("[server] запустился...\n" + new string('-', 50));
            
            #endregion

            for (int i = 0; i < maxReqAmnt*2; ++i)
            {
                new Thread(() =>
                {
                    while (true)
                    {
                        Socket listener = tcpSocket.Accept();

                        if(JIexa.CurrentCount == 0)
                        {
                            Console.WriteLine("[server] перегружен!");

                            string response = JsonSerializer.Serialize(ResultState.ServerOverloaded, typeof(ResultState));
                            //if(listener.SendAsync(tcpEndPoint, SocketFlags.None);
                            listener.Send(Encoding.UTF8.GetBytes(response));
                            Thread.Sleep(50); //вместо проверки на доставку сообщения...Как реализовать по-другому???
                            listener.Shutdown(SocketShutdown.Both);
                            listener.Close();
                        }
                        else
                        {
                            JIexa.Wait();
                            StringBuilder reqStr = new StringBuilder(); int reqStrSize = 0;
                            byte[] buffer = new byte[128];

                            do
                            {
                                reqStrSize = listener.Receive(buffer);
                                reqStr.Append(Encoding.UTF8.GetString(buffer, 0, reqStrSize));
                            }
                            while (listener.Available > 0);

                            #region sending

                            Thread.Sleep(2000);

                            ResultState reqPolindromeState = checkPalindromeState(reqStr.ToString());

                            string response = JsonSerializer.Serialize(reqPolindromeState, typeof(ResultState));

                            listener.Send(Encoding.UTF8.GetBytes(response));
                            listener.Shutdown(SocketShutdown.Both);
                            listener.Close();

                            Console.WriteLine($"[server] получил запрос : {reqStr.ToString().Substring(0, reqStr.Length > 64 ? 64 : reqStr.Length) }... - {reqPolindromeState}");
                            #endregion

                            JIexa.Release();
                        }
                        /*if (JIexa.CurrentCount <= maxReqAmnt)
                        {
                            JIexa.Wait();
                            StringBuilder reqStr = new StringBuilder(); int reqStrSize = 0;
                            byte[] buffer = new byte[128];

                            do
                            {
                                reqStrSize = listener.Receive(buffer);
                                reqStr.Append(Encoding.UTF8.GetString(buffer, 0, reqStrSize));
                            }
                            while (listener.Available > 0);

                            #region sending

                            Thread.Sleep(2000);

                            ResultState reqPolindromeState = checkPalindromeState(reqStr.ToString());

                            string response = JsonSerializer.Serialize(reqPolindromeState, typeof(ResultState));

                            listener.Send(Encoding.UTF8.GetBytes(response));
                            listener.Shutdown(SocketShutdown.Both);
                            listener.Close();

                            Console.WriteLine($"[server] получил запрос : {reqStr.ToString().Substring(0, reqStr.Length > 64 ? 64 : reqStr.Length) }... - {reqPolindromeState}");
                            #endregion

                            JIexa.Release();
                        }
                        else
                        {
                            Console.WriteLine("[server] перегружен!");

                            string response = JsonSerializer.Serialize(ResultState.ServerOverloaded, typeof(ResultState));

                            listener.Send(Encoding.UTF8.GetBytes(response));
                            listener.Shutdown(SocketShutdown.Both);
                            listener.Close();
                        }*/
                    }
                }).Start();
            }
        }

        static ResultState checkPalindromeState(string str)
        {
            ResultState result = ResultState.Palindrome;

            for(int i=0; i < str.Length/2; ++i)
            {
                if(str[i] != str[str.Length - 1 - i] && Char.ToLower(str[i]) != Char.ToLower(str[str.Length - 1 - i]))
                {
                    result = ResultState.NotPalindrome;
                    return result;
                }
            }
            return result;
        }
    }
}
