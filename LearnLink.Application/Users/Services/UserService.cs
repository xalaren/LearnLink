using LearnLink.Application.Data;
using LearnLink.Application.Security.Providers;
using LearnLink.Application.Shared.Pagination;
using LearnLink.Application.Shared.Responses;
using LearnLink.Application.Shared.Responses.Extensions;
using LearnLink.Application.Users.Mappers;
using LearnLink.Application.Users.Models;
using LearnLink.Application.Users.Validators;
using LearnLink.Domain.Entities.Users.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LearnLink.Application.Users.Services;

public class UserService(IApplicationDataContext context, IEncryptionProvider encryptionProvider, ILogger<UserService> logger)
{
    private readonly IApplicationDataContext _context = context;
    private readonly IEncryptionProvider _encryptionProvider = encryptionProvider;
    private readonly ILogger<UserService> _logger = logger;

    //public async Task<Response> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
    //{
    //    var responseBuilder = new ResponseBuilder();

    //    try
    //    {
    //        if (request == null)
    //        {
    //            return responseBuilder
    //                .Invalid()
    //                .WithMessage("Request is not provided")
    //                .Build();
    //        }

    //        var validation = new RegisterCommandValidator().Validate(request);

    //        if (!validation.IsValid)
    //        {
    //            return responseBuilder
    //                .Invalid()
    //                .WithMessage("One or more validation errors occured")
    //                .WithDetails(validation.AsErrors())
    //                .Build();
    //        }

    //        var exists = await _context
    //                .Users
    //                .AsNoTracking()
    //                .AnyAsync(user => user.Nickname == request.Nickname, cancellationToken);

    //        if (exists) return responseBuilder
    //                .Conflict()
    //                .WithMessage($"User with nickname \"{request.Nickname}\" is already exists")
    //                .Build();

    //        var user = User.Create(request.Nickname, request.Name, request.Lastname);

    //        var encryptedPassword = _encryptionProvider.Encrypt(request.Password);
    //        var credentials = Credentials.Create(encryptedPassword, user.Id, false, request.PasswordExpiration);

    //        _context.Users.Add(user);
    //        _context.Credentials.Add(credentials);

    //        await _context.CommitAsync(cancellationToken);

    //        return responseBuilder
    //            .Succeed()
    //            .WithMessage("User registered successfully")
    //            .Build();
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex, "Unhandled exception in UsersService.RegisterAsync for Nickname '{Nickname}'", request?.Nickname);

    //        return responseBuilder
    //            .Fail()
    //            .WithMessage("Unknown errors occured during user register")
    //            .Build();
    //    }
    //}

    public async Task<Response<PagedContent<UserDto>>> ListAsync(ListRequest request, CancellationToken cancellationToken = default)
    {
        var responseBuilder = new ResponseBuilder<PagedContent<UserDto>>();

        try
        {
            if(request == null)
            {
                return responseBuilder
                    .Invalid()
                    .WithMessage("Request is not provided")
                    .Build();
            }

            var validation = new ListRequestValidator().Validate(request);

            if(!validation.IsValid)
            {
                return responseBuilder
                    .Invalid()
                    .WithMessage("One or more validation error occured")
                    .WithDetails(validation.AsErrors())
                    .Build();
            }

            var query = _context
                    .Users
                    .AsNoTracking()
                    .Include(user => user.Role)
                    .Include(user => user.Avatar)
                    .SortBy(request);

            var count = await query.CountAsync(cancellationToken);

            var users = await query
                .Skip(request.Skip)
                .Take(request.Take)
                .Select(user => user.ToDto())
                .ToListAsync(cancellationToken);

            var pagedResponse = new PagedContent<UserDto>
            (
                Page: request.Page,
                PerPage: request.PerPage,
                Count: count,
                Items: users.AsReadOnly()
            );

            return responseBuilder
                .Succeed()
                .WithMessage("Users listed successfully")
                .WithContent(pagedResponse)
                .Build();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception in UserService.ListAsync");

            return responseBuilder
                .Fail()
                .WithMessage("Unknown errors occured during listing users")
                .Build();
        }
    }
}


