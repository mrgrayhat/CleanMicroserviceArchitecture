using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SaeedRezayi.Common;
using SaeedRezayi.Common.Messages;
using SaeedRezayi.DataLayer.Context;
using SaeedRezayi.DomainClasses.Blog.Posts;
using SaeedRezayi.Services.Contracts.Blog;
using Microsoft.EntityFrameworkCore;
using SaeedRezayi.ViewModels.Blog;
using SaeedRezayi.ViewModels.Types;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using SaeedRezayi.DomainClasses.Blog.Posts.Locales;

namespace SaeedRezayi.Services.Blog
{
    public class BlogService : IBlogService//, IDisposable
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<PostInfo> _posts;
        private readonly DbSet<PostLocaleInfo> _postsLocals;
        private readonly DbSet<CategoryInfo> _categories;

        private readonly ILogger<BlogService> _logger;

        public BlogService(IUnitOfWork uow, ILogger<BlogService> logger)
        {
            _logger = logger;
            _logger.CheckArgumentIsNull(nameof(logger));

            _uow = uow;
            _uow.CheckArgumentIsNull(nameof(_uow));

            _posts = _uow.Set<PostInfo>();
            _postsLocals = _uow.Set<PostLocaleInfo>();
            _categories = _uow.Set<CategoryInfo>();
        }

        public async Task<MessageContract> AddPostAsync(PostViewModel post)
        {
            post.CheckArgumentIsNull(nameof(post));

            foreach (var item in post.PostLocales)
            {
                if (await FindPostAsync(item.Title) != null)
                {
                    _logger.LogError($"Post already exist. Failed to add post: `{post.Id}`.");
                    return new MessageContract
                    {
                        StatusCode = MessageStatusCodeTypes.ERROR,
                        Message = "Post already exist!",
                        Exception = new InvalidOperationException("Post already exist")
                    };
                }
            }
            var cat = await _categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Title == post.Category.Title);
            if (cat != null)
            {
                post.Category = cat;
            }

            var added = await _posts.AddAsync((PostInfo)post);
            await _uow.SaveChangesAsync();

