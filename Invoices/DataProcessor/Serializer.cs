namespace Invoices.DataProcessor;

using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

using Boardgames.Utilities;
using Data;
using ExportDto;

public class Serializer
{
    private static XmlHelper xmlHelper;

    public static string ExportClientsWithTheirInvoices(InvoicesContext context, DateTime date)
    {
        xmlHelper = new XmlHelper();

        ExportClientDto[] clients = context.Clients.AsEnumerable().Where(c => c.Invoices.Any(i => i.IssueDate > date)).Select(c =>
                new ExportClientDto()
                {
                    ClientName = c.Name,
                    InvoicesCount = c.Invoices.Count,
                    VatNumber = c.NumberVat,
                    Invoices = c.Invoices.Select(i => new ExportInvoiceDto()
                    {
                        InvoiceNumber = i.Number,
                        InvoiceAmount = i.Amount,
                        IssueDate = i.IssueDate.ToString("d", CultureInfo.InvariantCulture),
                        DueDate = i.DueDate.ToString("d", CultureInfo.InvariantCulture),
                        Currency = i.CurrencyType.ToString()
                    })
                        .OrderBy(i => DateTime.Parse(i.IssueDate))
                        .ThenByDescending(i => DateTime.Parse(i.DueDate))
                        .ToArray()
                })
            .OrderByDescending(c => c.InvoicesCount)
            .ThenBy(c => c.ClientName)
            .ToArray();

        return xmlHelper.Serialize(clients, "Clients");
    }

    public static string ExportProductsWithMostClients(InvoicesContext context, int nameLength)
    {
        var products = context.Products.AsNoTracking().Where(p => p.ProductsClients.Any(pc => pc.Client.Name.Length >= nameLength))
            .Select(p => new
            {
                Name = p.Name,
                Price = decimal.Parse(p.Price.ToString("0.####")),
                Category = p.CategoryType.ToString(),
                Clients = p.ProductsClients.Where(pc => pc.Client.Name.Length >= nameLength)
                    .Select(pc => new
                    {
                        Name = pc.Client.Name,
                        NumberVat = pc.Client.NumberVat
                    })
                    .OrderBy(c => c.Name)
                    .ToArray()
            })
            .OrderByDescending(p => p.Clients.Length)
            .ThenBy(p => p.Name)
            .Take(5)
            .ToArray();

        return JsonConvert.SerializeObject(products, Formatting.Indented);
    }
}