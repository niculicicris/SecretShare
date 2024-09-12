using SecretShare.Application.Server.Common.Errors;

namespace SecretShare.Application.Server.Common.Results;

public class Result<TValue>
{
    private Result(TValue value)
    {
        IsSuccess = true;
        Value = value;
        Error = default;
    }

    private Result(Error error)
    {
        IsSuccess = false;
        Value = default;
        Error = error;
    }

    public Error? Error { get; }

    public TValue? Value { get; }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public static implicit operator Result<TValue>(TValue value)
    {
        return new Result<TValue>(value);
    }

    public static implicit operator Result<TValue>(Error error)
    {
        return new Result<TValue>(error);
    }

    public TResult Match<TResult>(Func<TValue, TResult> success, Func<Error, TResult> failure)
    {
        return IsSuccess ? success(Value!) : failure(Error!);
    }
}