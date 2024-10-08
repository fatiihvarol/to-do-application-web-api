using AutoMapper;
using MediatR;
using to_do_application_web_api.Base.Message;
using to_do_application_web_api.Base.Response;
using to_do_application_web_api.Business.Cqrs;
using to_do_application_web_api.Data.AppDbContext;
using to_do_application_web_api.Data.Entity;
using to_do_application_web_api.Data.Schema;

namespace to_do_application_web_api.Business.Command.TodoItemCommand
{
    public class TodoItemCommandHandler :
        IRequestHandler<CreateTodoItemCommand, ApiResponse<TodoItemResponse>>
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
            mapped.IsCompleted = false;
           

           var fromdb = await _appDbContext.AddAsync(mapped, cancellationToken);
           await _appDbContext.SaveChangesAsync(cancellationToken);

            return ApiResponse<TodoItemResponse>.Success(_mapper.Map<TodoItemResponse>(fromdb.Entity));
        }
    }
}
