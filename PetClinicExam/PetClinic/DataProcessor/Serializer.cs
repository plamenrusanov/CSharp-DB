namespace PetClinic.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Newtonsoft.Json;
    using PetClinic.Data;
    using PetClinic.DataProcessor.ExportDto;

    public class Serializer
    {
        public static string ExportAnimalsByOwnerPhoneNumber(PetClinicContext context, string phoneNumber)
        {
            var passportsSerialNumbers = context.Passports
                                                .Where(x => x.OwnerPhoneNumber == phoneNumber)
                                                .Select(x => new
                                                {
                                                    OwnerName = x.OwnerName,
                                                    AnimalName = x.Animal.Name,
                                                    Age = x.Animal.Age,
                                                    SerialNumber = x.SerialNumber,
                                                    RegisteredOn = x.RegistrationDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)
                                                })
                                                .OrderBy(x => x.Age)
                                                .ThenBy(x => x.SerialNumber)
                                                .ToArray();

            var jsonString = JsonConvert.SerializeObject(passportsSerialNumbers, Newtonsoft.Json.Formatting.Indented);
            return jsonString;
        }

        public static string ExportAllProcedures(PetClinicContext context)
        {
            StringBuilder sb = new StringBuilder();

            var procedures = context.Procedures
                .OrderBy(x => x.DateTime)
                .ThenBy(x => x.Animal.PassportSerialNumber)
                .Select(x => new ProcedureDto
                {
                    Passport = x.Animal.PassportSerialNumber,
                    OwnerNumber = x.Animal.Passport.OwnerPhoneNumber,
                    DateTime = x.DateTime.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture),
                    AnimalAids = x.ProcedureAnimalAids.Select(c => new AnimalAidDto
                    {
                        Name = c.AnimalAid.Name,
                        Price = c.AnimalAid.Price
                    }).ToList(),
                    TotalPrice = x.Cost
                }).ToArray();

            var serializer = new XmlSerializer(typeof(ProcedureDto[]), new XmlRootAttribute("Procedures"));
            var xmlNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            serializer.Serialize(new StringWriter(sb), procedures, xmlNamespaces);

            return sb.ToString().Trim();
        }
    }
}
