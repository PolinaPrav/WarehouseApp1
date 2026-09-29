using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Warehouse.Core.Models;
using Warehouse.Core.Services.Interfaces;

namespace Warehouse.Views
{
    public partial class CreateInvoiceWindow : Window
    {
        private readonly int _warehouseId;
        private readonly InvoiceType _type;
        private readonly List<Product> _products;
        private readonly IInvoiceService _invoiceService;

        public CreateInvoiceWindow(int warehouseId, InvoiceType type, List<Product> products, IInvoiceService invoiceService)
        {
            InitializeComponent();
            _warehouseId = warehouseId;
            _type = type;
            _products = products;
            _invoiceService = invoiceService;
            Owner = Application.Current.MainWindow;

            txtType.Text = type == InvoiceType.Incoming ? "ПРИХОДНАЯ" : "РАСХОДНАЯ";
            cmbProducts.ItemsSource = products;
            if (products.Count > 0)
                cmbProducts.SelectedIndex = 0;
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            if (cmbProducts.SelectedItem == null)
            {
                MessageBox.Show("Выберите товар", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Количество должно быть целым положительным числом", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(txtUnitPrice.Text, out decimal unitPrice) || unitPrice < 0)
            {
                MessageBox.Show("Цена должна быть неотрицательным числом", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectedProduct = (Product)cmbProducts.SelectedItem;
            var invoice = new Invoice
            {
                WarehouseId = _warehouseId,
                Type = _type,
                Date = DateTime.Now,
                Items = new List<InvoiceItem>
                {
                    new InvoiceItem
                    {
                        ProductId = selectedProduct.Id,
                        Quantity = quantity,
                        UnitPrice = unitPrice
                    }
                }
            };

            try
            {
                _invoiceService.CreateInvoice(invoice);
                MessageBox.Show($"Накладная создана. Остаток товара обновлён.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}