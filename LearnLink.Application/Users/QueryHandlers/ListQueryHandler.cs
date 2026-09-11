using LearnLink.Application.Abstractions.Data;
using LearnLink.Application.Abstractions.Messaging;
using LearnLink.Application.Shared.Responses;
using LearnLink.Application.Users.Mappers;
using LearnLink.Application.Users.Queries;
using Microsoft.EntityFrameworkCore;

namespace LearnLink.Application.Users.QueryHandlers;

public class ListQueryHandler(IApplicationDataContext repository) : IQueryHandler<ListQuery, ListQueryResult>
{
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
