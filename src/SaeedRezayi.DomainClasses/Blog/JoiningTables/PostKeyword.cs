using SaeedRezayi.DomainClasses.Blog.Posts;

namespace SaeedRezayi.DomainClasses.Blog.JoiningTables
{
    public class PostKeyword
    {
        public int KeywordId { get; set; }
        public KeywordInfo Keyword { get; set; }

        public int PostId { get; set; }
        public PostInfo Post { get; set; }
    }
}
