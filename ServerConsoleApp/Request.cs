using System.Net.Sockets;
using System.Text;

namespace ServerConsoleApp
{
    internal class Request : IRequest
    {
        string data = "";
        States state;
        public string Data
        {
            get { return data; }
            set { data = value; }
        }
        public States State
        {
            get { return state; }
            set { state = value; }
        }
    }

}