            return new MessageContract
            {
                StatusCode = MessageStatusCodeTypes.SUCCESS,
                Message = "Post Added Successfully!",
                Parameter = (PostViewModel)added.Entity
            };

        }

        public async Task<MessageContract> EditPostAsync(int postId, PostViewModel newPost)
        {
            newPost.CheckArgumentIsNull(nameof(newPost));

            var find = await _posts.FindAsync(newPost.Id);
            find = newPost;
            _posts.Update(find);
            await _uow.SaveChangesAsync();
            return new MessageContract
            {
                StatusCode = MessageStatusCodeTypes.SUCCESS,
                Message = "Post Successfuly Updated!",
            };
        }

        public async Task<PostViewModel> FindPostAsync(PostViewModel post)
        {
            var find = await _posts.FindAsync(post);

            return find;
        }

        ///<summary>
        /// Find a post with join relation's
        ///</summary>
        public async Task<PostViewModel> FindPostAsync(int postId)
        {
            PostViewModel model = await _posts
            .Include(c => c.Author)
            .Include(c => c.Category)
            .Include(c => c.Locales).ThenInclude(x => x.LocalCulture)
            //.Include(c => c.PostComments).ThenInclude(c => c.Comment)
            .Include(a => a.PostAttachments).ThenInclude(x => x.Attachment)
            .Include(t => t.PostTags).ThenInclude(x => x.Tag)
            .Include(c => c.PostKeywords).ThenInclude(x => x.Keyword)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == postId);
            return model;
        }
        public async Task<PostViewModel> FindPostAsync(string title)
        {
            PostViewModel find = await _posts
            .AsNoTracking()
            .Include(c => c.Author)
            .Include(c => c.Category)
            .Include(a => a.PostAttachments).ThenInclude(x => x.Attachment)
            .Include(t => t.PostTags).ThenInclude(x => x.Tag)
            .Include(c => c.PostKeywords).ThenInclude(x => x.Keyword)
            .Include(x => x.Locales)//.ThenInclude(x=>x.Post)
            .FirstOrDefaultAsync(x => x.Locales.Select(x => x.Title == title).FirstOrDefault());
            return find;
        }

        public async Task<PagedPostsListViewModel> GetPagedPostsListAsync(int pageNumber,
            int recordsPerPage,
            string sortByField, SortingOrderTypes sortOrder,
            bool showAllPosts = false)
        {
            var skipRecords = pageNumber * recordsPerPage;
            var query = _posts
                .Include(u => u.Author)
                .Include(u => u.Category)
                // all post locales + culture
                .Include(u => u.Locales).ThenInclude(c => c.LocalCulture)
                .Include(x => x.PostAttachments).ThenInclude(a => a.Attachment)
                .Include(y => y.PostTags).ThenInclude(t => t.Tag)
                .Include(c => c.PostKeywords).ThenInclude(x => x.Keyword)
                //.Include(c => c.PostComments).ThenInclude(c => c.Comment.IsArchive == false)
                .AsNoTracking();

            if (showAllPosts == false)
            {
                query = query.Where(x => !x.IsArchive);
            }
            _logger.LogInformation($"Getting Paged Post's List: `{recordsPerPage}` Posts.");

            query = sortByField switch
            {
                "Id" => sortOrder == SortingOrderTypes.Descending ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id),
                // by title alphabet
                //"Title" => sortOrder == SortingOrderTypes.Descending ? query.OrderByDescending(c => c.Locales.Title) : query.OrderBy(c => c.Locales.Title),
                // by tag
                "Tag" => sortOrder == SortingOrderTypes.Descending ? query.OrderByDescending(c => c.PostTags) : query.OrderBy(c => c.PostTags),
                // higher views
                "Visit" => sortOrder == SortingOrderTypes.Descending ? query.OrderByDescending(c => c.Visits) : query.OrderBy(c => c.Visits),
                // latest created and update
                _ => sortOrder == SortingOrderTypes.Descending ? query.OrderByDescending(c => c.CreatedAt).ThenByDescending(u => u.UpdatedAt) : query.OrderBy(c => c.CreatedAt).ThenBy(u => u.UpdatedAt),
            };
            PagedPostsListViewModel paged = null;
            try
            {
                paged = new PagedPostsListViewModel
                {
                    Paging =
                {
                    TotalItems = await query.CountAsync()
                },
                    Posts = await query.Skip(skipRecords)
                                       .Take(recordsPerPage)
                                       .Select(x => (PostViewModel)x)
                                       .ToListAsync()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed Select Posts From Blog Posts");
            }
            finally
            {
                _logger.LogInformation($"Selected {recordsPerPage} Post From Blog Posts");
            }
            return paged;
        }

        public async Task<MessageContract> RemovePostAsync(PostViewModel post)
        {
            return await RemovePostAsync(post.Id, false);
        }
        public async Task<MessageContract> RemovePostAsync(int postId)
        {
            return await RemovePostAsync(postId, false);
        }
        public async Task<MessageContract> RemovePostAsync(int postId, bool soft)
        {
            var result = await FindPostAsync(postId);
            if (result == null)
            {
                return new MessageContract
                {
                    StatusCode = MessageStatusCodeTypes.ERROR,
                    Message = "This Post is not exist!",
                };
            }

            _posts.Remove(result);
            await _uow.SaveChangesAsync();

            return new MessageContract
            {
                StatusCode = MessageStatusCodeTypes.SUCCESS,
                Message = "The post has been removed successfully!",
            }; ;
        }
        public async Task<MessageContract> ArchivePostAsync(PostViewModel post)
        {
            var find = await _posts.FirstOrDefaultAsync(p => p.Id == post.Id);
            if (find == null)
            {
                return new MessageContract
                {
                    StatusCode = MessageStatusCodeTypes.ERROR,
                    Message = "This Post is not exist!",
                    Exception = new InvalidOperationException("Post is not exist!")
                };
            }
            find.IsArchive = true;
            await UpdatePostLastActivityAsync(find.Id);

            //_posts.Update(find);
            //await _uow.SaveChangesAsync();
            return new MessageContract
            {
                StatusCode = MessageStatusCodeTypes.SUCCESS,
                Message = "Post Successfuly Archived!",
            };
        }
        public async Task<MessageContract> UpdatePostLastActivityAsync(int postId)
        {
            var post = await FindPostAsync(postId);
            post.CheckArgumentIsNull(nameof(post));

            if (post.UpdatedAt != null)
            {
                var updateLastActivityDate = TimeSpan.FromMinutes(2);
                var currentUtc = DateTimeOffset.UtcNow;
                var timeElapsed = currentUtc.Subtract(post.UpdatedAt.Value);
                if (timeElapsed < updateLastActivityDate)
                {
                    //return new MessageContract
                    //{
                    //    StatusCode = MessageStatusCodeTypes.ACCEPTED,
                    //    Message = $"Post {postId} Activity Updated."
                    //};
                }
            }
            else
            {
                return new MessageContract
                {
                    StatusCode = MessageStatusCodeTypes.INVALID,
                    Message = $"Post {postId} Not Found."
                };
            }
            post.UpdatedAt = DateTimeOffset.UtcNow;
            _posts.Update(post);
            await _uow.SaveChangesAsync();
            int status = await _uow.SaveChangesAsync();

            return new MessageContract
            {
                StatusCode = MessageStatusCodeTypes.ACCEPTED,
                Message = $"Post {postId} Activity Updated.",
                Parameter = status
            };
        }

        public async Task<PagedPostsListViewModel> SearchPagedPostsListAsync(SearchPostsViewModel model, int pageNumber, CancellationToken cancellationToken = default)
        {
            var skipRecords = pageNumber * model.MaxNumberOfRows;
            var query = _posts
                .Include(x => x.PostTags).ThenInclude(x => x.Tag)
                .Include(a => a.PostAttachments).ThenInclude(a => a.Attachment)
                .AsNoTracking();
            if (!string.IsNullOrWhiteSpace(model.TextToFind))
            {
                //model.TextToFind = model.TextToFind.ApplyCorrectYeKe();

                //if (model.IsPartOfContent)
                {
                    //query = query.Where(x => x.Content.Contains(model.TextToFind));
                    //}

                }
                query = query
                    .Where(t => t.Locales.Any(x => x.Title.Contains(model.TextToFind)) || t.Locales.Any(x => x.Content.Contains(model.TextToFind)));
            }

            if (!model.IsArchive)
            {
                query = query.Where(x => !x.IsArchive);
            }
            query = query.OrderBy(x => x.Id);

            return new PagedPostsListViewModel
            {
                Paging =
                {
                    TotalItems = await query.CountAsync()
                },
                Posts = await query.Skip(skipRecords)
                                .Take(model.MaxNumberOfRows)
                                .Select(x => (PostViewModel)x)
                                .ToListAsync()
            };
        }

        /// <summary>
        /// old method
        /// </summary>
        /// <param name="term"></param>
        /// <param name="searchIn"></param>
        /// <param name="pageNumber"></param>
        /// <param name="recordsPerPage"></param>
        /// <param name="sortByField"></param>
        /// <param name="sortOrder"></param>
        /// <param name="showAllPosts"></param>
        /// <returns></returns>
        public async Task<PagedPostsListViewModel> SearchInPostsAsync(string term, string searchIn, int pageNumber,
            int recordsPerPage,
            string sortByField, SortingOrderTypes sortOrder = SortingOrderTypes.Descending,
            bool showAllPosts = true)
        {
            var skipRecords = pageNumber * recordsPerPage;

            var query = _posts
                .Include(u => u.Author)
                .Include(c => c.Category)
                .Include(x => x.PostAttachments).ThenInclude(a => a.Attachment)
                .Include(y => y.PostTags).ThenInclude(t => t.Tag)
                .AsNoTracking();

            if (showAllPosts == false)
            {
                query = query.Where(x => !x.IsArchive);
            }
            #region Searching

            switch (searchIn)
            {
                case "Content":
                    query = query
                        .Where(s => s.Locales.Any(x => x.Content.Contains(term, StringComparison.OrdinalIgnoreCase)));
                    break;
                case "Title":
                    query = query
                        .Where(s => s.Locales.Any(x => x.Title.Contains(term)));
                    break;
                default:
                    query = query
                    .Where(s => s.Locales.Any(x => x.Content
                    .Contains(term) || s.Locales.Any(x => x.Title.Contains(term))));
                    break;
            }
            #endregion

            #region Sorting

            switch (sortByField)
            {
                default: // latest created and updated
                    query = sortOrder == SortingOrderTypes.Descending ? query.OrderByDescending(c => c.CreatedAt).ThenByDescending(u => u.UpdatedAt) : query.OrderBy(c => c.CreatedAt).ThenBy(u => u.UpdatedAt);
                    break;
            }
            #endregion

            _logger.LogInformation($"Searching in Post's: by term '{term}' .");
            PagedPostsListViewModel paged = null;
            try
            {
                paged = new PagedPostsListViewModel
                {
                    Paging = { TotalItems = await query.CountAsync() },
                    Posts = await query.Skip(skipRecords)
                                       .Take(recordsPerPage)
                                       .Select(x => (PostViewModel)x)
                                       .ToListAsync()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Search Post's Failed!");
            }
            return paged;
        }

        public async Task<PagedPostsListViewModel> FindPagedPostsListAsync(TagViewModel tag)
        {
            var query = _posts
                .AsNoTracking()
                .Include(t => t.PostTags).ThenInclude(x => x.Tag.Id == tag.Id)
                .Select(x => (PostViewModel)x);

            _logger.LogInformation($"Selected Post's By Tag: `{tag.Title}`.");
            return new PagedPostsListViewModel
            {
                Paging =
                {
                    TotalItems = await query.CountAsync()
                },
                Posts = await query.ToListAsync()
            };
        }

        public IAsyncEnumerable<PostViewModel> GetPostsByCategory(string category)
        {
            var query = _posts
                .Include(c => c.Category)
                .AsQueryable();

            query = query
                .Where(p => p.Category.Title
                .Contains(category, StringComparison.OrdinalIgnoreCase));

            return query
                .Select(x => (PostViewModel)x)
                .AsAsyncEnumerable();
        }
        public IAsyncEnumerable<CategoryViewModel> GetCategories()
        {
            var query = _posts
                //.Include(c => c.Category)
                .AsNoTracking();

            var categories = query
                .Select(post => (CategoryViewModel)post.Category)
                .Distinct()
                .AsAsyncEnumerable();

            return categories;
        }

        //private bool disposedValue;
        //protected virtual void Dispose(bool disposing)
        //{
        //    if (!disposedValue)
        //    {
        //        if (disposing)
        //        {
        //            if (_uow != null)
        //                _uow.Dispose();
        //        }

        //        disposedValue = true;
        //    }
        //}
        //~BlogService()
        //{
        //    Dispose(disposing: false);
        //}
        //public void Dispose()
        //{
        //    Dispose(disposing: true);
        //    GC.SuppressFinalize(this);
        //}
    }
}
