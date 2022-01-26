using System.Collections.ObjectModel;

namespace Client.ViewModels
{
    internal class FilesViewModel
    {
        ObservableCollection<FileViewModel> files = new ObservableCollection<FileViewModel>();
        ClientViewModel clientVM;

        public FilesViewModel(ClientViewModel clientVM)
        {
            this.clientVM = clientVM; 
        }
        public ObservableCollection<FileViewModel> Files 
        { 
            get { return files; } 
        }
        public void AddFile(string path)
        {
            files.Add(new FileViewModel(path, clientVM));
        }
        public void ClearFiles()
        {
            files.Clear();
        }
    }
}
