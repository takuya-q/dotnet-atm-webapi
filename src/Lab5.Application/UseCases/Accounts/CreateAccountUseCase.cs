using Itmo.ObjectOrientedProgramming.Lab5.Application.Abstractions.Repositories;
using Itmo.ObjectOrientedProgramming.Lab5.Application.Abstractions.Time;
using Itmo.ObjectOrientedProgramming.Lab5.Application.Contracts.Accounts;
using Itmo.ObjectOrientedProgramming.Lab5.Application.Contracts.Results;
using Itmo.ObjectOrientedProgramming.Lab5.Application.Dtos;
using Itmo.ObjectOrientedProgramming.Lab5.Domain.Accounts;
using Itmo.ObjectOrientedProgramming.Lab5.Domain.History;
using Itmo.ObjectOrientedProgramming.Lab5.Domain.Sessions;

namespace Itmo.ObjectOrientedProgramming.Lab5.Application.UseCases.Accounts;

public sealed class CreateAccountUseCase
{
    private readonly ISessionRepository _sessionRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IOperationHistoryRepository _operationHistoryRepository;
    private readonly ITimeProvider _timeProvider;

    private readonly int _accountNumberMinimumLength;
    private readonly int _accountNumberMaximumLength;
    private readonly int _pinCodeLength;

    public CreateAccountUseCase(
        ISessionRepository sessionRepository,
        IAccountRepository accountRepository,
        IOperationHistoryRepository operationHistoryRepository,
        ITimeProvider timeProvider)
    {
        _sessionRepository = sessionRepository;
        _accountRepository = accountRepository;
        _operationHistoryRepository = operationHistoryRepository;
        _timeProvider = timeProvider;

        _accountNumberMinimumLength = 6;
        _accountNumberMaximumLength = 32;
        _pinCodeLength = 4;
    }

    public Result<AccountDto> Execute(CreateAccount.Request request)
    {
        if (request.InitialBalance < 0m)
            return new Result<AccountDto>.Failure(new ErrorType.InvalidAmount());

        Session? session = _sessionRepository.FindById(request.SessionId);

        if (session is null)
            return new Result<AccountDto>.Failure(new ErrorType.SessionNotFound());

        if (session is not AdminSession)
            return new Result<AccountDto>.Failure(new ErrorType.Unauthorized());

        if (!TryNormalizeAccountNumber(request.AccountNumber, out string normalizedAccountNumber))
            return new Result<AccountDto>.Failure(new ErrorType.InvalidCredentials());

        if (!TryNormalizePinCode(request.PinCode, out string normalizedPinCode))
            return new Result<AccountDto>.Failure(new ErrorType.InvalidCredentials());

        var accountNumber = new AccountNumber(normalizedAccountNumber);

        Account? existingAccount = _accountRepository.FindByNumber(accountNumber);
        if (existingAccount is not null)
            return new Result<AccountDto>.Failure(new ErrorType.AccountAlreadyExists());

        var pinCode = new PinCode(normalizedPinCode);

        var accountId = new AccountId(Guid.NewGuid());
        var initialBalance = new Money(request.InitialBalance);

        var account = new Account(accountId, accountNumber, pinCode, initialBalance);
        _accountRepository.Add(account);

        var entryId = new OperationHistoryEntryId(Guid.NewGuid());
        DateTimeOffset occurredAt = _timeProvider.GetUtcNow();

        var historyEntry = new OperationHistoryEntry(
            entryId,
            account.Id,
            new OperationType.AccountCreated(),
            new Money(0m),
            account.Balance,
            occurredAt);

        _operationHistoryRepository.Add(historyEntry);

        var dto = new AccountDto(account.Number.Value, account.Balance.Value);
        return new Result<AccountDto>.Success(dto);
    }

    private bool TryNormalizeAccountNumber(string? accountNumber, out string normalized)
    {
        normalized = string.Empty;

        if (accountNumber is null)
            return false;

        string trimmed = accountNumber.Trim();

        if (trimmed.Length < _accountNumberMinimumLength)
            return false;

        if (trimmed.Length > _accountNumberMaximumLength)
            return false;

        if (!trimmed.All(char.IsDigit))
            return false;

        normalized = trimmed;
        return true;
    }

    private bool TryNormalizePinCode(string? pinCode, out string normalized)
    {
        normalized = string.Empty;

        if (pinCode is null)
            return false;

        string trimmed = pinCode.Trim();

        if (trimmed.Length != _pinCodeLength)
            return false;

        if (!trimmed.All(char.IsDigit))
            return false;

        normalized = trimmed;
        return true;
    }
}