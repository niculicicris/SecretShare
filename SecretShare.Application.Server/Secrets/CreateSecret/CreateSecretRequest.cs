using MediatR;
using SecretShare.Application.Server.Results;
using SecretShare.Application.Shared.Secrets;

namespace SecretShare.Application.Server.Secrets.CreateSecret;

public record CreateSecretRequest(string Content) : IRequest<Result<SecretCredentialsDto>>;