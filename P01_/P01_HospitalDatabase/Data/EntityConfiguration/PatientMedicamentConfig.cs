using Microsoft.EntityFrameworkCore;
using P01_HospitalDatabase.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace P01_HospitalDatabase.Data.EntityConfiguration
{
    public class PatientMedicamentConfig : IEntityTypeConfiguration<PatientMedicament>
    {
        public void Configure(EntityTypeBuilder<PatientMedicament> builder)
        {
            builder.HasKey(x => new { x.PatientId, x.MedicamentId });

            //builder.HasOne(x => x.Patient)
            //    .WithMany(x => x.Prescriptions)
            //    .HasForeignKey(x => x.PatientId);
        }
    }
}
