using System.Windows;
using Client.ViewModels;


namespace Client
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ClientViewModel clientViewModel = new ClientViewModel();
            listbox_files.DataContext = clientViewModel.FilesVM;
            DataContext = clientViewModel;   
        }
    }
}
