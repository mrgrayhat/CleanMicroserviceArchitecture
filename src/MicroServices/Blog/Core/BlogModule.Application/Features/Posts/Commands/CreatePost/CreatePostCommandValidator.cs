using BlogModule.Application.Interfaces.Repositories;
using FluentValidation;

namespace BlogModule.Application.Features.Posts.Commands.CreatePost
{
    public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
    {
        private readonly IPostRepositoryAsync _postRepository;

        public CreatePostCommandValidator(IPostRepositoryAsync postRepository)
        {
            this._postRepository = postRepository;

            //RuleForEach(p => p.Locales).ChildRules(locales =>
            //{
            //    locales.RuleFor(t => t.Title).NotEmpty().WithMessage("{PropertyName} is required.");
            //});

            RuleFor(p => p.Locales)
                .Must(x => x.Count > 0).WithMessage("at least 1 language specific data is required.");

            RuleForEach(p => p.Locales)
                .SetValidator(new PostLocaleValidator(_postRepository));
        }

        //private async Task<bool> IsUniqueTitle(string title, CancellationToken cancellationToken)
        //{
        //    return await _postRepository.IsUniqueTitleAsync(title);
        //}
    }
}
