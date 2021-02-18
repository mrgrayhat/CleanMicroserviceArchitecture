using System;
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

namespace StorageManagement.Application.Features.Contents.Commands.UpdateContent
{
    public class UpdateContentCommandValidator : AbstractValidator<UpdateContentCommand>
    {
        private static Dictionary<string, string> allowedList = new Dictionary<string, string>();

        private readonly IStorageRepositoryAsync _storageRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UpdateContentCommandValidator> _logger;

        public UpdateContentCommandValidator(ILogger<UpdateContentCommandValidator> logger, IConfiguration configuration, IStorageRepositoryAsync storageRepository)
        {
            _logger = logger;
            _storageRepository = storageRepository;
            _configuration = configuration;

            RuleFor(file => file.VerifiedHash)
                .MustAsync(async (hash, cancellation) =>
                {
                    var isUnique = await IsUniqueFile(hash, cancellation);
                    _logger.LogInformation("the file unique is {isUnique}", isUnique);
                    return isUnique;
                })
                .WithMessage(_configuration["Storage:Messages:DuplicateFile"] + " {PropertyName} are exist");

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
            bool isValid = false;
            long.TryParse(_configuration["Storage:UploadLimitSize"], out long maxSize);

            if (file.Length >= (long)1 && file.Length <= maxSize)
                isValid = true;
            return isValid;
        }
        /// <summary>
        /// validate/filter file extenstion and type, based on <see cref="IConfiguration"/> configured setting, in AllowedFileTypes Section
        /// </summary>
        /// <param name="file"><see cref="IFormFile"/> to validate</param>
        /// <returns>true if valid, otherwise false</returns>
        private bool IsValidType(IFormFile file)
        {
            bool isValid = false;
            var ext = Path.GetExtension(file.FileName);

            if (allowedList.Count < _configuration.GetSection("Storage:AllowedFileTypes").GetChildren().Count())
            {
                var types = _configuration.GetSection("Storage:AllowedFileTypes").GetChildren();
                allowedList = types.ToDictionary(x => x.Key, y => y.Value);
            }
            if (allowedList.Any(item => item.Key.Equals(ext, System.StringComparison.OrdinalIgnoreCase)))
            {
                isValid = true;
            }

            return isValid;
        }
        private async Task<bool> IsUniqueFile(string hash, CancellationToken cancellationToken = default)
        {
            return await _storageRepository.IsUniqueFileHashAsync(hash, cancellationToken);
        }
    }
}