using FluentValidation;
using FluentValidation.Results;
using Moq;
using SecretShare.Application.Server.Common.Errors;
using SecretShare.Application.Server.Encryption;
using SecretShare.Application.Server.Secrets.CreateSecret;
using SecretShare.Domain.Secrets;
using SecretShare.Domain.Secrets.Abstractions;

namespace SecretShare.Test.Server.Secrets;

public class CreateSecretHandlerTests
{
    private readonly Mock<IValidator<CreateSecretRequest>> _validatorMock = new();
    private readonly Mock<IPasswordService> _passwordServiceMock = new();
    private readonly Mock<ISecretService> _secretServiceMock = new();
    private readonly Mock<ISecretRepository> _secretRepositoryMock = new();
    private readonly CreateSecretHandler _createSecretHandler;

    public CreateSecretHandlerTests()
    {
        _createSecretHandler = new CreateSecretHandler(_validatorMock.Object, _passwordServiceMock.Object,
            _secretServiceMock.Object, _secretRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_InvalidRequest_ShouldReturnValidationError()
    {
        var request = new CreateSecretRequest("");
        var validationResult = new ValidationResult(new List<ValidationFailure>
        {
            new("TestField", "Test message.")
        });

        _validatorMock.Setup(validator => validator.ValidateAsync(request, default))
            .ReturnsAsync(validationResult);

        var response = await _createSecretHandler.Handle(request, default);

        Assert.True(response.IsFailure);
        Assert.Equal(ErrorType.Validation, response.Error!.Type);
    }

    [Fact]
    public async Task Handle_ValidRequest_ShouldReturnSecretCredentials()
    {
        var id = Guid.NewGuid();
        var content = "TestContent";
        var password = "TestPassword";
        var secret = new Secret(id, "EncryptedTestContent");

        var request = new CreateSecretRequest(content);

        _validatorMock.Setup(validator => validator.ValidateAsync(request, default))
            .ReturnsAsync(new ValidationResult());

        _passwordServiceMock.Setup(passwordService => passwordService.GeneratePassword())
            .Returns(password);

        _secretServiceMock.Setup(secretService => secretService.CreateSecret(content, password))
            .Returns(secret);

        var response = await _createSecretHandler.Handle(request, default);

        _secretRepositoryMock.Verify(secretRepository => secretRepository.InsertSecretAsync(secret), Times.Once);
        Assert.True(response.IsSuccess);
        Assert.Equal(id, response.Value!.Id);
        Assert.Equal(password, response.Value!.Password);
    }
}