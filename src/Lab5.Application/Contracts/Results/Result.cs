namespace Itmo.ObjectOrientedProgramming.Lab5.Application.Contracts.Results;

public abstract record Result<T>
{
    private Result() { }

    public sealed record Success(T Value) : Result<T>;

    public sealed record Failure(ErrorType ErrorType) : Result<T>;
}