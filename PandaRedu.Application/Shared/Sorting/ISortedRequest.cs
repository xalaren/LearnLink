using Newtonsoft.Json;

namespace PandaRedu.Application.Shared.Sorting;

/// <summary>
/// Represents sorting parameters for list requests.
/// </summary>
public interface ISortedRequest
{
    /// <summary>
    /// Indicates whether the sorting should be descending.
    /// </summary>
    [JsonProperty("descending")]
    bool Descending { get; }

    /// <summary>
    /// The field name to sort by. If null, a default sort will be applied.
    /// </summary>
    [JsonProperty("sortBy")]
    string? SortBy { get; }
}
