using AccountingOperations.Core.Entities.Operations;
using AccountingOperations.Core.Infrastructure.Data;

namespace AccountingOperations.Core.Infrastructure.Repositories;

public sealed class AccountingOperationRepository : IAccountingOperationRepository
{
    private readonly OperationsDbContext context;

    public AccountingOperationRepository(
        OperationsDbContext context)
    {
        this.context = context;
    }

    public async Task Create(AccountingOperation operation)
    {
        context.Set<AccountingOperation>().Add(operation);

        await context.SaveChangesAsync();
    }
}
