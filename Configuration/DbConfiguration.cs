namespace slp.light.Configuration;

using Microsoft.EntityFrameworkCore;

public static class DbConfiguration
{
    public static async Task<WebApplication> ConfigureDb(
        this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<AppDbContext>();

        await context.Database.MigrateAsync();

        return app;
    }
}
