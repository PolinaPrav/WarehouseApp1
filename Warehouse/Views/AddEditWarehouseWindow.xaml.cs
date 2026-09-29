using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Warehouse.Core.Models;
using Warehouse.Core.Services.Interfaces;

namespace Warehouse.Views
{
    public partial class AddEditWarehouseWindow : Window
    {
        private readonly Warehouse1 _editWarehouse;
        private readonly IWarehouseService _service;
        private readonly int _organizationId;

        public AddEditWarehouseWindow(Warehouse1 warehouse, IWarehouseService service, int orgId)
        {
            InitializeComponent();
            _service = service;
            _organizationId = orgId;
            _editWarehouse = warehouse;
            Owner = Application.Current.MainWindow;

            if (warehouse != null)
            {
                txtName.Text = warehouse.Name;
                txtAddress.Text = warehouse.Address;
                Title = "Редактирование склада";
            }
            else
            {
                Title = "Новый склад";
            }
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите наименование склада", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_editWarehouse == null)
            {
                var newWarehouse = new Warehouse1
                {
                    Name = txtName.Text.Trim(),
                    Address = txtAddress.Text.Trim(),
                    OrganizationId = _organizationId
                };
                _service.Add(newWarehouse);
            }
            else
            {
                _editWarehouse.Name = txtName.Text.Trim();
                _editWarehouse.Address = txtAddress.Text.Trim();
                _editWarehouse.OrganizationId = _organizationId;
                _service.Update(_editWarehouse);
            }
            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}