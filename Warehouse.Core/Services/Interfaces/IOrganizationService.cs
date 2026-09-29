using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Core.Models;

namespace Warehouse.Core.Services.Interfaces
{
    public interface IOrganizationService
    {
        List<Organization> GetAll();
        void Add(Organization org);
        void Update(Organization org);
        void Delete(int id);
    }
}
