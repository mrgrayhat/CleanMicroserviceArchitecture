using System;
using System.Collections.Generic;
using System.Text;
using SaeedRezayi.DomainClasses.Authentication;
using SaeedRezayi.DomainClasses.Blog.JoiningTables;
using SaeedRezayi.DomainClasses.Common;

namespace SaeedRezayi.DomainClasses.Blog.Posts
{
    public class AttachmentInfo : CommonEntityProperties<Guid>
    {
        public string Name { get; set; }
        public long Size { get; set; }
        public int TotalDownloads { get; set; }
        public bool IsValid { get; set; } = true;
        public string Url { get; set; }
        public string Password { get; set; }
        public bool? IsDeleted { get; set; }
        public int UserId { get; set; }
        public virtual UserInfo User { get; set; }
        public virtual ICollection<PostAttachment> PostAttachments { get; set; }
    }
}
