using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using to_do_application_web_api.Business.Cqrs;
using to_do_application_web_api.Data.Schema;

namespace to_do_application_web_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TodoItemsController(IMediator mediator)
        {
            
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> CreateTodoItem([FromBody] TodoItemRequest request)
        {
            var response = await _mediator.Send(new CreateTodoItemCommand(request));
            return Ok(response);
        }
    }
}
