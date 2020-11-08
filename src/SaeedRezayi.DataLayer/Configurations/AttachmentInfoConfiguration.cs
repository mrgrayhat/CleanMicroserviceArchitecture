using SaeedRezayi.DomainClasses.Blog.Posts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SaeedRezayi.DataLayer.Configurations
{
    public class AttachmentInfoConfiguration : IEntityTypeConfiguration<AttachmentInfo>
    {
        public void Configure(EntityTypeBuilder<AttachmentInfo> builder)
        {
            builder.HasIndex(index => index.Id).IsUnique();
            builder.HasIndex(index => index.Name).IsUnique();

            builder.Property(name => name.Id).ValueGeneratedNever();
            builder.Property(name => name.Name).HasMaxLength(50).IsRequired(false);
            builder.Property(name => name.UpdatedAt).ValueGeneratedOnUpdate();
            builder.Property(pass => pass.Password).HasMaxLength(128).IsRequired(false);
        }
    }
}
