using Microsoft.EntityFrameworkCore;
using Petricords.Infrastructure.Authentik;
using slp.light.Interfaces;
using slp.light.Model;

namespace Petricords.API.Workers;

public class AuthentikSynchronization : TimerService
{
    public static readonly SemaphoreSlim Locker = new(1, 1);
    private const int AuthentikMaxParenthesesLevel = 21;

    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<AuthentikSynchronization> _logger;

    public AuthentikSynchronization(IServiceProvider serviceProvider,
        ILogger<AuthentikSynchronization> logger)
        : base(TimeSpan.FromMinutes(5), logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public override async Task DoWork(CancellationToken stoppingToken)
    {
        await Locker.WaitAsync(stoppingToken);
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<IAppDbContext>();
            var authentikClient = scope.ServiceProvider.GetRequiredService<IAuthentikClient>();

            _logger.LogInformation("Synchronizing Authentik users into internal DB");
            var currentUsers = await context
                .Users
                .ToDictionaryAsync(x => x.Username, x => x, stoppingToken);

            var authentikUsers = authentikClient.GetAllUsers().WithCancellation(stoppingToken);
            var groups = authentikClient.GetAllGroups().WithCancellation(stoppingToken);

            var groupRolesMap = new Dictionary<Guid, RoleName>();
            var parentsMap = new Dictionary<Guid, Guid>();
            // fetch all groups and build parent map for further uses 
            // we need this here because Authentik won't "flatten" the permissions to a single array
            await foreach (var group in groups)
            {
                // add role to the group roles if it is relevant
                if (Enum.TryParse<RoleName>(group.Name, out var role))
                {
                    groupRolesMap.Add(group.Pk, role);
                }
                // and build map of parents
                if (group.ParentPk != null)
                {
                    parentsMap.Add(group.Pk, group.ParentPk.Value);
                }
            }

            var processedUsernames = new HashSet<string>();

            await foreach (var authUser in authentikUsers)
            {
                if (!processedUsernames.Add(authUser.Username.ToLowerInvariant()))
                {
                    _logger.LogWarning("Found username duplicate, differing only in casing. Skipping this one: {Username}", authUser.Username);
                    continue;
                }

                try
                {
                    if (currentUsers.TryGetValue(authUser.Username.ToLowerInvariant(), out var existingUser))
                    {
                        context.Users.Update(existingUser);  // todo: maybe not track this all the time, but only when needed
                    }
                    else
                    {
                        _logger.LogInformation("Creating user {User}", authUser.Username);
                        existingUser = new User(authUser.Username, authUser.Fullname);
                        context.Users.Add(existingUser);
                    }

                    // update properties
                    if (existingUser.Username != authUser.Username)
                    {
                        _logger.LogInformation("Updating username from {LocalUser} to {AuthentikUser}", existingUser.Username, authUser.Username);
                        existingUser.Username = authUser.Username;
                    }

                    if (existingUser.IsActive != authUser.ShouldBeActive)
                    {
                        _logger.LogInformation(
                            "Updating user {User} active status from {LocalStatus} to {AuthentikStatus} based on {@AuthentikUser}",
                            existingUser.Username,
                            existingUser.IsActive,
                            authUser.ShouldBeActive,
                            new
                            {
                                authUser.IsActive,
                                authUser.IsServiceAccount,
                                authUser.ShouldBeActive
                            });
                        existingUser.IsActive = authUser.ShouldBeActive;
                    }

                    var rolesToRemove = existingUser
                        .Roles
                        .Where(r => r != RoleName.PublicViewer && !authUser.Groups.Any(g => g.Name == r.ToString()));
                    foreach (var role in rolesToRemove)
                    {
                        _logger.LogInformation("Removing user {User} from role {Role}", existingUser.Username, role);
                        existingUser.Roles.Remove(role);
                    }

                    await context.SaveChanges(stoppingToken);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error while synchronizing user {User}: {Error}", authUser.Username, e.Message);
                }
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while synchronizing users: {Error}", e.Message);
        }
        finally
        {
            Locker.Release();
        }
    }
}