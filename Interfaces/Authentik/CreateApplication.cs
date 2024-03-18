using Newtonsoft.Json;

namespace slp.light.Interfaces.Authentik;

public record CreateApplication(
    [property: JsonProperty("name")] string Name,
    [property: JsonProperty("slug")] string Slug,
    [property: JsonProperty("group")] string Group = ""
);