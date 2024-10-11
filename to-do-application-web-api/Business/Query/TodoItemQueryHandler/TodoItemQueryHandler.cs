using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using to_do_application_web_api.Base.Message;
using to_do_application_web_api.Base.Response;
using to_do_application_web_api.Business.Cqrs;
using to_do_application_web_api.Data.AppDbContext;
using to_do_application_web_api.Data.Entity;
using to_do_application_web_api.Data.Schema;

namespace to_do_application_web_api.Business.Query.TodoItemQueryHandler
{
    public class TodoItemQueryHandler :
        IRequestHandler<GetMyTodoItems, ApiResponse<List<TodoItemResponse>>>,
        IRequestHandler<GetTodoItemById, ApiResponse<TodoItemResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _appDbContext;

        public TodoItemQueryHandler(IMapper mapper, IHttpContextAccessor httpContextAccessor, AppDbContext appDbContext)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _appDbContext = appDbContext;
        }
        private string? GetUserId()
        {
            return _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
        }
        public async Task<ApiResponse<List<TodoItemResponse>>> Handle(GetMyTodoItems request, CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return ApiResponse<List<TodoItemResponse>>.Failure(ErrorMessage.TokenErrorMessage.UserIdNotFound);
            }

            var todoItems = await _appDbContext.VpTodoItems
                .Where(t => t.UserId == Int32.Parse(userId))
                .OrderByDescending(t => t.Priority)
                .ToListAsync(cancellationToken);

            var mapped = _mapper.Map<List<VpTodoItem>, List<TodoItemResponse>>(todoItems);

            return ApiResponse<List<TodoItemResponse>>.Success(mapped);
        }

        public async Task<ApiResponse<TodoItemResponse>> Handle(GetTodoItemById request, CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return ApiResponse<TodoItemResponse>.Failure(ErrorMessage.TokenErrorMessage.UserIdNotFound);
            }

            var todoItem = await _appDbContext.VpTodoItems
                .FirstOrDefaultAsync(t => t.UserId == Int32.Parse(userId) && t.Id == request.Id, cancellationToken);

            if (todoItem == null)
                {
                return ApiResponse<TodoItemResponse>.Failure(ErrorMessage.TodoItemErrorMessage.TodoItemNotFound);
            }

            var mapped = _mapper.Map<VpTodoItem, TodoItemResponse>(todoItem);

            return ApiResponse<TodoItemResponse>.Success(mapped);
        }
    }
}
