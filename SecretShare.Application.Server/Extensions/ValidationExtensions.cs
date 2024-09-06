using FluentValidation.Results;
using SecretShare.Application.Server.Errors;

namespace SecretShare.Application.Server.Extensions;

public static class ValidationExtensions
{
    public static Error ToError(this ValidationResult validationResult)
    {
        var errors = validationResult.ToDictionary();
        return SecretErrors.ValidationError(errors);
    }
}