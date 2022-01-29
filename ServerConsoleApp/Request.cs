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
    }

}
