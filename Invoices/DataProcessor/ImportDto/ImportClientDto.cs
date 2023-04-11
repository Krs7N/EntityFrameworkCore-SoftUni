namespace Invoices.DataProcessor.ImportDto;

using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

using Common;

[XmlType("Client")]
public class ImportClientDto
{
    [XmlElement("Name")]
    [MinLength(ValidationConstants.ClientNameMinLength)]
    [MaxLength(ValidationConstants.ClientNameMaxLength)]
    [Required]
    public string Name { get; set; } = null!;

    [XmlElement("NumberVat")]
    [MinLength(ValidationConstants.ClientNumberVatMinLength)]
    [MaxLength(ValidationConstants.ClientNumberVatMaxLength)]
    [Required]
    public string NumberVat { get; set; } = null!;

    [XmlArray("Addresses")]
    public ImportAddressDto[] Addresses { get; set; } = null!;
}