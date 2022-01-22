using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Client.ViewModels;
using Client.Models;

namespace Client
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ClientModel clientModel = new ClientModel();
            FilesViewModel filesViewModel = new FilesViewModel();
            //TODO: связать filesVM с ClientModel и передавать ссылку на ClientModel в viewmodel юзер контрола
            //чтобы изменить DataContext кнопки "проверить" на команду модели
            ClientViewModel clientViewModel = new ClientViewModel(filesViewModel);
            listbox_files.DataContext = filesViewModel;
            DataContext = clientViewModel;   
        }

        private void btn_sendAll_Click(object sender, RoutedEventArgs e)
        {

        }

        private void add_file_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_setFolder_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
