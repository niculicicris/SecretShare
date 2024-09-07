using FluentValidation;
using MediatR;
using SecretShare.Application.Server.Common.Errors;
using SecretShare.Application.Server.Common.Extensions;
using SecretShare.Application.Server.Common.Results;
using SecretShare.Application.Server.Encryption;
using SecretShare.Application.Shared.Secrets;
using SecretShare.Domain.Secrets;

namespace SecretShare.Application.Server.Secrets.GetSecret;

public class GetSecretHandler(
    IValidator<GetSecretRequest> validator,
    IPasswordEncryptor passwordEncryptor,
    ISecretRepository secretRepository)
    : IRequestHandler<GetSecretRequest, Result<SecretContentDto>>
{
    public async Task<Result<SecretContentDto>> Handle(GetSecretRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) return validationResult.ToError();

        var secret = await secretRepository.GetSecretAsync(request.Id);
        if (secret is null) return SecretErrors.SecretNotFound(request.Id);

        if (IsSecretExpired(secret))
        {
            await secretRepository.DeleteSecretAsync(secret.Id);
            return SecretErrors.SecretNotFound(secret.Id);
        }

        if (!passwordEncryptor.Matches(secret.EncryptedContent, request.Password))
            return SecretErrors.InvalidSecretPassword(secret.Id);

        var content = passwordEncryptor.Decrypt(secret.EncryptedContent, request.Password);
        await secretRepository.DeleteSecretAsync(secret.Id);

        return new SecretContentDto(content);
    }

    private bool IsSecretExpired(Secret secret)
    {
        return secret.CreationDate.AddDays(1) < DateTime.UtcNow;
    }
}