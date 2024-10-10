using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using to_do_application_web_api.Base.Message;
using to_do_application_web_api.Base.Response;
using to_do_application_web_api.Business.Cqrs;
using to_do_application_web_api.Data.AppDbContext;
using to_do_application_web_api.Data.Entity;
using to_do_application_web_api.Data.Schema;

namespace to_do_application_web_api.Business.Command.TodoItemCommand
{
    public class TodoItemCommandHandler :
        IRequestHandler<CreateTodoItemCommand, ApiResponse<TodoItemResponse>>,
        IRequestHandler<UpdateTodoItemCommand, ApiResponse<TodoItemResponse>>,
        IRequestHandler<UpdateStatusCommand, ApiResponse<string>>,
        IRequestHandler<DeletetodoItemCommand, ApiResponse<string>>
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public TodoItemCommandHandler(AppDbContext appDbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        private string? GetUserId()
        {
            return _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
        }
        public async Task<ApiResponse<TodoItemResponse>> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
        {

            if (request.Model == null)
            {
                return ApiResponse<TodoItemResponse>.Failure(ErrorMessage.TokenErrorMessage.EmptyModelError);
            }
            var userId = GetUserId();

            if (userId == null)
            {
                return ApiResponse<TodoItemResponse>.Failure(ErrorMessage.TokenErrorMessage.UserIdNotFound);
            }

            _appDbContext.VpUsers.FirstOrDefault(u => u.Id == Int32.Parse(userId));

            if (userId == null)
            {
                return ApiResponse<TodoItemResponse>.Failure(ErrorMessage.TokenErrorMessage.UserNotFound);
            }


            var mapped =_mapper.Map<TodoItemRequest, VpTodoItem>(request.Model);
            mapped.UserId = Int32.Parse(userId);
            mapped.CreatedDate = DateTime.UtcNow;           

           var fromdb = await _appDbContext.AddAsync(mapped, cancellationToken);
           await _appDbContext.SaveChangesAsync(cancellationToken);

            return ApiResponse<TodoItemResponse>.Success(_mapper.Map<TodoItemResponse>(fromdb.Entity));
        }

        public async Task<ApiResponse<TodoItemResponse>> Handle(UpdateTodoItemCommand request, CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return ApiResponse<TodoItemResponse>.Failure(ErrorMessage.TokenErrorMessage.UserIdNotFound);
            }

            if (!int.TryParse(userId, out int parsedUserId))
            {
                return ApiResponse<TodoItemResponse>.Failure(ErrorMessage.TokenErrorMessage.UserIdNotFound);
            }

            var todoItem = await _appDbContext.VpTodoItems
                .FirstOrDefaultAsync(t => t.Id == request.Id && t.UserId == parsedUserId, cancellationToken);

            if (todoItem == null)
            {
                return ApiResponse<TodoItemResponse>.Failure(ErrorMessage.TodoItemErrorMessage.TodoItemNotFound);
            }

            
            _mapper.Map(request.Model, todoItem);  
            todoItem.ModifiedBy = userId;
            todoItem.ModifiedDate = DateTime.UtcNow;

            _appDbContext.VpTodoItems.Update(todoItem);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<TodoItemResponse>(todoItem);
            return ApiResponse<TodoItemResponse>.Success(response);
        }

        public async Task<ApiResponse<string>> Handle(UpdateStatusCommand request, CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return ApiResponse<string>.Failure(ErrorMessage.TokenErrorMessage.UserIdNotFound);
            }

            if (!int.TryParse(userId, out int parsedUserId))
            {
                return ApiResponse<string>.Failure(ErrorMessage.TokenErrorMessage.UserIdNotFound);
            }

            var todoItem = await _appDbContext.VpTodoItems
                .FirstOrDefaultAsync(t => t.Id == request.Id && t.UserId == parsedUserId, cancellationToken);

            if (todoItem == null)
            {
                return ApiResponse<string>.Failure(ErrorMessage.TodoItemErrorMessage.TodoItemNotFound);
            }
            if(todoItem.UserId != Int32.Parse(userId))
            {
                return ApiResponse<string>.Failure(ErrorMessage.TodoItemErrorMessage.NotAuthorized);

            }

            todoItem.IsCompleted = !todoItem.IsCompleted;
            todoItem.ModifiedBy = userId;
            todoItem.ModifiedDate = DateTime.UtcNow;

            _appDbContext.VpTodoItems.Update(todoItem);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return ApiResponse<string>.Success(SuccesMessage.ItemSuccesMessage.UpdatedSuccesfuly);
        }

        public async Task<ApiResponse<string>> Handle(DeletetodoItemCommand request, CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return ApiResponse<string>.Failure(ErrorMessage.TokenErrorMessage.UserIdNotFound);
            }

            if (!int.TryParse(userId, out int parsedUserId))
            {
                return ApiResponse<string>.Failure(ErrorMessage.TokenErrorMessage.UserIdNotFound);
            }

            var todoItem = await _appDbContext.VpTodoItems
                .FirstOrDefaultAsync(t => t.Id == request.Id && t.UserId == parsedUserId, cancellationToken);

            if (todoItem == null)
            {
                return ApiResponse<string>.Failure(ErrorMessage.TodoItemErrorMessage.TodoItemNotFound);
            }
            if (todoItem.UserId != Int32.Parse(userId))
            {
                return ApiResponse<string>.Failure(ErrorMessage.TodoItemErrorMessage.NotAuthorized);

            }

            _appDbContext.VpTodoItems.Remove(todoItem);
            await _appDbContext.SaveChangesAsync(cancellationToken);
            return ApiResponse<string>.Success(SuccesMessage.ItemSuccesMessage.DeletedSuccesfuly);
        }
    }
}
