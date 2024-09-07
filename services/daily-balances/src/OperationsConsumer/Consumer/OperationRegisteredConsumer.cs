using AccountingOperations.Core.Events;
using DailyBalances.Core.Payloads.UpdateBalance;
using FluentValidation;
using MassTransit;
using MediatR;

namespace DailyBalances.Worker.Consumer;

public sealed class OperationRegisteredConsumer : IConsumer<OperationRegistered>
{
    private readonly IValidator<UpdateBalancePayload> validator;
    private readonly ISender sender;

    public OperationRegisteredConsumer(
        IValidator<UpdateBalancePayload> validator,
        ISender sender)
    {
        this.validator = validator;
        this.sender = sender;
    }

    public async Task Consume(ConsumeContext<OperationRegistered> context)
    {
        var validationResult = validator.Validate(context.Message);

        if (!validationResult.IsValid)
        {
            return;
        }

        var command = context.Message.AsUpdateBalanceCommand();

        await sender.Send(command);
    }
}
