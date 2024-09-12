using FluentValidation;
using MediatR;
using SecretShare.Application.Server.Common.Extensions;
using SecretShare.Application.Server.Common.Results;
using SecretShare.Application.Server.Encryption;
using SecretShare.Application.Shared.Secrets;
using SecretShare.Domain.Secrets.Abstractions;

namespace SecretShare.Application.Server.Secrets.CreateSecret;

public class CreateSecretHandler(
    IValidator<CreateSecretRequest> validator,
    IPasswordService passwordService,
    ISecretService secretService,
    ISecretRepository secretRepository)
    : IRequestHandler<CreateSecretRequest, Result<SecretCredentialsDto>>
{
    public async Task<Result<SecretCredentialsDto>> Handle(CreateSecretRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) return validationResult.ToError();

        var password = passwordService.GeneratePassword();
        var secret = secretService.CreateSecret(request.Content, password);

        await secretRepository.InsertSecretAsync(secret);

        return new SecretCredentialsDto(secret.Id, password);
    }
}