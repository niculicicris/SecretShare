using FluentValidation;
using MediatR;
using SecretShare.Application.Server.Common.Extensions;
using SecretShare.Application.Server.Common.Results;
using SecretShare.Application.Server.Encryption;
using SecretShare.Application.Shared.Secrets;
using SecretShare.Domain.Secrets;

namespace SecretShare.Application.Server.Secrets.CreateSecret;

public class CreateSecretHandler(
    IValidator<CreateSecretRequest> validator,
    IPasswordGenerator passwordGenerator,
    IPasswordEncryptor passwordEncryptor,
    ISecretRepository secretRepository)
    : IRequestHandler<CreateSecretRequest, Result<SecretCredentialsDto>>
{
    public async Task<Result<SecretCredentialsDto>> Handle(CreateSecretRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) return validationResult.ToError();

        var password = passwordGenerator.GeneratePassword();
        var encryptedContent = passwordEncryptor.Encrypt(request.Content, password);
        var secret = new Secret(Guid.NewGuid(), encryptedContent);

        await secretRepository.InsertSecretAsync(secret);

        return new SecretCredentialsDto(secret.Id, password);
    }
}