using SaeedRezayi.DomainClasses.Common;

namespace SaeedRezayi.DomainClasses.Blog.Posts.Locales
{
    /// <summary>
    /// Blog Post Culture Based Entity (Localizable)
    /// </summary>
    public class PostLocaleInfo
    {
        public int Id { get; set; }

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
        public PostInfo Post { get; set; }
        
        public int CultureId { get; set; }
        public CultureInfo LocalCulture { get; set; }
    }
}
