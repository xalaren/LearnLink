using Newtonsoft.Json;

namespace LearnLink.Shared.Sorting;

public interface ISortedRequest
{
    [JsonProperty("descending")]
    bool Descending { get; }

    [JsonProperty("sortBy")]
    string? SortBy { get; }
}
