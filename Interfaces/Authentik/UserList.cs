using Newtonsoft.Json;

namespace slp.light.Interfaces.Authentik;

public class UserList
{
    [JsonProperty("pk")] public int Id { get; set; }
    [JsonProperty("username")] public required string Username { get; set; }
    [JsonProperty("name")] public string Fullname { get; set; } = "";
    [JsonProperty("email")] public string Email { get; set; } = "";
    [JsonProperty("is_active")] public bool IsActive { get; set; }
    [JsonProperty("last_login")] public string? LastLogin { get; set; }
    [JsonProperty("is_superuser")] public bool IsSuperuser { get; set; }
    [JsonProperty("groups_obj")] public List<Group> Groups { get; set; } = new();
    [JsonProperty("uid")] public string Uid { get; set; } = "";
    [JsonProperty("attributes")] public Dictionary<string, object> Attributes { get; set; } = new();

    private bool IsOutpost => Username.Contains("ak-outpost", StringComparison.OrdinalIgnoreCase);

    public bool IsServiceAccount => Attributes.Any(x =>
        x.Key.Contains("service-account", StringComparison.OrdinalIgnoreCase) &&
        x.Value is string stringValue &&
        bool.TryParse(stringValue, out var value) && value);

    public bool ShouldBeActive => !IsOutpost && !IsServiceAccount && IsActive;
}