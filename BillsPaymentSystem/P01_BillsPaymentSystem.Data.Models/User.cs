using P01_BillsPaymentSystem.Data.Models.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace P01_BillsPaymentSystem.Data.Models
{
    public class User
    {
        public User()
        {
            this.PaymentMethods = new List<PaymentMethod>();
        }

        [Key]
        public int UserId { get; set; }

        [MaxLength(50)]
        [Required]
        public string FirstName { get; set; }

        [MaxLength(50)]
        [Required]
        public string LastName { get; set; }

        [MaxLength(80)]
        [Required]
        [NonUnicode]
        public string Email { get; set; }

        [MaxLength(25)]
        [Required]
        [NonUnicode]
        public string Password { get; set; }

        public ICollection<PaymentMethod> PaymentMethods { get; set; }
    }
}
