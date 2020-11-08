using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SaeedRezayi.DomainClasses.Blog.Posts;

namespace SaeedRezayi.DataLayer.Configurations
{
     public class KeywordInfoConfiguration : IEntityTypeConfiguration<KeywordInfo>
    {
        public void Configure(EntityTypeBuilder<KeywordInfo> builder)
        {
            builder.HasIndex(index => index.Id).IsUnique();
            builder.HasIndex(index => index.Title).IsUnique();

            builder.Property(id => id.Id).ValueGeneratedOnAdd();
            builder.Property(name => name.Title).HasMaxLength(25).IsRequired();
        }
    }
}
