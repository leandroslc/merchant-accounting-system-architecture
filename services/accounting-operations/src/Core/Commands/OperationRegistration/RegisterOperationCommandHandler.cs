using AccountingOperations.Core.Events;
using AccountingOperations.Core.Infrastructure.Broker;
using AccountingOperations.Core.Infrastructure.Data;
using MediatR;

namespace AccountingOperations.Core.Commands.OperationRegistration;

public sealed class RegisterOperationCommandHandler
    : IRequestHandler<RegisterOperationCommand>
{
    private readonly OperationsDbContext context;
    private readonly MessageExchangeBus messageExchangeBus;
    private readonly MessageExchangeTopics messageExchangeTopics;

    public RegisterOperationCommandHandler(
        OperationsDbContext context,
        MessageExchangeBus messageExchangeBus,
        MessageExchangeTopics messageExchangeTopics)
    {
        this.context = context;
        this.messageExchangeBus = messageExchangeBus;
        this.messageExchangeTopics = messageExchangeTopics;
    }

    public async Task Handle(
        RegisterOperationCommand command,
        CancellationToken cancellationToken)
    {
        var operation = command.ToAccountingOperation();

        context.Add(operation);
        await context.SaveChangesAsync(cancellationToken);

        var operationRegistered = new OperationRegistered(operation);

        await messageExchangeBus.Send(
            operationRegistered,
            messageExchangeTopics.OperationRegistered);
    }
}
