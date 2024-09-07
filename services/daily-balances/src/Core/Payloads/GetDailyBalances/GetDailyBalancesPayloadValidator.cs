using FluentValidation;

namespace DailyBalances.Core.Payloads.GetDailyBalances;

public sealed class GetDailyBalancesPayloadValidator
    : AbstractValidator<GetDailyBalancesPayload>
{
    public GetDailyBalancesPayloadValidator()
    {
        RuleFor(p => p.Day)
            .NotEmpty()
            .Matches("\\d{4}-\\d{2}-\\d{2}")
                .WithMessage("'{PropertyName}' must be a date in the format YYYY-MM-DD.")
            .Must(p => DateTime.TryParse(p, out _))
                .WithMessage("'{PropertyName}' must be a valid date.")
            .WithName("day");
    }
}
