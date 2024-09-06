using FluentValidation;

namespace SecretShare.Application.Server.Secrets.GetSecret;

public class GetSecretValidator : AbstractValidator<GetSecretRequest>
{
    public GetSecretValidator()
    {
        RuleFor(request => request.Password).NotEmpty();
        RuleFor(request => request.Password).Length(32);
    }
}