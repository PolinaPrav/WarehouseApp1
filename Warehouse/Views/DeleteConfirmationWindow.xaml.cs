using System.Windows;

namespace Warehouse.Views
{
    public partial class DeleteConfirmationWindow : Window
    {
        private readonly string _orgName;
        public DeleteConfirmationWindow(string orgName)
        {
            InitializeComponent();
            _orgName = orgName;
            Owner = Application.Current.MainWindow;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (txtConfirm.Text.Trim() == _orgName)
                DialogResult = true;
            else
                MessageBox.Show("Название не совпадает", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}