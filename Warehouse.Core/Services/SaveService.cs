using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Warehouse.Core.Data;
using Warehouse.Core.Models;

namespace Warehouse.Core.Services
{
    public static class SaveService
    {
        public static string DefaultPath =>
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "warehouse.json");

        public static void SaveToJson(string path)
        {
            var data = new WarehouseData
            {
                Organizations = DataStore.Organizations,
                Warehouses = DataStore.Warehouses,
                Products = DataStore.Products,
                Invoices = DataStore.Invoices
            };

            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(data, options);
            File.WriteAllText(path, json);
        }

        public static void LoadFromJson(string path)
        {
            if (!File.Exists(path)) return;

            string json = File.ReadAllText(path);
            var data = JsonSerializer.Deserialize<WarehouseData>(json);
            if (data == null) return;

            if (data.Organizations != null)
                foreach (var org in data.Organizations)
                    if (DataStore.Organizations.All(o => o.Id != org.Id))
                        DataStore.Organizations.Add(org);

            if (data.Warehouses != null)
                foreach (var wh in data.Warehouses)
                    if (DataStore.Warehouses.All(w => w.Id != wh.Id))
                        DataStore.Warehouses.Add(wh);

            if (data.Products != null)
                foreach (var p in data.Products)
                    if (DataStore.Products.All(x => x.Id != p.Id))
                        DataStore.Products.Add(p);

            if (data.Invoices != null)
                foreach (var inv in data.Invoices)
                    if (DataStore.Invoices.All(i => i.Id != inv.Id))
                        DataStore.Invoices.Add(inv);
        }

        private class WarehouseData
        {
            public List<Organization> Organizations { get; set; } = new();
            public List<Warehouse1> Warehouses { get; set; } = new();
            public List<Product> Products { get; set; } = new();
            public List<Invoice> Invoices { get; set; } = new();
        }
    }
}