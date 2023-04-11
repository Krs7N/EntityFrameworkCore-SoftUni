namespace Invoices.Data.Models;

using System.ComponentModel.DataAnnotations;

using Common;

public class Client
{
    public Client()
    {
        Invoices = new HashSet<Invoice>();
        Addresses = new HashSet<Address>();
        ProductsClients = new HashSet<ProductClient>();
    }

    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(ValidationConstants.ClientNameMaxLength)]
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(ValidationConstants.ClientNumberVatMaxLength)]
    public string NumberVat { get; set; } = null!;

    public virtual ICollection<Invoice> Invoices { get; set; }

    public virtual ICollection<Address> Addresses { get; set; }

    public virtual ICollection<ProductClient> ProductsClients { get; set; }
}