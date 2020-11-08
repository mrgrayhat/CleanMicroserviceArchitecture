using System;
using System.Collections.Generic;
using System.Text;
using SaeedRezayi.DomainClasses.Blog.JoiningTables;
using SaeedRezayi.DomainClasses.Common;

namespace SaeedRezayi.DomainClasses.Blog.Comments
{
    public class CommentInfo : CommonEntityProperties<long>
    {
        public string User { get; set; }
        public string Content { get; set; }
        public long? ParentId { get; set; }
        public bool IsArchive { get; set; } = false;
        public CommentInfo Parent { get; set; }

        public PostComment PostComments { get; set; }
    }
}
