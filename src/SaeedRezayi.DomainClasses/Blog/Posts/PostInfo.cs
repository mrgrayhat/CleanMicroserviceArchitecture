using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using SaeedRezayi.DomainClasses.Authentication;
using SaeedRezayi.DomainClasses.Blog.JoiningTables;
using SaeedRezayi.DomainClasses.Blog.Posts.Locales;
using SaeedRezayi.DomainClasses.Common;

namespace SaeedRezayi.DomainClasses.Blog.Posts
{
    /// <summary>
    /// Blog Posts Entity
    /// </summary>
    public class PostInfo : CommonEntityProperties<int>
    {
        public PostInfo()
        {
            Locales = new HashSet<PostLocaleInfo>();
            PostAttachments = new HashSet<PostAttachment>();
            PostTags = new HashSet<PostTag>();
            PostKeywords = new HashSet<PostKeyword>();
        }
        /// <summary>
        /// small post picture address
        /// </summary>
        public string Thumbnail
        {
            get;
            set;
        }
        /// <summary>
        /// soft remove || display / hide
        /// </summary>
        public bool IsArchive { get; set; } = false;
        /// <summary>
        /// show in main page for all or limited?
        /// </summary>
        public bool IsPublic { get; set; } = true;
        public long Visits { get; set; } = 0;
        public int AuthorId { get; set; }
        public UserInfo Author { get; set; }

        //public int LocalId { get; set; }
        public int? CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public CategoryInfo Category { get; set; }
        /// <summary>
        /// post locale data for each language
        /// </summary>
        public ICollection<PostLocaleInfo> Locales { get; set; }
        /// <summary>
        /// Added Attachments to post. like a file, pic, doc ...
        /// </summary>
        public virtual ICollection<PostAttachment> PostAttachments { get; set; }
        /// <summary>
        /// post tags for find and search faster
        /// </summary>
        public virtual ICollection<PostTag> PostTags { get; set; }
        public virtual ICollection<PostKeyword> PostKeywords { get; set; }
        //public virtual ICollection<PostComment> PostComments { get; set; }

    }
}
