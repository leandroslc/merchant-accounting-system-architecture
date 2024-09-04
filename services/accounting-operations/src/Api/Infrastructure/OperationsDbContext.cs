using Microsoft.EntityFrameworkCore;

namespace AccountingOperations.Api.Infrastructure;

public sealed class OperationsDbContext : DbContext
{
    public OperationsDbContext(DbContextOptions<OperationsDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(OperationsDbContext).Assembly);
    }
}
