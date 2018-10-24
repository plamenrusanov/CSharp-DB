using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VaporStore.Data.Models
{
     public class User
    {
        public User()
        {
            this.Cards = new List<Card>();
        }
        //        •	Id – integer, Primary Key
        //•	Username – text with length[3, 20] (required)
        //•	FullName – text, which has two words, consisting of Latin letters.Both start with an upper letter and are separated by a single space(ex. "John Smith") (required)
        //•	Email – text(required)
        //•	Age – integer in the range[3, 103] (required)
        //•	Cards – collection of type Card
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20, MinimumLength =3)]
        public string Username { get; set; }

        [Required]
        [RegularExpression(@"^([A-Z]{1}[a-z]+ [A-Z]{1}[a-z]+)$")]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [Range(3, 103)]
        public int Age { get; set; }

        public ICollection<Card> Cards { get; set; }
    }
}
