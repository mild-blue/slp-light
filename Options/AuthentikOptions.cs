using System.ComponentModel.DataAnnotations;

namespace slp.light.Interfaces.Authentik;

public class AuthentikOptions
{
    public const string SchemeName = "Authentik";
    public const string ApplicationSlug = "slpblue";
    public const string ApplicationName = "slp.blue";
    public const string DefaultAdminUsername = "akadmin";

    [Required] public string AdminToken { get; set; } = "";
    [Required] public string ClientId { get; set; } = "";
    [Required] public string ClientSecret { get; set; } = "";
    [Required] public string BaseUrl { get; set; } = "";

    /// <summary>
    /// If not empty, the application will use this password for all test users.
    /// </summary>
    public string TestUsersPassword { get; set; } = "";

    /// <summary>
    /// URL for redirecting back to the application.
    /// </summary>
    [Required]
    public string RedirectUrl { get; set; } = "";

    /// <summary>
    /// External URL of the Authentik instance. Used for redirects.
    /// </summary>
    public string ExternalUrl { get; set; } = "";

    /// <summary>
    /// This URL should be used to redirect the user to the user management page.
    /// </summary>
    public string UserManagementUrl => string.IsNullOrEmpty(ExternalUrl)
        ? $"{BaseUrl}/if/admin/#/identity/users"
        : $"{ExternalUrl}/if/admin/#/identity/users";

    public string AuthorizationEndpoint => string.IsNullOrEmpty(ExternalUrl)
        ? $"{BaseUrl}/application/o/authorize/"
        : $"{ExternalUrl}/application/o/authorize/";

    public string TokenEndpoint => $"{BaseUrl}/application/o/token/";
    public string UserInformationEndpoint => $"{BaseUrl}/application/o/userinfo/";

    /// <summary>
    /// When true, the app will add one test user for each role.
    /// </summary>
    public required bool AddTestUsers { get; init; }
}