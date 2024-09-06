using SecretShare.Application.Server.Encryption;

namespace SecretShare.Infrastructure.Encryption;

public class PasswordGenerator : IPasswordGenerator
{
    public string GeneratePassword()
    {
        return Guid.NewGuid().ToString().Replace("-", "");
    }
}