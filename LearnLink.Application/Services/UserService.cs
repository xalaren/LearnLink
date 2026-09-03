using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using LearnLink.Application.Repositories;
using LearnLink.Application.Security;
using LearnLink.Application.Transactions;
using LearnLink.Application.Validators.Users;
using LearnLink.Domain.Entities.Users.Models;
using LearnLink.Shared.Users;
using Microsoft.Extensions.Logging;

namespace LearnLink.Application.Services;

public class UserService(IUserRepository userRepository, ICredentialsRepository credentialsRepository, IUnitOfWork unitOfWork, IEncryptionService encryptionService, ILogger<UserService> logger)
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ICredentialsRepository _credentialsRepository = credentialsRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IEncryptionService _encryptionService = encryptionService;
    private readonly ILogger<UserService> _logger = logger;

    public async Task<Result> RegisterAsync(RegisterRequest request, string plainPassword)
    {
        try
        {
            if(string.IsNullOrWhiteSpace(plainPassword))
            {
                return Result.Invalid(new ValidationError("Password is required"));
            }

            var registerRequestValidator = new RegisterRequestValidator();
            var validationResult = registerRequestValidator.Validate(request);

            if(!validationResult.IsValid)
            {
                return Result.Invalid(validationResult.AsErrors());
            }

            var exists = await _userRepository.ExistsAsync(user => user.Nickname == request.Nickname);
            if (exists) return Result.Invalid(new ValidationError($"User with nickname '{request.Nickname}' already exists"));

            var user = User.Create(request.Nickname, request.Name, request.Lastname);

            var password = _encryptionService.Encrypt(plainPassword);
            var credentials = Credentials.Create(password, user.Id, false, request.PasswordExpiration);

            _userRepository.Add(user);
            _credentialsRepository.Add(credentials);

            await _unitOfWork.CommitAsync();

            return Result.Success();
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception in UsersService.RegisterAsync for Nickname '{Nickname}'", request?.Nickname);
            return Result.Error(ex.Message);
        }
    }
}


