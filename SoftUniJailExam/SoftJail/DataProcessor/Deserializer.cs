namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.Data.Models.Enums;
    using SoftJail.DataProcessor.ImportDto;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Deserializer
    {
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            var deserializedDepartments = JsonConvert.DeserializeObject<DepartmentDto[]>(jsonString);
            List<Department> departments = new List<Department>();
            foreach (var departmentDto in deserializedDepartments)
            {
                if (!IsValid(departmentDto))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                bool isValid = true;
                List<Cell> cells = new List<Cell>();
                foreach (var cellDto in departmentDto.Cells)
                {
                    if (!IsValid(cellDto))
                    {
                        isValid = false;
                        break;
                    }
                    Cell cell = new Cell()
                    {
                        CellNumber = cellDto.CellNumber,
                        HasWindow = cellDto.HasWindow
                    };
                    cells.Add(cell);
                }
                if (!isValid)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                Department department = new Department()
                {
                    Name = departmentDto.Name,
                    Cells = cells,
                };

                departments.Add(department);
                sb.AppendLine($"Imported {department.Name} with {cells.Count} cells");
            }
            context.Departments.AddRange(departments);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
         
            var deserializedPrisoners = JsonConvert.DeserializeObject<PrisonerDto[]>(jsonString);

            List<Prisoner> prisoners = new List<Prisoner>();

            foreach (var prisonerDto in deserializedPrisoners)
            {
                if (!IsValid(prisonerDto))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
                bool isValid = true;
                List<Mail> mails = new List<Mail>();
                foreach (var mailDto in prisonerDto.Mails)
                {
                    if (!IsValid(mailDto))
                    {
                        isValid = false;
                        break;
                    }
                    Mail mail = new Mail()
                    {
                        Description = mailDto.Description,
                        Address = mailDto.Address,
                        Sender = mailDto.Sender,
                    };
                    mails.Add(mail);
                }
                if (!isValid)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
               
               
                Prisoner prisoner = new Prisoner()
                {
                    CellId = prisonerDto.CellId,
                    Bail = prisonerDto.Bail,
                    Age = prisonerDto.Age,
                    FullName = prisonerDto.FullName,
                    Nickname = prisonerDto.Nickname,
                    IncarcerationDate = DateTime.ParseExact(prisonerDto.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),                  
                    Mails = mails,
                };

                if (prisonerDto.ReleaseDate != null)
                {
                    DateTime date = DateTime.ParseExact(prisonerDto.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    prisoner.ReleaseDate = date;
                }
                prisoners.Add(prisoner);
                sb.AppendLine($"Imported {prisoner.FullName} {prisoner.Age} years old");
            }

            context.Prisoners.AddRange(prisoners);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            //var inputXml = @"<Officers><Officer><Name>Riccardo Fockes</Name><Money>3623.98</Money><Position>Overseer</Position><Weapon>Pistol</Weapon><DepartmentId>3</DepartmentId><Prisoners><Prisoner id=""10"" /><Prisoner id=""16"" /><Prisoner id=""15"" /></Prisoners></Officer><Officer><Name>Arleen Zannolli</Name><Money>3539.40</Money><Position>Guard</Position><Weapon>FlashPulse</Weapon><DepartmentId>2</DepartmentId><Prisoners><Prisoner id=""2"" /></Prisoners></Officer><Officer><Name>Hailee Kennon</Name><Money>3652.49</Money><Position>Labour</Position><Weapon>Sniper</Weapon><DepartmentId>5</DepartmentId><Prisoners><Prisoner id=""3"" /><Prisoner id=""14"" /></Prisoners></Officer><Officer><Name>Lev de Chastelain</Name><Money>2442.80</Money><Position>Guard</Position><Weapon>Sniper</Weapon><DepartmentId>2</DepartmentId><Prisoners><Prisoner id=""13"" /><Prisoner id=""12"" /></Prisoners></Officer></Officers>";
            StringBuilder sb = new StringBuilder();
            List<Officer> officers = new List<Officer>();

            var deserializer = new XmlSerializer(typeof(OfficerDto[]), new XmlRootAttribute("Officers"));

            var deserializedOfficers = (OfficerDto[])deserializer.Deserialize(new StringReader(xmlString));

            foreach (var officerDto in deserializedOfficers)
            {
                if (!IsValid(officerDto))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                Position position;
                bool isValidPosition = Enum.TryParse(officerDto.Position, out position);

                Weapon weapon;
                bool isValidWeapon = Enum.TryParse(officerDto.Weapon, out weapon);

                if (!isValidPosition || !isValidWeapon)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                Officer officer = new Officer()
                {
                    FullName = officerDto.FullName,
                    Salary = officerDto.Salary,
                    Position = position,
                    Weapon = weapon,
                    DepartmentId = officerDto.DepartmentId,
                   
                };

                List<OfficerPrisoner> officerPrisoners = new List<OfficerPrisoner>();

                foreach (var item in officerDto.Prisoners)
                {
                    
                        OfficerPrisoner officerPrisoner = new OfficerPrisoner()
                        {
                            PrisonerId = item.Id,
                        };
                        officerPrisoners.Add(officerPrisoner);
                    
                    
                }

                foreach (var item in officerPrisoners)
                {
                    officer.OfficerPrisoners.Add(item);
                }

                officers.Add(officer);

                sb.AppendLine($"Imported {officer.FullName} ({officerPrisoners.Count} prisoners)");
            }

            context.Officers.AddRange(officers);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);

            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, validationContext, validationResult, true);
        }
    }
}