using System.Windows;
using Warehouse.ViewModels;

namespace Warehouse.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void Close_Click(object sender, RoutedEventArgs e) => Close();
    }
}