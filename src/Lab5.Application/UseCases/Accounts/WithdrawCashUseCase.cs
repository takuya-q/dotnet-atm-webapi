using Itmo.ObjectOrientedProgramming.Lab5.Application.Abstractions.Repositories;
using Itmo.ObjectOrientedProgramming.Lab5.Application.Abstractions.Time;
using Itmo.ObjectOrientedProgramming.Lab5.Application.Contracts.Accounts;
using Itmo.ObjectOrientedProgramming.Lab5.Application.Contracts.Results;
using Itmo.ObjectOrientedProgramming.Lab5.Domain.Accounts;
using Itmo.ObjectOrientedProgramming.Lab5.Domain.History;
using Itmo.ObjectOrientedProgramming.Lab5.Domain.Sessions;

namespace Itmo.ObjectOrientedProgramming.Lab5.Application.UseCases.Accounts;

public sealed class WithdrawCashUseCase
{
    private readonly ISessionRepository _sessionRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IOperationHistoryRepository _operationHistoryRepository;
    private readonly ITimeProvider _timeProvider;

    public WithdrawCashUseCase(
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

    public Result<decimal> Execute(WithdrawCash.Request request)
    {
        if (request.Amount <= 0m)
            return new Result<decimal>.Failure(new ErrorType.InvalidAmount());

        Session? session = _sessionRepository.FindById(request.SessionId);

        if (session is null)
            return new Result<decimal>.Failure(new ErrorType.SessionNotFound());

        if (session is not UserSession userSession)
            return new Result<decimal>.Failure(new ErrorType.Unauthorized());

        Account? account = _accountRepository.FindById(userSession.AccountId);
        if (account is null)
            return new Result<decimal>.Failure(new ErrorType.AccountNotFound());

        if (account.Balance.Value < request.Amount)
            return new Result<decimal>.Failure(new ErrorType.InsufficientFunds());

        account.Withdraw(new Money(request.Amount));
        _accountRepository.Update(account);

        var entryId = new OperationHistoryEntryId(Guid.NewGuid());
        DateTimeOffset occurredAt = _timeProvider.GetUtcNow();

        var historyEntry = new OperationHistoryEntry(
            entryId,
            account.Id,
            new OperationType.CashWithdrawn(),
            new Money(request.Amount),
            account.Balance,
            occurredAt);

        _operationHistoryRepository.Add(historyEntry);

        return new Result<decimal>.Success(account.Balance.Value);
    }
}