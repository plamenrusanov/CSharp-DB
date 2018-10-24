using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductShop.Models
{
    public class User
    {
        public User()
        {
            this.ProductsBought = new List<Product>();
            this.ProductsSold = new List<Product>();
        }
        public int Id { get; set; }

        public string FirstName { get; set; }

        [Required]
        [MinLength(3)]
        public string LastName { get; set; }

        public int? Age { get; set; }

        public ICollection<Product>  ProductsSold { get; set; }

        public ICollection<Product> ProductsBought { get; set; }

    }
}
