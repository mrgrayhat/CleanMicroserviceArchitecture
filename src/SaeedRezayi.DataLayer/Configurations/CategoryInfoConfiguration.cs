using SaeedRezayi.DomainClasses.Blog.Posts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SaeedRezayi.DataLayer.Configurations
{

    public class CategoryInfoConfiguration : IEntityTypeConfiguration<CategoryInfo>
    {
        public void Configure(EntityTypeBuilder<CategoryInfo> builder)
        {
            builder.HasKey(k => k.Id);

            builder.HasIndex(x=>x.Id).IsUnique();
            builder.HasIndex(x=>x.Title).IsUnique();

            builder.HasOne(c=>c.Parent)
                      .WithMany(u => u.SubCategories)
                      .HasForeignKey(ut => ut.ParentId);
            //builder.Property(category => category.Id).ValueGeneratedOnAdd();
            builder.Property(category => category.Title).HasMaxLength(25).IsRequired();
            builder.Property(category => category.UpdatedAt).ValueGeneratedOnUpdate();
            builder.Property(category => category.Description).HasMaxLength(254).IsRequired(false);
        }
    }
}
