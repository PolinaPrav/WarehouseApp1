using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Warehouse.Core.Models;
using Warehouse.Core.Services;

namespace Warehouse.Tests
{
    [TestClass]
    public class WarehouseServiceTests
    {
        [TestMethod]
        public void GetByOrganizationId_ReturnsCorrectWarehouses()
        {
            var list = new List<Warehouse1>
            {
                new Warehouse1 { Id = 1, OrganizationId = 10, Name = "WH1" },
                new Warehouse1 { Id = 2, OrganizationId = 20, Name = "WH2" },
                new Warehouse1 { Id = 3, OrganizationId = 10, Name = "WH3" }
            };
            var service = new WarehouseService(list);
            var result = service.GetByOrganizationId(10);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(w => w.Name == "WH1"));
            Assert.IsTrue(result.Any(w => w.Name == "WH3"));
        }

        [TestMethod]
        public void Add_Warehouse_AssignsId()
        {
            var list = new List<Warehouse1>();
            var service = new WarehouseService(list);
            var wh = new Warehouse1 { Name = "Test", Address = "Addr", OrganizationId = 5 };
            service.Add(wh);
            Assert.AreEqual(1, wh.Id);
            Assert.AreEqual(1, service.GetByOrganizationId(5).Count);
        }

        [TestMethod]
        public void Update_Warehouse_ChangesProperties()
        {
            var list = new List<Warehouse1> { new Warehouse1 { Id = 1, Name = "Old", Address = "OldAddr", OrganizationId = 10 } };
            var service = new WarehouseService(list);
            service.Update(new Warehouse1 { Id = 1, Name = "New", Address = "NewAddr", OrganizationId = 20 });
            var updated = service.GetByOrganizationId(20).First();
            Assert.AreEqual("New", updated.Name);
            Assert.AreEqual("NewAddr", updated.Address);
        }

        [TestMethod]
        public void Delete_Warehouse_Removes()
        {
            var list = new List<Warehouse1> { new Warehouse1 { Id = 1, Name = "WH" } };
            var service = new WarehouseService(list);
            service.Delete(1);
            Assert.AreEqual(0, service.GetByOrganizationId(0).Count);
        }
    }
}