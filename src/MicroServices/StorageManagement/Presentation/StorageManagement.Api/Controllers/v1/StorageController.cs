using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StorageManagement.Application.DTOs;
using StorageManagement.Application.Features.Contents.Commands.CreateContent;
using StorageManagement.Application.Features.Contents.Commands.DeleteContent;
using StorageManagement.Application.Features.Contents.Commands.UpdateContent;
using StorageManagement.Application.Features.Contents.Queries.GetAllContents;
using StorageManagement.Application.Features.Contents.Queries.GetContenById;
using StorageManagement.Application.Features.Contents.Queries.SearchContents;
using StorageManagement.Application.Helpers;
using StorageManagement.Application.Interfaces;
using StorageManagement.Application.Wrappers;

namespace StorageManagement.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    public class StorageController : BaseApiController
    {
        private readonly ILogger<StorageController> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IStorageFileSystemProvider _storageFileSystemProvider;
        private readonly IConfiguration _configuration;

        public StorageController(ILogger<StorageController> logger, IWebHostEnvironment hostingEnvironment, IConfiguration configuration, IStorageFileSystemProvider storageFileSystemProvider)
        {
            _logger = logger ?? throw new
                ArgumentNullException($"nameof{(logger)} can't be null.");
            _hostingEnvironment = hostingEnvironment ?? throw new
                ArgumentNullException($"nameof{(hostingEnvironment)} can't be null.");
            _configuration = configuration ?? throw new
                ArgumentNullException($"nameof{(configuration)} can't be null.");
            _storageFileSystemProvider = storageFileSystemProvider ?? throw new
                ArgumentNullException($"nameof{(configuration)} can't be null."); ;
        }

        [HttpPost("multipleUpload")]
        [RequestFormLimits(MultipartBodyLengthLimit = 209_715_200)]
        [RequestSizeLimit(209_715_200)]
        public async Task<IActionResult> Upload([FromForm] IEnumerable<IFormFile> files)
        {
            if (files.GetEnumerator().MoveNext() is false)
                return BadRequest(_configuration["Storage:Messages:EmptyFile"]);
            files.GetEnumerator().Reset();
            var data = new List<CreateContentCommand>();

            foreach (var item in files)
            {
                data.Add(new CreateContentCommand
                {
                    File = item,
                    Name = item.FileName,
                });
            }
            return Ok(await UploadBase(data));
        }

        [HttpPost("upload")]
        [RequestFormLimits(MultipartBodyLengthLimit = 209_715_200)]
        [RequestSizeLimit(209_715_200)]
        public async Task<ActionResult<Response<ItemDto>>> Upload([FromForm] CreateContentCommand createContentCommand)
        {
            if (createContentCommand.File is null)
                return BadRequest(_configuration["Storage:Messages:EmptyFile"]);
            return Ok(await UploadBase(new List<CreateContentCommand> { createContentCommand }));
        }

        private async Task<IEnumerable<Response<ItemDto>>> UploadBase(IEnumerable<CreateContentCommand> createContentCommands)
        {
            var response = new List<Response<ItemDto>>();
            //Response<ItemDto> response = new Response<ItemDto>();
            if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                response[0].Errors.Add("The request couldn't be processed (Error 1).");
                response[0].Succeeded = false;
                response[0].Message = "Invalid Content type. Could'nt find multipart data.";
            }
            else
            {

                foreach (var item in createContentCommands)
                {
                    Response<ItemDto> data = await Mediator.Send(item);
                    response.Add(data);
                }
            }
            return response;
        }

        [HttpGet("download/{id}")]
        public async Task<IActionResult> Download(int id, CancellationToken cancellationToken)
        {
            Response<ItemDto> itemDto = await Mediator.Send(new GetContentByIdQuery { Id = id }, cancellationToken);
            if (itemDto is null)
            {
                return NotFound($"{_configuration["Storage:Messages:NotFound"]} with id {id}");
            }
            Stream stream = await _storageFileSystemProvider
                .RetriveAsync(itemDto.Data.Url, cancellationToken);

            if (stream is null)
                return NotFound($"{_configuration["Storage:Messages:NotFound"]} with id {id}");

            _logger.LogInformation("[Download]: Sending File ({fileUrl}) with size {fileSize} To Client ...", itemDto.Data.Url, itemDto.Data.Size);
            return File(stream, "application/octet-stream");
        }
        [HttpGet("downloadFileStream")]
        public async Task<FileStreamResult> DownloadFileStream(string id)
        {
            string filePath = Path.Combine(_hostingEnvironment.ContentRootPath, _configuration["Storage:StoragePath"], "1.jpg");
            Stream stream = await _storageFileSystemProvider.RetriveAsync(filePath, CancellationToken.None);
            string mimeType = "application/octet-stream";

            _logger.LogInformation("[DownloadFileStream]: Sending File ({filePath}) To Client ...", filePath);

            return new FileStreamResult(stream, mimeType)
            {
                FileDownloadName = "downloadedFile.jpg"
            };
        }

        // GET: api/v1/<controller>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllContentsParameter filter = null)
        {
            return Ok(await Mediator.Send(new GetAllContentsQuery()
            {
                PageSize = filter.PageSize <= 0 ? 10 : filter.PageSize,
                PageNumber = filter.PageNumber <= 0 ? 1 : filter.PageNumber,
                SortOrder = filter.SortOrder
            }));
        }

        // GET api/v1/<controller>/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Response<ItemDto>>> GetById(int id)
        {
            return Ok(await Mediator.Send(new GetContentByIdQuery { Id = id }));
        }

        // GET api/v1/<controller>/search/text
        [HttpGet("search/{text}")]
        public async Task<ActionResult<Response<ItemDto>>> Search([FromQuery] SearchContentsParameter filter = null)
        {
            return Ok(await Mediator.Send(new SearchContentsParameter
            {
                Text = filter.Text,
                PageSize = filter.PageSize <= 0 ? 10 : filter.PageSize,
                PageNumber = filter.PageNumber <= 0 ? 1 : filter.PageNumber,
                SortOrder = filter.SortOrder
            }));
        }

        // Delete api/v1/<controller>/1
        [HttpDelete("{id}")]
        [Consumes("application/json")]
        //[Authorize(Roles = "SuperAdmin,Writer")]
        public async Task<ActionResult<Response<int>>> Delete(int id)
        {
            return await Mediator.Send(new DeleteContentByIdCommand
            {
                Id = id
            });
        }

        // PUT api/v1/<controller>/1
        [HttpPut("{id}")]
        //[Authorize(Roles = "SuperAdmin,Writer")]
        public async Task<ActionResult<Response<int>>> Put(int id, UpdateContentCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        // Post api/v1/<controller>/1
        [HttpPost]
        //[Authorize(Roles = "SuperAdmin,Writer")]
        public async Task<ActionResult<Response<int>>> Post(CreateContentCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

    }
}
