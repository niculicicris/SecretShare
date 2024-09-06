namespace SecretShare.Domain.Secrets;

public class Secret
{
    public Secret(Guid id, string encryptedContent)
    {
        Id = id;
        EncryptedContent = encryptedContent;
    }

    public Guid Id { get; }

    public string EncryptedContent { get; }

    public DateTime CreationDate { get; private set; } = DateTime.Now;
}