using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace P01_StudentSystem.Data.Models.Configuration
{
    public class StudentConfig : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(x => x.StudentId);

            builder.Property(x => x.Name)
                .HasMaxLength(100)
                .IsUnicode()
                .IsRequired();

            builder.Property(X => X.PhoneNumber)
                .HasColumnType("char(10)")
                .IsUnicode(false)
                .IsRequired(false);

            builder
                .HasMany(x => x.CourseEnrollments)
                .WithOne(s => s.Student)
                .HasForeignKey(fk => fk.StudentId);

            builder
                .HasMany(x => x.HomeworkSubmissions)
                .WithOne(s => s.Student)
                .HasForeignKey(fk => fk.StudentId);
        }
    }
}
