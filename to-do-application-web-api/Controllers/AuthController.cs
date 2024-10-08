using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using to_do_application_web_api.Business.Cqrs;
using to_do_application_web_api.Data.Schema.Auth;

namespace to_do_application_web_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginVM model)
        {
            var response = await _mediator.Send(new CreateTokenCommand(model));
            return Ok(response);
        }
    }
}
