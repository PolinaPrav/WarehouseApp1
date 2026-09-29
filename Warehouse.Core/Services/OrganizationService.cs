using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Core.Models;
using Warehouse.Core.Services.Interfaces;

namespace Warehouse.Core.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly List<Organization> _organizations;
        public OrganizationService(List<Organization> organizations) => _organizations = organizations;

        public List<Organization> GetAll() => _organizations.ToList();

        public void Add(Organization org)
        {
            org.Id = _organizations.Count > 0 ? _organizations.Max(o => o.Id) + 1 : 1;
            _organizations.Add(org);
        }

        public void Update(Organization org)
        {
            var existing = _organizations.FirstOrDefault(o => o.Id == org.Id);
            if (existing != null) existing.Name = org.Name;
        }

        public void Delete(int id)
        {
            var org = _organizations.FirstOrDefault(o => o.Id == id);
            if (org != null) _organizations.Remove(org);
        }
    }
}
