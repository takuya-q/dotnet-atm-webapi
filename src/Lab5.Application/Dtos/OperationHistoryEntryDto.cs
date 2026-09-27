namespace Itmo.ObjectOrientedProgramming.Lab5.Application.Dtos;

public sealed record OperationHistoryEntryDto(
    string OperationType,
    decimal Amount,
    decimal BalanceAfter,
    DateTimeOffset OccurredAt);