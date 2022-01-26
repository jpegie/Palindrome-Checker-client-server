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
using Client.Models;
namespace Client.ViewModels
{
    internal class FilesViewModel : INotifyPropertyChanged
    {
        ObservableCollection<File> files = new ObservableCollection<File>();
        public event PropertyChangedEventHandler? PropertyChanged;
        ClientModel clientM;
        public FilesViewModel(ClientModel clientM)
        {
            this.clientM = clientM;
        }

        public ClientModel ClientM
        {
            get { return clientM; }
            set { clientM = value; }
        }
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
        public ObservableCollection<File> Files 
        { 
            get { return files; } 
            set { files = value; } 
        }
        public void AddFile(string path)
        {
            files.Add(new File(path, clientM));
        }
        public void ClearFiles()
        {
            files.Clear();
        }
    }
}
