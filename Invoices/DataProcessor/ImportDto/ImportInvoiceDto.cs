namespace Invoices.DataProcessor.ImportDto;

using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

using Common;
using Data.Models.Enums;

public class ImportInvoiceDto
{
    [JsonProperty("Number")]
    [Range(ValidationConstants.InvoiceMinNumberValue, ValidationConstants.InvoiceMaxNumberValue)]
    [Required]
    public int Number { get; set; }

    [JsonProperty("IssueDate")]
    [Required]
    public string IssueDate { get; set; } = null!;

    [JsonProperty("DueDate")]
    [Required]
    public string DueDate { get; set; } = null!;

    [JsonProperty("Amount")]
    [Required]
    public decimal Amount { get; set; }

    [JsonProperty("CurrencyType")]
    [Range(ValidationConstants.InvoiceCurrencyTypeMinValue, ValidationConstants.InvoiceCurrencyTypeMaxValue)]
    [Required]
    public CurrencyType CurrencyType { get; set; }

    [JsonProperty("ClientId")]
    [Required]
    public int ClientId { get; set; }
}