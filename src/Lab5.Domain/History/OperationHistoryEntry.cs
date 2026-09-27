using Itmo.ObjectOrientedProgramming.Lab5.Domain.Accounts;

namespace Itmo.ObjectOrientedProgramming.Lab5.Domain.History;

public sealed class OperationHistoryEntry
{
    public OperationHistoryEntryId Id { get; }

    public AccountId AccountId { get; }

    public OperationType Type { get; }

    public Money Amount { get; }

    public Money BalanceAfter { get; }

    public DateTimeOffset OccurredAt { get; }

    public OperationHistoryEntry(
        OperationHistoryEntryId id,
        AccountId accountId,
        OperationType type,
        Money amount,
        Money balanceAfter,
        DateTimeOffset occurredAt)
    {
        ArgumentNullException.ThrowIfNull(type);

        if (occurredAt == default)
            throw new ArgumentException("OccurredAt is required", nameof(occurredAt));

        Id = id;
        AccountId = accountId;
        Type = type;
        Amount = amount;
        BalanceAfter = balanceAfter;
        OccurredAt = occurredAt;
    }
}