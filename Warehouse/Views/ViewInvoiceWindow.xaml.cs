using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Warehouse.Core.Data;
using Warehouse.Core.Models;

namespace Warehouse.Views
{
    public partial class ViewInvoiceWindow : Window
    {
        public class InvoiceRow
        {
            public string ProductName { get; set; }
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal Total => Quantity * UnitPrice;
        }

        public ViewInvoiceWindow(Invoice invoice)
        {
            InitializeComponent();
            Owner = Application.Current.MainWindow;

            txtHeader.Text = $"Накладная №{invoice.Id}";
            txtDate.Text = $"Дата: {invoice.Date:g}";
            txtType.Text = invoice.Type == InvoiceType.Incoming
                ? "Тип: ПРИХОДНАЯ"
                : "Тип: РАСХОДНАЯ";

            var rows = new List<InvoiceRow>();
            foreach (var item in invoice.Items)
            {
                var product = DataStore.Products.FirstOrDefault(p => p.Id == item.ProductId);
                rows.Add(new InvoiceRow
                {
                    ProductName = product?.Name ?? $"Товар ID {item.ProductId}",
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                });
            }

            gridItems.ItemsSource = rows;
        }

        private void Close_Click(object sender, RoutedEventArgs e) => Close();
    }
}