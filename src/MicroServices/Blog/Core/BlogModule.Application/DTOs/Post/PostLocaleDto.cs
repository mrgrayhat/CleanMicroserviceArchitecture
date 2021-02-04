using System;

namespace BlogModule.Application.DTOs.Post
{
    public class PostLocaleDto
    {
        public int Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }

        /// <summary>
        /// post title display in view
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// seo friendly title use in url
        /// </summary>
        public string Slug{ get; set; }
        /// <summary>
        /// body of post may contain markups
        /// </summary>
        public string Content { get; set; }

        public int PostId { get; set; }
        //public Post Post { get; set; }

        public int CultureId { get; set; }
        //public Culture LocaleCulture { get; set; }
    }
}
