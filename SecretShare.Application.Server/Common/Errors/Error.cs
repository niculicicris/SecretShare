namespace SecretShare.Application.Server.Common.Errors;

public class Error
{
    private Error(string title, string description, ErrorType type, IDictionary<string, string[]>? errors = null)
    {
        Title = title;
        Description = description;
        Type = type;
        Errors = errors;
    }

    public string Title { get; }

    public string Description { get; }

    public ErrorType Type { get; }

    public IDictionary<string, string[]>? Errors { get; }

    public static Error Authorization(string title, string description, IDictionary<string, string[]>? errors = null)
    {
        return new Error(title, description, ErrorType.Authorization, errors);
    }

    public static Error NotFound(string title, string description, IDictionary<string, string[]>? errors = null)
    {
        return new Error(title, description, ErrorType.NotFound, errors);
    }

    public static Error Validation(string title, string description, IDictionary<string, string[]>? errors)
    {
        return new Error(title, description, ErrorType.Validation, errors);
    }
}