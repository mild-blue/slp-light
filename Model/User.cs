
namespace slp.light.Model;

public class User
{
    public const string SystemUserName = "system";

    /// <summary>
    /// ORM constructor only.
    /// </summary>
    private User()
    {
        Username = "";
    }

    public User(string username, string name)
    {
        Username = username;
        FullName = name;
    }

    public Guid Id { get; set; }

    public string Username { get; set; } = "";

    public string FullName { get; set; } = "";

    public string? LegacyKey { get; init; }
    public List<RoleName> Roles { get; private set; } = [RoleName.PublicViewer];
    public bool IsActive { get; set; } = true;
}