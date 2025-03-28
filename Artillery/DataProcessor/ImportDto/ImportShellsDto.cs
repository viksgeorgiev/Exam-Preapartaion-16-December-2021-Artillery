﻿namespace Artillery.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Shell")]
    public class ImportShellsDto
    {
        [XmlElement(nameof(ShellWeight))]
        [Required]
        [Range(2, 1680)]
        public double ShellWeight { get; set; }

        [XmlElement(nameof(Caliber))]
        [Required]
        [MinLength(4)]
        [MaxLength(30)]
        public string Caliber { get; set; } = null!;

    }
}
