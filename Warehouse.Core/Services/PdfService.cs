using System.Collections.Generic;
using System.IO;
using System.Text;
using Warehouse.Core.Models;

namespace Warehouse.Core.Services
{
    public static class PdfService
    {
        public static void ExportProductsToPdf(string path, List<Product> products)
        {
            var sb = new StringBuilder();
            sb.AppendLine("СПИСОК ТОВАРОВ");
            sb.AppendLine("========================================");
            sb.AppendLine("Наименование | Категория | Цена | Остаток | Скидка %");
            sb.AppendLine("----------------------------------------");

            foreach (var p in products)
            {
                sb.AppendLine($"{p.Name} | {p.Category} | {p.Price:N2} | {p.StockQuantity} | {p.DiscountPercent}");
            }

            WriteSimplePdf(path, sb.ToString());
        }

        public static void ExportInvoicesToPdf(string path, List<Invoice> invoices, List<Product> products)
        {
            var sb = new StringBuilder();
            sb.AppendLine("СПИСОК НАКЛАДНЫХ");
            sb.AppendLine("========================================");

            foreach (var inv in invoices)
            {
                sb.AppendLine($"Накладная №{inv.Id} | {inv.Type} | {inv.Date:g}");
                foreach (var item in inv.Items)
                {
                    var prod = products.Find(p => p.Id == item.ProductId);
                    string name = prod != null ? prod.Name : $"Товар ID {item.ProductId}";
                    sb.AppendLine($"    {name} | Кол-во: {item.Quantity} | Цена: {item.UnitPrice:N2}");
                }
                sb.AppendLine("----------------------------------------");
            }

            WriteSimplePdf(path, sb.ToString());
        }

        private static void WriteSimplePdf(string path, string text)
        {
            var sb = new StringBuilder();

            sb.Append("%PDF-1.4\n");

            int obj1 = sb.Length;
            sb.Append("1 0 obj\n<< /Type /Catalog /Pages 2 0 R >>\nendobj\n");

            int obj2 = sb.Length;
            sb.Append("2 0 obj\n<< /Type /Pages /Kids [3 0 R] /Count 1 >>\nendobj\n");

            int obj3 = sb.Length;
            sb.Append("3 0 obj\n<< /Type /Page /Parent 2 0 R /MediaBox [0 0 595 842] /Contents 4 0 R /Resources << /Font << /F1 5 0 R >> >> >>\nendobj\n");

            string escaped = text
                .Replace("\\", "\\\\")
                .Replace("(", "\\(")
                .Replace(")", "\\)");

            var lines = escaped.Split('\n');
            var content = new StringBuilder();
            content.Append("BT\n/F1 10 Tf\n12 TL\n40 800 Td\n");
            foreach (var line in lines)
                content.Append($"({line}) Tj\nT*\n");
            content.Append("ET\n");

            string contentStr = content.ToString();

            int obj4 = sb.Length;
            sb.Append("4 0 obj\n<< /Length " + contentStr.Length + " >>\nstream\n");
            sb.Append(contentStr);
            sb.Append("endstream\nendobj\n");

            int obj5 = sb.Length;
            sb.Append("5 0 obj\n<< /Type /Font /Subtype /Type1 /BaseFont /Courier >>\nendobj\n");

            int xref = sb.Length;
            sb.Append("xref\n0 6\n");
            sb.Append("0000000000 65535 f \n");
            sb.Append(obj1.ToString("D10") + " 00000 n \n");
            sb.Append(obj2.ToString("D10") + " 00000 n \n");
            sb.Append(obj3.ToString("D10") + " 00000 n \n");
            sb.Append(obj4.ToString("D10") + " 00000 n \n");
            sb.Append(obj5.ToString("D10") + " 00000 n \n");

            sb.Append("trailer\n<< /Size 6 /Root 1 0 R >>\nstartxref\n");
            sb.Append(xref + "\n");
            sb.Append("%%EOF");

            File.WriteAllText(path, sb.ToString(), Encoding.ASCII);
        }
    }
}