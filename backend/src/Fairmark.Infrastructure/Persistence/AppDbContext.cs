using Microsoft.EntityFrameworkCore;

namespace Fairmark.Infrastructure.Persistence;

/// <summary>
/// The EF Core session with the PostgreSQL database. Entities are added together with the data model (FM-21).
/// </summary>
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Each entity is configured in its own IEntityTypeConfiguration<T> class in this project;
        // this line picks them all up, so OnModelCreating does not grow with every entity.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
