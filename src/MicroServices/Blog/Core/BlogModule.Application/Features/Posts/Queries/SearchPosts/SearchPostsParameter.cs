using BlogModule.Application.Parameters;

namespace BlogModule.Application.Features.Posts.Queries.SearchPosts
{
    public class SearchPostsParameter : RequestParameter
    {
        public string Text { get; set; }
        public string SortOrder { get; set; }
    }
}
