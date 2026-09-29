using System;
using System.Windows;
using Microsoft.Win32;
using Warehouse.Core.Models;
using Warehouse.Core.Services.Interfaces;

namespace Warehouse.Views
{
    public partial class AddEditProductWindow : Window
    {
        private readonly Product _editProduct;
        private readonly IProductService _service;
        private readonly int _warehouseId;

        public AddEditProductWindow(Product product, IProductService service, int warehouseId)
        {
            InitializeComponent();
            _service = service;
            _warehouseId = warehouseId;
            _editProduct = product;
            Owner = Application.Current.MainWindow;

            if (product != null)
            {
                LoadProductData(product);
                Title = "Редактирование товара";
            }
            else
            {
                Title = "Новый товар";
            }
        }

        private void LoadProductData(Product p)
        {
            txtName.Text = p.Name;
            txtCategory.Text = p.Category;
            txtManufacturer.Text = p.Manufacturer;
            txtSupplier.Text = p.Supplier;
            txtPrice.Text = p.Price.ToString();
            txtStock.Text = p.StockQuantity.ToString();
            txtDiscount.Text = p.DiscountPercent.ToString();
            txtImagePath.Text = p.ImagePath;
        }

        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Изображения|*.jpg;*.jpeg;*.png;*.bmp|Все файлы|*.*";
            if (dialog.ShowDialog() == true)
            {
                txtImagePath.Text = dialog.FileName;
            }
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            // Простая валидация
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите наименование товара", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!decimal.TryParse(txtPrice.Text, out decimal price) || price < 0)
            {
                MessageBox.Show("Цена должна быть положительным числом", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!int.TryParse(txtStock.Text, out int stock) || stock < 0)
            {
                MessageBox.Show("Остаток должен быть целым неотрицательным числом", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!int.TryParse(txtDiscount.Text, out int discount) || discount < 0 || discount > 100)
            {
                MessageBox.Show("Скидка должна быть целым числом от 0 до 100", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_editProduct == null)
            {
                var newProduct = new Product
                {
                    Name = txtName.Text.Trim(),
                    Category = txtCategory.Text.Trim(),
                    Manufacturer = txtManufacturer.Text.Trim(),
                    Supplier = txtSupplier.Text.Trim(),
                    Price = price,
                    StockQuantity = stock,
                    DiscountPercent = discount,
                    ImagePath = txtImagePath.Text,
                    WarehouseId = _warehouseId
                };
                _service.Add(newProduct);
            }
            else
            {
                _editProduct.Name = txtName.Text.Trim();
                _editProduct.Category = txtCategory.Text.Trim();
                _editProduct.Manufacturer = txtManufacturer.Text.Trim();
                _editProduct.Supplier = txtSupplier.Text.Trim();
                _editProduct.Price = price;
                _editProduct.StockQuantity = stock;
                _editProduct.DiscountPercent = discount;
                _editProduct.ImagePath = txtImagePath.Text;
                _editProduct.WarehouseId = _warehouseId;
                _service.Update(_editProduct);
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
