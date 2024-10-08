using MediatR;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using to_do_application_web_api.Base.Response;
using to_do_application_web_api.Business.Cqrs;
using to_do_application_web_api.Data.AppDbContext;
using to_do_application_web_api.Data.Entity;
using to_do_application_web_api.Data.Schema.Auth;

namespace to_do_application_web_api.Business.Command.TokenCommand
{
    public class TokenCommandHandler :
        IRequestHandler<CreateTokenCommand, ApiResponse<AuthResponseVM>>,
        IRequestHandler<RefreshTokenCommand, ApiResponse<AuthResponseVM>>
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _dbContext;

        public TokenCommandHandler(IConfiguration configuration, AppDbContext dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }

        public async Task<ApiResponse<AuthResponseVM>> Handle(CreateTokenCommand request, CancellationToken cancellationToken)
        {
            // Validate the request model
            if (string.IsNullOrEmpty(request.Model.Password) || string.IsNullOrEmpty(request.Model.Email))
            {
                return ApiResponse<AuthResponseVM>.Failure(ErrorMessage.TokenErrorMessage.EmptyModelError);
            }

            var hashedPassword = Md5Extension.Create(request.Model.Password);

            // Fetch user from the database
            var user = await _dbContext.VpApplicationUsers
                .FirstOrDefaultAsync(u => u.Email == request.Model.Email && u.Password == hashedPassword);

            // Handle case where user is not found
            if (user == null)
            {
                return ApiResponse<AuthResponseVM>.Failure(ErrorMessage.TokenErrorMessage.UserNotFound);
            }

            // Token creation logic
            var tokenExpirationInMinutes = int.Parse(_configuration["Token:TokenExpirationInMinutes"]!);
            Claim[] claims = GetClaims(user);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:Secret"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Token:Issuer"],
                audience: _configuration["Token:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(tokenExpirationInMinutes),
                signingCredentials: creds
            );
     

            // Prepare the response model
            var response = new AuthResponseVM
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                ExpiresAt = token.ValidTo
            };

            // Return a successful response
            return ApiResponse<AuthResponseVM>.Success(response);
        }

        private Claim[] GetClaims(VpApplicationUser user)
        {
            return new[]
            {
                new Claim("Id", user.Id.ToString()),
                new Claim("Email", user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

    }
}
