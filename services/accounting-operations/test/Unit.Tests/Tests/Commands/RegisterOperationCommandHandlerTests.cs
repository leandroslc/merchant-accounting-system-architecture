using AccountingOperations.Core.Commands.OperationRegistration;
using AccountingOperations.Core.Entities.Operations;
using AccountingOperations.Core.Events;
using AccountingOperations.Core.Infrastructure.Broker;
using AccountingOperations.Core.Infrastructure.Repositories;
using NSubstitute;

namespace AccountingOperations.UnitTests.Tests.Commands;

public class RegisterOperationCommandHandlerTests
{
    private readonly IAccountingOperationRepository accountingOperationRepository;
    private readonly IMessageExchangeBus messageExchangeBus;
    private readonly RegisterOperationCommandHandler handler;
    private readonly RegisterOperationCommand validCommand = new()
    {
        MerchantId = "8a61cbd3-d8b1-4ca7-b9a7-acb46b478617",
        RegistrationDate = new DateTime(2024, 05, 10),
        Value = 34.89M,
        Type = AccountingOperationType.Debit,
    };

    public RegisterOperationCommandHandlerTests()
    {
        accountingOperationRepository = Substitute.For<IAccountingOperationRepository>();
        messageExchangeBus = Substitute.For<IMessageExchangeBus>();
        handler = new(accountingOperationRepository, messageExchangeBus);
    }

    [Fact]
    public async Task Should_SaveOperation()
    {
        // Act
        await handler.Handle(validCommand, CancellationToken.None);

        // Assert
        await accountingOperationRepository
            .Received()
            .Create(Arg.Is<AccountingOperation>(p =>
                p.MerchantId == "8a61cbd3-d8b1-4ca7-b9a7-acb46b478617" &&
                p.RegistrationDate == new DateTime(2024, 05, 10) &&
                p.Value == 34.89M &&
                p.Type == AccountingOperationType.Debit));
    }

    [Fact]
    public async Task Should_SendOperationRegisteredMessage()
    {
        // Act
        await handler.Handle(validCommand, CancellationToken.None);

        // Assert
        await messageExchangeBus
            .Received()
            .Send(Arg.Is<OperationRegistered>(p =>
                p.MerchantId == "8a61cbd3-d8b1-4ca7-b9a7-acb46b478617" &&
                p.RegistrationDate == new DateTime(2024, 05, 10) &&
                p.Value == 34.89M &&
                p.Type == AccountingOperationType.Debit));
    }
}
