using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BlogModule.Application.Exceptions;
using BlogModule.Application.Interfaces.Repositories;
using BlogModule.Application.Wrappers;
using BlogModule.Domain.Entities;
using MediatR;

namespace BlogModule.Application.Features.Posts.Commands.UpdatePost
{
    public class UpdatePostCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        //public string Title { get; set; }
        //public string Body { get; set; }
        //public string Thumbnail { get; set; }
        //public string Slug { get; set; }
        public ICollection<PostLocale> Locales { get; set; }
        public int CategoryId { get; set; }
        public string Tags { get; set; }
        public string Thumbnail { get; set; }
        public bool IsArchive { get; set; }
        public bool IsPublic { get; set; }
        //public int Visits { get; set; }
        public string Description { get; set; }


        public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, Response<int>>
        {
            private readonly IPostRepositoryAsync _postRepository;
            public UpdatePostCommandHandler(IPostRepositoryAsync productRepository)
            {
                _postRepository = productRepository;
            }
            public async Task<Response<int>> Handle(UpdatePostCommand command, CancellationToken cancellationToken)
            {
                var post = await _postRepository.GetByIdAsync(command.Id);

                if (post == null)
                {
                    throw new ApiException($"Post Not Found.");
                }
                else
                {
                    //post.Title = command.Title;
                    //post.Body = command.Body;
                    //post.Slug = command.Slug;
                    post.Locales = command.Locales;
                    post.IsArchive = command.IsArchive;
                    post.IsPublic = command.IsPublic;
                    post.Thumbnail = command.Thumbnail;
                    post.CategoryId = command.CategoryId;
                    post.Description = command.Description;
                    //post.Tags = command.Tags;

                    await _postRepository.UpdateAsync(post);
                    return new Response<int>(post.Id);
                }
            }
        }
    }
}
