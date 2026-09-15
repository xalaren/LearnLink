namespace RustyTail.Application.Security.Models;

/// <summary>
/// Represents a pair of access and refresh tokens returned after successful authentication.
/// </summary>
/// <param name="AccessToken">Signed access token (e.g., JWT).</param>
/// <param name="RefreshToken">Refresh token used to obtain new access tokens.</param>
public record TokenPair(string AccessToken, string RefreshToken);
