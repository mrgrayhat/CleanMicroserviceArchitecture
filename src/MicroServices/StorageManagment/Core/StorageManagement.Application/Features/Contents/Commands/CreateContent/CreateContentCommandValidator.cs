using FluentValidation;
using StorageManagement.Application.Extensions;
using StorageManagement.Application.Interfaces.Repositories;

namespace StorageManagement.Application.Features.Contents.Commands.CreateContent
{
    public class CreateContentCommandValidator : AbstractValidator<CreateContentCommand>
    {
        private readonly IStorageRepositoryAsync _storageRepository;

        public CreateContentCommandValidator(IStorageRepositoryAsync postRepository)
        {
            this._storageRepository = postRepository;

            //RuleForEach(p => p.Locales).ChildRules(locales =>
            //{
            //    locales.RuleFor(t => t.Title).NotEmpty().WithMessage("{PropertyName} is required.");
            //});

            RuleFor(p => p.File)
                .Must(x => x.IsValid()).WithMessage("Invalid File Type!");
        }
        
        //private async Task<bool> IsUniqueTitle(string title, CancellationToken cancellationToken)
        //{
        //    return await _postRepository.IsUniqueTitleAsync(title);
        //}
    }
}
