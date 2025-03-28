namespace Artillery.DataProcessor.ImportDto
{
    using Artillery.Data.Models.Enums;
    using System.ComponentModel.DataAnnotations;

    public class ImportGunsJson
    {
        [Required]
        public int ManufacturerId { get; set; }

        [Required]
        [Range(100, 1350000)]
        public int GunWeight { get; set; }

        [Required]
        [Range(2.00, 35.00)]
        public double BarrelLength { get; set; }

        public int? NumberBuild { get; set; }

        [Required]
        [Range(1, 100000)]
        public int Range { get; set; }

        [Required] public string GunType { get; set; } = null!;

        [Required]
        public int ShellId { get; set; }

        public ImportCountriesIDDto[] Countries { get; set; } = null!;
    }
}
