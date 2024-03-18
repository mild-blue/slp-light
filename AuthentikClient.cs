
using Microsoft.Extensions.Options;
using Pathoschild.Http.Client;
using slp.light.Interfaces;
using slp.light.Interfaces.Authentik;
using System.Net;

namespace Petricords.Infrastructure.Authentik;

public class AuthentikClient : IAuthentikClient
{
    private readonly IClient _client;
    private readonly AuthentikOptions _options;
    private readonly ILogger<AuthentikClient> _logger;
    private readonly IAppDbContext _dbContext;

    public AuthentikClient(IOptions<AuthentikOptions> options,
        HttpClient client,
        ILogger<AuthentikClient> logger,
        IAppDbContext dbContext)
    {
        _logger = logger;
        _options = options.Value;
        _dbContext = dbContext;
        _client = new FluentClient(new Uri(_options.BaseUrl), client);
        _client.AddDefault(x => x.WithBearerAuthentication(_options.AdminToken));
    }
    public async IAsyncEnumerable<UserList> GetAllUsers(int pageSize = 1000, string? path = null)
    {
        var page = 1;
        PaginatedResponse<UserList> response;
        do
        {
            var request = _client
                .GetAsync("api/v3/core/users/")
                .WithArgument("page_size", pageSize)
                .WithArgument("page", page++);

            if (!string.IsNullOrEmpty(path))
            {
                request = request.WithArgument("path", path);
            }

            response = await request.As<PaginatedResponse<UserList>>();
            foreach (var user in response.Results)
            {
                yield return user;
            }
        } while (page <= response.Pagination.TotalPages);
    }

    public async IAsyncEnumerable<Group> GetAllGroups(int pageSize = 1000)
    {
        var page = 1;
        PaginatedResponse<Group> response;
        do
        {
            response = await _client
                .GetAsync("api/v3/core/groups/")
                .WithArgument("page_size", pageSize)
                .WithArgument("page", page++)
                .As<PaginatedResponse<Group>>();

            foreach (var group in response.Results)
            {
                yield return group;
            }
        } while (page <= response.Pagination.TotalPages);
    }

    public async Task Initialize()
    {
        _logger.LogInformation("Initializing Authentik");
        await UpsertApplication();
    }

    private async Task UpsertApplication()
    {
        _logger.LogInformation("Making sure that the application exists");
        Application application;

        try
        {
            application = await _client
                .GetAsync($"api/v3/core/applications/{AuthentikOptions.ApplicationSlug}/")
                .As<Application>();
            _logger.LogInformation("Application exists");
        }
        catch (ApiException e) when (e.Status == HttpStatusCode.NotFound)
        {
            _logger.LogInformation("Application does not exist, creating it");
            var request = new CreateApplication(AuthentikOptions.ApplicationName, AuthentikOptions.ApplicationSlug);
            application = await _client
                .PostAsync("api/v3/core/applications/", request)
                .As<Application>();
        }

        await UpsertProvider(application);
    }
    private async Task UpsertProvider(Application application)
    {
        _logger.LogInformation("Making sure oAuth2 provider exists");
        if (application.Provider.HasValue)
        {
            _logger.LogInformation("Provider already assigned");
            return;
        }

        var flow = await GetDefaultFlow();
        var createProvider = new CreateOAuth2Provider(
            "SLP",
            flow.Pk,
            _options.ClientId,
            _options.ClientSecret,
            _options.RedirectUrl
        );

        _logger.LogInformation("Creating provider");
        var provider = await _client
            .PostAsync("api/v3/providers/oauth2/", createProvider)
            .As<Provider>();
        _logger.LogInformation("Provider created");

        _logger.LogInformation("Updating application. Assigning provider");
        await _client.PatchAsync($"api/v3/core/applications/{application.Slug}/", new UpdateApplication(provider.Pk));
        _logger.LogInformation("Application updated. Provider assigned");
    }
    private async Task<Flow> GetDefaultFlow()
    {
        var flows = await _client
            .GetAsync("api/v3/flows/instances/")
            .WithArgument("designation", "authorization")
            .WithArgument("ordering", "slug")
            .As<PaginatedResponse<Flow>>();

        return flows.Results.First();
    }
}