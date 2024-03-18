using Newtonsoft.Json;

namespace slp.light.Interfaces.Authentik;

public class Group
{
    [JsonProperty("pk")] public Guid Pk { get; set; }
    [JsonProperty("name")] public string Name { get; set; } = "";
    [JsonProperty("is_superuser")] public bool IsSuperuser { get; set; }

    [JsonProperty("parent")] public Guid? ParentPk { get; set; } = null;
    [JsonProperty("parent_name")] public string? ParentName { get; set; } = null;
}