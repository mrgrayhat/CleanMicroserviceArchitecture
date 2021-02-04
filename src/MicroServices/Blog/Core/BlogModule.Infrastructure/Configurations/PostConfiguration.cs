using BlogModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogModule.Infrastructure.Configurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasKey(k => k.Id);
            builder.HasIndex(index => index.Id).IsUnique();

            //builder.Property(n => n.LastModified).ValueGeneratedOnUpdate().IsRequired(false);
            builder.Property(n => n.Thumbnail).HasMaxLength(500).IsRequired(false);
        }
    }
}
