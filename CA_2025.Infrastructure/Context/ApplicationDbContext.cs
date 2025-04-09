using CA_2025.Domain.Abstraction;
using CA_2025.Domain.Employees;
using GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace CA_2025.Infrastructure.Context;

internal sealed class ApplicationDbContext(DbContextOptions options) : DbContext(options), IUnitOfWork
{
    public DbSet<Employee> Employees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTimeOffset.Now;
        foreach (var entry in ChangeTracker.Entries<Entity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property(p => p.CreateAt).CurrentValue = now;
                entry.Property(p => p.UpdateAt).CurrentValue = now;
            }

            if (entry.State == EntityState.Modified)
            {
                if (entry.Property(p => p.IsDeleted).CurrentValue == true)
                {
                    entry.Property(p => p.DeleteAt).CurrentValue = now;
                }
                else
                {
                    entry.Property(p => p.UpdateAt).CurrentValue = now;
                }
            }
            if (entry.State == EntityState.Deleted)
            {
                throw new ArgumentException("Db'den direkt silme işlemi yapamazsınız");
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}