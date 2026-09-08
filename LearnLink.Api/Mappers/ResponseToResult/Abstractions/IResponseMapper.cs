using LearnLink.Application.Shared.Responses;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.Api.Mappers.ResponseToResult.Abstractions;

public interface IResponseMapper
{
    public IResult Map(Response response);
}
