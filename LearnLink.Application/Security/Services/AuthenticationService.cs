using LearnLink.Application.Data;
using LearnLink.Application.Security.Models;
using LearnLink.Application.Security.Providers;
using LearnLink.Application.Shared.Responses;
using LearnLink.Domain.Entities.Users.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LearnLink.Application.Security.Services;

public class AuthenticationService(IApplicationDataContext context, IEncryptionProvider encryptionProvider, ITokenProvider tokenProvider, ILogger<AuthenticationService> logger)
{
    private readonly IApplicationDataContext _context = context;
    private readonly IEncryptionProvider _encryptionProvider = encryptionProvider;
    private readonly ITokenProvider _tokenProvider = tokenProvider;
    private readonly ILogger<AuthenticationService> _logger = logger;
    public async Task<Response<TokenPair>> LoginAsync(string nickname, string password)
    {
        var responseBuilder = new ResponseBuilder<TokenPair>();
        try
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return responseBuilder
                    .Invalid()
                    .WithMessage("Nickname is required")
                    .Build();
            }

            if (string.IsNullOrWhiteSpace(nickname))
            {
                return responseBuilder
                    .Invalid()
                    .WithMessage("Password is required")
                    .Build();
            }

            var credentials = await _context
                .Credentials
                .Include(creds => creds.User)
                .ThenInclude(user => user.Role)
                .FirstOrDefaultAsync(creds => creds.User.Nickname == nickname);

            if(credentials == null)
            {
                return responseBuilder
                    .NotFound()
                    .WithMessage("User not exists")
                    .Build();

            }

            var verified = _encryptionProvider.Verify(password, credentials.Password);

            if(!verified)
            {
                return responseBuilder
                    .Forbid()
                    .WithMessage("User nickname or password are incorrect")
                    .Build();
            }

            var user = credentials.User;
            var tokenPair = new TokenPair
            (
                AccessToken: _tokenProvider.GenerateAccessToken(user),
                RefreshToken: _tokenProvider.GenerateRefreshToken()
            );

            _context
                .RefreshTokens
                .Add(RefreshToken.Create(user.Id, tokenPair.RefreshToken));

            await _context.CommitAsync();

            return responseBuilder
                .Succeed()
                .WithMessage("User logged in successfully")
                .WithContent(tokenPair)
                .Build();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception in AuthenticationService.LoginAsync for Nickname '{Nickname}'", nickname);

            return responseBuilder
                .Fail()
                .WithMessage("Unknown error occured during login")
                .Build();
        }
    }
}
