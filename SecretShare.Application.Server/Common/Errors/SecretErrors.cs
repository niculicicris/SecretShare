namespace SecretShare.Application.Server.Common.Errors;

public static class SecretErrors
{
    public static Error ValidationError(IDictionary<string, string[]> errors)
    {
        return Error.Validation(
            "Error.Validation",
            "One or more fields failed validation. Please check the errors for each field.",
            errors
        );
    }

    public static Error SecretNotFound(Guid id)
    {
        return Error.NotFound(
            "Secret.NotFound",
            $"The secret with Id = '{id.ToString()}' was not found."
        );
    }

    public static Error InvalidSecretPassword(Guid id)
    {
        return Error.Authorization(
            "Secret.InvalidPassword",
            $"The password is not valid for the secret with Id = '{id.ToString()}'."
        );
    }
}