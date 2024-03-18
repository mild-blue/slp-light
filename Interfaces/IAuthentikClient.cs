using slp.light.Interfaces.Authentik;

namespace slp.light.Interfaces;

public interface IAuthentikClient
{
    IAsyncEnumerable<UserList> GetAllUsers(int pageSize = 1000, string? path = null);

    IAsyncEnumerable<Group> GetAllGroups(int pageSize = 1000);

    Task Initialize();
}