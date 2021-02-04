using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Logger.API.Controllers
{
    /// <summary>
    /// using for controller's need versioning
    /// </summary>
    [ApiController]
    [Route("api/v{version}/[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        private IMediator _mediator;
        protected IMediator Mediator
        {
            get
            {
                return _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
            }
        }
    }


    /// <summary>
    /// for controller who don't need versioning
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseApiControllerNoVersioning : ControllerBase
    {
        private IMediator _mediator;
        protected IMediator Mediator
        {
            get
            {
                return _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
            }
        }
    }
}
