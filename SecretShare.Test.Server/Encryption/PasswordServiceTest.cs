using SecretShare.Application.Server.Encryption;
using SecretShare.Infrastructure.Encryption;

namespace SecretShare.Test.Server.Encryption;

public class PasswordServiceTest
{
    private readonly IPasswordService _passwordService = new PasswordService();

    [Fact]
    public void Generate_ShouldReturnValidPassword()
    {
        var password = _passwordService.GeneratePassword();
        Assert.True(password.Length == 32);
    }
}