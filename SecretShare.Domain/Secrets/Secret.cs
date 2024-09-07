namespace SecretShare.Domain.Secrets;

public class Secret(Guid id, string encryptedContent)
{
    public Guid Id { get; } = id;

    public string EncryptedContent { get; } = encryptedContent;

    public DateTime CreationDate { get; } = DateTime.Now;
}