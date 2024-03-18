using Newtonsoft.Json;

namespace slp.light.Interfaces.Authentik;

public record Provider(
    [property: JsonProperty("pk")] int Pk,
    [property: JsonProperty("name")] string Name,
    [property: JsonProperty("authorization_flow")] string AuthorizationFlow,
    [property: JsonProperty("property_mappings")] IReadOnlyList<string> PropertyMappings,
    [property: JsonProperty("component")] string Component,
    [property: JsonProperty("assigned_application_slug")] string AssignedApplicationSlug,
    [property: JsonProperty("assigned_application_name")] string AssignedApplicationName,
    [property: JsonProperty("verbose_name")] string VerboseName,
    [property: JsonProperty("verbose_name_plural")] string VerboseNamePlural,
    [property: JsonProperty("meta_model_name")] string MetaModelName
);