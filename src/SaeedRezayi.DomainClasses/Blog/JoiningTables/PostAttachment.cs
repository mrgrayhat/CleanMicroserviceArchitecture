using System;
using SaeedRezayi.DomainClasses.Blog.Posts;

namespace SaeedRezayi.DomainClasses.Blog.JoiningTables
{
    public class PostAttachment
    {
        public int PostId { get; set; }
        public PostInfo Post { get; set; }

        public Guid AttachmentId { get; set; }
        public AttachmentInfo Attachment { get; set; }
    }
}
