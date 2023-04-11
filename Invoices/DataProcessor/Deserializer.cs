namespace Invoices.DataProcessor;

using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using Newtonsoft.Json;

using Boardgames.Utilities;
using Data.Models;
using ImportDto;

using Data;

public class Deserializer
{
    private const string ErrorMessage = "Invalid data!";

    private const string SuccessfullyImportedClients
        = "Successfully imported client {0}.";

    private const string SuccessfullyImportedInvoices
        = "Successfully imported invoice with number {0}.";

    private const string SuccessfullyImportedProducts
        = "Successfully imported product - {0} with {1} clients.";

    private static XmlHelper xmlHelper;

    public static string ImportClients(InvoicesContext context, string xmlString)
    {
        StringBuilder sb = new StringBuilder();
        xmlHelper = new XmlHelper();

        ImportClientDto[] clientDtos = xmlHelper.Deserialize<ImportClientDto[]>(xmlString, "Clients");

        ICollection<Client> validClients = new HashSet<Client>();

        foreach (var clientDto in clientDtos)
        {
            if (!IsValid(clientDto))
            {
                sb.AppendLine(ErrorMessage);
                continue;
            }

            ICollection<Address> validAddresses = new HashSet<Address>();

            foreach (var clientDtoAddress in clientDto.Addresses)
            {
                if (!IsValid(clientDtoAddress))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Address address = new Address()
                {
                    StreetName = clientDtoAddress.StreetName,
                    StreetNumber = clientDtoAddress.StreetNumber,
                    PostCode = clientDtoAddress.PostCode,
                    City = clientDtoAddress.City,
                    Country = clientDtoAddress.Country
                };

                validAddresses.Add(address);
            }

            Client client = new Client()
            {
                Name = clientDto.Name,
                NumberVat = clientDto.NumberVat,
                Addresses = validAddresses
            };

            validClients.Add(client);
            sb.AppendLine(string.Format(SuccessfullyImportedClients, client.Name));
        }

        context.AddRange(validClients);
        context.SaveChanges();

        return sb.ToString().TrimEnd();
    }


    public static string ImportInvoices(InvoicesContext context, string jsonString)
    {
        StringBuilder sb = new StringBuilder();

        ImportInvoiceDto[] invoiceDtos = JsonConvert.DeserializeObject<ImportInvoiceDto[]>(jsonString);

        ICollection<Invoice> validInvoices = new HashSet<Invoice>();
        ICollection<int> existingClientIds = context.Clients.Select(c => c.Id).ToArray();

        foreach (var invoiceDto in invoiceDtos)
        {
            if (!IsValid(invoiceDto) || DateTime.Parse(invoiceDto.DueDate, CultureInfo.InvariantCulture) < DateTime.Parse(invoiceDto.IssueDate, CultureInfo.InvariantCulture))
            {
                sb.AppendLine(ErrorMessage);
                continue;
            }

            if (!existingClientIds.Contains(invoiceDto.ClientId))
            {
                sb.AppendLine(ErrorMessage);
                continue;
            }

            Invoice invoice = new Invoice()
            {
                Number = invoiceDto.Number,
                IssueDate = DateTime.Parse(invoiceDto.IssueDate, CultureInfo.InvariantCulture),
                DueDate = DateTime.Parse(invoiceDto.DueDate, CultureInfo.InvariantCulture),
                Amount = invoiceDto.Amount,
                CurrencyType = invoiceDto.CurrencyType,
                ClientId = invoiceDto.ClientId
            };

            validInvoices.Add(invoice);
            sb.AppendLine(string.Format(SuccessfullyImportedInvoices, invoice.Number));
        }

        context.Invoices.AddRange(validInvoices);
        context.SaveChanges();

        return sb.ToString().TrimEnd();
    }

    public static string ImportProducts(InvoicesContext context, string jsonString)
    {
        StringBuilder sb = new StringBuilder();

        ImportProductDto[] productDtos = JsonConvert.DeserializeObject<ImportProductDto[]>(jsonString);

        ICollection<Product> validProducts = new HashSet<Product>();
        ICollection<int> existingClientIds = context.Clients.Select(c => c.Id).ToArray();

        foreach (var productDto in productDtos)
        {
            if (!IsValid(productDto))
            {
                sb.AppendLine(ErrorMessage);
                continue;
            }

            Product product = new Product()
            {
                Name = productDto.Name,
                Price = productDto.Price,
                CategoryType = productDto.CategoryType
            };

            foreach (var clientId in productDto.ClientIds.Distinct())
            {
                if (!existingClientIds.Contains(clientId))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                ProductClient pc = new ProductClient()
                {
                    Product = product,
                    ClientId = clientId
                };

                product.ProductsClients.Add(pc);
            }

            validProducts.Add(product);
            sb.AppendLine(string.Format(SuccessfullyImportedProducts, product.Name,
                product.ProductsClients.Count));
        }

        context.Products.AddRange(validProducts);
        context.SaveChanges();

        return sb.ToString().TrimEnd();
    }

    public static bool IsValid(object dto)
    {
        var validationContext = new ValidationContext(dto);
        var validationResult = new List<ValidationResult>();

        return Validator.TryValidateObject(dto, validationContext, validationResult, true);
    }
}