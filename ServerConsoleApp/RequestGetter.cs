using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerConsoleApp
{
    internal class RequestGetter
    {
        Socket listener;
        IRequest request;

        public RequestGetter(Socket listener)
        {
            this.listener = listener;
        }
        public IRequest GetRequest()
        {
            request = new Request();
            request.State = States.NotChecked;
            StringBuilder request_str = new StringBuilder(); int reqStrSize = 0;
            byte[] request_buffer = new byte[128]; //буфер, в который записывается полученный запрос от клиента
            
            do
            {
                //оберну получение запроса в try...catch, если получить запрос не получается ->
                //-> то отправлю ответ в виде "попытайтесь еще раз" (TryAgain)
                try
                {
                    reqStrSize = listener.Receive(request_buffer);
                    request_str.Append(Encoding.UTF8.GetString(request_buffer, 0, reqStrSize));
                }
                catch (SocketException)
                {
                    request.State = States.TryAgain;
                }
            }
            while (listener.Available > 0 && request.State != States.TryAgain);
            request.Data = request_str.ToString();
            return request;
        }
    }
}
