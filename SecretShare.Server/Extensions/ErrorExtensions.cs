using SecretShare.Application.Server.Common.Errors;

namespace SecretShare.Server.Extensions;

public static class ErrorExtensions
{
    public static IResult ToProblemDetails(this Error error)
    {
        return Results.Problem(
            title: error.Title,
            detail: error.Description,
            statusCode: GetStatusCode(error.Type),
            extensions: GetExtensions(error.Errors)
        );
    }

    private static int GetStatusCode(ErrorType errorType)
    {
        switch (errorType)
        {
            case ErrorType.Authorization:
                return 401;
            case ErrorType.NotFound:
                return 404;
            case ErrorType.Validation:
                return 400;
            default:
                return 500;
        }
    }

    private static IDictionary<string, object?> GetExtensions(IDictionary<string, string[]>? errors)
    {
        if (errors == null || errors.Count == 0) return new Dictionary<string, object?>();

        return new Dictionary<string, object?>
        {
            { "errors", errors }
        };
    }
}