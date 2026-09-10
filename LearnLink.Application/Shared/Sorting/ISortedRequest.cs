using Newtonsoft.Json;

namespace LearnLink.Application.Shared.Sorting;

public interface ISortedRequest
{
    [JsonProperty("descending")]
    bool Descending { get; }

    [JsonProperty("sortBy")]
    string? SortBy { get; }
}
