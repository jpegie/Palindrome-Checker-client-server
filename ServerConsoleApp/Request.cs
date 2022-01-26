using System.Net.Sockets;
using System.Text;

namespace ServerConsoleApp
{
    internal class Request : IRequest
    {
        string data = "";
        ResultState state;
        public string Data
        {
            get { return data; }
            set { data = value; }
        }
        public ResultState State
        {
            get { return state; }
            set { state = value; }
        }
        public void Listen(Socket listener)
        {
            StringBuilder req = new StringBuilder(); int reqStrSize = 0;
            byte[] buffer = new byte[128]; //буфер, в который записывается полученный запрос от клиента
            state = ResultState.NotChecked;
            do
            {
                //оберну получение запроса в try...catch, если получить запрос не получается ->
                //-> то отправлю ответ в виде "попытайтесь еще раз" (TryAgain)
                try
                {
                    reqStrSize = listener.Receive(buffer);
                    req.Append(Encoding.UTF8.GetString(buffer, 0, reqStrSize));
                }
                catch (SocketException)
                {
                    state = ResultState.TryAgain;
                }
            }
            while (listener.Available > 0 && state != ResultState.TryAgain);
            data = req.ToString();
        }
    }

}
