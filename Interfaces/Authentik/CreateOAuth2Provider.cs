using Newtonsoft.Json;

namespace slp.light.Interfaces.Authentik;

public record CreateOAuth2Provider(
    [property: JsonProperty("name")] string Name,
    [property: JsonProperty("authorization_flow")] string AuthorizationFlow,
    [property: JsonProperty("client_id")] string ClientId,
    [property: JsonProperty("client_secret")] string ClientSecret,
    [property: JsonProperty("redirect_uris")] string RedirectUris,
    [property: JsonProperty("client_type")] string ClientType = "confidential"
);