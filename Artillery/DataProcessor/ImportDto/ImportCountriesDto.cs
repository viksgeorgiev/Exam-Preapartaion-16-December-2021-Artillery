using System.Xml.Serialization;

namespace Artillery.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    [XmlType("Country")]
    public class ImportCountriesDto
    {
        [XmlElement(nameof(CountryName))]
        [Required]
        [MinLength(4)]
        [MaxLength(60)]
        public string CountryName { get; set; } = null!;

        [XmlElement(nameof(ArmySize))]
        [Required]
        [Range(50000, 10000000)]
        public int ArmySize { get; set; }
    }
}
