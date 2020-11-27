using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SaeedRezayi.Common.Messages;
using SaeedRezayi.ViewModels.Blog;
using SaeedRezayi.ViewModels.Types;

namespace SaeedRezayi.Services.Contracts.Blog
{
    /// <summary>
    /// Blog Service Contract
    /// </summary>
    public interface IBlogService
    {
        #region Get

        Task<PagedPostsListViewModel> GetPagedPostsListAsync(int pageNumber, int recordsPerPage, string sortByField, SortingOrderTypes sortOrder, bool showAllPosts = false);

        IAsyncEnumerable<PostViewModel> GetPostsByCategory(string category);
        IAsyncEnumerable<CategoryViewModel> GetCategories();
        #endregion

        #region Add

        Task<MessageContract> AddPostAsync(PostViewModel post);

        #endregion

        #region Update
        Task<MessageContract> EditPostAsync(int postId, PostViewModel newPost);
        Task<MessageContract> UpdatePostLastActivityAsync(int postId);
        #endregion

        #region Delete
        Task<MessageContract> RemovePostAsync(int postId, bool soft);
        Task<MessageContract> RemovePostAsync(int postId);
        Task<MessageContract> RemovePostAsync(PostViewModel post);
        #endregion

        #region Extra Operations
        Task<MessageContract> ArchivePostAsync(PostViewModel post);
        #endregion

        #region Search

        Task<PagedPostsListViewModel> SearchPagedPostsListAsync(SearchPostsViewModel model,
            int pageNumber, CancellationToken cancellationToken = default);
        Task<PagedPostsListViewModel> SearchInPostsAsync(string term, string searchIn,
            int pageNumber, int recordsPerPage, string sortByField,
            SortingOrderTypes sortOrder = SortingOrderTypes.Descending, bool showAllPosts = false);
        Task<PagedPostsListViewModel> FindPagedPostsListAsync(TagViewModel tag);
        Task<PostViewModel> FindPostAsync(int postId, bool track = false);
        Task<PostViewModel> FindPostAsync(string title);
        Task<PostViewModel> FindPostAsync(PostViewModel post);
        #endregion
    }
}