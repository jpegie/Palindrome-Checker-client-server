using System.Net.Sockets;

namespace ServerConsoleApp
{
    interface IStatable
    {
        ResultState State { get; set; }
    }
    interface IRequest : IStatable
    {
        string Data { get; set; }
        void Listen(Socket listener);
    }
    interface IResponse
    {
        bool SendResponse(Socket listener, IRequest request);
        bool SendResponse(Socket listener, ResultState state);
        void SendResponse(Socket listener, string response);
    }
}
