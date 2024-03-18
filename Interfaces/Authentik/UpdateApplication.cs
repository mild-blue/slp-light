using Newtonsoft.Json;

namespace slp.light.Interfaces.Authentik;

public record UpdateApplication(
    [property: JsonProperty("provider")] int? Provider
);