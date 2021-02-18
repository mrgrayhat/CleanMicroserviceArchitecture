using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StorageManagement.Application.Interfaces.Repositories;

namespace StorageManagement.Application.Features.Contents.Commands.CreateContent
{
    public class CreateContentCommandValidator : AbstractValidator<CreateContentCommand>
    {
        private static Dictionary<string, string> allowedList = new Dictionary<string, string>();

        private readonly IStorageRepositoryAsync _storageRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CreateContentCommandValidator> _logger;

        public CreateContentCommandValidator(ILogger<CreateContentCommandValidator> logger, IConfiguration configuration, IStorageRepositoryAsync storageRepository)
        {
            _logger = logger;
            _storageRepository = storageRepository;
            _configuration = configuration;

            //RuleForEach(p => p.File).ChildRules(files =>
            //{

            //});

            RuleFor(file => file.File)
            .NotNull()
            .WithMessage("{PropertyName} is required.");

            RuleFor(file => file.Name)
            .MaximumLength(50)
            .WithMessage("No More than 50 character are allowed in {PropertyName}");

            RuleFor(file => file.Tags)
            .MaximumLength(70)
            .WithMessage("No More than 70 character are allowed in {PropertyName}");

            RuleFor(file => file.Description)
            .MaximumLength(500)
            .WithMessage("No More than 50 character are allowed in {PropertyName}");

            RuleFor(file => file.File)
                .NotNull()
                .WithMessage(_configuration["Storage:Messages:EmptyFile"]);

            RuleFor(file => file.File)
                .Must(file => IsValidSize(file))
                .WithMessage(_configuration["Storage:Messages:InvalidFileSize"]);

            RuleFor(file => file.File)
                .Must(type => IsValidType(type))
                .WithMessage(_configuration["Storage:Messages:InvalidFileType"]);
        }
        private bool IsValidSize(IFormFile file)
        {
            long.TryParse(_configuration["Storage:UploadLimitSize"], out long maxSize);
            long minSize = 1;
            if (file.Length < minSize || file.Length > maxSize)
                return false;
            // else
            return true;
        }
        /// <summary>
        /// validate/filter file extenstion and type, based on <see cref="IConfiguration"/> configured setting, in AllowedFileTypes Section
        /// </summary>
        /// <param name="file"><see cref="IFormFile"/> to validate</param>
        /// <returns>true if valid, otherwise false</returns>
        private bool IsValidType(IFormFile file)
        {
            //var ext = file.GetFileExtension();
            var ext = Path.GetExtension(file.FileName);

            if (allowedList.Count < _configuration.GetSection("Storage:AllowedFileTypes").GetChildren().Count())
            {
                var types = _configuration.GetSection("Storage:AllowedFileTypes").GetChildren();
                allowedList = types.ToDictionary(x => x.Key, y => y.Value);
            }
            if (allowedList.Any(item => item.Key.Equals(ext, System.StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }

            return false;
        }
        private async Task<bool> IsUniqueFile(string hash, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _storageRepository.IsUniqueFileHashAsync(hash, cancellationToken);
        }
    }
}
