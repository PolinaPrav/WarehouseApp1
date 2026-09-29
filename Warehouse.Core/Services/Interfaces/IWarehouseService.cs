using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Core.Models;

namespace Warehouse.Core.Services.Interfaces
{
    public interface IWarehouseService
    {
        List<Warehouse1> GetByOrganizationId(int orgId);
        void Add(Warehouse1 warehouse);
        void Update(Warehouse1 warehouse);
        void Delete(int id);
    }
}
