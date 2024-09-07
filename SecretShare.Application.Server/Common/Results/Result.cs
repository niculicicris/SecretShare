using SecretShare.Application.Server.Common.Errors;

namespace SecretShare.Application.Server.Common.Results;

public class Result<TValue>
{
    private readonly Error? _error;
    private readonly TValue? _value;

    private Result(TValue value)
    {
        IsSuccess = true;
        _value = value;
        _error = default;
    }

    private Result(Error error)
    {
        IsSuccess = false;
        _value = default;
        _error = error;
    }

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
        return IsSuccess ? success(_value!) : failure(_error!);
    }
}