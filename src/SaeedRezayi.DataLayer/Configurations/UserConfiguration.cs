using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SaeedRezayi.DomainClasses.Authentication;

namespace SaeedRezayi.DataLayer.Configurations
{
    public class UserConfiguration: IEntityTypeConfiguration<UserInfo>
    {
        public void Configure(EntityTypeBuilder<UserInfo> builder)
        {
            builder.HasIndex(x=>x.Id).IsUnique();
            builder.HasIndex(x=>x.Username).IsUnique();
            builder.HasIndex(x=>x.EmailAddress).IsUnique();

            builder.Property(name => name.Id).ValueGeneratedOnAdd();
            builder.Property(name => name.DisplayName).HasMaxLength(32)
                .IsRequired().IsUnicode();
            builder.Property(name => name.Username).HasMaxLength(24)
                .IsRequired().IsUnicode();
            builder.Property(name => name.Password).HasMaxLength(128).IsRequired();
            builder.Property(name => name.Address).HasMaxLength(255)
                .IsRequired(false).IsUnicode();
            builder.Property(name => name.Tell).HasMaxLength(20).IsRequired(false);
            builder.Property(name => name.ProfilePicture).HasMaxLength(500)
                .IsRequired(false).IsUnicode();
            builder.Property(name => name.Country).HasMaxLength(25).IsRequired(false);
            builder.Property(name => name.EmailAddress).HasMaxLength(128)
                .IsRequired().IsUnicode();
        }
    }
}
