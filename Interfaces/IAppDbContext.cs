using Microsoft.EntityFrameworkCore;
using slp.light.Model;

namespace slp.light.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<User> Users { get; set; }

        Task SaveChanges(CancellationToken cancellationToken = default);
    }
}
