using System;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace ServerConsoleApp
{
    internal class Response : IResponse
    {
        /// <summary>
        /// Используется в ситуации, когда удалось определить полиндромность
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool SendResponse(Socket listener, IRequest request)
        {
            string response = "";
            if (SendResponse(listener, request.State) == false)
            {
                Console.WriteLine($"[server] получил запрос : {request.Data.Substring(0, request.Data.Length > 64 ? 64 : request.Data.Length) }... - {request.State}");
                response = JsonSerializer.Serialize(request.State, typeof(ResultState));
            }
            try
            {
                SendResponse(listener, response);
                return true;
            }
            catch (SocketException)
            {
                return false;
            }
        }
        /// <summary>
        /// Используется для отправки отклика, если статус ServerOverloaded или TryAgain
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public bool SendResponse(Socket listener, ResultState state)
        {
            string response = "";
            if (state == ResultState.ServerOverloaded || state == ResultState.TryAgain)
            {
                if (state == ResultState.ServerOverloaded)
                    Console.WriteLine("[server] перегружен!");
                else Console.WriteLine("[server] SocketException (прервано получение запроса клиента)!");

                response = JsonSerializer.Serialize(state, typeof(ResultState));

                try
                {
                    SendResponse(listener, response);
                    return true;
                }
                catch (SocketException)
                {
                    return false;
                }
            }
            else return false;
        }
        /// <summary>
        /// Отправка отклика в виде строки
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="response"></param>
        public void SendResponse(Socket listener, string response)
        {
            listener.Send(Encoding.UTF8.GetBytes(response));
            Thread.Sleep(50);
            listener.Shutdown(SocketShutdown.Both);
            listener.Close();
        }
    }
}
