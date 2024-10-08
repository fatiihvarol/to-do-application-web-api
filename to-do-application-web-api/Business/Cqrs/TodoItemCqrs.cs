using MediatR;
using to_do_application_web_api.Base.Response;
using to_do_application_web_api.Data.Schema;
using to_do_application_web_api.Data.Schema.Auth;

namespace to_do_application_web_api.Business.Cqrs
{
    public record CreateTodoItemCommand(TodoItemRequest Model) : IRequest<ApiResponse<TodoItemResponse>>;



}
