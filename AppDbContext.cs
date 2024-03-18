using Microsoft.EntityFrameworkCore;
using slp.light.Interfaces;
using slp.light.Model;

namespace slp.light;

public class AppDbContext : DbContext, IAppDbContext
{
#nullable disable
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    public Task SaveChanges(CancellationToken cancellationToken = default) =>
        SaveChangesAsync(cancellationToken);
}