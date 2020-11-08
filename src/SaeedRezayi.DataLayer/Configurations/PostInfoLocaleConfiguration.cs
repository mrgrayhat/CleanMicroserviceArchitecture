using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SaeedRezayi.DomainClasses.Blog.Posts.Locales;

namespace SaeedRezayi.DataLayer.Configurations
{
    public class PostInfoLocaleConfiguration : IEntityTypeConfiguration<PostLocaleInfo>
    {
        public void Configure(EntityTypeBuilder<PostLocaleInfo> builder)
        {
            builder.HasIndex(index => index.Id).IsUnique();
            builder.HasIndex(index => index.Title).IsUnique();
            builder.HasOne(p => p.Post).WithMany(l => l.Locales).HasForeignKey(fk=>fk.PostId);

            builder.Property(n => n.Title).HasMaxLength(100).IsRequired();
            builder.Property(n => n.Content).HasMaxLength(5000).IsUnicode().IsRequired();
            builder.Property(n => n.Slug).HasMaxLength(50).IsUnicode().IsRequired();
        }
    }
}
