using FluentValidation;
using MediatR;
using SecretShare.Application.Server.Encryption;
using SecretShare.Application.Server.Errors;
using SecretShare.Application.Server.Extensions;
using SecretShare.Application.Server.Results;
using SecretShare.Application.Shared.Secrets;
using SecretShare.Domain.Secrets;

namespace SecretShare.Application.Server.Secrets.GetSecret;

public class GetSecretHandler : IRequestHandler<GetSecretRequest, Result<SecretContentDto>>
{
    private readonly IPasswordEncryptor _passwordEncryptor;
    private readonly ISecretRepository _secretRepository;
    private readonly IValidator<GetSecretRequest> _validator;

    public GetSecretHandler(IValidator<GetSecretRequest> validator, IPasswordEncryptor passwordEncryptor,
        ISecretRepository secretRepository)
    {
        _validator = validator;
        _passwordEncryptor = passwordEncryptor;
        _secretRepository = secretRepository;
    }

    public async Task<Result<SecretContentDto>> Handle(GetSecretRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) return validationResult.ToError();

        var secret = await _secretRepository.GetSecretAsync(request.Id);

        if (secret is null) return SecretErrors.SecretNotFound(request.Id);
        if (!_passwordEncryptor.Matches(secret.EncryptedContent, request.Password))
            return SecretErrors.InvalidSecretPassword(request.Id);

        return new SecretContentDto(_passwordEncryptor.Decrypt(secret.EncryptedContent, request.Password));
    }
}