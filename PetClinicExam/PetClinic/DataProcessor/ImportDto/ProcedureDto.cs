using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace PetClinic.DataProcessor.ImportDto
{
    [XmlType("Procedure")]
    public class ProcedureDto
    {

        [Required]
        [StringLength(40, MinimumLength = 3)]
        public string Vet { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z]{7}[0-9]{3}$")]
        public string Animal { get; set; }

        [Required]
        public string DateTime { get; set; }

        public List<AnimalAidDto> AnimalAids { get; set; }
    }
}
