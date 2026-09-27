namespace Itmo.ObjectOrientedProgramming.Lab5.Application.Contracts.Results;

public abstract record ErrorType
{
    private ErrorType() { }

    public sealed record Unauthorized : ErrorType;

    public sealed record SessionNotFound : ErrorType;

    public sealed record AccountNotFound : ErrorType;

    public sealed record AccountAlreadyExists : ErrorType;

    public sealed record InvalidAmount : ErrorType;

    public sealed record InsufficientFunds : ErrorType;

    public sealed record InvalidCredentials : ErrorType;

    public sealed record InvalidPassword : ErrorType;
}