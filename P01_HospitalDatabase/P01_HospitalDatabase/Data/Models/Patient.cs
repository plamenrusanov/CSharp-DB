using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P01_HospitalDatabase.Data.Models
{
    [Table("Patients")]
    public class Patient
    {
        public Patient()
        {
            this.Prescriptions = new HashSet<PatientMedicament>();
            this.Diagnoses = new HashSet<Diagnose>();
            this.Visitations = new HashSet<Visitation>();
        }

        [Key]
        public int PatientId { get; set; }

        [MaxLength(50)]
        public string FirstName { get; set; }

        [StringLength(30, MinimumLength = 10)]
        public string LastName { get; set; }

        [Range(typeof(decimal), "0", "123456789012345")]
        public decimal Price { get; set; }

        [Required]
        public string Address { get; set; }

        [NotMapped] //-do not mapping in database because its calculation property
        public string Email { get; set; }

        [Column("HasInsurance", TypeName ="bit")]
        public bool HasInsurance { get; set; }

        public ICollection<PatientMedicament> Prescriptions { get; set; }

        public ICollection<Diagnose> Diagnoses { get; set; }

        public ICollection<Visitation> Visitations { get; set; }
    }
}
