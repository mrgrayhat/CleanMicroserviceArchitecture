using System;
using System.Threading.Tasks;
using SaeedRezayi.Common;
using SaeedRezayi.Services.Contracts.Blog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SaeedRezayi.ViewModels.Blog;
using SaeedRezayi.ViewModels.Types;
using Microsoft.AspNetCore.Http;
using SaeedRezayi.Common.Messages;
using SaeedRezayi.Services.Contracts.Account;
using System.Linq;
using System.Collections.Generic;

namespace SaeedRezayi.Api.Areas.Blog.Controllers
{
    [Area(AreaConstants.BlogArea)]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class BlogController : ControllerBase
    {
        private readonly ILogger<BlogController> _logger;
        private readonly IUsersService _usersService;
        private IBlogService _blogService;

        public BlogController(ILogger<BlogController> logger, IUsersService usersService, IBlogService blogService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(_logger));
            _usersService = usersService;
            _usersService.CheckArgumentIsNull(nameof(usersService));
            _blogService = blogService;
        }

        // GET: api/<BlogController>
        ///<summary>
        /// get all blog post's with pagination
        ///</summary>
        //[ResponseCache(Duration = 20)]
        [HttpGet]
        public async Task<ActionResult<PagedPostsListViewModel>> Index(int? page = 1)
        {
            return await GetPagedPosts(page.Value);
        }

        ///<summary>
        /// get all blog post's with pagination
        ///</summary>
        [HttpGet("posts")]
        public async Task<ActionResult<PagedPostsListViewModel>> GetPagedPosts(
            int page = 1, int maxRecords = 10, string field = "",
            SortingOrderTypes order = SortingOrderTypes.Descending)
        {
            maxRecords = maxRecords > 200 ? 10 : maxRecords;

            PagedPostsListViewModel model = await _blogService.
            GetPagedPostsListAsync(pageNumber: page - 1,
                recordsPerPage: maxRecords,
                sortByField: field,
                sortOrder: order,
                showAllPosts: true);

            model.Paging.CurrentPage = page;
            model.Paging.ItemsPerPage = maxRecords;
            model.Paging.ShowFirstLast = false;

            return Ok(model);
        }

        /// <summary>
        /// Get a post with id from blog
        /// </summary>
        /// <param name="id">The id of the post you want to get</param>
        /// <returns>An ActionResult of type Post</returns>
        /// <remarks>
        /// Sample request (this request find the post by id) \
        /// GET /api/blog/id \
        /// [ \
        ///     { \
        ///       "postId": "1", \
        ///       } \
        /// ] \
        /// </remarks>
        [ResponseCache(Duration = 20)]
        [HttpGet("posts/{id}")]
        public async Task<ActionResult<PostViewModel>> GetPost(int id)
        {
            PostViewModel model = await _blogService.FindPostAsync(id);
            if (model == null)
            {
                return BadRequest("NotFound");
            }
            _logger.LogInformation($"Post {model.Id} Requested By {User.Identity.Name}!");
            return Ok(model);
        }

        //TODO: Remove this func before publish
        [HttpGet("[action]")]
        public async Task<ActionResult<PostViewModel>> AddJunkPost()
        {
            int rndNumber = new Random().Next(100, 10000);
            var post = new PostViewModel
            {
                PostLocales = new List<PostLocaleViewModel>(3)
                {
                    new PostLocaleViewModel
                    {
                        Title = $"مطلب {rndNumber} عنوان",
                        Content = $"مطلب {rndNumber} محتوای",
                        Slug = $"مطلب-{rndNumber}",
                        LocaleCultureId = 1,
                        LocaleCultureCode = "fa-ir"
                    },
                    new PostLocaleViewModel
                    {
                        Title = $"Post {rndNumber} Title",
                        Content = $"Post {rndNumber} Content",
                        Slug = $"post-{rndNumber}",
                        LocaleCultureId = 2,
                        LocaleCultureCode = "en-us"
                    },
                    new PostLocaleViewModel
                    {
                        Title = $"عنوان المحتوى {rndNumber}",
                        Content = $"{rndNumber} المحتوى",
                        Slug = $"المحتوى-{rndNumber}",
                        LocaleCultureId = 3,
                        LocaleCultureCode = "ar-sa"
                    },
                },
                Keywords = new List<KeywordViewModel>(2)
                {
                    new KeywordViewModel
                    {
                        Title = $"keyword1 {rndNumber}"
                    },
                    new KeywordViewModel
                    {
                        Title = $"keyword2 {rndNumber}"
                    }
                },
                Tags = new List<TagViewModel>(2)
                {
                    new TagViewModel
                    {
                         Title = $"tag1 {rndNumber}"
                    },
                    new TagViewModel
                    {
                        Title = $"tag2 {rndNumber}"
                    }
                },
                //Attachments = new List<AttachmentViewModel>(2)
                //{
                //    new AttachmentViewModel
                //    {
                //        Id = Guid.NewGuid(),
                //        Name = $"attachment1 {rndNumber}"
                //    },
                //    new AttachmentViewModel
                //    {
                //        Id = Guid.NewGuid(),
                //        Name = $"attachment2 {rndNumber}"
                //    }
                //},
                //Category = new CategoryViewModel
                //{
                //    //Title = $"caetgory {rndNumber}"
                //    Title = "عمومی"
                //},
                IsArchive = false,
                IsPublic = true,
                Visits = 1,
                Author = await _usersService.FindUserAsync(3),
            };
            MessageContract result = await _blogService.AddPostAsync(post);

            if (result.StatusCode == MessageStatusCodeTypes.SUCCESS)
            {
                _logger.LogInformation($"User {post.Author?.Username} created new post {post.PostLocales.FirstOrDefault().Title}");

                return Created($"https://localhost:5001//Blog//Posts//{post.Id}", result.Parameter);
            }
            else
                return BadRequest(result.Message);
        }

