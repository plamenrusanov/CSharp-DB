namespace PetClinic.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Newtonsoft.Json;
    using PetClinic.Data;
    using PetClinic.DataProcessor.ImportDto;
    using PetClinic.Models;

    public class Deserializer
    {

        public static string ImportAnimalAids(PetClinicContext context, string jsonString)
        {
            var deserializedAnimalAids = JsonConvert.DeserializeObject<AnimalAidDto[]>(jsonString);
            List<AnimalAid> aids = new List<AnimalAid>();
            StringBuilder sb = new StringBuilder();
            foreach (var item in deserializedAnimalAids)
            {
                if (!IsValid(item))
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }
                if (context.AnimalAids.Any(x => x.Name == item.Name) || aids.Any(x => x.Name == item.Name))
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }

                AnimalAid animalAid = new AnimalAid()
                {
                    Name = item.Name,
                    Price = item.Price,
                };
                aids.Add(animalAid);
                sb.AppendLine($"Record {animalAid.Name} successfully imported.");
            }
            context.AnimalAids.AddRange(aids);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportAnimals(PetClinicContext context, string jsonString)
        {
            var deserializedAnimals = JsonConvert.DeserializeObject<AnimalDto[]>(jsonString);
            List<Animal> animals = new List<Animal>();
            List<Passport> passports = new List<Passport>();

            StringBuilder sb = new StringBuilder();

            foreach (var animalDTo in deserializedAnimals)
            {
                if (!IsValid(animalDTo))
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }

                if (!IsValid(animalDTo.Passport))
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }

                if (context.Passports.Any(x => x.SerialNumber == animalDTo.Passport.SerialNumber) || animals.Any(x => x.PassportSerialNumber == animalDTo.Passport.SerialNumber))
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }
                var date = DateTime.ParseExact(animalDTo.Passport.RegistrationDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);


                Animal animal = new Animal()
                {
                    Name = animalDTo.Name,
                    Age = animalDTo.Age,
                    Type = animalDTo.Type,
                    PassportSerialNumber = animalDTo.Passport.SerialNumber,
                    Passport = new Passport()
                    {
                        SerialNumber = animalDTo.Passport.SerialNumber,
                        OwnerName = animalDTo.Passport.OwnerName,
                        OwnerPhoneNumber = animalDTo.Passport.OwnerPhoneNumber,
                        RegistrationDate = date,                        
                    }
                };

                animals.Add(animal);
                sb.AppendLine($"Record {animal.Name} Passport №: {animal.PassportSerialNumber} successfully imported.");
            }

            context.Animals.AddRange(animals);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportVets(PetClinicContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            List<Vet> vets = new List<Vet>();

            var deserializer = new XmlSerializer(typeof(VetDto[]), new XmlRootAttribute("Vets"));

            var deserializedOrders = (VetDto[])deserializer.Deserialize(new StringReader(xmlString));

            foreach (var vetDto in deserializedOrders)
            {
                if (!IsValid(vetDto))
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }

                if (context.Vets.Any(x => x.PhoneNumber == vetDto.PhoneNumber) || vets.Any(x => x.PhoneNumber == vetDto.PhoneNumber))
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }

                Vet vet = new Vet()
                {
                    Name = vetDto.Name,
                    Profession = vetDto.Name,
                    PhoneNumber = vetDto.PhoneNumber,
                    Age = vetDto.Age,
                };

                vets.Add(vet);
                sb.AppendLine($"Record {vet.Name} successfully imported.");
            }

            context.Vets.AddRange(vets);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportProcedures(PetClinicContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            List<Procedure> procedures = new List<Procedure>();

            var deserializer = new XmlSerializer(typeof(ProcedureDto[]), new XmlRootAttribute("Procedures"));

            var deserializedProcedures = (ProcedureDto[])deserializer.Deserialize(new StringReader(xmlString));

            foreach (var procedureDto in deserializedProcedures)
            {
                Vet vet = context.Vets.FirstOrDefault(x => x.Name == procedureDto.Vet);
                Animal animal = context.Animals.FirstOrDefault(x => x.PassportSerialNumber == procedureDto.Animal);
                DateTime date = DateTime.ParseExact(procedureDto.DateTime, "dd-MM-yyyy", CultureInfo.InvariantCulture);

                if (vet == null || animal == null)
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }

                List<AnimalAid> pAids = new List<AnimalAid>();
                bool isExist = true;
                foreach (var aid in procedureDto.AnimalAids)
                {
                    var procedureAid = context.AnimalAids.FirstOrDefault(x => x.Name == aid.Name);
                    if (procedureAid == null)
                    {
                        isExist = false;
                        break;
                    }
                  
                    if (pAids.Any(x => x.Name == procedureAid.Name))
                    {
                        isExist = false;
                        break;
                    }
                    pAids.Add(procedureAid);
                }
                
                if (!isExist)
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }
              
                Procedure procedure = new Procedure()
                {
                    Vet = vet,
                    Animal = animal,
                    DateTime = date,
                   //ProcedureAnimalAids = pAids,
                };

                procedures.Add(procedure);
                sb.AppendLine($"Record successfully imported.");
            }
            context.Procedures.AddRange(procedures);
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
