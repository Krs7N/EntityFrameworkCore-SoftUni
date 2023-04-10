namespace Boardgames.DataProcessor.ImportDto;

using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

using Common;
using Data.Models.Enums;

[XmlType("Boardgame")]
public class ImportBoardgameDto
{
    [XmlElement("Name")]
    [Required]
    [MinLength(ValidationConstants.BoardgameNameMinLength)]
    [MaxLength(ValidationConstants.BoardgameNameMaxLength)]
    public string Name { get; set; } = null!;

    [XmlElement("Rating")]
    [Required]
    [Range(ValidationConstants.BoardgameRatingMinValue,
        ValidationConstants.BoardgameRatingMaxValue)]
    public double Rating { get; set; }

    [XmlElement("YearPublished")]
    [Required]
    [Range(ValidationConstants.BoardgameYearPublishedMinValue,
        ValidationConstants.BoardgameYearPublishedMaxValue)]
    public int YearPublished { get; set; }

    [XmlElement("CategoryType")]
    [Required]
    [Range(ValidationConstants.BoardgameCategoryTypeMinValue,
        ValidationConstants.BoardgameCategoryTypeMaxValue)]
    public int CategoryType { get; set; }

    [XmlElement("Mechanics")]
    [Required]
    public string Mechanics { get; set; } = null!;
}