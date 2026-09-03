namespace LearnLink.Shared.Sorting;

public sealed record SortedRequest(bool Descending, string? SortBy) : ISortedRequest;
