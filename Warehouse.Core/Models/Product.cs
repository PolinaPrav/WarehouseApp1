using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Core.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Category { get; set; } = "";
        public string Manufacturer { get; set; } = "";
        public string Supplier { get; set; } = "";
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int DiscountPercent { get; set; }
        public string ImagePath { get; set; } = "";
        public int WarehouseId { get; set; }
    }
}
