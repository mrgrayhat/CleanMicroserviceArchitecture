using SaeedRezayi.DomainClasses.Blog.Comments;
using SaeedRezayi.DomainClasses.Blog.Posts;

namespace SaeedRezayi.DomainClasses.Blog.JoiningTables
{
    public class PostComment
    {
        public long CommentId { get; set; }
        public CommentInfo Comment { get; set; }

        public int PostId { get; set; }
        public PostInfo Post{ get; set; }
    }
}
