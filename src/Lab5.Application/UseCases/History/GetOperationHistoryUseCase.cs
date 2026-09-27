using Itmo.ObjectOrientedProgramming.Lab5.Application.Abstractions.Repositories;
using Itmo.ObjectOrientedProgramming.Lab5.Application.Abstractions.Time;
using Itmo.ObjectOrientedProgramming.Lab5.Application.Contracts.History;
using Itmo.ObjectOrientedProgramming.Lab5.Application.Contracts.Results;
using Itmo.ObjectOrientedProgramming.Lab5.Application.Dtos;
using Itmo.ObjectOrientedProgramming.Lab5.Domain.Accounts;
using Itmo.ObjectOrientedProgramming.Lab5.Domain.History;
using Itmo.ObjectOrientedProgramming.Lab5.Domain.Sessions;

namespace Itmo.ObjectOrientedProgramming.Lab5.Application.UseCases.History;

public sealed class GetOperationHistoryUseCase
{
    private readonly ISessionRepository _sessionRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IOperationHistoryRepository _operationHistoryRepository;
    private readonly ITimeProvider _timeProvider;

    public GetOperationHistoryUseCase(
        ISessionRepository sessionRepository,
        IAccountRepository accountRepository,
        IOperationHistoryRepository operationHistoryRepository,
        ITimeProvider timeProvider)
    {
        _sessionRepository = sessionRepository;
        _accountRepository = accountRepository;
        _operationHistoryRepository = operationHistoryRepository;
        _timeProvider = timeProvider;
    }

    public Result<IReadOnlyCollection<OperationHistoryEntryDto>> Execute(GetOperationHistory.Request request)
    {
        Session? session = _sessionRepository.FindById(request.SessionId);

        if (session is null)
            return new Result<IReadOnlyCollection<OperationHistoryEntryDto>>.Failure(new ErrorType.SessionNotFound());

        if (session is not UserSession userSession)
            return new Result<IReadOnlyCollection<OperationHistoryEntryDto>>.Failure(new ErrorType.Unauthorized());

        Account? account = _accountRepository.FindById(userSession.AccountId);
        if (account is null)
            return new Result<IReadOnlyCollection<OperationHistoryEntryDto>>.Failure(new ErrorType.AccountNotFound());

        IReadOnlyCollection<OperationHistoryEntry> entries = _operationHistoryRepository.GetByAccountId(account.Id);

        OperationHistoryEntryDto[] dtos = entries
            .Select(x => new OperationHistoryEntryDto(
                GetOperationTypeName(x.Type),
                x.Amount.Value,
                x.BalanceAfter.Value,
                x.OccurredAt))
            .ToArray();

        var entryId = new OperationHistoryEntryId(Guid.NewGuid());
        DateTimeOffset occurredAt = _timeProvider.GetUtcNow();

        var historyViewedEntry = new OperationHistoryEntry(
            entryId,
            account.Id,
            new OperationType.OperationHistoryViewed(),
            new Money(0m),
            account.Balance,
            occurredAt);

        _operationHistoryRepository.Add(historyViewedEntry);

        return new Result<IReadOnlyCollection<OperationHistoryEntryDto>>.Success(dtos);
    }

    private string GetOperationTypeName(OperationType operationType)
    {
        return operationType switch
        {
            OperationType.AccountCreated => "AccountCreated",
            OperationType.BalanceViewed => "BalanceViewed",
            OperationType.CashDeposited => "CashDeposited",
            OperationType.CashWithdrawn => "CashWithdrawn",
            OperationType.OperationHistoryViewed => "OperationHistoryViewed",
            _ => "Unknown",
        };
    }
}