using LearnLink.Application.Shared.Responses;
using Microsoft.AspNetCore.Mvc;

namespace LearnLink.Api.Mappers.ResponseToResult.Abstractions;

/// <summary>
/// Maps application-level <see cref="Response"/> instances to minimal API
/// <see cref="IResult"/> values.
/// </summary>
public interface IResponseMapper
{
    /// <summary>
    /// Maps the provided <see cref="Response"/> to an <see cref="IResult"/>.
    /// </summary>
    public IResult Map(Response response);
}
