﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace CarDealer.Models
{
    public class Car
    {
        public Car()
        {
            this.PartCars = new List<PartCars>();
        }
        public int Id { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public int TravelledDistance { get; set; }

        public Sale Sale { get; set; }

        public ICollection<PartCars> PartCars { get; set; }

    }
}
