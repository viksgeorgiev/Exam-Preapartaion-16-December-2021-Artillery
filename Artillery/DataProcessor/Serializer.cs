using Artillery.Data.Models.Enums;
using Artillery.DataProcessor.ExportDto;
using Artillery.Utilities;
using Newtonsoft.Json;

namespace Artillery.DataProcessor
{
    using Artillery.Data;

    public class Serializer
    {
        public static string ExportShells(ArtilleryContext context, double shellWeight)
        {
            var shells = context
                .Shells
                .Where(s => s.ShellWeight > shellWeight)
                .Select(s => new
                {
                    s.ShellWeight,
                    s.Caliber,
                    Guns = s.Guns
                        .Where(x => ((int)x.GunType) == 3)
                        .Select(g => new
                        {
                            GunType = g.GunType.ToString(),
                            g.GunWeight,
                            g.BarrelLength,
                            Range = g.Range > 3000 ? "Long-range" : "Regular range",
                        })
                        .OrderByDescending(g => g.GunWeight)
                        .ToArray()
                })
                .OrderBy(s => s.ShellWeight)
                .ToArray();

            string result = JsonConvert.SerializeObject(shells,Formatting.Indented);
            return result;

        }

        public static string ExportGuns(ArtilleryContext context, string manufacturer)
        {
            var guns = context
                .Guns
                .Where(g => g.Manufacturer.ManufacturerName == manufacturer)
                .OrderBy(g => g.BarrelLength)
                .Select(g => new ExportGunsDto()
                {
                    Manufacturer = g.Manufacturer.ManufacturerName,
                    GunType = g.GunType.ToString(),
                    BarrelLength = g.BarrelLength.ToString(),
                    GunWeight = g.GunWeight.ToString(),
                    Range = g.Range.ToString(),
                    Countries = g.CountriesGuns
                        .Where(gc => gc.Country.ArmySize > 4500000)
                        .OrderBy(gc => gc.Country.ArmySize)
                        .Select(gc => new ExportCountryDto()
                        {
                            Country = gc.Country.CountryName,
                            ArmySize = gc.Country.ArmySize.ToString(),
                        })
                        .ToArray()
                })
                .ToArray();

            string gunsExport = XmlHelper.Serialize(guns, "Guns");

            return gunsExport;
        }
    }
}
