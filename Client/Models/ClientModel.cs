using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Client.Models
{
    //логика общения с сервером
    internal class ClientModel
    {
        const string ip = "127.0.0.1";
        const int port = 8080;
        IPEndPoint tcpEndPoint;
        public ClientModel()
        {
            tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            Socket tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            tcpSocket.Bind(tcpEndPoint);    
        }
        public void SendToCheckPalindrome(File file)
        {
            //model в конструктор доложна принимать 
        }
    }
    
}
