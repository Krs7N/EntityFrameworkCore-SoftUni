namespace Invoices.DataProcessor.ImportDto;

using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

using Common;

[XmlType("Address")]
public class ImportAddressDto
{
    [XmlElement("StreetName")]
    [MinLength(ValidationConstants.AddressStreetNameMinLength)]
    [MaxLength(ValidationConstants.AddressStreetNameMaxLength)]
    [Required]
    public string StreetName { get; set; } = null!;

    [XmlElement("StreetNumber")]
    [Required]
    public int StreetNumber { get; set; }

    [XmlElement("PostCode")]
    [Required]
    public string PostCode { get; set; } = null!;

    [XmlElement("City")]
    [MinLength(ValidationConstants.AddressCityMinLength)]
    [MaxLength(ValidationConstants.AddressCityMaxLength)]
    [Required]
    public string City { get; set; } = null!;

    [XmlElement("Country")]
    [MinLength(ValidationConstants.AddressCountryMinLength)]
    [MaxLength(ValidationConstants.AddressCountryMaxLength)]
    [Required]
    public string Country { get; set; } = null!;
}