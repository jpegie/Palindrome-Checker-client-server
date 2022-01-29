using System;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace ServerConsoleApp
{
    internal class ResponseSender : IResponseSender
    {
        Socket socketListener;
        public ResponseSender(Socket socketListener)
        {
            this.socketListener = socketListener;
        }
        /// <summary>
        /// Отправка состояния полиндромности/ошибки сервера
        /// </summary>
        /// <param name="state">Состояние запроса</param>
        /// <returns></returns>
        public bool SendStateAsResponse(States state)
        {
            string response = JsonSerializer.Serialize(state, typeof(States));
            try
            {
                SendStringAsResponse(response);
                return true;
            }
            catch (SocketException)
            {
                return false;
            }
        }

        /// <summary>
        /// Отправка отклика в виде строки
        /// </summary>
        /// <param name="response"></param>
        public void SendStringAsResponse(string response)
        {
            socketListener.Send(Encoding.UTF8.GetBytes(response));
            Thread.Sleep(50);
            socketListener.Shutdown(SocketShutdown.Both);
            socketListener.Close();
        }
    }
}
