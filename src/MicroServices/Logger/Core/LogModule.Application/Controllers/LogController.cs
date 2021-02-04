using System.Threading.Tasks;
using LogModule.Application.Features.Logs.Queries.GetAllLogs;
using LogModule.Application.Features.Logs.Queries.GetLogById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LogModule.Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogController : ControllerBase
    {
        private readonly ILogger<LogController> _logger;
        private IMediator _mediator;
        public LogController(ILogger<LogController> logger, IMediator mediator)
        {
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
            _mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        // GET: api/<controller>
        /// <summary>
        /// Get paged list of log's, based on filters.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetAllLogsParameter filter)
        {
            _logger.LogInformation($"Log Module Built-in Controller Get Logs action executed!");
            return Ok(await _mediator.Send(new GetAllLogsQuery()
            {
                PageSize = filter.PageSize,
                PageNumber = filter.PageNumber,
                LogLevel = filter.LogLevel,
                IP = HttpContext.Connection.RemoteIpAddress.ToString()
            }));
        }

        // GET api/<controller>/abcd1234efgh5678
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            _logger.LogInformation($"Log Module Built-in Controller Get Log with id {id} action executed!");
            return Ok(await _mediator.Send(new GetLogByIdQuery { Id = id }));
        }

        // Delete api/<controller>/abcd1234efgh5678
        [HttpDelete("{id}")]
        [HttpPost("{id}")]
        public async Task<IActionResult> Post(string id)
        {
            _logger.LogInformation($"Log Controller Delete Log with id {id} action executed!");
            return Ok(await _mediator.Send(new GetLogByIdQuery { Id = id }));
        }
    }
}