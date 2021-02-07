using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StorageManagement.Domain.Entities;

namespace StorageManagement.Infrastructure.Configurations
{
    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.HasKey(k => k.Id);
            builder.HasIndex(index => index.Id).IsUnique();

            //builder.Property(n => n.LastModified).ValueGeneratedOnUpdate().IsRequired(false);
            builder.Property(n => n.Name).HasMaxLength(50).IsRequired(false);
            builder.Property(n => n.Description).HasMaxLength(250).IsRequired(false);
            builder.Property(n => n.ContentType).HasMaxLength(25).IsRequired(false);
            builder.Property(n => n.VerifiedHash)
                .ValueGeneratedOnAddOrUpdate()
                .HasMaxLength(32).IsRequired(false);
        }
    }
}
