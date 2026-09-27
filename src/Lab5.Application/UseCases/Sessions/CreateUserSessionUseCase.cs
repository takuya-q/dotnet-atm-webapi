using Itmo.ObjectOrientedProgramming.Lab5.Application.Abstractions.Repositories;
using Itmo.ObjectOrientedProgramming.Lab5.Application.Contracts.Results;
using Itmo.ObjectOrientedProgramming.Lab5.Domain.Accounts;
using Itmo.ObjectOrientedProgramming.Lab5.Domain.Sessions;

namespace Itmo.ObjectOrientedProgramming.Lab5.Application.UseCases.Sessions;

public class CreateUserSessionUseCase
{
    private const int AccountNumberMinimumLength = 6;
    private const int AccountNumberMaximumLength = 32;
    private const int PinCodeLength = 4;

    private readonly IAccountRepository _accountRepository;
    private readonly ISessionRepository _sessionRepository;

    public CreateUserSessionUseCase(IAccountRepository accountRepository, ISessionRepository sessionRepository)
    {
        _accountRepository = accountRepository;
        _sessionRepository = sessionRepository;
    }

    public Result<SessionId> Execute(string accountNumber, string pinCode)
    {
        if (!TryNormalizeAccountNumber(accountNumber, out string normalizedAccountNumber))
            return new Result<SessionId>.Failure(new ErrorType.InvalidCredentials());

        if (!TryNormalizePinCode(pinCode, out string normalizedPinCode))
            return new Result<SessionId>.Failure(new ErrorType.InvalidCredentials());

        var accountNumberValue = new AccountNumber(normalizedAccountNumber);
        Account? account = _accountRepository.FindByNumber(accountNumberValue);

        if (account is null)
            return new Result<SessionId>.Failure(new ErrorType.AccountNotFound());

        var pinCodeValue = new PinCode(normalizedPinCode);

        if (!account.IsPinValid(pinCodeValue))
            return new Result<SessionId>.Failure(new ErrorType.InvalidCredentials());

        var sessionId = new SessionId(Guid.NewGuid());
        var session = new UserSession(sessionId, account.Id);

        _sessionRepository.Add(session);

        return new Result<SessionId>.Success(sessionId);
    }

    private bool TryNormalizeAccountNumber(string? accountNumber, out string normalized)
    {
        normalized = string.Empty;

        if (accountNumber is null)
            return false;

        string trimmed = accountNumber.Trim();

        if (trimmed.Length < AccountNumberMinimumLength)
            return false;

        if (trimmed.Length > AccountNumberMaximumLength)
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

        if (trimmed.Length != PinCodeLength)
            return false;

        if (!trimmed.All(char.IsDigit))
            return false;

        normalized = trimmed;
        return true;
    }
}