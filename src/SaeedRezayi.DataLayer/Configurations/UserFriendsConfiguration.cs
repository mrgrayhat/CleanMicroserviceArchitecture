using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SaeedRezayi.DomainClasses.Authentication;

namespace SaeedRezayi.DataLayer.Configurations
{
    public class UserFriendsConfiguration : IEntityTypeConfiguration<UserFriendsInfo>
    {
        public void Configure(EntityTypeBuilder<UserFriendsInfo> builder)
        {
            builder.HasKey(e => new { e.User1Id, e.User2Id });

            builder.HasIndex(e => e.User1Id);
            builder.HasIndex(e => e.User2Id);

            builder.Property(e => e.User1Id);
            builder.Property(e => e.User2Id);
            builder.HasOne(d => d.User1).WithMany().HasForeignKey(d => d.User1Id).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(d => d.User2).WithMany().HasForeignKey(d => d.User2Id)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}