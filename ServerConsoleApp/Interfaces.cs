using System.Net.Sockets;

namespace ServerConsoleApp
{
    interface IStatable
    {
        States State { get; set; }
    }
    interface IRequest : IStatable
    {
        string Data { get; set; }  
    }
    interface IResponseSender
    {
        bool SendStateAsResponse(States state);
        void SendStringAsResponse(string response);
    }
}
