using FluentValidation;
using MediatR;
using SecretShare.Application.Server.Encryption;
using SecretShare.Application.Server.Extensions;
using SecretShare.Application.Server.Results;
using SecretShare.Application.Shared.Secrets;
using SecretShare.Domain.Secrets;

namespace SecretShare.Application.Server.Secrets.CreateSecret;

public class CreateSecretHandler : IRequestHandler<CreateSecretRequest, Result<SecretCredentialsDto>>
{
    private readonly IPasswordEncryptor _passwordEncryptor;
    private readonly IPasswordGenerator _passwordGenerator;
    private readonly ISecretRepository _secretRepository;
    private readonly IValidator<CreateSecretRequest> _validator;

    public CreateSecretHandler(IValidator<CreateSecretRequest> validator, IPasswordGenerator passwordGenerator,
        IPasswordEncryptor passwordEncryptor, ISecretRepository secretRepository)
    {
        _validator = validator;
        _passwordGenerator = passwordGenerator;
        _passwordEncryptor = passwordEncryptor;
        _secretRepository = secretRepository;
    }

    public async Task<Result<SecretCredentialsDto>> Handle(CreateSecretRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) return validationResult.ToError();

        var password = _passwordGenerator.GeneratePassword();
        var encryptedContent = _passwordEncryptor.Encrypt(request.Content, password);
        var secret = new Secret(Guid.NewGuid(), encryptedContent);

        await _secretRepository.InsertSecretAsync(secret);

        return new SecretCredentialsDto(secret.Id, password);
    }
}