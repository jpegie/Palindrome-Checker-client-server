using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Ookii.Dialogs.Wpf;
using System.Windows.Input;
using System.Net.Sockets;
using System.Net;
using System.Text.Json;

namespace Client.ViewModels
{
    internal class ClientViewModel
    {
        ICommand? checkpalindromeCommand;
        ICommand? checkAllCommand;
        ICommand? setFolderCommand;

        const string ip = "127.0.0.1";
        const int port = 8080;
        IPEndPoint? tcpEndPoint;

        FilesViewModel filesViewModel;
        string folderPath = "";

        public ClientViewModel()
        {
            tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            filesViewModel = new FilesViewModel(this);
        }
        public FilesViewModel FilesVM
        {
            get { return filesViewModel; }
            set { filesViewModel = value; }
        }
        
        public ICommand SetFolder
        {
            get
            {
                if (setFolderCommand == null)
                    setFolderCommand = new _SetFolderCommand(SetFolderFunc);
                return setFolderCommand;
            }
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
        
        public ICommand CheckAll
        {
            get
            {
                if (checkAllCommand == null)
                {
                    checkAllCommand = new _CheckAllCommand(__CheckAll);
                    return checkAllCommand;
                }
                else return checkAllCommand;
            }
        }

        private void ScanFolderForFiles()
        {
            string[] files_paths = Directory.GetFiles(folderPath, searchPattern: "*.txt");
            foreach (string file_path in files_paths)
            {
                filesViewModel.AddFile(file_path);
            }
        }
        private void SetFolderFunc()
        {
            Microsoft.Win32.OpenFileDialog fileDialog = new Microsoft.Win32.OpenFileDialog();
            VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog();
            dialog.Description = "Выберите папку с текстовыми файлами для проверки на полиндром";
            dialog.UseDescriptionForTitle = true;
            if (dialog.ShowDialog() == true)
            {
                folderPath = dialog.SelectedPath;
                filesViewModel.ClearFiles();
                ScanFolderForFiles();
            }
            else folderPath = "";  
        }

        public async Task SendToCheckPalindrome(FileViewModel file)
        {
            file.IsPalindrome = States.SentToCheck; //поменяю статус файла на "Отправлен на проверку"
            file.ButtonEnabled = false; //выключу кнопку
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
                        "Ошибка");
                    file.ButtonEnabled = true; //после неудачи активирую кнопку снова 
                    file.IsPalindrome = States.TryAgain; //после возникновения ошибки статус файла сменю на TryAgain
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
                        MessageBox.Show(ex.Message, "Ошибка");
                        file.ButtonEnabled = true; //после неудачи активирую кнопку снова 
                        file.IsPalindrome = States.TryAgain; //после возникновения ошибки статус файла сменю на TryAgain
                    }
                }
                while (tcpSocket.Available > 0);

                if (response.Length > 0)
                {
                    States respState;
                    try
                    {
                        respState = (States)JsonSerializer.Deserialize(response.ToString(), typeof(States));
                        tcpSocket.Shutdown(SocketShutdown.Both);
                        tcpSocket.Close();
                        file.IsPalindrome = respState;
                    }
                    catch (JsonException ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка");
                    }
                }
            });
            file.ButtonEnabled = true; //после проведения всей работы включу кнопку
        }
        public void __CheckPalindrome(object file)
        {
            _ = SendToCheckPalindrome((FileViewModel)file); 
        }
        public void __CheckAll()
        {
            foreach (FileViewModel f in filesViewModel.Files)
               _ = SendToCheckPalindrome(f);
        }

        internal class _CheckAllCommand : ICommand
        {
            public event EventHandler CanExecuteChanged;
            private Action execute_checkAll;
            //конструктор принимает указатель на метод, передаваемый как параметр, в данном случае это DownloadImage()
            public _CheckAllCommand(Action execute)
            {
                this.execute_checkAll = execute;
            }

            //метод, отвечающий за возможность осуществления команды или нет, для начала константно true
            public bool CanExecute(object parameter)
            {
                return true;
            }
            //при вызове команды запускается метод execute, то есть DownloadImage()
            public void Execute(object parameter)
            {
                execute_checkAll();
            }
        }
        internal class _SetFolderCommand : ICommand
        {
            public event EventHandler CanExecuteChanged;
            private Action execute_setting;
            //конструктор принимает указатель на метод, передаваемый как параметр, в данном случае это DownloadImage()
            public _SetFolderCommand(Action execute)
            {
                this.execute_setting = execute;
            }

            //метод, отвечающий за возможность осуществления команды или нет, для начала константно true
            public bool CanExecute(object parameter)
            {
                return true;
            }
            //при вызове команды запускается метод execute, то есть DownloadImage()
            public void Execute(object parameter)
            {
                execute_setting();
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
}
