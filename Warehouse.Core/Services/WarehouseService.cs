using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Core.Models;
using Warehouse.Core.Services.Interfaces;


namespace Warehouse.Core.Services
{
    public class WarehouseService : IWarehouseService
    {
        private readonly List<Warehouse1> _warehouses;
        public WarehouseService(List<Warehouse1> warehouses) => _warehouses = warehouses;

        public List<Warehouse1> GetByOrganizationId(int orgId) =>
            _warehouses.Where(w => w.OrganizationId == orgId).ToList();

        public void Add(Warehouse1 warehouse)
        {
            warehouse.Id = _warehouses.Count > 0 ? _warehouses.Max(w => w.Id) + 1 : 1;
            _warehouses.Add(warehouse);
        }

        public void Update(Warehouse1 warehouse)
        {
            var existing = _warehouses.FirstOrDefault(w => w.Id == warehouse.Id);
            if (existing != null)
            {
                existing.Name = warehouse.Name;
                existing.Address = warehouse.Address;
                existing.OrganizationId = warehouse.OrganizationId;
            }
        }

        public void Delete(int id)
        {
            var wh = _warehouses.FirstOrDefault(w => w.Id == id);
            if (wh != null) _warehouses.Remove(wh);
        }
    }
}
