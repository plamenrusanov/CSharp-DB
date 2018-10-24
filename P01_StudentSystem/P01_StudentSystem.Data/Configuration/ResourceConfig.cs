using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace P01_StudentSystem.Data.Models.Configuration
{
    public class ResourceConfig : IEntityTypeConfiguration<Resource>
    {
        public void Configure(EntityTypeBuilder<Resource> builder)
        {
            builder.HasKey(x => x.ResourceId);

            builder.Property(x => x.Name)
                .HasMaxLength(50)
                .IsUnicode();

            builder.Property(x => x.Url)
                .IsUnicode(false)
                .IsRequired();
        }
    }
}
