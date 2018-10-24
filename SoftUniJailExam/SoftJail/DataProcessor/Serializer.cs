namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using SoftJail.DataProcessor.ExportDto;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        { 

            var prisoners = context.Prisoners
                .Where(x => ids.Any(c => c == x.Id))
                .ToArray()
                .Select(x => new
                {
                    Id = x.Id,
                    Name = x.FullName,
                    CellNumber = x.Cell.CellNumber,
                    Officers = x.PrisonerOfficers.Select(c => new
                    {
                        OfficerName = c.Officer.FullName,
                        Department = c.Officer.Department.Name
                    }).OrderBy(v => v.OfficerName).ToArray(),
                    TotalOfficerSalary = Math.Round(x.PrisonerOfficers.Sum(c => c.Officer.Salary), 2)
                })
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Id);

            var jsonString = JsonConvert.SerializeObject(prisoners, Newtonsoft.Json.Formatting.Indented);
            return jsonString;
        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            StringBuilder sb = new StringBuilder();
            var prisonersArray = prisonersNames.Split(",");
            var prisoners = context.Prisoners
                .Where(x => prisonersArray.Any(c => c == x.FullName))
                .Select(x => new PrisonerDto
                {
                    Id = x.Id,
                    Name = x.FullName,
                    IncarcerationDate = x.IncarcerationDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    EncryptedMessages = x.Mails.Select(c => new MessageDto
                    {
                        Description = Reverse(c.Description),
                    }).ToArray()
                })
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Id)
                .ToArray();


            var serializer = new XmlSerializer(typeof(PrisonerDto[]), new XmlRootAttribute("Prisoners"));
            var xmlNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            serializer.Serialize(new StringWriter(sb), prisoners, xmlNamespaces);

            return sb.ToString().Trim();
        }

        private static string Reverse(string description)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = description.Length - 1;  i >= 0 ;  i--)
            {
                sb.Append(description[i]);
            }
            return sb.ToString().Trim();
        }
    }
}