using SecretShare.Domain.Secrets;
using SecretShare.Domain.Secrets.Abstractions;

namespace SecretShare.Test.Server.Secrets;

public class SecretExpirationServiceTests
{
    private readonly ISecretExpirationService _secretExpirationService = new SecretExpirationService();

    [Fact]
    public void IsSecretExpired_ExpiredSecret_ShouldReturnTrue()
    {
        var secret = new Secret(Guid.NewGuid(), "TestEncryptedContent", DateTime.UtcNow.AddDays(-1));
        Assert.True(_secretExpirationService.IsSecretExpired(secret));
    }

    [Fact]
    public void IsSecretExpired_ValidSecret_ShouldReturnFalse()
    {
        var secret = new Secret(Guid.NewGuid(), "TestEncryptedContent");
        Assert.False(_secretExpirationService.IsSecretExpired(secret));
    }
}