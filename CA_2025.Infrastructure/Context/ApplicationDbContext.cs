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
}
