using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Warehouse.Core.Models;
using Warehouse.Core.Services;

namespace Warehouse.Tests
{
    [TestClass]
    public class ProductServiceTests
    {
        [TestMethod]
        public void GetByWarehouseId_ReturnsFiltered()
        {
            var list = new List<Product>
            {
                new Product { Id = 1, WarehouseId = 100, Name = "P1" },
                new Product { Id = 2, WarehouseId = 200, Name = "P2" },
                new Product { Id = 3, WarehouseId = 100, Name = "P3" }
            };
            var service = new ProductService(list);
            var result = service.GetByWarehouseId(100);
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void Add_Product_AssignsId()
        {
            var list = new List<Product>();
            var service = new ProductService(list);
            var p = new Product { Name = "Test" };
            service.Add(p);
            Assert.AreEqual(1, p.Id);
            Assert.AreEqual(1, service.GetByWarehouseId(0).Count);
        }

        [TestMethod]
        public void Update_Product_ChangesFields()
        {
            var list = new List<Product> { new Product { Id = 1, Name = "Old", Category = "OldCat", Price = 10, StockQuantity = 5 } };
            var service = new ProductService(list);
            var updated = new Product { Id = 1, Name = "New", Category = "NewCat", Manufacturer = "M", Supplier = "S", Price = 20, StockQuantity = 10, DiscountPercent = 10, ImagePath = "img", WarehouseId = 99 };
            service.Update(updated);
            var prod = service.GetByWarehouseId(99).First();
            Assert.AreEqual("New", prod.Name);
            Assert.AreEqual(20, prod.Price);
            Assert.AreEqual(10, prod.StockQuantity);
        }

        [TestMethod]
        public void Delete_Product_Removes()
        {
            var list = new List<Product> { new Product { Id = 1, Name = "Test" } };
            var service = new ProductService(list);
            service.Delete(1);
            Assert.AreEqual(0, service.GetByWarehouseId(0).Count);
        }
    }
}