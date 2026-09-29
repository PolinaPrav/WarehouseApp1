using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using Warehouse.Core.Data;
using Warehouse.Core.Models;
using Warehouse.Core.Services;
using Warehouse.ViewModels;

namespace Warehouse.Views
{
    public partial class WarehouseManageWindow : Window
    {
        public WarehouseManageWindow(Organization org)
        {
            InitializeComponent();
            DataContext = new WarehouseManageViewModel(org);
        }

        private void Close_Click(object sender, RoutedEventArgs e) => Close();

        private void ImportProducts_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Импорт товаров (заглушка). Формат CSV/Excel.",
                "Импорт", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Save_Click(object sender, RoutedEventArgs e) => SaveJson_Click(sender, e);

        private void SaveJson_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog
            {
                Filter = "JSON файлы (*.json)|*.json",
                FileName = "warehouse.json"
            };
            if (dlg.ShowDialog() == true)
            {
                SaveService.SaveToJson(dlg.FileName);
                MessageBox.Show("Данные сохранены в JSON.", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void LoadJson_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Filter = "JSON файлы (*.json)|*.json"
            };

            if (dlg.ShowDialog() == true)
            {
                try
                {
                    SaveService.LoadFromJson(dlg.FileName);

                    if (DataContext is WarehouseManageViewModel vm)
                    {
                        var org = vm.CurrentOrganization;
                        DataContext = new WarehouseManageViewModel(org);
                    }

                    MessageBox.Show("Данные загружены.", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка загрузки",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ExportProductsPdf_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog
            {
                Filter = "PDF файлы (*.pdf)|*.pdf",
                FileName = "products.pdf"
            };
            if (dlg.ShowDialog() == true)
            {
                PdfService.ExportProductsToPdf(dlg.FileName, DataStore.Products);
                MessageBox.Show("PDF сохранён.", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ExportInvoicesPdf_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog
            {
                Filter = "PDF файлы (*.pdf)|*.pdf",
                FileName = "invoices.pdf"
            };
            if (dlg.ShowDialog() == true)
            {
                PdfService.ExportInvoicesToPdf(dlg.FileName, DataStore.Invoices, DataStore.Products);
                MessageBox.Show("PDF сохранён.", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ClearAll_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Удалить все данные? Действие необратимо.",
                    "Подтверждение", MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                DataStore.Organizations.Clear();
                DataStore.Warehouses.Clear();
                DataStore.Products.Clear();
                DataStore.Invoices.Clear();
                MessageBox.Show("Данные очищены. Перезапустите приложение.",
                    "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void InvoicesGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is WarehouseManageViewModel vm && vm.SelectedInvoice != null)
                vm.ViewInvoiceCommand.Execute(null);
        }
    }
}