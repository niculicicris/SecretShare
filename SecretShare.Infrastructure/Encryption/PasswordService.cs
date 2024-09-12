using SecretShare.Application.Server.Encryption;

namespace SecretShare.Infrastructure.Encryption;

public class PasswordService : IPasswordService
{
    public string GeneratePassword()
    {
        return Guid.NewGuid().ToString().Replace("-", "");
    }
}