using System.Threading;
using System.Threading.Tasks;
using BlogModule.Application.DTOs.Post;
using BlogModule.Application.Interfaces.Repositories;
using FluentValidation;

namespace BlogModule.Application.Features.Posts.Commands.CreatePost
{
    public class PostLocaleValidator : AbstractValidator<PostLocaleDto>
    {
        private readonly IPostRepositoryAsync _postRepository;
        public PostLocaleValidator(IPostRepositoryAsync postRepository)
        {
            this._postRepository = postRepository;

            RuleFor(p => p.Title)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(25).WithMessage("{PropertyName} must not exceed 50 characters.")
                .MustAsync(IsUniqueTitle).WithMessage("{PropertyName} already exists.");

            RuleFor(p => p.Content)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(5000).WithMessage("{PropertyName} must not exceed 50 characters.");
            
            RuleFor(p => p.CultureId)
                .NotNull()
                .GreaterThan(0);
        }

        private async Task<bool> IsUniqueTitle(string title, CancellationToken cancellationToken)
        {
            return await _postRepository.IsUniqueTitleAsync(title);
        }
    }
}
