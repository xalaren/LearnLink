using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using LearnLink.Application.Extensions;
using LearnLink.Application.Mappers.Users;
using LearnLink.Application.Repositories;
using LearnLink.Application.Security;
using LearnLink.Application.Transactions;
using LearnLink.Application.Validators.Users;
using LearnLink.Domain.Entities.Users.Models;
using LearnLink.Shared.Model.Users;
using LearnLink.Shared.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LearnLink.Application.Services;

public class UserService(IUnitOfWork unitOfWork, IEncryptionService encryptionService, ILogger<UserService> logger)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IEncryptionService _encryptionService = encryptionService;
    private readonly ILogger<UserService> _logger = logger;

    public async Task<Result> RegisterAsync(RegisterRequest request, string password)
    {
        try
        {
            if(request == null)
            {
                return Result.Invalid(new ValidationError(nameof(request), "Request is not provided"));
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                return Result.Invalid(new ValidationError(nameof(password), "Password is required"));
            }

            var registerRequestValidator = new RegisterRequestValidator();
            var validationResult = registerRequestValidator.Validate(request);

            if (!validationResult.IsValid)
            {
                return Result.Invalid(validationResult.AsErrors());
            }

            var exists = await _unitOfWork
                    .Repository
                    .Users
                    .AsNoTracking()
                    .AnyAsync(user => user.Nickname == request.Nickname);

            if (exists) return Result.Invalid(new ValidationError(nameof(request.Nickname).ToCamelCase(), $"User with nickname \"{request.Nickname}\" is already exists"));

            var user = User.Create(request.Nickname, request.Name, request.Lastname);

            var encryptedPassword = _encryptionService.Encrypt(password);
            var credentials = Credentials.Create(encryptedPassword, user.Id, false, request.PasswordExpiration);

            _unitOfWork.Repository.Users.Add(user);
            _unitOfWork.Repository.Credentials.Add(credentials);

            await _unitOfWork.CommitAsync();

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception in UsersService.RegisterAsync for Nickname '{Nickname}'", request?.Nickname);
            return Result.Error(ex.Message);
        }
    }

    public async Task<Result<PagedResponse<UserDto>>> ListAsync(ListRequest request)
    {
        try
        {
            var validator = new ListRequestValidator();
            var validationResult = validator.Validate(request);

            if(!validationResult.IsValid) return Result.Invalid(validationResult.AsErrors());

            var baseQuery = _unitOfWork
                    .Repository
                    .Users
                    .AsNoTracking()
                    .Include(user => user.Role)
                    .Include(user => user.Avatar);


            var sortExpression = UsersSortingMapper.Resolve(request.SortBy);

            var sorted = request.Descending ?
                baseQuery.OrderByDescending(sortExpression) :
                baseQuery.OrderBy(sortExpression);
                    

            var count = await sorted.CountAsync();

            var paged = sorted
                    .Skip(request.Skip)
                    .Take(request.Take);

            var users = await paged
                .Select(user => user.ToDto())
                .ToListAsync();

            var pagedResponse = new PagedResponse<UserDto>
            (
                Page: request.Page, 
                PageSize: request.PageSize,
                TotalCount: count,
                Items: users.AsReadOnly()
            );

            return Result.Success(pagedResponse, "Users listed successfully");                
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception in UsersService.ListAsync");
            return Result.Error(ex.Message);
        }
    }

    public static IOrderedQueryable<User> Sort(IQueryable<User> query, bool descending, string? sortBy = null)
    {
        var sortExpression = UsersSortingMapper.Resolve(sortBy);
        if (descending)
        {
            return query.OrderByDescending(sortExpression);
        }

        return query.OrderBy(sortExpression);
    }
}


