using System;
using System.Collections.Generic;
using BlogModule.Application.DTOs.Post;
using BlogModule.Domain.Entities;

namespace BlogModule.Application.DTOs.Post
{
    /// <summary>
    /// Post ViewModel Response
    /// </summary>
    public class GetPostDto
    {
        public int Id { get; set; }
        public string Thumbnail { get; set; }
        public string Tags { get; set; }
        //public string Title { get; set; }
        //public string Slug { get; set; }
        //public string Body { get; set; }

        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedBy { get; set; }

        /// <summary>
        /// soft remove || display / hide
        /// </summary>
        public bool IsArchive { get; set; } = false;
        /// <summary>
        /// show in main page for all or limited?
        /// </summary>
        public bool IsPublic { get; set; } = true;
        public long Visits { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// list of post locales (contain 1 or more multilingual content's)
        /// </summary>
        public ICollection<PostLocaleDto> Locales { get; set; }
        /// <summary>
        /// list of post comment's
        /// </summary>
        public ICollection<Comment> Comments { get; set; }
        //public virtual ICollection<Attachment> Attachments { get; set; }
        //public virtual ICollection<PostTag> PostTags { get; set; }
        //public virtual ICollection<PostKeyword> PostKeywords { get; set; }
        public int? CategoryId { get; set; }
        public Category Category { get; set; }

    }
}