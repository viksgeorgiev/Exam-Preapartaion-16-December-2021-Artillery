using System.Security.AccessControl;
using System.Text;
using Artillery.Data.Models;
using Artillery.Data.Models.Enums;
using Artillery.DataProcessor.ImportDto;
using Artillery.Utilities;
using Newtonsoft.Json;

namespace Artillery.DataProcessor
{
    using Artillery.Data;
    using System.ComponentModel.DataAnnotations;

    public class Deserializer
    {
        private const string ErrorMessage =
            "Invalid data.";
        private const string SuccessfulImportCountry =
            "Successfully import {0} with {1} army personnel.";
        private const string SuccessfulImportManufacturer =
            "Successfully import manufacturer {0} founded in {1}.";
        private const string SuccessfulImportShell =
            "Successfully import shell caliber #{0} weight {1} kg.";
        private const string SuccessfulImportGun =
            "Successfully import gun {0} with a total weight of {1} kg. and barrel length of {2} m.";

        public static string ImportCountries(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            ImportCountriesDto[]? countriesDtos
                = XmlHelper.Deserialize<ImportCountriesDto[]>(xmlString, "Countries");

            if (countriesDtos != null && countriesDtos.Length > 0)
            {
                foreach (ImportCountriesDto importCountriesDto in countriesDtos)
                {
                    if (!IsValid(importCountriesDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Country country = new Country()
                    {
                        CountryName = importCountriesDto.CountryName,
                        ArmySize = importCountriesDto.ArmySize,
                    };

                    context.Countries.Add(country);
                    sb.AppendLine(string.Format(SuccessfulImportCountry, country.CountryName, country.ArmySize));
                }
                context.SaveChanges();
            }
            return sb.ToString().TrimEnd();
        }

        public static string ImportManufacturers(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            ImportManufacturerDto[]? importManufacturerDtos =
                XmlHelper.Deserialize<ImportManufacturerDto[]>(xmlString, "Manufacturers");

            if (importManufacturerDtos != null && importManufacturerDtos.Length > 0)
            {
                foreach (ImportManufacturerDto manufacturerDto in importManufacturerDtos)
                {
                    if (!IsValid(manufacturerDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Manufacturer manufacturer = new Manufacturer()
                    {
                        ManufacturerName = manufacturerDto.ManufacturerName,
                        Founded = manufacturerDto.Founded,
                    };

                    string[] manufacturerPosition =
                        manufacturer.Founded.Split(", ", StringSplitOptions.RemoveEmptyEntries);

                    string townName = manufacturerPosition[manufacturerPosition.Length - 2];
                    string countryName = manufacturerPosition[manufacturerPosition.Length - 1];

                    string[] manufacturersNames = context.Manufacturers.Select(m => m.ManufacturerName).ToArray();

                    if (manufacturersNames.Contains(manufacturer.ManufacturerName))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    string position = $"{townName}, {countryName}";

                    context.Manufacturers.Add(manufacturer);
                    sb.AppendLine(string.Format(SuccessfulImportManufacturer, manufacturer.ManufacturerName, position));
                    context.SaveChanges();
                }
            }
            return sb.ToString().TrimEnd();
        }

        public static string ImportShells(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            ImportShellsDto[]? importShellsDtos 
                = XmlHelper.Deserialize<ImportShellsDto[]>(xmlString, "Shells");

            if (importShellsDtos != null && importShellsDtos.Length > 0)
            {
                foreach (ImportShellsDto shellsDto in importShellsDtos)
                {
                    if (!IsValid(shellsDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Shell shell = new Shell()
                    {
                        ShellWeight = shellsDto.ShellWeight,
                        Caliber = shellsDto.Caliber,
                    };

                    context.Shells.Add(shell);
                    sb.AppendLine(string.Format(SuccessfulImportShell, shell.Caliber, shell.ShellWeight));
                }

                context.SaveChanges();
            }
            return sb.ToString().TrimEnd();
        }

        public static string ImportGuns(ArtilleryContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ImportGunsJson[]? importGunsJsons = JsonConvert.DeserializeObject<ImportGunsJson[]>(jsonString);

            if (importGunsJsons != null && importGunsJsons.Length > 0)
            {
                foreach (var gunsJsonDto in importGunsJsons)
                {
                    if (!IsValid(gunsJsonDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    bool isvalid = Enum.TryParse<GunType>(gunsJsonDto.GunType, out GunType parseGunType);


                    if (!isvalid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    

                    Gun gun = new Gun()
                    {
                        ManufacturerId = gunsJsonDto.ManufacturerId,
                        GunWeight = gunsJsonDto.GunWeight,
                        BarrelLength = gunsJsonDto.BarrelLength,
                        NumberBuild = gunsJsonDto.NumberBuild,
                        Range = gunsJsonDto.Range,
                        GunType = parseGunType,
                        ShellId = gunsJsonDto.ShellId,
                    };

                    foreach (ImportCountriesIDDto importCountriesIdDto in gunsJsonDto.Countries)
                    {
                        CountryGun country = new CountryGun()
                        {
                            Gun = gun,
                            CountryId = importCountriesIdDto.Id,
                        };

                        context.CountriesGuns.Add(country);
                    }

                    sb.AppendLine(string.Format(SuccessfulImportGun, gun.GunType.ToString(), gun.GunWeight,gun.BarrelLength));
                    context.Guns.Add(gun);
                }

                context.SaveChanges();
            }

            return sb.ToString().TrimEnd();
        }
        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}