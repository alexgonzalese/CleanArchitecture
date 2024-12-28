namespace CleanArchitecture.Domain.Abstractions;

public class Result
{
    protected Result(bool isSuccess, Error? error)
    {
        if (isSuccess && error != Error.None)
        {
            throw new InvalidOperationException();
        }

        if (!isSuccess && error == Error.None)
        {
            throw new InvalidOperationException();
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error? Error { get; }

    public static Result Success() => new(true, Error.None);
    public static Result Failure(Error error) => new(false, error);
    private static Result<TValue> Success<TValue>(TValue value) => new(true, Error.None, value);
    private static Result<TValue> Failure<TValue>(Error error) => new(false, error, default!);
    protected static Result<TValue> Create<TValue>(TValue value) => value is not null ? Success(value) : Failure<TValue>(Error.NullValue);
}

public class Result<TValue> : Result
{
    private TValue _value { get; }

    public Result(bool isSuccess, Error? error, TValue value) : base(isSuccess, error)
    {
        _value = value;
    }

    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("No value for failure result.");

    public static implicit operator Result<TValue>(TValue value) => Create(value);
}