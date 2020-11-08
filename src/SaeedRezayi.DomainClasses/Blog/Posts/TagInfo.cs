using System;
using System.Collections.Generic;
using System.Text;
using SaeedRezayi.DomainClasses.Blog.JoiningTables;

namespace SaeedRezayi.DomainClasses.Blog.Posts
{
    public class TagInfo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Visits { get; set; }

        public virtual ICollection<PostTag> PostTags { get; set; }
    }
}
