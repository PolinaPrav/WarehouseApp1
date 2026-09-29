using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Warehouse.Core.Models;
using Warehouse.Core.Services;

namespace Warehouse.Tests
{
    [TestClass]
    public class OrganizationServiceTests
    {
        [TestMethod]
        public void Add_Organization_IncreasesCountAndAssignsId()
        {
            var list = new List<Organization>();
            var service = new OrganizationService(list);
            var org = new Organization { Name = "Тест" };
            service.Add(org);
            Assert.AreEqual(1, service.GetAll().Count);
            Assert.AreEqual(1, org.Id);
        }

        [TestMethod]
        public void Update_Organization_ChangesName()
        {
            var list = new List<Organization> { new Organization { Id = 1, Name = "Старое" } };
            var service = new OrganizationService(list);
            var updated = new Organization { Id = 1, Name = "Новое" };
            service.Update(updated);
            Assert.AreEqual("Новое", service.GetAll().First().Name);
        }

        [TestMethod]
        public void Delete_Organization_RemovesIt()
        {
            var list = new List<Organization> { new Organization { Id = 1, Name = "Org" } };
            var service = new OrganizationService(list);
            service.Delete(1);
            Assert.AreEqual(0, service.GetAll().Count);
        }

        [TestMethod]
        public void Delete_NonexistentId_DoesNothing()
        {
            var list = new List<Organization> { new Organization { Id = 1, Name = "Org" } };
            var service = new OrganizationService(list);
            service.Delete(999);
            Assert.AreEqual(1, service.GetAll().Count);
        }
    }
}