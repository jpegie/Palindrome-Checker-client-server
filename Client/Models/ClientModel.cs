using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Windows.Input;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;
using System.Threading;

namespace Client.Models
{
    internal class ClientModel
    {
        ICommand? checkpalindromeCommand;

        const string ip = "127.0.0.1";
        const int port = 8080;

        IPEndPoint? tcpEndPoint;
       
        public ClientModel()
        {
            tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        }
        public async Task SendToCheckPalindrome(File file)
        {
            await Task.Run(async () => 
            {
                Socket tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    await tcpSocket.ConnectAsync(tcpEndPoint);
                }
                catch (SocketException)
                {
                    MessageBox.Show(
                        "Не удалось подключиться к серверу!\n" +
                        "Попробуйте позже.",
                        "Ошибка",
                        MessageBoxButtons.OK);
                    return;
                }

                tcpSocket.Send(Encoding.UTF8.GetBytes(file.WholeText));
                byte[] buffer = new byte[128];
                int respStrSize = 0;
                StringBuilder response = new StringBuilder();
                do
                {
                    try
                    {
                        respStrSize = tcpSocket.Receive(buffer);
                        response.Append(Encoding.UTF8.GetString(buffer, 0, respStrSize));
                    }
                    catch (SocketException ex) 
                    {
                        /*
                         * SocketException вылетает при многократном вызове метода проверки
                         */
                    }  
                } 
                while (tcpSocket.Available > 0);

                if (response.Length > 0)
                {
                    ResultState respState;
                    try
                    {
                        respState = (ResultState)JsonSerializer.Deserialize(response.ToString(), typeof(ResultState));
                        tcpSocket.Shutdown(SocketShutdown.Both);
                        tcpSocket.Close();
                        file.IsPalindrome = respState;
                    }
                    catch (JsonException ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK);
                    }
                }
            });

        }
        public ICommand CheckPalindrome
        {
            get
            {
                if (checkpalindromeCommand == null)
                {
                    checkpalindromeCommand = new _CheckPalindrome(__CheckPalindrome);
                    return checkpalindromeCommand;
                }
                else return checkpalindromeCommand;
            }
        }
        public void __CheckPalindrome(object file)
        {
            _ = SendToCheckPalindrome((File)file);
        }
    }

    internal class _CheckPalindrome : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        private Action<object> execute_checking;
        
        public _CheckPalindrome(Action<object> execute_checking)
        {
            this.execute_checking = execute_checking;
        }
        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            execute_checking(parameter);
        }
    }

}
