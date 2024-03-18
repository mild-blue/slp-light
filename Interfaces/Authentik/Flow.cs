using Newtonsoft.Json;

namespace slp.light.Interfaces.Authentik;

public record Flow(
    [property: JsonProperty("pk")] string Pk,
    [property: JsonProperty("name")] string Name,
    [property: JsonProperty("slug")] string Slug,
    [property: JsonProperty("title")] string Title
);