using System.Xml.Serialization;

namespace Artillery.DataProcessor.ExportDto
{
    [XmlType("Country")]
    public class ExportCountryDto
    {
        [XmlAttribute(nameof(Country))]
        public string Country { get; set; } = null!;

        [XmlAttribute(nameof(ArmySize))]
        public string ArmySize { get; set; } = null!;
    }
}