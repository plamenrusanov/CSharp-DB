using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace P01_StudentSystem.Data.Models.Configuration
{
    public class CourseConfig : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(x => x.CourseId);

            builder.Property(x => x.Name)
                .HasMaxLength(80)
                .IsUnicode()
                .IsRequired();

            builder.Property(x => x.Description)
                .IsUnicode()
                .IsRequired(false);

            builder.HasMany(x => x.Resources)
                .WithOne(c => c.Course)
                .HasForeignKey(fk => fk.CourseId);

            builder.HasMany(x => x.StudentsEnrolled)
                .WithOne(c => c.Course)
                .HasForeignKey(fk => fk.CourseId);

            builder.HasMany(x => x.HomeworkSubmissions)
                .WithOne(c => c.Course)
                .HasForeignKey(fk => fk.CourseId);
        }
    }
}
