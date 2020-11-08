using System;
using System.Collections.Generic;
using System.Text;
using SaeedRezayi.DomainClasses.Blog.Posts;

namespace SaeedRezayi.DomainClasses.Blog.JoiningTables
{
    public class PostTag
    {
        public int PostId { get; set; }
        public PostInfo Post { get; set; }

        public int TagId { get; set; }
        public TagInfo Tag { get; set; }
    }
}
