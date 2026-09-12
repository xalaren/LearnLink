using LearnLink.Application.Shared.Sorting;

namespace LearnLink.Shared.Sorting;

/// <summary>
/// Simple record representing sorting options for requests.
/// </summary>
/// <param name="Descending">Whether sorting should be descending.</param>
/// <param name="SortBy">Optional field name to sort by.</param>
public sealed record SortedRequest(bool Descending, string? SortBy) : ISortedRequest;
