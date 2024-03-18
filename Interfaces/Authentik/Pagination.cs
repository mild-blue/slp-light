using Newtonsoft.Json;

namespace slp.light.Interfaces.Authentik;

public class Pagination
{
    [JsonProperty("next")] public int Next { get; set; }
    [JsonProperty("previous")] public int Previous { get; set; }
    [JsonProperty("count")] public int Count { get; set; }
    [JsonProperty("current")] public int Current { get; set; }
    [JsonProperty("total_pages")] public int TotalPages { get; set; }
    [JsonProperty("start_index")] public int StartIndex { get; set; }
    [JsonProperty("end_index")] public int EndIndex { get; set; }
}