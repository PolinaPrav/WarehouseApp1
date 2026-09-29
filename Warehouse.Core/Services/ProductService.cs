using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Core.Models;
using Warehouse.Core.Services.Interfaces;

namespace Warehouse.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly List<Product> _products;
        public ProductService(List<Product> products) => _products = products;

        public List<Product> GetByWarehouseId(int warehouseId) =>
            _products.Where(p => p.WarehouseId == warehouseId).ToList();

        public void Add(Product product)
        {
            product.Id = _products.Count > 0 ? _products.Max(p => p.Id) + 1 : 1;
            _products.Add(product);
        }

        public void Update(Product product)
        {
            var existing = _products.FirstOrDefault(p => p.Id == product.Id);
            if (existing != null)
            {
                existing.Name = product.Name;
                existing.Category = product.Category;
                existing.Manufacturer = product.Manufacturer;
                existing.Supplier = product.Supplier;
                existing.Price = product.Price;
                existing.StockQuantity = product.StockQuantity;
                existing.DiscountPercent = product.DiscountPercent;
                existing.ImagePath = product.ImagePath;
                existing.WarehouseId = product.WarehouseId;
            }
        }

        public void Delete(int id)
        {
            var prod = _products.FirstOrDefault(p => p.Id == id);
            if (prod != null) _products.Remove(prod);
        }
    }
}
