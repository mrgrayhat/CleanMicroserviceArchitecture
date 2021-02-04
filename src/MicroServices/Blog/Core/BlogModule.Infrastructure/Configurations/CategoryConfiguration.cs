using BlogModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogModule.Infrastructure.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(k => k.Id);

            builder.HasIndex(x => x.Id).IsUnique();
            builder.HasIndex(x => x.Name).IsUnique();

            builder.HasOne(c => c.Parent)
                      .WithMany(u => u.SubCategories)
                      .HasForeignKey(ut => ut.ParentId)
                      .OnDelete(DeleteBehavior.NoAction);
            //builder.Property(category => category.Id).ValueGeneratedOnAdd();
            builder.Property(category => category.Name).HasMaxLength(25).IsRequired();
            //builder.Property(category => category.LastModified).ValueGeneratedOnUpdate();
            builder.Property(category => category.Description).HasMaxLength(254).IsRequired(false);
        }
    }
}
