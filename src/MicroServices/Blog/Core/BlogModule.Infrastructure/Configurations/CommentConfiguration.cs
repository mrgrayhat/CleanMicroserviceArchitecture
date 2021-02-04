using BlogModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogModule.Infrastructure.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasIndex(index => index.Id).IsUnique();
            builder.HasIndex(index => index.Title).IsUnique();

            builder
                .HasOne(p => p.Post)
                .WithMany(l => l.Comments)
                .HasForeignKey(fk => fk.PostId);

            builder.Property(n => n.Title).HasMaxLength(100).IsRequired();
            builder.Property(n => n.Body).HasMaxLength(500).IsUnicode().IsRequired();
        }

    }
}
