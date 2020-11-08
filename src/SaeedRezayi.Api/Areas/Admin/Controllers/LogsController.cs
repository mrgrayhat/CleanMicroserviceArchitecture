using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SaeedRezayi.Common;
using SaeedRezayi.LogModule.Models;
using SaeedRezayi.LogModule.Services;

namespace SaeedRezayi.Api.Areas.Admin.Controllers
{
    [Area(AreaConstants.AdminArea)]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    [AllowAnonymous] // TODO: fix access
    public class LogsController : ControllerBase
    {
        private ILogService _logService;
        private readonly ILogger<LogsController> _logger;
        private const int DefaultPageSize = 5;


        public LogsController(ILogger<LogsController> logger,
            ILogService logService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(_logger));
            _logService = logService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedLogsListViewModel>> Index(int? page = 1)
        {
            return await GetPagedLogs(page.Value);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<PagedLogsListViewModel>> GetPagedLogs(int page = 1, int maxRecords = 10, string field = "", string order = "Desc", LogLevels logLevel = LogLevels.Information)
        {
            maxRecords = maxRecords > 200 ? 10 : maxRecords;

            PagedLogsListViewModel logs = await _logService
                .GetLogs(page - 1, field, order, maxRecords, logLevel);

            logs.Paging.CurrentPage = page;
            logs.Paging.MaxPagerItems = maxRecords;
            logs.Paging.ItemsPerPage = maxRecords;
            logs.Paging.ShowFirstLast = false;

            //var json = new JsonResult(logs);
            // string json = string.Empty;
            // try
            // {
            //     json = JsonConvert.SerializeObject(logs);
            // }
            // catch (Exception ex)
            // {
            //     return Ok(ex);
            // }

            return Ok(logs);
        }

        // FindLog 5f8819ee2f67b86200b2f2d9
        [HttpGet("{id}")]
        public async Task<ActionResult<LogInfo>> FindLog(string id)
        {
            var result = await _logService.Get(id);
            if (result == null)
            {
                return NotFound(id);
            }
            return Ok(result);
        }

        // Delete 5f8819f22f67b86200b2f2e4
        [HttpDelete("{id}")]
        public async Task<ActionResult<LogInfo>> Delete(string id)
        {
            // id.Length = 24
            var log = await _logService.Get(id);

            if (log == null)
            {
                return NotFound(id);
            }

            var removed = await _logService.Remove(log.Id);
            return Ok(removed);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<PagedLogsListViewModel>> Filter(
            string term, string field = "",
            int? page = 1, int maxRecords = 10,
            string order = "Desc", LogLevels logLevel = LogLevels.Information)
        {
            if (string.IsNullOrEmpty(term))
            {
                term.CheckArgumentIsNull(nameof(term));
            }

            var logs = await _logService
                .FilterLogs(term, field, page.Value - 1, order, maxRecords, logLevel);

            logs.Paging.CurrentPage = page.Value;
            logs.Paging.MaxPagerItems = maxRecords;
            logs.Paging.ItemsPerPage = maxRecords;
            logs.Paging.ShowFirstLast = false;

            return Ok(logs);

        }
    }
}
