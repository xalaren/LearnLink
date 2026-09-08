using LearnLink.Api.Mappers.ResponseToActionResult.MappingStrategies;
using LearnLink.Api.Mappers.ResponseToActionResult.Abstractions;
using LearnLink.Application.Shared.Responses;
using LearnLink.Application.Shared.Responses.Enums;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.Api.Mappers.ResponseToActionResult;

public class MapContext
{
    private readonly Dictionary<ResponseTypes, IResponseMapper> _responseMapperMatching = new()
    {
        { ResponseTypes.Invalid,  new ToBadRequestMapper() },
        { ResponseTypes.Forbidden,  new ToForbiddenMapper() },
        { ResponseTypes.Conflict,  new ToConfilctMapper() },
        { ResponseTypes.NotFound,  new ToNotFoundMapper() },
        { ResponseTypes.Failed,  new ToInternalServerErrorMapper() },
    };

    public ActionResult Execute(Response response, ControllerBase controller)
    {
        if(_responseMapperMatching.TryGetValue(response.Type, out var mapper))
        {
            return mapper.Map(response, controller);
        }

        throw new InvalidOperationException("No mapper for this response type defined");
    }
}
