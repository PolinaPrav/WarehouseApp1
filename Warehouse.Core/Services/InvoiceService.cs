using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Core.Models;
using Warehouse.Core.Services.Interfaces;

namespace Warehouse.Core.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly List<Invoice> _invoices;
        private readonly List<Product> _products;

        public InvoiceService(List<Invoice> invoices, List<Product> products)
        {
            _invoices = invoices;
            _products = products;
        }

        public void CreateInvoice(Invoice invoice)
        {
            invoice.Id = _invoices.Count > 0 ? _invoices.Max(i => i.Id) + 1 : 1;
            _invoices.Add(invoice);

            // Изменяем остатки товаров
            foreach (var item in invoice.Items)
            {
                var product = _products.FirstOrDefault(p => p.Id == item.ProductId);
                if (product != null)
                {
                    if (invoice.Type == InvoiceType.Incoming)
                        product.StockQuantity += item.Quantity;
                    else if (invoice.Type == InvoiceType.Outgoing)
                    {
                        if (product.StockQuantity >= item.Quantity)
                            product.StockQuantity -= item.Quantity;
                        else
                            throw new Exception($"Недостаточно товара {product.Name} на складе");
                    }
                }
            }
        }

        public List<Invoice> GetInvoicesByWarehouse(int warehouseId) =>
            _invoices.Where(i => i.WarehouseId == warehouseId).ToList();

        public void Delete(int id)
        {
            var inv = _invoices.FirstOrDefault(i => i.Id == id);
            if (inv != null) _invoices.Remove(inv);
        }
    }
}
