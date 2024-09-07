using FluentValidation.Results;
using SecretShare.Application.Server.Common.Errors;

namespace SecretShare.Application.Server.Common.Extensions;

public static class ValidationExtensions
{
    public static Error ToError(this ValidationResult validationResult)
    {
        var errors = validationResult.ToDictionary();
        return SecretErrors.ValidationError(errors);
    }
}