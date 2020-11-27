using SaeedRezayi.DataLayer.Configurations;
using SaeedRezayi.DomainClasses.Authentication;
using SaeedRezayi.DomainClasses.Blog.JoiningTables;
using SaeedRezayi.DomainClasses.Common;
using SaeedRezayi.DomainClasses.Blog.Posts;
using Microsoft.EntityFrameworkCore;
using SaeedRezayi.DomainClasses.Authentication.JoiningTables;
using SaeedRezayi.DomainClasses.Blog.Posts.Locales;

namespace SaeedRezayi.DataLayer.Context
{
    public class ApplicationDbContext : DbContext, IUnitOfWork
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

        #region User & Authentication Tables

        public virtual DbSet<UserInfo> Users { set; get; }
        public virtual DbSet<RoleInfo> Roles { set; get; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<UserTokenInfo> UserTokens { get; set; }
        public virtual DbSet<UserFriendsInfo> UserFriends { get; set; }
        #endregion

        #region Blog Tables

        public virtual DbSet<PostInfo> Posts { set; get; }
        public virtual DbSet<PostLocaleInfo> PostLocales { set; get; }
        public virtual DbSet<CategoryInfo> Categories { set; get; }
        public virtual DbSet<AttachmentInfo> Attachments { set; get; }
        public virtual DbSet<TagInfo> Tags { set; get; }
        public virtual DbSet<KeywordInfo> Keywords { set; get; }
        //public virtual DbSet<CommentInfo> Comments { set; get; }
        // join tables n*n
        public virtual DbSet<PostTag> PostTags { get; set; }
        public virtual DbSet<PostAttachment> PostAttachments { get; set; }
        public virtual DbSet<PostKeyword> PostKeywords { get; set; }
        //public virtual DbSet<PostComment> PostComments { get; set; }
        #endregion

        #region System Tables
        public virtual DbSet<CultureInfo> Cultures { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // it should be placed here, otherwise it will rewrite the following settings!
            base.OnModelCreating(builder);

            // Custom application mappings
            builder.Entity<UserInfo>(entity =>
            {
                entity.Property(e => e.Username).HasMaxLength(450).IsRequired();
                entity.HasIndex(e => e.Username).IsUnique();
                entity.Property(e => e.Password).IsRequired();
                entity.Property(e => e.SerialNumber).HasMaxLength(450);
            });

            builder.Entity<RoleInfo>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(450).IsRequired();
                entity.HasIndex(e => e.Name).IsUnique();
            });

            builder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });
                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.RoleId);
                entity.Property(e => e.UserId);
                entity.Property(e => e.RoleId);
                entity.HasOne(d => d.Role).WithMany(p => p.UserRoles).HasForeignKey(d => d.RoleId);
                entity.HasOne(d => d.User).WithMany(p => p.UserRoles).HasForeignKey(d => d.UserId);
            });

            builder.Entity<UserTokenInfo>(entity =>
            {
                entity.HasOne(ut => ut.User)
                      .WithMany(u => u.UserTokens)
                      .HasForeignKey(ut => ut.UserId);

                entity.Property(ut => ut.RefreshTokenIdHash).HasMaxLength(450).IsRequired();
                entity.Property(ut => ut.RefreshTokenIdHashSource).HasMaxLength(450);
            });

            #region PostAttachment Join Table Configuration

            builder.Entity<PostAttachment>()
                    .HasKey(p => new { p.PostId, p.AttachmentId });
            builder.Entity<PostAttachment>().HasIndex(e => e.AttachmentId);
            builder.Entity<PostAttachment>().HasIndex(e => e.PostId);
            builder.Entity<PostAttachment>().Property(e => e.PostId);
            builder.Entity<PostAttachment>().Property(e => e.AttachmentId);

            builder.Entity<PostAttachment>()
                .HasOne(x => x.Post)
                .WithMany(x => x.PostAttachments)
                .HasForeignKey(x => x.PostId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<PostAttachment>()
                .HasOne(x => x.Attachment)
                .WithMany(x => x.PostAttachments)
                .HasForeignKey(x => x.AttachmentId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region PostKeyWord Join Table Configuration
            builder.Entity<PostKeyword>()
                .HasKey(p => new { p.PostId, p.KeywordId });
            builder.Entity<PostKeyword>().HasIndex(e => e.KeywordId);
            builder.Entity<PostKeyword>().HasIndex(e => e.PostId);
            builder.Entity<PostKeyword>().Property(e => e.KeywordId);
            builder.Entity<PostKeyword>().Property(e => e.PostId);

            builder.Entity<PostKeyword>()
                .HasOne(x => x.Post)
                .WithMany(x => x.PostKeywords)
                .HasForeignKey(x => x.PostId);
            builder.Entity<PostKeyword>()
                .HasOne(x => x.Keyword)
                .WithMany(x => x.PostKeywords)
                .HasForeignKey(x => x.KeywordId);
            #endregion

            #region PostTag Join Table Configuration
            builder.Entity<PostTag>()
                .HasKey(p => new { p.PostId, p.TagId });
            builder.Entity<PostTag>().HasIndex(e => e.TagId);
            builder.Entity<PostTag>().HasIndex(e => e.PostId);
            builder.Entity<PostTag>().Property(e => e.TagId);
            builder.Entity<PostTag>().Property(e => e.PostId);

            builder.Entity<PostTag>()
                .HasOne(x => x.Post)
                .WithMany(x => x.PostTags)
                .HasForeignKey(x => x.PostId);
            builder.Entity<PostTag>()
                .HasOne(x => x.Tag)
                .WithMany(x => x.PostTags)
                .HasForeignKey(x => x.TagId);
            #endregion

            #region PostComment Join Table Configuration
            /*builder.Entity<PostComment>()
                .HasKey(p => new { p.PostId, p.CommentId });
            builder.Entity<PostComment>().HasIndex(e => e.CommentId);
            builder.Entity<PostComment>().HasIndex(e => e.PostId);
            builder.Entity<PostComment>().Property(e => e.CommentId);
            builder.Entity<PostComment>().Property(e => e.PostId);

            builder.Entity<PostComment>()
                .HasOne(x => x.Post)
                .WithMany(x => x.PostComments)
                .HasForeignKey(x => x.CommentId);
            builder.Entity<PostComment>()
                .HasOne(x => x.Comment)
                .WithMany(x => x.PostComments)
                .HasForeignKey(x => x.CommentId);*/
            #endregion

            // to add custom configurations
            builder.AddCustomMappings();
        }
    }
}
