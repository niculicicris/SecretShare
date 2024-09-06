using MediatR;
using SecretShare.Application.Server.Results;
using SecretShare.Application.Shared.Secrets;

namespace SecretShare.Application.Server.Secrets.GetSecret;

public record GetSecretRequest(Guid Id, string Password) : IRequest<Result<SecretContentDto>>;