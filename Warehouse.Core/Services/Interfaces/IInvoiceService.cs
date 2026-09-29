using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Core.Models;


namespace Warehouse.Core.Services.Interfaces
{
    public interface IInvoiceService
    {
        void CreateInvoice(Invoice invoice);
        List<Invoice> GetInvoicesByWarehouse(int warehouseId);
        void Delete(int id);
    }
}
