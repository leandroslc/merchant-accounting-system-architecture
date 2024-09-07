using FluentValidation;

namespace DailyBalances.Core.Payloads.UpdateBalance;

public sealed class UpdateBalancePayloadValidator
    : AbstractValidator<UpdateBalancePayload>
{
    public UpdateBalancePayloadValidator()
    {
        RuleFor(p => p.MerchantId)
            .NotEmpty();

        RuleFor(p => p.RegistrationDate)
            .NotEmpty();

        RuleFor(p => p.Value)
            .NotEmpty()
            .GreaterThanOrEqualTo(0);

        RuleFor(p => p.Type)
            .NotEmpty()
            .IsInEnum();
    }
}
