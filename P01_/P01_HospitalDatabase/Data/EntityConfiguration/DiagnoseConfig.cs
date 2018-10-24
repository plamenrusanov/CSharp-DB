﻿using Microsoft.EntityFrameworkCore;
using P01_HospitalDatabase.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace P01_HospitalDatabase.Data.EntityConfiguration
{
    public class DiagnoseConfig : IEntityTypeConfiguration<Diagnose>
    {
        public void Configure(EntityTypeBuilder<Diagnose> builder)
        {
            builder.HasKey(x => x.DiagnoseId);

            builder.Property(x => x.Name)
                .HasMaxLength(50)
                .IsUnicode();

            builder.Property(x => x.Comments)
               .HasMaxLength(250)
               .IsUnicode();
        }
    }
}
