using SaeedRezayi.DomainClasses.Blog.Posts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SaeedRezayi.DataLayer.Configurations
{
    public class PostInfoConfiguration : IEntityTypeConfiguration<PostInfo>
    {
        public void Configure(EntityTypeBuilder<PostInfo> builder)
        {
            builder.HasIndex(index => index.Id).IsUnique();
            //builder.OwnsMany(l => l.Locales);

            builder.Property(n => n.UpdatedAt).ValueGeneratedOnUpdate().IsRequired(false);
            builder.Property(n => n.Thumbnail).HasMaxLength(500).IsRequired(false);
        }
    }
}
