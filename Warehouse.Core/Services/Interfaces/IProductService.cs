using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Core.Models;

namespace Warehouse.Core.Services.Interfaces
{
    public interface IProductService
    {
        List<Product> GetByWarehouseId(int warehouseId);
        void Add(Product product);
        void Update(Product product);
        void Delete(int id);
    }
}
