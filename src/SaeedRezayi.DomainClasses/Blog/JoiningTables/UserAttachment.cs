using System;
using SaeedRezayi.DomainClasses.Authentication;
using SaeedRezayi.DomainClasses.Blog.Posts;

namespace SaeedRezayi.DomainClasses.Blog.JoiningTables
{
    public class UserAttachment
    {
        public int UserId { get; set; }
        public UserInfo User { get; set; }

        public Guid AttachmentId { get; set; }
        public AttachmentInfo Attachment { get; set; }
    }
}
