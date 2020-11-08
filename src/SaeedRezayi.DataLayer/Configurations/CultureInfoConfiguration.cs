using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SaeedRezayi.DomainClasses.Common;

namespace SaeedRezayi.DataLayer.Configurations
{
    public class CultureInfoConfiguration : IEntityTypeConfiguration<CultureInfo>
    {
        public void Configure(EntityTypeBuilder<CultureInfo> builder)
        {
            builder.HasIndex(index => index.Id).IsUnique();
            builder.HasIndex(index => index.Code).IsUnique();

            builder.Property(n => n.DisplayName).HasMaxLength(24).IsRequired(false);
            builder.Property(n => n.Code).HasMaxLength(8).IsRequired(false);
        }
    }
}
