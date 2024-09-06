using AccountingOperations.Core.Entities.Operations;

namespace AccountingOperations.Core.Infrastructure.Repositories;

public interface IAccountingOperationRepository
{
    Task Create(AccountingOperation operation);
}
