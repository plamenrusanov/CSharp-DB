using AutoMapper;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Xml.Serialization;

namespace CarDealer.App
{
    using CarDealer.Data;
    using CarDealer.Models;
    using DtoImport;
    using Newtonsoft.Json;
    using System;
    using System.Linq;

    class StartUp
    {
        static void Main(string[] args)
        {
            #region ImportXml
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });

            var mapper = config.CreateMapper();
            var context = new CarDealerContext();

            // ImportSuppliers(mapper);
            // ImportParts(mapper);

            //var xmlString = File.ReadAllText("./../../../Resources/parts.xml");

            //var serializer = new XmlSerializer(typeof(PartDto[]), new XmlRootAttribute("parts"));

            //var deserializedParts = (PartDto[])serializer.Deserialize(new StringReader(xmlString));

            //List<Part> parts = new List<Part>();

            //foreach (var partDto in deserializedParts)
            //{
            //    if (!IsValid(partDto))
            //    {
            //        continue;
            //    }

            //    var part = mapper.Map<Part>(partDto);

            //    var supplier = new Random().Next(1, 31);

            //    part.SupplierId = supplier;
            //    part.Supplier = context.Suppliers.FirstOrDefault(x => x.Id == supplier);

            //    parts.Add(part);

            //}

            //context.Parts.AddRange(parts);
            //context.SaveChanges();
            #endregion

            #region ExportJson
            CustomersTotalSales(context);
            #endregion
        }

        private static void CustomersTotalSales(CarDealerContext context)
        {
            var customers = context.Customers
                            .Where(x => x.Sales.Count >= 1)
                            .Select(s => new
                            {
                                fullName = s.Name,
                                bouthCars = s.Sales.Count(),
                                spentMoney = s.Sales.Sum(x => x.Car.PartCars.Sum(v => v.Part.Price)) - (s.Sales.Sum(x => x.Car.PartCars.Sum(v => v.Part.Price)) * s.Sales.Sum(K => K.Discount) / 100)
                            })
                            .OrderByDescending(x => x.spentMoney)
                            .ThenByDescending(x => x.bouthCars)
                            .ToArray();

            var jsonString = JsonConvert.SerializeObject(customers, Formatting.Indented);

            File.WriteAllText("./../../../customers_total_sales.json", jsonString);
        }

        private static void ImportParts(IMapper mapper)
        {
            var xmlString = File.ReadAllText("./../../../Resources/parts.xml");

            var serializer = new XmlSerializer(typeof(PartDto[]), new XmlRootAttribute("parts"));

            var deserializedParts = (PartDto[])serializer.Deserialize(new StringReader(xmlString));

            List<Part> parts = new List<Part>();
            var context = new CarDealerContext();
            foreach (var partDto in deserializedParts)
            {
                if (!IsValid(partDto))
                {
                    continue;
                }

                var part = mapper.Map<Part>(partDto);

                var supplier = new Random().Next(1, 31);

                part.SupplierId = supplier;
                part.Supplier = context.Suppliers.FirstOrDefault(x => x.Id == supplier);

                parts.Add(part);

            }

            context.Parts.AddRange(parts);
            context.SaveChanges();
        }

        private static void ImportSuppliers(IMapper mapper)
        {
            var xmlString = File.ReadAllText("./../../../Resources/suppliers.xml");

            var serializer = new XmlSerializer(typeof(SupplierDto[]), new XmlRootAttribute("suppliers"));

            var deserializedSuppliers = (SupplierDto[])serializer.Deserialize(new StringReader(xmlString));

            List<Supplier> suppliers = new List<Supplier>();

            foreach (var supplierDto in deserializedSuppliers)
            {
                if (!IsValid(supplierDto))
                {
                    continue;
                }

                var supplier = mapper.Map<Supplier>(supplierDto);

                suppliers.Add(supplier);

            }

            var context = new CarDealerContext();
            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();
        }

        public static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);

            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, validationContext, validationResult, true);
        }
    }

   
}
