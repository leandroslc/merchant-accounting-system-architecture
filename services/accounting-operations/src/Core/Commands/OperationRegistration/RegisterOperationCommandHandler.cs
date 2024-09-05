using MediatR;

namespace AccountingOperations.Core.Commands.OperationRegistration;

public sealed class RegisterOperationCommandHandler
    : IRequestHandler<RegisterOperationCommand>
{
    public Task Handle(
        RegisterOperationCommand request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
