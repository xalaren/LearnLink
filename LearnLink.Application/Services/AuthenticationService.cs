using Ardalis.Result;
using LearnLink.Application.Data;
using LearnLink.Application.Security;
using LearnLink.Domain.Entities.Users.Models;
using LearnLink.Shared.Model.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LearnLink.Application.Services;

public class AuthenticationService(IApplicationDataContext context, IEncryptionProvider encryptionProvider, ITokenProvider tokenProvider, ILogger<AuthenticationService> logger)
{
    private readonly IApplicationDataContext _context = context;
    private readonly IEncryptionProvider _encryptionProvider = encryptionProvider;
    private readonly ITokenProvider _tokenProvider = tokenProvider;
    private readonly ILogger<AuthenticationService> _logger = logger;
    public async Task<Result<TokenPair>> LoginAsync(string nickname, string password)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return Result.Invalid(new ValidationError(nameof(nickname), "Nickname is required"));
            }

            if (string.IsNullOrWhiteSpace(nickname))
            {
                return Result.Invalid(new ValidationError(nameof(password), "Password is required"));
            }

            var credentials = await _context
                .Credentials
                .Include(creds => creds.User)
                .ThenInclude(user => user.Role)
                .FirstOrDefaultAsync(creds => creds.User.Nickname == nickname);

            if(credentials == null)
            {
                return Result.NotFound($"Nickname or password was incorrect");
            }

            var verified = _encryptionProvider.Verify(password, credentials.Password);

            if(!verified)
            {
                return Result.Forbidden("Nickname or password was incorrect");
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

            return Result.Success(tokenPair, "User logged in successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception in AuthenticationService.LoginAsync for Nickname '{Nickname}'", nickname);
            return Result.Error(ex.Message);
        }
    }
}
