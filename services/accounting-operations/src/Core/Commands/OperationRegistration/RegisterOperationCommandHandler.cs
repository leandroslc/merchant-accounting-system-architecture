using AccountingOperations.Core.Events;
using AccountingOperations.Core.Infrastructure.Broker;
using AccountingOperations.Core.Infrastructure.Repositories;
using MediatR;

namespace AccountingOperations.Core.Commands.OperationRegistration;

public sealed class RegisterOperationCommandHandler
    : IRequestHandler<RegisterOperationCommand>
{
    private readonly IAccountingOperationRepository accountingOperationRepository;
    private readonly IMessageExchangeBus messageExchangeBus;
    private readonly MessageExchangeTopics messageExchangeTopics;

    public RegisterOperationCommandHandler(
        IAccountingOperationRepository accountingOperationRepository,
        IMessageExchangeBus messageExchangeBus,
        MessageExchangeTopics messageExchangeTopics)
    {
        this.accountingOperationRepository = accountingOperationRepository;
        this.messageExchangeBus = messageExchangeBus;
        this.messageExchangeTopics = messageExchangeTopics;
    }

    public async Task Handle(
        RegisterOperationCommand command,
        CancellationToken cancellationToken)
    {
        var operation = command.ToAccountingOperation();

        await accountingOperationRepository.Create(operation);

        var operationRegistered = new OperationRegistered(operation);

        await messageExchangeBus.Send(
            operationRegistered,
            messageExchangeTopics.OperationRegistered);
    }
}
