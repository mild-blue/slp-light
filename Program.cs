using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Petricords.API.Configuration;
using Petricords.API.Workers;
using Petricords.Infrastructure.Authentik;
using slp.light;
using slp.light.Configuration;
using slp.light.Interfaces;
using slp.light.Interfaces.Authentik;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<AuthentikSynchronization>();
builder.Services.AddSingleton<AuthentikSynchronization>();

AddAuthentik(builder.Services, builder.Configuration.GetSection("Authentik"));

var connectionString = builder.Configuration.GetConnectionString("db") ?? "";
AddPersistence(builder.Services, connectionString);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.ConfigureAuthentik();

await app.ConfigureDb();

app.Run();


static IServiceCollection AddAuthentik(IServiceCollection services, IConfiguration configuration)
{
    services
        .AddOptions<AuthentikOptions>()
        .Bind(configuration)
        .ValidateDataAnnotations()
        .ValidateOnStart();

    services.AddOptions<CookieAuthenticationOptions>(CookieAuthenticationDefaults.AuthenticationScheme)
        .Configure<ITicketStore>((options, store) => { options.SessionStore = store; });

    services.AddHttpClient<IAuthentikClient, AuthentikClient>();
    return services;
}

static IServiceCollection AddPersistence(IServiceCollection services, string connectionString,
        bool sensitiveDataLoggingEnabled = false)
{
    services.AddDbContext<IAppDbContext, AppDbContext>(options =>
    {
        options.UseNpgsql(connectionString);
        options.EnableSensitiveDataLogging(sensitiveDataLoggingEnabled);
        options.UseProjectables();
    });

    return services;
}
