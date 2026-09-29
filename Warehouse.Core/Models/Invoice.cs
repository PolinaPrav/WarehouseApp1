using System;
using System.Collections.Generic;

namespace Warehouse.Core.Models
{
    public enum InvoiceType { Incoming, Outgoing }

    public class Invoice
    {
        public int Id { get; set; }
        public int WarehouseId { get; set; }
        public InvoiceType Type { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public List<InvoiceItem> Items { get; set; } = new();
    }

    public class InvoiceItem
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}