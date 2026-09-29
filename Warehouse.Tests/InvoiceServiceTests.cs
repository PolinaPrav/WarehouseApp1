using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using Warehouse.Core.Models;
using Warehouse.Core.Services;

namespace Warehouse.Tests
{
    [TestClass]
    public class InvoiceServiceTests
    {
        [TestMethod]
        public void CreateIncomingInvoice_IncreasesStock()
        {
            var products = new List<Product> { new Product { Id = 1, Name = "Test", StockQuantity = 10, WarehouseId = 1 } };
            var invoices = new List<Invoice>();
            var service = new InvoiceService(invoices, products);
            var invoice = new Invoice
            {
                WarehouseId = 1,
                Type = InvoiceType.Incoming,
                Items = new List<InvoiceItem> { new InvoiceItem { ProductId = 1, Quantity = 5, UnitPrice = 100 } }
            };
            service.CreateInvoice(invoice);
            Assert.AreEqual(15, products.First(p => p.Id == 1).StockQuantity);
        }

        [TestMethod]
        public void CreateOutgoingInvoice_DecreasesStock()
        {
            var products = new List<Product> { new Product { Id = 1, Name = "Test", StockQuantity = 10, WarehouseId = 1 } };
            var invoices = new List<Invoice>();
            var service = new InvoiceService(invoices, products);
            var invoice = new Invoice
            {
                WarehouseId = 1,
                Type = InvoiceType.Outgoing,
                Items = new List<InvoiceItem> { new InvoiceItem { ProductId = 1, Quantity = 3, UnitPrice = 100 } }
            };
            service.CreateInvoice(invoice);
            Assert.AreEqual(7, products.First(p => p.Id == 1).StockQuantity);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Недостаточно товара")]
        public void CreateOutgoingInvoice_WithInsufficientStock_ThrowsException()
        {
            var products = new List<Product> { new Product { Id = 1, Name = "Test", StockQuantity = 2, WarehouseId = 1 } };
            var invoices = new List<Invoice>();
            var service = new InvoiceService(invoices, products);
            var invoice = new Invoice
            {
                WarehouseId = 1,
                Type = InvoiceType.Outgoing,
                Items = new List<InvoiceItem> { new InvoiceItem { ProductId = 1, Quantity = 5, UnitPrice = 100 } }
            };
            service.CreateInvoice(invoice);
        }

        [TestMethod]
        public void GetInvoicesByWarehouse_ReturnsCorrect()
        {
            var invoices = new List<Invoice>
            {
                new Invoice { Id = 1, WarehouseId = 10 },
                new Invoice { Id = 2, WarehouseId = 20 },
                new Invoice { Id = 3, WarehouseId = 10 }
            };
            var service = new InvoiceService(invoices, new List<Product>());
            var result = service.GetInvoicesByWarehouse(10);
            Assert.AreEqual(2, result.Count);
        }
    }
}