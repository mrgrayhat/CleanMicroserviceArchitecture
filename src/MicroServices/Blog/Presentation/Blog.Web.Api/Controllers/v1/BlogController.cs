using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Api.Controllers;
using BlogModule.Application.DTOs.Post;
using BlogModule.Application.Features.Posts.Commands.CreatePost;
using BlogModule.Application.Features.Posts.Commands.DeletePostById;
using BlogModule.Application.Features.Posts.Commands.UpdatePost;
using BlogModule.Application.Features.Posts.Queries.GetAllPosts;
using BlogModule.Application.Features.Posts.Queries.GetPostById;
using BlogModule.Application.Features.Posts.Queries.SearchPosts;
using BlogModule.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BlogModule.Web.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    public class BlogController : BaseApiController
    {
        private readonly ILogger<BlogController> _logger;

        public BlogController(ILogger<BlogController> logger)
        {
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
            //_mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        // GET: api/v1/<controller>
        [HttpGet]
        public async Task<ActionResult<PagedResponse<IEnumerable<GetPostDto>>>> Index([FromQuery] GetAllPostsParameter filter = null)
        {
            _logger.LogInformation($"BLog MicroService Built-in Controller, {nameof(Index)} action executed!");

            return Ok(await Mediator.Send(new GetAllPostsQuery()
            {
                PageSize = filter.PageSize <= 0 ? 10 : filter.PageSize,
                PageNumber = filter.PageNumber <= 0 ? 1 : filter.PageNumber
            }));
        }

        // GET api/v1/<controller>/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Response<GetPostDto>>> GetById(int id)
        {
            _logger.LogInformation($"Blog MicroService Built-in Controller, {nameof(GetById)} action with input: {id} executed!");
            return Ok(await Mediator.Send(new GetPostByIdQuery { Id = id }));
        }

        // GET api/v1/<controller>/search/text
        [HttpGet("search/{text}")]
        public async Task<ActionResult<Response<GetPostDto>>> Search(string text,
            string sortOrder = "Desc")
        {
            _logger.LogInformation($"Blog MicroService Built-in Controller, {nameof(Search)} action with filter: {text} executed!");
            return Ok(await Mediator.Send(new SearchPostsQuery
            {
                Text = text,
                SortOrder = sortOrder == "Desc" ? sortOrder : "Asc"
            }));
        }

        // Delete api/v1/<controller>/1
        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin,Writer")]
        public async Task<ActionResult<Response<int>>> Delete(int id)
        {
            _logger.LogInformation($"Blog MicroService Delete Post with id {id} action executed!");
            return Ok(await Mediator.Send(new DeletePostByIdCommand
            {
                Id = id
            }));

        }
        // PUT api/v1/<controller>/1
        [HttpPut("{id}")]
        [Authorize(Roles = "SuperAdmin,Writer")]
        public async Task<ActionResult<Response<int>>> Put(int id, UpdatePostCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        // Post api/v1/<controller>/1
        [HttpPost()]
        [Authorize(Roles = "SuperAdmin,Writer")]
        public async Task<ActionResult<Response<int>>> Post(CreatePostCommand command)
        {
            return Ok(await Mediator.Send(command));
        }


    }
}
