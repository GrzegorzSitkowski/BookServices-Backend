using BookServices.Application.Logic.Services;
using BookServices.Infrastructure.Auth;
using BookServices.WebApi.Application.Auth;
using BookServices.WebApi.Application.Response;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BookServices.WebApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class WorkersController : BaseController
    {
        public WorkersController(ILogger<WorkersController> logger,
            IMediator mediator) : base(logger, mediator)
        {
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrUpdate([FromBody] CreateOrUpdateCommand.Request model)
        {
            var data = await _mediator.Send(model);
            return Ok(data);
        }

        [HttpPost]
        public async Task<ActionResult> Delete([FromBody] DeleteCommand.Request model)
        {
            var data = await _mediator.Send(model);
            return Ok(data);
        }

        [HttpGet]
        public async Task<ActionResult> GetWorker([FromQuery] GetQuery.Request model)
        {
            var data = await _mediator.Send(model);
            return Ok(data);
        }

        [HttpGet]
        public async Task<ActionResult> List([FromQuery] ListQuery.Request model)
        {
            var data = await _mediator.Send(model);
            return Ok(data);
        }

    }
}
