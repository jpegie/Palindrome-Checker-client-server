﻿using System.Net.Sockets;

namespace ServerConsoleApp
{
    interface IStatable
    {
        ResultState State { get; set; }
    }
    interface IRequest : IStatable
    {
        string Data { get; set; }  
    }
    interface IResponseSender
    {
        bool SendStateAsResponse(ResultState state);
        void SendStringAsResponse(string response);
    }
}
