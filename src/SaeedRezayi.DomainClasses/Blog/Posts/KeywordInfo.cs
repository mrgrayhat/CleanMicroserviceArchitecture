using System.Collections.Generic;
using SaeedRezayi.DomainClasses.Blog.JoiningTables;

namespace SaeedRezayi.DomainClasses.Blog.Posts
{
    public class KeywordInfo
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public virtual ICollection<PostKeyword> PostKeywords { get; set; }
    }
}
