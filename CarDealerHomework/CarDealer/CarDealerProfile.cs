using AutoMapper;
using CarDealer.App.DtoImport;
using CarDealer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealer.App
{
    public class CarDealerProfile :Profile
    {
        public CarDealerProfile()
        {
            CreateMap<Supplier, SupplierDto>().ReverseMap();
            CreateMap<Part, PartDto>().ReverseMap();
        }
    }
}