        // POST api/<BlogController>
        /// <summary>
        /// Add a new post to the blog
        /// </summary>
        /// <param name="post">the post model</param>
        /// <returns>Success(201)+post address Or BadRequest(400) + error</returns>
        //[Authorize(Policy = CustomRolesViewModel.Admin)]
        //[Authorize(Policy = CustomRolesViewModel.Writer)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpPost("[action]")]
        public async Task<ActionResult<PostViewModel>> AddPost([FromBody] PostViewModel post)
        {
            //if (!User.Identity.IsAuthenticated)
            //    return Unauthorized();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            post.Author = await _usersService.GetCurrentUserAsync();
            var result = await _blogService.AddPostAsync(post);

            if (result.StatusCode == MessageStatusCodeTypes.SUCCESS)
                return Created($"https://localhost:5001//{post.Id}", post);
            else
                return BadRequest(result.Message);
        }

        // PUT api/<BlogController>/5
        /// <summary>
        /// Update a post
        /// </summary>
        /// <param name="id">Id of post</param>
        /// <param name="post">the new post</param>
        /// <returns>Success(200) Or BadRequest(400)</returns>
        //[Authorize(Policy = CustomRolesViewModel.Admin)]
        //[Authorize(Policy = CustomRolesViewModel.Writer)]
        [HttpPut("post/{id}")]
        public async Task<ActionResult<MessageContract>> UpdatePost(int id, [FromBody] PostViewModel post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _blogService.EditPostAsync(id, post);
            if (result.StatusCode == MessageStatusCodeTypes.SUCCESS)
                return Ok(result);
            else
                return BadRequest(result.Message);
        }

        // DELETE api/<BlogController>/5
        /// <summary>
        /// Remove a post from blog
        /// </summary>
        /// <param name="id">id of post</param>
        /// <returns>Success(200) Or BadRequest(400)</returns>
        //[Authorize(Policy = CustomRolesViewModel.Admin)]
        //[Authorize(Policy = CustomRolesViewModel.Writer)]
        [HttpDelete("post/{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            id.CheckArgumentIsNull(nameof(id));

            var result = await _blogService.RemovePostAsync(id);
            if (result.StatusCode == MessageStatusCodeTypes.SUCCESS)
                return Ok(result);
            else
                return BadRequest(result.Message);
        }
        [HttpGet("[action]")]
        public async Task<ActionResult<PagedPostsListViewModel>> Search(SearchPostsViewModel searchModel, int page = 1)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var results = await _blogService.SearchPagedPostsListAsync(searchModel, page);
            if (results == null || !results.Posts.Any())
            {
                return NotFound($"Search for '{searchModel.TextToFind}' has no result!");
            }
            return results;
        }

        [HttpGet("Search/{term}")]
        public async Task<ActionResult<PagedPostsListViewModel>> Search(
            string term, string searchIn, int? page = 1, int maxRecords = 10,
            string sortByField = "", SortingOrderTypes sortingOrder = SortingOrderTypes.Descending, bool showAll = false)
        {
            if (string.IsNullOrEmpty(term))
            {
                term.CheckArgumentIsNull(nameof(term));
            }

            PagedPostsListViewModel results;
            results = await _blogService.SearchInPostsAsync(term, searchIn, page.Value - 1, maxRecords, sortByField, sortingOrder, showAll);

            if (results == null || !results.Posts.Any())
            {
                return NotFound($"search for '{term}' has no result.");
            }
            results.Paging.CurrentPage = page.Value;
            results.Paging.ItemsPerPage = maxRecords;
            results.Paging.ShowFirstLast = false;

            return Ok(results);
        }
    }
}
