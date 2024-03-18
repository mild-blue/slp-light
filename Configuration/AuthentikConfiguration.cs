using slp.light.Interfaces;

namespace Petricords.API.Configuration;

public static class AuthentikConfiguration
{
    public static async Task<WebApplication> ConfigureAuthentik(
        this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var authentik = services.GetRequiredService<IAuthentikClient>();
        await authentik.Initialize();

        return app;
    }
}
