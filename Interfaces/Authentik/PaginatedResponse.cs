using Newtonsoft.Json;

namespace slp.light.Interfaces.Authentik;

public class PaginatedResponse<T>
{
    [JsonProperty("pagination")] public Pagination Pagination { get; set; } = new();
    [JsonProperty("results")] public List<T> Results { get; set; } = new();
}