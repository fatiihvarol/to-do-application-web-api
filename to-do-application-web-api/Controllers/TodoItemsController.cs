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
        [HttpPost("Create")]
        public async Task<IActionResult> CreateTodoItem([FromBody] TodoItemRequest request)
        {
            var response = await _mediator.Send(new CreateTodoItemCommand(request));
            return Ok(response);
        }
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateTodoItem(int id, [FromBody] TodoItemRequest request)
        {
            var response = await _mediator.Send(new UpdateTodoItemCommand(id, request));
            return Ok(response);
        }
        [HttpPut("UpdateStatus/{id}")]
        public async Task<IActionResult> UpdateStatus(int id)
        {
            var response = await _mediator.Send(new UpdateStatusCommand(id));
            return Ok(response);
        }
        [HttpGet("GetMyTodoItems")]
        public async Task<IActionResult> GetMyTodoItems()
        {
            var response = await _mediator.Send(new GetMyTodoItems());
            return Ok(response);
        }

        [HttpGet("GetTodoItemById/{id}")]
        public async Task<IActionResult> GetTodoItemById(int id)
        {
            var response = await _mediator.Send(new GetTodoItemById(id));
            return Ok(response);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteTodoItem(int id)
        {
            var response = await _mediator.Send(new DeletetodoItemCommand(id));
            return Ok(response);
        }
    }
}
