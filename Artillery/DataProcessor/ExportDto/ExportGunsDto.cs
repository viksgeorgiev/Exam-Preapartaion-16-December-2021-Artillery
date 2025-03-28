using System.Xml.Serialization;

namespace Artillery.DataProcessor.ExportDto
{
    [XmlType("Gun")]
    public class ExportGunsDto
    {
        [XmlAttribute(nameof(Manufacturer))]
        public string Manufacturer { get; set; } = null!;

        [XmlAttribute(nameof(GunType))]
        public string GunType { get; set; } = null!;

        [XmlAttribute(nameof(GunWeight))]
        public string GunWeight { get; set; } = null!;

        [XmlAttribute(nameof(BarrelLength))]
        public string BarrelLength { get; set; } = null!;

        [XmlAttribute(nameof(Range))]
        public string Range { get; set; } = null!;

        [XmlArray(nameof(Countries))]
        public ExportCountryDto[] Countries { get; set; } = null!;
    }
}
