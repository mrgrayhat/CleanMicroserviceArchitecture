using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using StorageManagement.Application.Wrappers;

namespace StorageManagement.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    public class StorageController : BaseApiController
    {
        private readonly ILogger<StorageController> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;

        public StorageController(ILogger<StorageController> logger, IWebHostEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _logger = logger ?? throw new
                ArgumentNullException($"nameof{(logger)} can't be null.");
            _hostingEnvironment = hostingEnvironment ?? throw new
                ArgumentNullException($"nameof{(hostingEnvironment)} can't be null.");
            _configuration = configuration ?? throw new
                ArgumentNullException($"nameof{(configuration)} can't be null."); ;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(List<IFormFile> files)
        {
            if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                ModelState.AddModelError("File",
                    $"The request couldn't be processed (Error 1).");
                // Log error
                return BadRequest(ModelState);
            }
            long.TryParse(_configuration.GetSection("Storage:UploadLimitSize").Value, out long maxSize);

            long fileSize = files.Sum(f => f.Length);

            foreach (var formFile in files)
            {
                if (formFile.Length > 0 && formFile.Length <= maxSize)
                {
                    if (formFile.ValidateFileExtension(_configuration))
                    {
                        var filePath = Path.Combine(_hostingEnvironment.WebRootPath, _configuration.GetSection("Storage:StoragePath").Value, Path.GetRandomFileName()
                            .Split('.')[0] + "." + formFile.FileName.Split('.')[1]);
                        using var stream = System.IO.File.Create(filePath);
                        await formFile.CopyToAsync(stream);
                    }
                    else
                    {
                        return BadRequest($"{_configuration.GetSection("Storage:Messages:InvalidFileType").Value} for {formFile.FileName}");
                    }
                }
                else
                {
                    return BadRequest($"{_configuration.GetSection("Storage:Messages:InvalidFileSize").Value} for {formFile.FileName}");
                }
            }

            return Ok(new { count = files.Count, fileSize });
        }

        [HttpPost]
        public async Task<FileContentResult> Download(int id)
        {
            var path = Path.Combine(_hostingEnvironment.WebRootPath, "Files", "1.jpg");
            var f = await System.IO.File.ReadAllBytesAsync(path);
            return File(f, "image/jpeg");
        }

        // GET: api/v1/<controller>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllContentsParameter filter = null)
        {
            return Ok(await Mediator.Send(new GetAllContentsQuery()
            {
                PageSize = filter.PageSize <= 0 ? 10 : filter.PageSize,
                PageNumber = filter.PageNumber <= 0 ? 1 : filter.PageNumber
            }));
        }

        // GET api/v1/<controller>/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Response<ItemDto>>> GetById(int id)
        {
            _logger.LogInformation($"Storage Management MicroService Built-in Controller, {nameof(GetById)} action with input: {id} executed!");
            return Ok(await Mediator.Send(new GetContentByIdQuery { Id = id }));
        }

        // GET api/v1/<controller>/search/text
        [HttpGet("search/{text}")]
        public async Task<ActionResult<Response<ItemDto>>> Search(string text,
            string sortOrder = "Desc")
        {
            _logger.LogInformation($"Storage MicroService Built-in Controller, {nameof(Search)} action with filter: {text} executed!");
            return Ok(await Mediator.Send(new SearchContentsQuery
            {
                Text = text,
                SortOrder = sortOrder == "Desc" ? sortOrder : "Asc"
            }));
        }

        // Delete api/v1/<controller>/1
        [HttpDelete("{id}")]
        //[Authorize(Roles = "SuperAdmin,Writer")]
        public async Task<ActionResult<Response<int>>> Delete(int id)
        {
            _logger.LogInformation($"Storage MicroService Delete Post with id {id} action executed!");
            return Ok(await Mediator.Send(new DeleteContentByIdCommand
            {
                Id = id
            }));
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

        //// Post api/v1/<controller>/1
        //[HttpPost]
        ////[Authorize(Roles = "SuperAdmin,Writer")]
        //public async Task<ActionResult<Response<int>>> Post(CreateContentCommand command)
        //{
        //    return Ok(await Mediator.Send(command));
        //}

    }
}
