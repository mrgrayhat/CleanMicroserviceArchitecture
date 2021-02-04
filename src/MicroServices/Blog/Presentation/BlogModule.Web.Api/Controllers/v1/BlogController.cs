using System.Threading.Tasks;
using BlogModule.Application.Features.Posts.Commands.CreatePost;
using BlogModule.Application.Features.Posts.Commands.DeletePostById;
using BlogModule.Application.Features.Posts.Commands.UpdatePost;
using BlogModule.Application.Features.Posts.Queries.GetAllPosts;
using BlogModule.Application.Features.Posts.Queries.GetPostById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SaeedrezayiWebsite.Api.WebApi.Controllers;

namespace BlogModule.Web.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    public class BlogController : BaseApiController
    {
        private readonly ILogger<BlogController> _logger;
        private readonly IMediator _mediator;
        public BlogController(ILogger<BlogController> logger, IMediator mediator)
        {
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
            _mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        // GET: api/<controller>
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetAllPostsParameter filter)
        {
            _logger.LogInformation($"BLog Module Built-in Controller Get Posts action executed!");
            return Ok(await _mediator.Send(new GetAllPostsQuery()
            {
                PageSize = filter.PageSize,
                PageNumber = filter.PageNumber
            }));
        }

        // GET api/<controller>/1
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation($"Blog Module Built-in Controller Get Post with id {id} action executed!");
            return Ok(await _mediator.Send(new GetPostByIdQuery { Id = id }));
        }

        // Delete api/<controller>/1
        [HttpDelete("{id}")]
        [Authorize(Roles ="SuperAdmin,Writer")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation($"Blog Controller Delete Post with id {id} action executed!");
            return Ok(await Mediator.Send(new DeletePostByIdCommand { Id = id }));

        }
        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize(Roles ="SuperAdmin,Writer")]
        public async Task<IActionResult> Put(int id, UpdatePostCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }
        // Post api/<controller>/1
        [HttpPost()]
        [Authorize(Roles ="SuperAdmin,Writer")]
        public async Task<IActionResult> Post(CreatePostCommand command)
        {
            return Ok(await _mediator.Send(command));
        }


    }
}
