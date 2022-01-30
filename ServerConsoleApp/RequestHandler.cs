using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServerConsoleApp
{
    internal class RequestHandler
    {
        Socket requestListener;
        public RequestHandler(Socket requestListener)
        {
            this.requestListener = requestListener;
        }
        public IRequest Handle()
        {
            Request request = new Request();
            request.State = States.NotChecked;
            StringBuilder requestedJson = new StringBuilder(); int reqStrSize = 0;
            byte[] request_buffer = new byte[128];
            do
            {
                try
                {
                    reqStrSize = requestListener.Receive(request_buffer);
                    requestedJson.Append(Encoding.UTF8.GetString(request_buffer, 0, reqStrSize));
                }
                catch (SocketException)
                {
                    request.State = States.TryAgain;
                }
            }
            while (requestListener.Available > 0 && request.State != States.TryAgain);
            request.Data = (String)JsonSerializer.Deserialize(requestedJson.ToString(), typeof(string));
            return request;
        }
    }
}
