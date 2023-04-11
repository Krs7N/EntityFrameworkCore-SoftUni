namespace Invoices.DataProcessor.ImportDto;

using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

using Common;
using Data.Models.Enums;

public class ImportProductDto
{
    [JsonProperty("Name")]
    [MinLength(ValidationConstants.ProductNameMinLength)]
    [MaxLength(ValidationConstants.ProductNameMaxLength)]
    [Required]
    public string Name { get; set; } = null!;

    [JsonProperty("Price")]
    [Range(ValidationConstants.ProductPriceMinValue, ValidationConstants.ProductPriceMaxValue)]
    [Required]
    public decimal Price { get; set; }

    [JsonProperty("CategoryType")]
    [Range(ValidationConstants.ProductCategoryTypeMinValue, ValidationConstants.ProductCategoryTypeMaxValue)]
    [Required]
    public CategoryType CategoryType { get; set; }

    [JsonProperty("Clients")]
    [Required]
    public int[] ClientIds { get; set; } = null!;
}