﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SoftJail.Data.Models
{
    public class Mail
    {
        //        •	Id – integer, Primary Key
        //•	Description– text(required)
        //•	Sender – text(required)
        //•	Address – text, consisting only of letters, spaces and numbers, which ends with “ str.” (required) (Example: “62 Muir Hill str.“)
        //•	PrisonerId - integer, foreign key
        //•	Prisoner – the mail's Prisoner (required)
        [Key]
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Sender { get; set; }

        [Required]
        [RegularExpression(@"^([0-9a-zA-Z ]*) str.$")]
        public string Address { get; set; }

        public int PrisonerId { get; set; }
        [Required]
        public Prisoner Prisoner { get; set; }

    }
}
