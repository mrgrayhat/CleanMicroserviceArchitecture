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
            builder.Property(n => n.Url).HasMaxLength(254);
            builder.Property(n => n.CreatedBy).HasMaxLength(16);
            builder.Property(n => n.LastModifiedBy).HasMaxLength(16).IsRequired(false);
            builder.Property(n => n.Tags).HasMaxLength(254).IsRequired(false);
            builder.Property(n => n.VerifiedHash)
                .ValueGeneratedOnAddOrUpdate()
                .HasMaxLength(32);
        }
    }
}
