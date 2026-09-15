using RustyTail.Api.Mappers.ResponseToResult.Strategies;
using RustyTail.Api.Mappers.ResponseToResult.Abstractions;
using RustyTail.Application.Shared.Responses;
using RustyTail.Application.Shared.Responses.Enums;

namespace RustyTail.Api.Mappers.ResponseToResult;

public class MapContext
{
    private readonly Dictionary<ResponseTypes, IResponseMapper> _responseMapperMatching = new()
    {
        { ResponseTypes.Succeeded, new ToSuccessMapper() },
        { ResponseTypes.Invalid,  new ToBadRequestMapper() },
        { ResponseTypes.Forbidden,  new ToForbiddenMapper() },
        { ResponseTypes.Conflict,  new ToConfilctMapper() },
        { ResponseTypes.NotFound,  new ToNotFoundMapper() },
        { ResponseTypes.Failed,  new ToInternalServerErrorMapper() },
    };

    /// <summary>
    /// Executes mapping from an application <see cref="Response"/> to a minimal API <see cref="IResult"/>.
    /// </summary>
    public IResult Execute(Response response)
    {
        if(_responseMapperMatching.TryGetValue(response.Type, out var mapper))
        {
            return mapper.Map(response);
        }

        throw new InvalidOperationException("No mapper for this response type defined");
    }
}
