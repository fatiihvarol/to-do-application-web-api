using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using to_do_application_web_api.Base.Response;
using to_do_application_web_api.Data.Schema.Auth;

namespace to_do_application_web_api.Business.Cqrs
{
    public record CreateTokenCommand(LoginRequest Model) : IRequest<ApiResponse<AuthResponseVM>>;

}
