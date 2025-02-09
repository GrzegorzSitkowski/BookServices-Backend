using BookServices.Application.Logic.User;
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
    public class AccountsController : BaseController
    {
        public AccountsController(ILogger<AccountsController> logger,
            IMediator mediator) : base(logger, mediator)
        {
        }

        [HttpGet]
        public async Task<ActionResult> GetCurrentAccount()
        {
            var data = await _mediator.Send(new CurrentAccountQuery.Request() { });
            return Ok(data);
        }

    }
}
