using System.Collections.Generic;
using BlogModule.Domain.Common;

namespace BlogModule.Domain.Entities
{
    public class Post : AuditableBaseEntity
    {
        public Post()
        {
            Locales = new HashSet<PostLocale>();
            Comments = new HashSet<Comment>();
            //Attachments = new HashSet<Attachment>();
            //PostTags = new HashSet<PostTag>();
            //PostKeywords = new HashSet<PostKeyword>();
        }

        //public string Title { get; set; }
        //public string Slug { get; set; }
        //public string Body { get; set; }

        //public DateTimeOffset CreatedAt { get; set; }
        //public DateTimeOffset UpdatedAt { get; set; }

        /// <summary>
        /// soft remove || display / hide
        /// </summary>
        public bool IsArchive { get; set; } = false;
        /// <summary>
        /// show in main page for all or limited?
        /// </summary>
        public bool IsPublic { get; set; } = true;
        public long Visits { get; set; } = 0;
        public string Thumbnail { get; set; }
        //public int AuthorId { get; set; }
        //public User Author { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// list of post multilingual content's
        /// </summary>
        public string Tags { get; set; }
        public virtual ICollection<PostLocale> Locales { get; set; }
        /// <summary>
        /// list of post comment's
        /// </summary>
        public virtual ICollection<Comment> Comments { get; set; }
        //public virtual ICollection<Attachment> Attachments { get; set; }
        //public virtual ICollection<PostTag> PostTags { get; set; }
        //public virtual ICollection<PostKeyword> PostKeywords { get; set; }
        public int? CategoryId { get; set; }
        public Category Category { get; set; }

    }
}
