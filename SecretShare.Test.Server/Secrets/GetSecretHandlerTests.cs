using FluentValidation;
using FluentValidation.Results;
using Moq;
using SecretShare.Application.Server.Common.Errors;
using SecretShare.Application.Server.Secrets.GetSecret;
using SecretShare.Domain.Secrets.Abstractions;

namespace SecretShare.Test.Server.Secrets;

public class GetSecretHandlerTests
{
    private readonly Mock<IValidator<GetSecretRequest>> _validatorMock = new();
    private readonly Mock<ISecretService> _secretService = new();
    private readonly GetSecretHandler _getSecretHandler;

    public GetSecretHandlerTests()
    {
        _getSecretHandler = new GetSecretHandler(_validatorMock.Object, _secretService.Object);
    }

    [Fact]
    public async Task Handle_InvalidRequest_ShouldReturnValidationError()
    {
        var request = new GetSecretRequest(Guid.Empty, "");
        var validationResult = new ValidationResult(new List<ValidationFailure>
        {
            new("TestField", "Test message.")
        });

        _validatorMock.Setup(validator => validator.ValidateAsync(request, default))
            .ReturnsAsync(validationResult);

        var response = await _getSecretHandler.Handle(request, default);

        Assert.True(response.IsFailure);
        Assert.Equal(ErrorType.Validation, response.Error!.Type);
    }

    [Fact]
    public async Task Handle_InvalidSecret_ShouldReturnNotFoundError()
    {
        var id = Guid.NewGuid();
        var password = "TestPassword";
        var request = new GetSecretRequest(id, password);

        _validatorMock.Setup(validator => validator.ValidateAsync(request, default))
            .ReturnsAsync(new ValidationResult());

        _secretService.Setup(secretService => secretService.RetrieveSecret(id, password))
            .ReturnsAsync((string?)null);

        var response = await _getSecretHandler.Handle(request, default);

        Assert.True(response.IsFailure);
        Assert.Equal(ErrorType.NotFound, response.Error!.Type);
    }

    [Fact]
    public async Task Handle_ValidRequest_ShouldReturnSecretContent()
    {
        var id = Guid.NewGuid();
        var content = "TestContent";
        var password = "TestPassword";
        var request = new GetSecretRequest(id, password);

        _validatorMock.Setup(validator => validator.ValidateAsync(request, default))
            .ReturnsAsync(new ValidationResult());

        _secretService.Setup(secretService => secretService.RetrieveSecret(id, password))
            .ReturnsAsync(content);

        var response = await _getSecretHandler.Handle(request, default);

        Assert.True(response.IsSuccess);
        Assert.Equal(content, response.Value!.Content);
    }
}