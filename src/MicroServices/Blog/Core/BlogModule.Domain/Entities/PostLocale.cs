using BlogModule.Domain.Common;

namespace BlogModule.Domain.Entities
{
    public class PostLocale : AuditableBaseEntity
    {
        /// <summary>
        /// post title display in view
        /// </summary>
        public string Title
        {
            get;
            set;
        }
        /// <summary>
        /// seo friendly title use in url
        /// </summary>
        public string Slug
        {
            get;
            set;
        }
        /// <summary>
        /// body of post may contain markups
        /// </summary>
        public string Content
        {
            get;
            set;
        }

        public int PostId { get; set; }
        public Post Post { get; set; }

        public int CultureId { get; set; }
        public Culture LocalCulture { get; set; }

    }
}
