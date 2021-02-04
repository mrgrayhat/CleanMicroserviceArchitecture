using System.Threading;
using System.Threading.Tasks;
using BlogModule.Application.Exceptions;
using BlogModule.Application.Interfaces.Repositories;
using BlogModule.Application.Wrappers;
using BlogModule.Domain.Entities;
using MediatR;

namespace BlogModule.Application.Features.Posts.Commands.DeletePostById
{
    public class DeletePostByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeletePostByIdCommandHandler : IRequestHandler<DeletePostByIdCommand, Response<int>>
        {
            private readonly IPostRepositoryAsync _postRepository;
            public DeletePostByIdCommandHandler(IPostRepositoryAsync postRepository)
            {
                _postRepository = postRepository;
            }
            public async Task<Response<int>> Handle(DeletePostByIdCommand command, CancellationToken cancellationToken)
            {
                Post post = await _postRepository.GetByIdAsync(command.Id);
                if (post == null) throw new ApiException($"Log Not Found.");
                await _postRepository.DeleteAsync(post);
                return new Response<int>(post.Id);
            }
        }
    }
}
