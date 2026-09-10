using LearnLink.Application.Data;
using LearnLink.Application.Security.Models;
using LearnLink.Application.Security.Providers;
using LearnLink.Application.Security.Validators;
using LearnLink.Application.Shared.Responses;
using LearnLink.Application.Shared.Responses.Extensions;
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
    public async Task<Response<TokenPair>> LoginAsync(LoginRequest loginRequest, CancellationToken cancellationToken = default)
    {
        var responseBuilder = new ResponseBuilder<TokenPair>();
        try
        {
            if(loginRequest == null)
            {
                return responseBuilder
                    .Invalid()
                    .WithMessage("Request is not provided")
                    .Build();
            }

            var validation = new LoginRequestValidator().Validate(loginRequest);

            if(!validation.IsValid)
            {
                return responseBuilder
                    .Invalid()
                    .WithMessage("One or more validation errors occured")
                    .WithDetails(validation.AsErrors())
                    .Build();
            }

            var credentials = await _context
                .Credentials
                .Include(creds => creds.User)
                .ThenInclude(user => user.Role)
                .FirstOrDefaultAsync(creds => creds.User.Nickname == loginRequest.Nickname, cancellationToken);

            if(credentials == null)
            {
                return responseBuilder
                    .NotFound()
                    .WithMessage("User not exists")
                    .Build();

            }

            var verified = _encryptionProvider.Verify(loginRequest.Password, credentials.Password);

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

            await _context.CommitAsync(cancellationToken);

            return responseBuilder
                .Succeed()
                .WithMessage("User logged in successfully")
                .WithContent(tokenPair)
                .Build();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception in AuthenticationService.LoginAsync for Nickname '{Nickname}'", loginRequest.Nickname);

            return responseBuilder
                .Fail()
                .WithMessage("Unknown error occured during login process")
                .Build();
        }
    }
}
