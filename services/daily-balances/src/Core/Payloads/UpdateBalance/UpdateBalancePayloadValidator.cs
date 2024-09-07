using FluentValidation;

namespace DailyBalances.Core.Payloads.UpdateBalance;

public sealed class UpdateBalancePayloadValidator
    : AbstractValidator<UpdateBalancePayload>
{
    public UpdateBalancePayloadValidator()
    {
        RuleFor(p => p.MerchantId)
            .NotEmpty()
            .WithName("MerchantId");

        RuleFor(p => p.RegistrationDate)
            .NotEmpty()
            .WithName("RegistrationDate");

        RuleFor(p => p.Value)
            .NotEmpty()
            .GreaterThanOrEqualTo(0)
            .WithName("Value");

        RuleFor(p => p.Type)
            .NotEmpty()
            .IsInEnum()
            .WithName("Type");
    }
}
