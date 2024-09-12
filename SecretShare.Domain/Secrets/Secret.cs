namespace SecretShare.Domain.Secrets;

public class Secret(Guid id, string encryptedContent, DateTime creationDate)
{
    public Secret(Guid id, string encryptedContent) : this(id, encryptedContent, DateTime.UtcNow)
    {
    }

    public Guid Id { get; } = id;

    public string EncryptedContent { get; } = encryptedContent;

    public DateTime CreationDate { get; } = creationDate;
}