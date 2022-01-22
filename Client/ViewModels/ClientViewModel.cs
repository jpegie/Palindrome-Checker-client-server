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

namespace Client.ViewModels
{
    internal class ClientViewModel
    {
        FilesViewModel filesVM;
        string folder_path = "";
        ICommand SetFolderCommand;

        public ClientViewModel(FilesViewModel filesVM)
        {
            this.filesVM = filesVM;
        }
        public string FolderPath { set { folder_path = value; } }
        private void ScanFolderForFiles()
        {
            string[] files_paths = Directory.GetFiles(folder_path, searchPattern: "*.txt");
            foreach (string file_path in files_paths)
            {
                filesVM.AddFile(file_path);
            }
        }

        public ICommand SetFolder
        {
            get
            {
                if (SetFolderCommand == null)
                    SetFolderCommand = new __SetFolderCommand(SetFolderFunc);
                return SetFolderCommand;
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
                folder_path = dialog.SelectedPath;
                filesVM.ClearFiles();
                ScanFolderForFiles();
            }
            else folder_path = "";  
        }

        internal class __SetFolderCommand : ICommand
        {
            public event EventHandler CanExecuteChanged;
            private Action execute_setting;
            //конструктор принимает указатель на метод, передаваемый как параметр, в данном случае это DownloadImage()
            public __SetFolderCommand(Action execute)
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
    }
}
