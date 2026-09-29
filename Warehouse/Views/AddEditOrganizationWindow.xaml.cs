using System.Windows;
using Warehouse.Core.Models;
using Warehouse.Core.Services.Interfaces;

namespace Warehouse.Views
{
    public partial class AddEditOrganizationWindow : Window
    {
        private Organization _editOrg;
        private readonly IOrganizationService _service;
        public Organization ResultOrg { get; private set; }

        public AddEditOrganizationWindow(Organization org, IOrganizationService service)
        {
            InitializeComponent();
            _service = service;
            _editOrg = org;
            if (org != null)
                txtName.Text = org.Name;
            Owner = Application.Current.MainWindow;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите название организации", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (_editOrg == null)
            {
                var newOrg = new Organization { Name = txtName.Text.Trim() };
                _service.Add(newOrg);
                ResultOrg = newOrg;
            }
            else
            {
                _editOrg.Name = txtName.Text.Trim();
                _service.Update(_editOrg);
                ResultOrg = _editOrg;
            }
            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}