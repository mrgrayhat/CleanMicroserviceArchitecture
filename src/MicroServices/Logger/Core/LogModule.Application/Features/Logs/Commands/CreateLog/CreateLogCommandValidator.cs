using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using LogModule.Application.Interfaces.Repositories;

namespace LogModule.Application.Features.Logs.Commands.CreateLog
{
    public class CreateLogCommandValidator : AbstractValidator<CreateLogCommand>
    {
        private readonly ILogRepositoryAsync LogRepository;

        public CreateLogCommandValidator(ILogRepositoryAsync LogRepository)
        {
            this.LogRepository = LogRepository;

            RuleFor(p => p.Guid)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.")
                .MustAsync(IsUniqueBarcode).WithMessage("{PropertyName} already exists.");

            RuleFor(p => p.Description)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");
        }

        private async Task<bool> IsUniqueBarcode(string barcode, CancellationToken cancellationToken)
        {
            return await LogRepository.IsUniqueBarcodeAsync(barcode);
        }
    }
}
