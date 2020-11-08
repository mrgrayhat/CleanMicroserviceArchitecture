using SaeedRezayi.DomainClasses.Blog.Posts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SaeedRezayi.DataLayer.Configurations
{
    public class TagInfoConfiguration : IEntityTypeConfiguration<TagInfo>
    {
        public void Configure(EntityTypeBuilder<TagInfo> builder)
        {
            builder.HasIndex(x=>x.Id).IsUnique();
            builder.HasIndex(x=>x.Title).IsUnique();
            builder.Property(name => name.Id).ValueGeneratedOnAdd();
            builder.Property(name => name.Title).HasMaxLength(10).IsRequired();
        }
    }
}
