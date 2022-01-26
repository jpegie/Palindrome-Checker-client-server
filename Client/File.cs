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

namespace Client
{
    internal class File : INotifyPropertyChanged
    {
        string path = "";

        char[] previewText = new char[128];
        string wholeText = "";

        ResultState textPalindromeState = ResultState.NotChecked;
        ClientModel clientModel;

        public File(string path, ClientModel clientModel)
        {
            this.clientModel = clientModel;
            this.path = path;
            SetViewAndWholeText(); 
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
        public ClientModel ClientM 
        { 
            get { return clientModel; } 
            set { clientModel = value; } 
        }
        private void SetViewAndWholeText()
        {
            using (StreamReader reader = new StreamReader(path))
            {
                WholeText = reader.ReadToEnd();
                previewText = (WholeText.Substring(0, WholeText.Length > 128 ? 128 : WholeText.Length) + "...").ToArray();
            }
        }
        public string PreviewText 
        { 
            get { return new string(previewText); } 
            set { previewText = value.ToArray(); } 
        }
        public string WholeText
        {
            get { return wholeText; }
            set { wholeText = value; }
        }
        public ResultState IsPalindrome 
        { 
            get { return textPalindromeState; } 
            set { textPalindromeState = value; OnPropertyChanged("IsPalindrome"); } 
        }

    }
}
