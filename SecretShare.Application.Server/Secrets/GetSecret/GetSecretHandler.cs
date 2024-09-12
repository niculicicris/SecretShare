using FluentValidation;
using MediatR;
using SecretShare.Application.Server.Common.Errors;
using SecretShare.Application.Server.Common.Extensions;
using SecretShare.Application.Server.Common.Results;
using SecretShare.Application.Shared.Secrets;
using SecretShare.Domain.Secrets.Abstractions;

namespace SecretShare.Application.Server.Secrets.GetSecret;

public class GetSecretHandler(
    IValidator<GetSecretRequest> validator,
    ISecretService secretService)
    : IRequestHandler<GetSecretRequest, Result<SecretContentDto>>
{
    public async Task<Result<SecretContentDto>> Handle(GetSecretRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) return validationResult.ToError();

        var content = await secretService.RetrieveSecret(request.Id, request.Password);
        if (content is null) return SecretErrors.SecretNotFound(request.Id);

        return new SecretContentDto(content);
    }
}