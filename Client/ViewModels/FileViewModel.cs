using System.ComponentModel;
using System.IO;
using System.Linq;
using Client.ViewModels;

namespace Client
{
    internal class FileViewModel : INotifyPropertyChanged
    {
        ResultState textPalindromeState = ResultState.NotChecked;
        ClientViewModel clientVM;

        string path = "";
        char[] previewText = new char[128];
        string wholeText = "";

        bool buttonEnabled = true;

        public FileViewModel(string path, ClientViewModel clientVM)
        {
            this.path = path;
            this.clientVM = clientVM;
            SetViewAndWholeText(); 
        }
        private void SetViewAndWholeText()
        {
            using (StreamReader reader = new StreamReader(path))
            {
                WholeText = reader.ReadToEnd();
                previewText = (WholeText.Substring(0, WholeText.Length > 128 ? 128 : WholeText.Length) + "...").ToArray();
            }
        }

        #region PROPERTIES
        public ClientViewModel ClientVM
        {
            get { return clientVM; }
        }
        public ResultState IsPalindrome
        {
            get { return textPalindromeState; }
            set { textPalindromeState = value; OnPropertyChanged("IsPalindrome"); }
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
        public bool ButtonEnabled
        {
            get { return buttonEnabled; }
            set { buttonEnabled = value; OnPropertyChanged("ButtonEnabled"); }
        }
        #endregion
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
        #endregion
    }
}
