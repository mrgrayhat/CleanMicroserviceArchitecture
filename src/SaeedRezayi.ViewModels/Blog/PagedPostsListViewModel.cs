using System.Collections.Generic;
using cloudscribe.Web.Pagination;

namespace SaeedRezayi.ViewModels.Blog
{
    public class PagedPostsListViewModel
    {

        public PagedPostsListViewModel()
        {
            Paging = new PaginationSettings();

        }
        public List<PostViewModel> Posts { get; set; }

        public PaginationSettings Paging { get; set; }
    }
}
