using Newtonsoft.Json;

namespace slp.light.Interfaces.Authentik;

public record Application(
    [property: JsonProperty("pk")] string Pk,
    [property: JsonProperty("name")] string Name,
    [property: JsonProperty("slug")] string Slug,
    [property: JsonProperty("provider")] int? Provider,
    [property: JsonProperty("provider_obj")] Provider ProviderObj,
    [property: JsonProperty("launch_url")] string LaunchUrl,
    [property: JsonProperty("open_in_new_tab")] bool OpenInNewTab,
    [property: JsonProperty("meta_launch_url")] string MetaLaunchUrl,
    [property: JsonProperty("meta_icon")] object MetaIcon,
    [property: JsonProperty("meta_description")] string MetaDescription,
    [property: JsonProperty("meta_publisher")] string MetaPublisher,
    [property: JsonProperty("policy_engine_mode")] string PolicyEngineMode,
    [property: JsonProperty("group")] string Group
);