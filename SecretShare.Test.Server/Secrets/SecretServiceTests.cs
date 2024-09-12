using Moq;
using SecretShare.Domain.Encryption;
using SecretShare.Domain.Secrets;
using SecretShare.Domain.Secrets.Abstractions;

namespace SecretShare.Test.Server.Secrets;

public class SecretServiceTests
{
    private readonly Mock<IEncryptionService> _encryptionServiceMock = new();
    private readonly Mock<ISecretExpirationService> _expirationServiceMock = new();
    private readonly Mock<ISecretRepository> _secretRepositoryMock = new();
    private readonly ISecretService _secretService;

    public SecretServiceTests()
    {
        _secretService = new SecretService(_encryptionServiceMock.Object, _expirationServiceMock.Object,
            _secretRepositoryMock.Object);
    }

    [Fact]
    public void CreateSecret_ValidEncryption_ShouldCreateSecret()
    {
        var content = "TestContent";
        var password = "TestPassword";
        var encryptedContent = "EncryptedTestContent";

        _encryptionServiceMock.Setup(encryptionService => encryptionService.Encrypt(content, password))
            .Returns(encryptedContent);

        var secret = _secretService.CreateSecret(content, password);

        Assert.Equal(encryptedContent, secret.EncryptedContent);
    }

    [Fact]
    public async Task RetrieveSecret_InvalidSecret_ShouldReturnNull()
    {
        var id = Guid.NewGuid();
        var password = "TestPassword";

        _secretRepositoryMock.Setup(secretRepository => secretRepository.GetSecretAsync(id))
            .ReturnsAsync((Secret?)null);

        var content = await _secretService.RetrieveSecret(id, password);

        Assert.Null(content);
    }

    [Fact]
    public async Task RetrieveSecret_ExpiredSecret_ShouldReturnNull()
    {
        var id = Guid.NewGuid();
        var password = "TestPassword";
        var secret = new Secret(id, "EncryptedTestContent");

        _secretRepositoryMock.Setup(secretRepository => secretRepository.GetSecretAsync(id))
            .ReturnsAsync(secret);

        _expirationServiceMock.Setup(expirationService => expirationService.IsSecretExpired(secret))
            .Returns(true);

        var content = await _secretService.RetrieveSecret(id, password);

        _expirationServiceMock.Verify(expirationService => expirationService.IsSecretExpired(secret), Times.Once);
        _secretRepositoryMock.Verify(secretRepository => secretRepository.DeleteSecretAsync(id), Times.Once);
        Assert.Null(content);
    }

    [Fact]
    public async Task RetrieveSecret_WrongPassword_ShouldReturnNull()
    {
        var id = Guid.NewGuid();
        var password = "WrongTestPassword";
        var secret = new Secret(id, "EncryptedTestContent");

        _secretRepositoryMock.Setup(secretRepository => secretRepository.GetSecretAsync(id))
            .ReturnsAsync(secret);

        _encryptionServiceMock.Setup(encryptionService => encryptionService.Matches(secret.EncryptedContent, password))
            .Returns(false);

        var content = await _secretService.RetrieveSecret(id, password);

        _encryptionServiceMock.Verify(encryptionService => encryptionService.Matches(secret.EncryptedContent, password),
            Times.Once);
        Assert.Null(content);
    }

    [Fact]
    public async Task RetrieveSecret_ShouldReturnContent()
    {
        var id = Guid.NewGuid();
        var originalContent = "TestContent";
        var password = "TestPassword";
        var secret = new Secret(id, "EncryptedTestContent");

        _secretRepositoryMock.Setup(secretRepository => secretRepository.GetSecretAsync(id))
            .ReturnsAsync(secret);

        _encryptionServiceMock.Setup(encryptionService => encryptionService.Matches(secret.EncryptedContent, password))
            .Returns(true);

        _encryptionServiceMock.Setup(encryptionService => encryptionService.Decrypt(secret.EncryptedContent, password))
            .Returns(originalContent);

        var content = await _secretService.RetrieveSecret(id, password);

        _secretRepositoryMock.Verify(secretRepository => secretRepository.DeleteSecretAsync(id), Times.Once);
        Assert.Equal(originalContent, content);
    }
}