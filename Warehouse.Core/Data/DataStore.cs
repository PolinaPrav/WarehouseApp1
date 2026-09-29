using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Core.Models;
using Warehouse.Core.Services.Interfaces;
using Warehouse.Core.Services;

namespace Warehouse.Core.Data
{
    public static class DataStore
    {
        public static List<Organization> Organizations { get; private set; } = new();
        public static List<Warehouse1> Warehouses { get; private set; } = new();
        public static List<Product> Products { get; private set; } = new();
        public static List<Invoice> Invoices { get; private set; } = new();

        public static IOrganizationService OrganizationService { get; private set; }
        public static IWarehouseService WarehouseService { get; private set; }
        public static IProductService ProductService { get; private set; }
        public static IInvoiceService InvoiceService { get; private set; }

        static DataStore()
        {
            var org1 = new Organization { Id = 1, Name = "Рога и копыта, ООО" };
            var org2 = new Organization { Id = 2, Name = "Пупкин и сыновья, ООО" };
            Organizations.AddRange(new[] { org1, org2 });

            var wh1 = new Warehouse1 { Id = 1, Name = "Склад Рога и копыта №1", Address = "ул. Ленина 1", OrganizationId = 1 };
            var wh2 = new Warehouse1 { Id = 2, Name = "Склад Рога и копыта №2", Address = "ул. Мира 10", OrganizationId = 1 };
            var wh3 = new Warehouse1 { Id = 3, Name = "Склад Пупкина №1", Address = "ул. Садовая 5", OrganizationId = 2 };
            Warehouses.AddRange(new[] { wh1, wh2, wh3 });

            Products.AddRange(new[]
            {
                new Product { Id = 1, Name = "Гвоздь 100мм", Category = "Крепёж", Manufacturer = "ООО Метиз", Supplier = "ТД Стройка", Price = 5.50m, StockQuantity = 150, DiscountPercent = 0, ImagePath = "", WarehouseId = 1 },
                new Product { Id = 2, Name = "Молоток", Category = "Инструмент", Manufacturer = "Инструмент-Мастер", Supplier = "ТД Стройка", Price = 450m, StockQuantity = 12, DiscountPercent = 5, ImagePath = "", WarehouseId = 1 },
                new Product { Id = 3, Name = "Шуруповёрт", Category = "Электроинструмент", Manufacturer = "Bosch", Supplier = "ТД Стройка", Price = 5900m, StockQuantity = 7, DiscountPercent = 10, ImagePath = "", WarehouseId = 2 },
                new Product { Id = 4, Name = "Саморез 4х50", Category = "Крепёж", Manufacturer = "ООО Метиз", Supplier = "ТД Стройка", Price = 2.20m, StockQuantity = 500, DiscountPercent = 0, ImagePath = "", WarehouseId = 3 }
            });

            OrganizationService = new OrganizationService(Organizations);
            WarehouseService = new WarehouseService(Warehouses);
            ProductService = new ProductService(Products);
            InvoiceService = new InvoiceService(Invoices, Products);
        }
    }
}
