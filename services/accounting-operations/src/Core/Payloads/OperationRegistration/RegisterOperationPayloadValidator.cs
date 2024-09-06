using FluentValidation;

namespace AccountingOperations.Core.Payloads.OperationRegistration;

public sealed class RegisterOperationPayloadValidator
    : AbstractValidator<RegisterOperationPayload>
{
    public RegisterOperationPayloadValidator()
    {
        RuleFor(p => p.RegistrationDate)
            .NotEmpty()
            .Must(date => date.Kind == DateTimeKind.Utc)
                .WithMessage("'{PropertyName}' must be an UTC date.")
            .WithName("registrationDate");

        RuleFor(p => p.Value)
            .NotEmpty()
            .GreaterThanOrEqualTo(0)
            .WithName("value");
    }
}
