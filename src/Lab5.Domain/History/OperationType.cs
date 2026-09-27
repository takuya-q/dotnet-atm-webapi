namespace Itmo.ObjectOrientedProgramming.Lab5.Domain.History;

public abstract record OperationType
{
    private OperationType() { }

    public sealed record AccountCreated : OperationType;

    public sealed record BalanceViewed : OperationType;

    public sealed record CashDeposited : OperationType;

    public sealed record CashWithdrawn : OperationType;

    public sealed record OperationHistoryViewed : OperationType;
}