using MediatR;
using Microsoft.OpenApi.Any;
using to_do_application_web_api.Base.Response;
using to_do_application_web_api.Data.Schema;
using to_do_application_web_api.Data.Schema.Auth;

namespace to_do_application_web_api.Business.Cqrs
{
    public record CreateTodoItemCommand(TodoItemRequest Model) : IRequest<ApiResponse<TodoItemResponse>>;
    public record UpdateTodoItemCommand(int Id ,TodoItemRequest Model) : IRequest<ApiResponse<TodoItemResponse>>;
    public record UpdateStatusCommand(int Id ) : IRequest<ApiResponse<string>>;
    public record DeletetodoItemCommand(int Id ) : IRequest<ApiResponse<string>>;



    public record GetMyTodoItems() : IRequest<ApiResponse<List<TodoItemResponse>>>;
    public record GetTodoItemById(int Id) : IRequest<ApiResponse<TodoItemResponse>>;



}
