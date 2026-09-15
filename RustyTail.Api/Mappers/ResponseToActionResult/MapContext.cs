using RustyTail.Api.Mappers.ResponseToActionResult.MappingStrategies;
using RustyTail.Api.Mappers.ResponseToActionResult.Abstractions;
using RustyTail.Application.Shared.Responses;
using RustyTail.Application.Shared.Responses.Enums;
using Microsoft.AspNetCore.Mvc;

namespace RustyTail.Api.Mappers.ResponseToActionResult;

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
    /// Executes mapping from an application <see cref="Response"/> to an <see cref="ActionResult"/>.
    /// </summary>
    public ActionResult Execute(Response response, ControllerBase controller)
    {
        if(_responseMapperMatching.TryGetValue(response.Type, out var mapper))
        {
            return mapper.Map(response, controller);
        }

        throw new InvalidOperationException("No mapper for this response type defined");
    }
}
