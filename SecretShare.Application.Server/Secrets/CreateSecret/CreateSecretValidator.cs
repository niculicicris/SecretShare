using FluentValidation;

namespace SecretShare.Application.Server.Secrets.CreateSecret;

public class CreateSecretValidator : AbstractValidator<CreateSecretRequest>
{
    public CreateSecretValidator()
    {
        RuleFor(request => request.Content).MinimumLength(1);
        RuleFor(request => request.Content).MaximumLength(500);
    }
}