using System;
using System.Collections.Generic;
using System.Text;

namespace ProductShop.Models
{
    public class Product
    {
        public Product()
        {
            this.CategoryProducts = new List<CategoryProduct>();
        }
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price{ get; set; }

        public int SellarId { get; set; }
        public User Sellar { get; set; }

        public int? BuyerId { get; set; }
        public User Buyer { get; set; }

        public ICollection<CategoryProduct> CategoryProducts { get; set; }

    }
}
