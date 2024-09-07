using MediatR;
using Microsoft.AspNetCore.Mvc;
using SecretShare.Application.Server.Secrets.CreateSecret;
using SecretShare.Application.Server.Secrets.GetSecret;
using SecretShare.Application.Shared.Secrets;
using SecretShare.Server.Extensions;

namespace SecretShare.Server.Controllers;

[ApiController]
[Route("secret/")]
public class SecretController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IResult> CreateSecret([FromBody] SecretContentDto secretContent,
        CancellationToken cancellationToken)
    {
        var request = new CreateSecretRequest(secretContent.Content);
        var response = await mediator.Send(request, cancellationToken);

        return response.Match(
            credentials => Results.Created("", credentials),
            error => error.ToProblemDetails()
        );
    }

    [HttpPost("{id:guid}")]
    public async Task<IResult> GetSecret(Guid id, [FromBody] SecretPasswordDto secretPassword,
        CancellationToken cancellationToken)
    {
        var request = new GetSecretRequest(id, secretPassword.Password);
        var response = await mediator.Send(request, cancellationToken);

        return response.Match(
            content => Results.Ok(content),
            error => error.ToProblemDetails()
        );
    }
}