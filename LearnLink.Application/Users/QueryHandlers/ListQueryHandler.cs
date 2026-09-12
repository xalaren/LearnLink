using LearnLink.Application.Abstractions.Data;
using LearnLink.Application.Abstractions.Messaging;
using LearnLink.Application.Shared.Responses;
using LearnLink.Application.Users.Mappers;
using LearnLink.Application.Users.Queries;
using Microsoft.EntityFrameworkCore;

namespace LearnLink.Application.Users.QueryHandlers;

/// <summary>
/// Handles <see cref="ListQuery"/> requests by querying the repository,
/// applying sorting, paging and mapping users to DTOs.
/// </summary>
public class ListQueryHandler(IApplicationDataContext repository) : IQueryHandler<ListQuery, ListQueryResult>
{
    /// <summary>
    /// Processes the list query and returns a paged result of users.
    /// </summary>
    /// <param name="query">The list query containing paging and sorting options.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="Response{ListQueryResult}"/> containing paged users.</returns>
    public async Task<Response<ListQueryResult>> Handle(ListQuery query, CancellationToken cancellationToken = default)
    {
        var responseBuilder = new ResponseBuilder();

        var sortQuery = repository
                    .Users
                    .AsNoTracking()
                    .Include(user => user.Role)
                    .Include(user => user.Avatar)
                    .SortBy(query);

        var count = await sortQuery.CountAsync(cancellationToken);

        var users = await sortQuery
            .Skip(query.Skip)
            .Take(query.Take)
            .Select(user => user.ToDto())
            .ToListAsync(cancellationToken);

        var result = new ListQueryResult
        (
            Page: query.Page,
            PerPage: query.PerPage,
            Count: count,
            Items: users.AsReadOnly()
        );

        return responseBuilder
            .Succeed()
            .WithMessage("Users listed successfully")
            .WithContent(result)
            .Build();
    }
}
